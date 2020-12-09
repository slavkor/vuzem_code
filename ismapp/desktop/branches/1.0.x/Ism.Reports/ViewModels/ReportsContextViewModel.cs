using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using System.Collections.ObjectModel;
using Ism.Infrastructure.Model;
using Prism.Commands;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Repository;

namespace Ism.Reports.ViewModels
{
    class ReportsContextViewModel : ViewModelBase
    {

        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<Report> _reports;
        private bool _reportsVisible;
        public ReportsContextViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
            try
            {
                ReportRequestCommand = new DelegateCommand<Report>(OnReportRequestCommand);
                ReportInteractionRequest = new InteractionRequest<ReportInteraction<Report>>();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<Report> ReportRequestCommand { get; }

        public InteractionRequest<ReportInteraction<Report>> ReportInteractionRequest { get; }

        public bool ReportsVisible
        {
            get
            {
                return _reportsVisible;
            }
            set
            {
                SetProperty(ref _reportsVisible, value);
            }
        }

        public ObservableCollection<Report> Reports
        {
            get { return _reports; }
            set
            {
                SetProperty(ref _reports, value);
            }
        }

        public string Context { get; set; }
        public Action<string, Action<string>> MetaDataProvider { get; set; }
        public Action<Report, Document> ReportDocumentAction { get; set; }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                Context = navigationContext.Parameters["context"] as string;
                MetaDataProvider = navigationContext.Parameters["metaprovider"] as Action<string, Action<string>>;
                ReportDocumentAction = navigationContext.Parameters["documentaction"] as Action<Report, Document>; 
                RefreshReportsForContext();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            navigationContext.NavigationService.Region.RemoveAll();
        }

        private void RefreshReportsForContext()
        {
            try
            {
                if (string.IsNullOrEmpty(Context)) return;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Report>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(false), $"reports/list/context/{Context}/{_securityService.GetCurrentUser().UserName}").ToString(),
                        _securityService.GetCurrentUser().AccessToken,
                        (list) =>
                        {
                            try
                            {
                                list.ForEach((rep) =>
                                {
                                    rep.Command = ReportRequestCommand;
                                    rep.MetaDataProvider = MetaDataProvider;
                                    rep.ReportDocumentAction = ReportDocumentAction;
                                });

                                Reports = new ObservableCollection<Report>(list.OrderBy(item => item.ReportId));

                                ReportsVisible = Reports.Count > 0;
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnReportRequestCommand(Report obj)
        {

            try
            {
                ReportInteractionRequest.Raise(new ReportInteraction<Report>() { Title = "Izpis", ReportEventArgs = null, InteractionObject = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }


        }


    }
}
