using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;
using System.Collections.ObjectModel;

namespace Ism.Security.ViewModels
{
    class UserEditViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private User _user;
        private EditInteraction<User> _notification;
        private ObservableCollection<Scope> _scopes;

        public UserEditViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
            try
            {
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveComand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                AddScopeCommand = new DelegateCommand(OnAddScopeCommand);
                AddScopeInteractionRequest = new InteractionRequest<EditInteraction<Scope>>();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public User User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand AddScopeCommand { get; }

        public InteractionRequest<EditInteraction<Scope>> AddScopeInteractionRequest { get; }

        public ObservableCollection<Scope> Scopes { get { return _scopes; } set { SetProperty(ref _scopes, value); } }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    var notificaton = value as EditInteraction<User>;
                    if (notificaton == null) return;


                    User = notificaton.EditMode == EditMode.Edit ? notificaton.InteractionObject : new User() { UuId = Guid.NewGuid().ToString()};
                    User.Password = null;
                    User.IsDirty = false;
                    User.PropertyDeletegate = (model) =>
                    {
                        SaveCommand.RaiseCanExecuteChanged();
                    };

                    SetProperty(ref _notification, notificaton);

                    RefreshScopes();
                    SaveCommand.RaiseCanExecuteChanged();



                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                    OnFinishInteraction();
                }
            }
        }

        public Action FinishInteraction { get; set; }

        

        private bool CanExecuteSaveComand()
        {
            try
            {
                return User != null && User.IsDirty;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return false;
        }

        private void OnSaveCommand()
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveUserCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = true, PayLoad = User });

                _notification.SaveAction?.Invoke(User, EditMode.Undefined);
                OnFinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                FinishInteraction?.Invoke();
            }
        }

        private void OnConfirmSaveUserCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed)
                {
                    FinishInteraction();
                    return;
                }

                switch (_notification.EditMode)
                {
                    case EditMode.New:
                        AddUser(args.PayLoad as User);
                        break;
                    case EditMode.Edit:
                        UpdateUser(args.PayLoad as User);
                        break;
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void AddUser(User user)
        {
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                using (var rep = _serviceLocator.GetInstance<IRestRepository<User, User>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetAuthServer(), "/users/add").ToString(), user, _securityService.GetCurrentToken(), (u) =>
                    {
                        FinishInteraction();
                    });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void UpdateUser(User user)
        {
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                using (var rep = _serviceLocator.GetInstance<IRestRepository<User, User>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetAuthServer(), $"/users/update").ToString(), user, _securityService.GetCurrentToken(), (u) =>
                    {
                        FinishInteraction();
                    });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnFinishInteraction()
        {
            try
            {
                Clear();
                FinishInteraction?.Invoke();
                NavigateBack();

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void Clear()
        {
            try
            {
                User = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        private void RefreshScopes()
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Scope>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetAuthServer(), $"/scope/list").ToString(),_securityService.GetCurrentToken(), (list) =>
                    {
                        list.ForEach(ScopeChangeHandler);
                        Scopes = new ObservableCollection<Scope>(list);
                    });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ScopeChangeHandler(Scope scope)
        {
            try
            {
                if (User?.Scopes == null) return;

                if (User.Scopes.Any(s => s.Identifier == scope.Identifier))
                    scope.IsSelected = true;

                scope.PropertyDeletegate = (model) =>
                {
                    if (User == null) return;
                    try
                    {
                        using (var rep = _serviceLocator.GetInstance<IRestRepository<User, Scope>>())
                        {
                            var mscope = model as Scope;
                            var url = mscope.IsSelected ? new Uri(_settingsService.GetAuthServer(), $"/users/{User.UuId}/addscope").ToString() : new Uri(_settingsService.GetAuthServer(), $"/users/{User.UuId}/removescope").ToString();

                            rep.PostRequestAsync(url, mscope, _securityService.GetCurrentToken(), (u) =>
                            {
                                int a = 0;
                            });
                        }

                    }
                    catch (Exception exc)
                    {
                        _exceptionService.RaiseException(exc);
                    }
                };
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
          
        }

        private void OnAddScopeCommand()
        {
            try
            {
                var interaction = new EditInteraction<Scope>() { EditMode = EditMode.New, InteractionObject = null, Title = "Dodajanje nove pravice"};
                AddScopeInteractionRequest.Raise(interaction, OnAddScopeInteractionRequestCallback);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnAddScopeInteractionRequestCallback(EditInteraction<Scope> obj)
        {
            try
            {
                RefreshScopes();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
