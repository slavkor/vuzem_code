using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Model;

namespace Ism.Sys.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly IExceptionService _exceptionService;
        private EditInteraction<object> _notification;
        private string _apiServer;
        private string _authServer;
        private string _printServer;
        private string _printServerUser;
        private string _printServerPwd;

        public SettingsViewModel(IExceptionService exceptionService)
        {

            _exceptionService = exceptionService;

            try
            {
                SaveSettingsCommand = new DelegateCommand(OnSaveSettingsCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                using (var settingsService = _serviceLocator.GetInstance<ISettingsService>())
                {
                    ApiServer = settingsService.GetApiServer()?.ToString();
                    AuthServer = settingsService.GetAuthServer()?.ToString();

                    if (string.IsNullOrEmpty(ApiServer)) settingsService.SetApiServer("https://api.ismvuzem.si/");
                    if (string.IsNullOrEmpty(AuthServer)) settingsService.SetAuthServer("https://auth.ismvuzem.si/");

                    var ps = settingsService.GetPrintServer();
                    //if (null == ps) settingsService.SetPrintServer("http://api.ismvuzem.si:8080/jasperserver/rest_v2/reports/", "jasperadmin", "");
                    ps = settingsService.GetPrintServer();

                    PrintServer = ps.ServerUri.ToString();
                    PrintServerUser = ps.User;
                    PrintServerPwd = ps.Password;
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #region Commands
        public DelegateCommand SaveSettingsCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        #endregion

        #region Public properties

        public string ApiServer
        {
            get { return _apiServer; }
            set { SetProperty(ref _apiServer, value); }
        }

        public string AuthServer
        {
            get { return _authServer; }
            set { SetProperty(ref _authServer, value); }
        }

        public string PrintServer
        {
            get { return _printServer; }
            set { SetProperty(ref _printServer, value); }
        }

        public string PrintServerUser
        {
            get { return _printServerUser; }
            set { SetProperty(ref _printServerUser,value); }
        }

        public string PrintServerPwd
        {
            get { return _printServerPwd; }
            set { SetProperty(ref _printServerPwd, value); }
        }

        #endregion

        #region Command Handlers
        private void OnSaveSettingsCommand()
        {
            try
            {
                using (var settingsService = _serviceLocator.GetInstance<ISettingsService>())
                {
                    settingsService.SetApiServer(_apiServer);
                    settingsService.SetAuthServer(_authServer);
                    settingsService.SetPrintServer(_printServer, _printServerUser, _printServerPwd);
                }
                _notification.Confirmed = true;
                FinishInteraction?.Invoke();
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
                _notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        #endregion


        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notificaiton = value as EditInteraction<object>;
                if (null == notificaiton) return;
                SetProperty(ref _notification, notificaiton);
            }
        }

        public Action FinishInteraction { get; set; }

        
    }
}
