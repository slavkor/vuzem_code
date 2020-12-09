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
    public class EditScopeViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private Scope _scope;
        private EditInteraction<Scope> _notification;
        private ObservableCollection<Scope> _scopes;

        public EditScopeViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public Scope Scope
        {
            get { return _scope; }
            set
            {
                SetProperty(ref _scope, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }


        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    var notificaton = value as EditInteraction<Scope>;
                    if (notificaton == null) return;


                    Scope = notificaton.EditMode == EditMode.Edit ? notificaton.InteractionObject : new Scope() { UuId = Guid.NewGuid().ToString() };
                    Scope.IsDirty = false;
                    Scope.PropertyDeletegate = (model) =>
                    {
                        SaveCommand.RaiseCanExecuteChanged();
                    };

                    SetProperty(ref _notification, notificaton);
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
                return Scope != null && Scope.IsDirty;
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
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveScopeCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = true, PayLoad = Scope });

                _notification.SaveAction?.Invoke(Scope, EditMode.Undefined);
                OnFinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                FinishInteraction?.Invoke();
            }
        }

        private void OnConfirmSaveScopeCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
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
                        AddScope(args.PayLoad as Scope);
                        break;
                    //case EditMode.Edit:
                    //    UpdateScope(args.PayLoad as Scope);
                    //    break;
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void AddScope(Scope scope)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Scope, Scope>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetAuthServer(), "/scope/add").ToString(), scope, _securityService.GetCurrentToken(), (u) =>
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

        //private void UpdateScope(Scope scope)
        //{
        //    try
        //    {
        //        FinishInteraction();
        //        return;

        //        using (var rep = _serviceLocator.GetInstance<IRestRepository<Scope, Scope>>())
        //        {
        //            rep.PostRequestAsync(new Uri(_settingsService.GetAuthServer(), $"/scopes/update").ToString(), scope, _securityService.GetCurrentUser().AccessToken, (u) =>
        //            {
        //                FinishInteraction();
        //            });
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        _exceptionService.RaiseException(exc);
        //    }
        //}

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
                Scope = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

    }
}
