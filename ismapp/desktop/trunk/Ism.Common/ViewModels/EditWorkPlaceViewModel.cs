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

namespace Ism.Common.ViewModels
{
    public class EditWorkPlaceViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private WorkPlace _workplace;
        private EditInteraction<WorkPlace> _notification;

        public EditWorkPlaceViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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

        public WorkPlace WorkPlace
        {
            get { return _workplace; }
            set
            {
                SetProperty(ref _workplace, value);
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
                    var notificaton = value as EditInteraction<WorkPlace>;
                    if (notificaton == null) return;


                    WorkPlace = notificaton.EditMode == EditMode.Edit ? notificaton.InteractionObject : new WorkPlace() { UuId = Guid.NewGuid().ToString()};
                    WorkPlace.IsDirty = false;
                    WorkPlace.PropertyDeletegate = (model) =>
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

        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

        }

        private bool CanExecuteSaveComand()
        {
            try
            {
                return WorkPlace != null && WorkPlace.IsDirty;
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
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnSaveWorkPlaceConfirm, Title = "ALO", Content = "Želiš dodati delovno mesto?", PayLoad = WorkPlace });


                //if (!obj.Confirmed) return;

                //    using (var rep = _serviceLocator.GetInstance<IRestRepository<WorkPlace, WorkPlace>>())
                //    {
                //        rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(true), "shrd/workplace/add").ToString(), obj.InteractionObject, _securityService.GetCurrentUser().AccessToken,
                //            list =>
                //            {
                //                RefreshWorkPlaces();
                //            });
                //    }
             

                //_notification.SaveAction?.Invoke(WorkPlace, EditMode.Undefined);
                OnFinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                FinishInteraction?.Invoke();
            }
        }

        private void OnSaveWorkPlaceConfirm(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;
                var workplace = args.PayLoad as WorkPlace;
                if (null == workplace) return;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<WorkPlace, WorkPlace>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(true), "shrd/workplace/add").ToString(), workplace, _securityService.GetCurrentToken(), wp =>
                        {
                            OnFinishInteraction();
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
                WorkPlace = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
