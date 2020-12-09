using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Repository;
using System.IO;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;

namespace Ism.Security.ViewModels
{
    public class ComanyChangeViewModel: ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;
        private Company _currentCompany;
        private string _logoImagePath;

        public ComanyChangeViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {

            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
            try
            {
                LogoImagePath = string.Empty;
                ChangeCompanyCommand = new DelegateCommand(OnChangeCompanyCommand, CanExecuteChangeCompanyCommand);
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteChangeCompanyCommand()
        {
            return !string.IsNullOrEmpty(LogoImagePath) && !_securityService.HasPermissionExcplicit("foreman");
        }

        public string LogoImagePath
        {
            get { return _logoImagePath; }
            set
            {
                SetProperty(ref _logoImagePath, value); 
                ChangeCompanyCommand?.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ChangeCompanyCommand { get; set; }

        public Company CurrentCompany
        {
            get { return _currentCompany; }
            set { SetProperty(ref _currentCompany, value); }
        }

        
        private void OnChangeCompanyCommand()
        {
            try
            {
                _eventAggregator.GetEvent<ListEvent<Company>>().Publish(new ListEventArgs<Company>() {SelectAction = OnCompanyListEventCallback, RememberSelection = true, ForceListSelection = true});
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
                if (null == securityService) return;
                securityService.SetCurrentCompany(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnCompanySelectedEvent(Company obj)
        {
            try
            {
                CurrentCompany = obj;
                DownloadCompanyLogo(CurrentCompany);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void DownloadCompanyLogo(Company obj)
        {
            try
            {
                if(null == obj)
                    return;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Document>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(true), $"company/{obj.UuId}/documents/LOGO").ToString(), _securityService.GetCurrentUser().AccessToken, documents =>
                    {
                        if (documents == null) return;
                        if (documents.Count == 0) return;

                        var logoDoc = documents.FirstOrDefault();

                        if (logoDoc == null) return;

                        using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                        {
                            var file = logoDoc?.Files.FirstOrDefault();
                            if (null == file) return;

                            string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_{file.UniqueName}_{file.Name}");
                            var url = new Uri(_settingsService.GetApiServer(false), $"documents/{logoDoc.UuId}/files/{file.UuId}");
                            repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentUser(), null, inputStream =>
                            {
                                using (inputStream)
                                {
                                    using (var outputStream = System.IO.File.OpenWrite(fileName))
                                    {
                                        inputStream.CopyTo(outputStream);
                                    }
                                }
                                LogoImagePath = fileName;
                            }, "Pridobivam logo datoteko...", false);
                        }
                    }
                    );
                }

                if (obj?.Logo?.Files == null) return;

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

    }
}
