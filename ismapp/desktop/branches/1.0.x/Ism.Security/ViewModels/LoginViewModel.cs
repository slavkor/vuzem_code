using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Model;
using Prism.Events;
using Ism.Infrastructure.Events;

namespace Ism.Security.ViewModels
{

    public class LoginViewModel : WindowAware, IInteractionRequestAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IServiceLocator _serviceLocator;
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        private string _userName;
        private string _password;
        private List<Company> _companies;
        private LoginConfirmation _notification;

        public LoginViewModel(IRegionManager regionManager, IServiceLocator serviceLocator, IEventAggregator eventAggregator, IExceptionService exceptionService)
        {
            if (null == regionManager)
                throw new ArgumentNullException(nameof(regionManager));
            if (null == serviceLocator)
                throw new ArgumentNullException(nameof(serviceLocator));
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));
            _regionManager = regionManager;
            _serviceLocator = serviceLocator;
            _eventAggregator = eventAggregator;

            _exceptionService = exceptionService;
            try
            {
                LoginCommand = new DelegateCommand(OnLogin);
                CancelCommand = new DelegateCommand(OnCancel);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        public string UserName { get { return _userName; } set { SetProperty(ref _userName, value); } }
        //private SecureString password;
        //public SecureString Password { get { return this.password; } set { SetProperty(ref this.password, value); } }

        public string Password { get { return _password; } set { SetProperty(ref _password, value); } }


        public List<Company> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies ,value); }
        }

        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        private void OnLogin()
        {
            try
            {
                if (_notification != null)
                {
                    _notification.Confirmed = true;
                    _notification.UserName = _userName;
                    _notification.Password = _password;
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
