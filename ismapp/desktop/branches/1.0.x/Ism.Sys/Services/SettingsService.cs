using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using Prism.Events;

namespace Ism.Sys.Services
{
    public class SettingsService : ISettingsService, IDisposable
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly IServiceLocator _serviceLocator;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private const string RegistryPath = @"SOFTWARE\Ism";
        private const string ApiKey = "ApiServer";
        private const string PrintKey = "PrintServer";
        private const string PrintKeyUser = "PrintServerUser";
        private const string PrintKeyPwd = "PrintServerPwd";
        private const string AuthKey = "AuthServer";
        private const string FirmIdKey = "FirmId";
        private RegistryKey _registryKey;
        private RegistryKey _userRegistryKey;
        private IDictionary<string, string> _companySettings;
        private IDictionary<string, string> _userSettings;
        private IList<Scope> _userScopes;

        #region Ctor

        public SettingsService(IEventAggregator eventAggregator, IServiceLocator serviceLocator, IExceptionService exceptionService)
        {
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));
            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _exceptionService = exceptionService;
            try
            {
                if (null == _registryKey)
                {
                    RegistryKey software = Registry.CurrentUser.OpenSubKey(@"SOFTWARE", true);
                    if (software != null)
                    {
                        software.CreateSubKey("Ism");
                        software.Close();
                    }
                }
                _registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath, true);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            try
            {
                _securityService = _serviceLocator.TryResolve<ISecurityService>();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        ~SettingsService()
        {
            Dispose(false);
        }

        #endregion

        #region ISettingsService

        public Uri GetApiServer()
        {
            try
            {
                return GetApiServer(false);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return null;
        }

        public Uri GetApiServer(bool global)
        {
          
            try
            {
                return GetApiServer( global? string.Empty: _securityService?.GetCurrentCompany()?.UuId);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return null;

        }

        public Uri GetApiServer(string company)
        {
            Uri serverUri = null;
            try
            {
                var url = _registryKey.GetValue(ApiKey, string.Empty, RegistryValueOptions.None).ToString();
                if (string.IsNullOrEmpty(url)) return null;

                //if (!url.StartsWith("http://")) url = "http://" + url;
                serverUri = new Uri(url);
                return string.IsNullOrEmpty(company) ? serverUri : new Uri(serverUri, $"f/{company}/");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return serverUri;
        }
        public Uri GetAuthServer()
        {
            Uri serverUri = null;
            try
            {
                var url = _registryKey.GetValue(AuthKey, string.Empty, RegistryValueOptions.None).ToString();
                if (string.IsNullOrEmpty(url)) return null;
                //if (!url.StartsWith("http://")) url = "http://" + url;

                serverUri = new Uri(url);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return serverUri;
        }

        public void SetApiServer(string apiServer)
        {
            try
            {
                _registryKey.SetValue(ApiKey, apiServer, RegistryValueKind.String);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public void SetAuthServer(string authServer)
        {
            try
            {
                _registryKey.SetValue(AuthKey, authServer, RegistryValueKind.String);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public string GetLastUsedFirm(string user)
        {
            var lastFirmId = string.Empty;
            try
            {
                var security = _serviceLocator.GetInstance<ISecurityService>();
                var cuser = security?.GetCurrentUser();
                if (cuser == null)
                    return string.Empty;

                if (null == _userRegistryKey)
                {
                    if (_registryKey != null) _userRegistryKey = _registryKey.CreateSubKey(cuser.UserName);
                }

                if (_userRegistryKey != null)
                    lastFirmId = _userRegistryKey.GetValue(FirmIdKey, string.Empty, RegistryValueOptions.None)
                        .ToString();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return lastFirmId;
        }

        public void SetLastUsedFirm(string user, string firmId)
        {
            try
            {
                _userRegistryKey?.SetValue(FirmIdKey, firmId);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public void SetPrintServer(string printServer, string user, string pwd)
        {
            try
            {
                _registryKey.SetValue(PrintKey, printServer, RegistryValueKind.String);
                _registryKey.SetValue(PrintKeyUser, user, RegistryValueKind.String);
                _registryKey.SetValue(PrintKeyPwd, pwd, RegistryValueKind.String);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public PrintServer GetPrintServer()
        {
            PrintServer printServer = null;
            try
            {
                var url = _registryKey.GetValue(PrintKey, string.Empty, RegistryValueOptions.None).ToString();

                if (string.IsNullOrEmpty(url)) return null;

                if (!url.StartsWith("http://"))
                    url = "http://" + url;
                var serverUri = new Uri(url);

                var u = _registryKey.GetValue(PrintKeyUser, string.Empty, RegistryValueOptions.None).ToString();
                var p = _registryKey.GetValue(PrintKeyPwd, string.Empty, RegistryValueOptions.None).ToString();

                printServer = new PrintServer() {ServerUri = serverUri, User = u, Password = p};
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return printServer;
        }

        public void SetCompanySettings(IDictionary<string, string> settings)
        {
            _companySettings = settings;
        }

        public IDictionary<string, string> GetCompanySettings()
        {
            return _companySettings;
        }

        public void SetUserSettings(IDictionary<string, string> settings)
        {
            _userSettings = settings;
        }

        public IDictionary<string, string> GetUserSettings()
        {
            return _userSettings;
        }

        public void SetUserScopes(IList<Scope> scopes)
        {
            _userScopes = scopes;
        }

        public IList<Scope> GetUserScopes()
        {
            return _userScopes;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            // free managed resources  
            if (_registryKey != null)
            {
                _registryKey.Dispose();
                _registryKey = null;
            }
            if (_userRegistryKey != null)
            {
                _userRegistryKey.Dispose();
                _userRegistryKey = null;
            }

            ////////// free native resources if there are any.  
            ////////if (nativeResource != IntPtr.Zero)
            ////////{
            ////////    Marshal.FreeHGlobal(nativeResource);
            ////////    nativeResource = IntPtr.Zero;
            ////////}
        }

        #endregion
    }
}
