using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Prism.Commands;
using System.Security;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Prism.Events;
using Ism.Infrastructure.Events;
using Microsoft.Practices.ServiceLocation;
using Ism.Infrastructure.Services;
using System.Linq;
using System.Windows.Media.Imaging;
using Ism.Infrastructure.Interaction;
using Prism.Regions;
using System.IdentityModel.Tokens.Jwt;
namespace Ism.Security.ViewModels
{
    public class NavLoginViewModel : WindowAware, IInteractionRequestAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private INotification _notification;
        private bool _canLogin = true;
        private string _userName;
        private SecureString _password;
        private string _caption;
        private readonly IRegionManager _regionManager;

        public NavLoginViewModel(IEventAggregator eventAggregator, IServiceLocator serviceLocator, IRegionManager regionManager, IExceptionService exceptionService)
        {
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));
            if (null == serviceLocator)
                throw new ArgumentNullException(nameof(serviceLocator));
            if (null == regionManager)
                throw new ArgumentNullException(nameof(regionManager));

            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _regionManager = regionManager;
            _settings = _serviceLocator.GetInstance<ISettingsService>();
            _securityService = _serviceLocator.GetInstance<ISecurityService>();
            _exceptionService = exceptionService;
            _eventAggregator.GetEvent<LoginEvent>().Subscribe(OnLoginEvent);
            _eventAggregator.GetEvent<LogoutEvent>().Subscribe(OnLogoutEvent);
            _eventAggregator.GetEvent<SettingsEvent>().Subscribe(OnSettingsEvent);
            LoginLogOutCommand = new DelegateCommand(OnLoginLogOutCommand);
            LoginConfirmationRequest = new InteractionRequest<LoginConfirmation>();

            CompanySelectRequest = new InteractionRequest<ListInteraction<Company>>();
            _eventAggregator.GetEvent<ListEvent<Company>>().Subscribe(OnCompanyListEvent);

            Caption = "Login";
            CanLogin = true;
        }

        private void OnSettingsEvent()
        {
            try
            {
                var security = _serviceLocator.GetInstance<ISecurityService>();
                var user = security.GetCurrentToken();
                if(null == user) return;

                _regionManager.RequestNavigate(Infrastructure.RegionNames.NavigaionRegion, "NavUsers");
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnCompanyListEventCallback(Company obj)
        {
            try
            {
                var securityService = _serviceLocator.GetInstance<ISecurityService>();
                securityService?.SetCurrentCompany(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnLogoutEvent(User obj)
        {
            Caption = "Login";
            CanLogin = true;
        }

        private void OnLoginEvent(Token obj)
        {
            Caption = "Exit";
            CanLogin = false;
        }

        public bool CanLogin
        {
            get { return _canLogin; }
            set { SetProperty(ref _canLogin, value); }
        }

        public string Caption
        {
            get { return _caption; }
            set { SetProperty(ref _caption, value); }
        }

        public string UserName { get { return _userName; } set { SetProperty(ref _userName, value); } }
        public SecureString Password { get { return _password; } set { SetProperty(ref _password, value); } }

        public DelegateCommand LoginLogOutCommand { get; }

        public InteractionRequest<LoginConfirmation> LoginConfirmationRequest { get; }
        public InteractionRequest<ListInteraction<Company>> CompanySelectRequest { get; }
        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value); }
        }

        public Action FinishInteraction { get; set; }

        private void OnLoginLogOutCommand()
        {
            try
            {
                if (CanLogin)
                    LoginConfirmationRequest.Raise(new LoginConfirmation() {Title = "Enter login"}, OnLoginResponse);
                else
                    _eventAggregator.GetEvent<LogoutEvent>().Publish(null);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        protected void OnLoginResponse(LoginConfirmation context)
        {
            try
            {
                if (!context.Confirmed)
                    return;
                
                var userCredentials = new UserCredentials() {GrantType = "password", ClinetId = "", ClinetSecret = "abc123", UserName = context.UserName, PassWord = context.Password };
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Token, UserCredentials>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetAuthServer(), "/access").ToString(), userCredentials, (token) =>
                    {



                        if(null == token) return;

                        _eventAggregator.GetEvent<LoginEvent>().Publish(token);

                        //using (var userRep = _serviceLocator.GetInstance<IRestRepository<User, string>>())
                        //{
                        //    userRep.GetRequestAsync(new Uri(_settings.GetAuthServer(), "access/get/token").ToString(), token, (u) =>
                        //    {
                        //        if (null == u) return;
                        //        u.AccessToken = token;

                               
                        //    });
                        //}
                    });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCompanyListEvent(ListEventArgs<Company> obj)
        {
            try
            {
                bool raiseSelectRequest = true;

                if (obj.RememberSelection && ! obj.ForceListSelection)
                {
                    var firmId = _settings.GetLastUsedFirm(_securityService.GetCurrentToken().GetClaim("sub"));
                    if (!string.IsNullOrEmpty(firmId))
                    {
                        var common = _serviceLocator.TryResolve<ICommonService>();

                        var compnay = common?.GetCompanies().Where(c => c.UuId.Equals(firmId)).FirstOrDefault();

                        if (null != compnay)
                        {
                            _securityService.SetCurrentCompany(common.GetCompanies().Where(c => c.UuId.Equals(firmId)).FirstOrDefault());
                            raiseSelectRequest = false;
                        }
                        else
                            raiseSelectRequest = true; 
                    }
                }



                if (raiseSelectRequest)
                    CompanySelectRequest.Raise(new ListInteraction<Company>() { Title = "Izberi podjetje", SelectAction = obj.SelectAction, ListEventArgs = obj }, OnCompanySelectRequestCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnCompanySelectRequestCallback(ListInteraction<Company> obj)
        {
            try
            {
                if (!obj.Confirmed) return;

                if (obj.ListEventArgs.RememberSelection)
                {
                    _settings.SetLastUsedFirm(_securityService.GetCurrentToken().GetClaim("sub"), obj.InteractionObject.UuId);
                }

                obj.SelectAction?.Invoke(obj.InteractionObject);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private BitmapImage LoadImage(string uri, UriKind uriKind)
        {
            return new BitmapImage(new Uri(uri, uriKind));
        }

    }
}
