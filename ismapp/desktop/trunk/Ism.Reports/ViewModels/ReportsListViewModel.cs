using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;
using System.ComponentModel;
using System.Windows.Data;


namespace Ism.Reports.ViewModels
{
    public class ReportsListViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<Report> _reports;
        private Report _selected;

        public ReportsListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
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

                DoubleClickCommand = new DelegateCommand<Report>(OnDoubleClickCommand);

                //_eventAggregator.GetEvent<ListEvent<Employee>>().Subscribe(OnListEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDoubleClickCommand(Report report)
        {
            try
            {
                if (null == report) return;

                _eventAggregator.GetEvent<EditEvent<Report>>().Publish(new EditEventArgs<Report>() { EditMode = EditMode.Edit, EditObject = report });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand<Report> DoubleClickCommand { get; }


        public ObservableCollection<Report> Reports { get { return _reports; } set { SetProperty(ref _reports, value); } }

        public Report SelectedReport
        {
            get { return _selected; }
            set
            {
                try
                {
                    SetProperty(ref _selected, value);
                    _eventAggregator.GetEvent<SelectedEvent<Report>>().Publish(new SelectedEventArgs<Report>(_selected));
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                }
            }
        }



        private void RefreshReports(bool global = false)
        {
            try
            {
                Reports = null;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Report>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "reports/list/all").ToString(),
                        _securityService.GetCurrentToken(),
                        (list) =>
                        {
                            Reports = new ObservableCollection<Report>(list.OrderBy(item => item.ReportId));
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedReport  = null;
            base.OnNavigatedTo(navigationContext);
            RefreshReports();
        }

    }
}
