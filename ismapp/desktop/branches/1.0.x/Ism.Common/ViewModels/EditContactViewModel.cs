using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System;
using Ism.Infrastructure;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;

namespace Ism.Common.ViewModels
{
    class EditContactViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private Contact _contact;
        private EditInteraction<Contact> _notification;
        private Uri _url;

        public EditContactViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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

        public Contact Contact
        {
            get { return _contact; }
            set { SetProperty(ref _contact, value); }
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
                    var notificaton = value as EditInteraction<Contact>;
                    if (notificaton == null) return;
                    Contact = notificaton.EditMode == EditMode.Edit ? notificaton.InteractionObject : new Contact() {UuId = Guid.NewGuid().ToString()};

                    if (null == notificaton.InteractionObject && notificaton.EditMode == EditMode.New)
                        notificaton.InteractionObject = Contact;
                    Contact.IsDirty = false;
                    Contact.PropertyDeletegate = (model) =>
                    {
                        SaveCommand.RaiseCanExecuteChanged();
                    };

                    SetProperty(ref _notification, notificaton);
                    SaveCommand.RaiseCanExecuteChanged();
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                    FinishInteraction();
                }
            }
        }

        public Action FinishInteraction { get; set; }

        

        private bool CanExecuteSaveComand()
        {
            try
            {
                return Contact != null && Contact.IsDirty;
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
                _notification.SaveAction?.Invoke(Contact, _notification.EditMode);
                OnFinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                FinishInteraction?.Invoke();
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
                Contact = null;
                SaveCommand.RaiseCanExecuteChanged();
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}
