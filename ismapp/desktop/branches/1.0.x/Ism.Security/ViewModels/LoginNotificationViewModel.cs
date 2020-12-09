using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;

namespace Ism.Security.ViewModels
{
    public  class LoginNotificationViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly IExceptionService _exceptionService;
        private string _userName;
        private string _password;
        private List<Company> _companies;
        private LoginConfirmation _notification;

        public LoginNotificationViewModel(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
            try
            {
                LoginCommand = new DelegateCommand<object>(OnLogin);
                SettingsCommand = new DelegateCommand(OnSettingsCommand);
                CancelCommand = new DelegateCommand(OnCancel);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnSettingsCommand()
        {
            try
            {
                _eventAggregator.GetEvent<ShowSettingsEvent>().Publish();
            }
            catch (Exception error)
            {
                _exceptionService.RaiseException(error);
            }
        }

        public string UserName { get { return _userName; } set { SetProperty(ref _userName, value); } }
        //private SecureString password;
        //public SecureString Password { get { return this.password; } set { SetProperty(ref this.password, value); } }

        public string Password { get { return _password; } set { SetProperty(ref _password, value); } }

        public List<Company> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }

        public DelegateCommand<object> LoginCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SettingsCommand { get; }
        
        private void OnLogin(object pwd)
        {
            try
            {
                if (_notification != null)
                {
                    PasswordBox box = pwd as PasswordBox;

                    _notification.Confirmed = true;
                    _notification.UserName = _userName;
                    _notification.Password = box?.Password;
                }
                FinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCancel()
        {
            try
            {
                if (_notification != null)
                    _notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                UserName = string.Empty;
                Password = string.Empty;
                SetProperty(ref _notification, value as LoginConfirmation);
            }
        }

        public Action FinishInteraction
        {
            get;
            set;
        }

        
        #endregion



    }
}
