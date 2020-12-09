using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;

namespace Ism.Reports.ViewModels
{
    public class ReportsOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _security;
        private readonly IExceptionService _exceptionService;
        private Report _currentReportMetaData;
        private string _lastNavigated = "";

        public ReportsOptionsViewModel(ISettingsService settings, ISecurityService security, IExceptionService exceptionService)
        {
            try
            {
                _settings = settings;
                _security = security;
                _exceptionService = exceptionService;
                ListCommand = new DelegateCommand(OnListCommand);
                EditCommand = new DelegateCommand<Report>(OnEditCommand, CanExecuteEditCommand);
                AddCommand = new DelegateCommand(OnAddCommand);
                BindCommand = new DelegateCommand(OnBindCommand);
                _eventAggregator.GetEvent<SelectedEvent<Report>>().Subscribe(OnReportSelectedEvent);


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
                OnListCommand();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                Current = null;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnReportMetaDataEditEvent(Report obj)
        {
            try
            {
                if (EditCommand.CanExecute(obj)) EditCommand.Execute(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand ListCommand { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand<Report> EditCommand { get; }

        public DelegateCommand BindCommand { get; }
        //public DelegateCommand<Report> ReportMetaDataDelete { get; }

        public Report Current
        {
            get { return _currentReportMetaData; }
            set
            {
                SetProperty(ref _currentReportMetaData, value);
                RaiseCanExecuteChanged();
            }
        }

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                Current = null;
                _lastNavigated = navigationResult.Context.Uri.OriginalString;
                var b = !navigationResult.Result;
                if (b != null && (bool)b)
                {
                    _exceptionService.RaiseException(navigationResult.Error);
                }

                RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnListCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Pregled prijavljenih izpisov" });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.ReportsRegion, "ReportsList", NavigaionCallback, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnAddCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Report>()
                {
                    Header = "Prijava novega izpisa",
                    EditInteraction = new EditInteraction<Report>()
                    {
                        Title = "Dodajanje novega zaposlenega",
                        InteractionObject = null,
                        EditMode = EditMode.New
                    }
                });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.ReportsRegion, "ReportEdit", NavigaionCallback, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteEditCommand(Report arg)
        {
            return Current != null;
        }
        private void OnEditCommand(Report Report)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Report>() { Header = "Urejanje prijave izpisa", EditInteraction = new EditInteraction<Report>() { Title = "Urejanje zaposlenega", InteractionObject = Report, EditMode = EditMode.Edit } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.ReportsRegion, "ReportEdit", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void OnReportSelectedEvent(SelectedEventArgs<Report> args)
        {
            try
            {
                Current = null;
                if (args?.SelectedData == null) return;
                Current = args.SelectedData;


            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnBindCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Report>()
                {
                    Header = "Povezovanje izpisov z uporabniki",
                });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.ReportsRegion, "ReportsUserBind", NavigaionCallback, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RaiseCanExecuteChanged()
        {
            try
            {
                ListCommand.RaiseCanExecuteChanged();
                EditCommand.RaiseCanExecuteChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

    }
}
