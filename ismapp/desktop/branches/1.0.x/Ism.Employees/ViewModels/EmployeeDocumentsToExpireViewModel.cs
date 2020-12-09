using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;

namespace Ism.Employees.ViewModels
{
    public class EmployeeDocumentsToExpireViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private IList<EmployeeDocumentToExpire> _documents;

        public EmployeeDocumentsToExpireViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;

            try
            {
                //UploadDocumentCommand = new DelegateCommand(OnUploadDocumentCommand);
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCompanySelectedEvent(Company obj)
        {
            try
            {
                RefreshDocuments();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public DelegateCommand UploadDocumentCommand { get; }

        #region IRegionMemberLifetime
        public bool KeepAlive => true;

        #endregion

        public IList<EmployeeDocumentToExpire> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            RefreshDocuments();
        }


        private void RefreshDocuments()
        {
            try
            {
                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDocumentToExpire>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"employees/documentstoexpire").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (
                            e) =>
                        {
                            if (null == e) return;
                            Documents = e;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}

