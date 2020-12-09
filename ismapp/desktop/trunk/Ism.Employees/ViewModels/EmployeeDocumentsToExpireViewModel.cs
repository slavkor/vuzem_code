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
using Ism.Infrastructure.Extensions;

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
                RefreshDocumentsCommand = new DelegateCommand(OnRefreshDocumentsCommand);
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

        public DelegateCommand RefreshDocumentsCommand { get; set; }

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

        private DateTime _dateFrom;

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private DateTime _dateTo;

        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }



        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            DateFrom = DateTime.Now;
            DateTo = DateFrom.AddMonths(1);
            RefreshDocuments();
        }

        private void RefreshDocuments()
        {
            try
            {
                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDocumentToExpire>, Range>>())
                {

                    var range = new Range(DateFrom, DateTo);

                    rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"employees/documentstoexpire").ToString(), range, _securityService.GetCurrentToken(), (list) =>
                    {

                        try
                        {
                            if (null == list) return;
                            Documents = list;
                        }
                        catch (Exception e)
                        {
                            _exceptionService.RaiseException(e);
                        }
                    });

                    //rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"employees/documentstoexpire").ToString(),
                    //    _securityService.GetCurrentUser().AccessToken, (
                    //        e) =>
                    //    {
                    //        if (null == e) return;
                    //        Documents = e;
                    //    });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        private void OnRefreshDocumentsCommand()
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
    }
}

