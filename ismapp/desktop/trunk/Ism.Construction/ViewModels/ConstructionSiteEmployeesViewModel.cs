using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;
using System.ComponentModel;
using System.Windows.Data;
using Telerik.Windows.Controls.GanttView;
using System.Collections;
using Telerik.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Data;

namespace Ism.Construction.ViewModels
{
    public class ConstructionSiteEmployeesViewModel : Infrastructure.Mvvm.ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly IEmployeeService _employeeService;
        

        public ConstructionSiteEmployeesViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService, IEmployeeService employeeService)
        {
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));


            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            _employeeService = employeeService;

            try
            {
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);
                SearchGridsCommand = new DelegateCommand<GridSearch>(OnSearchGridsCommand);
                ShowAllCommand = new DelegateCommand<GridSearch>(OnShowAllCommand);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<GridSearch> SearchGridsCommand { get; }

        public DelegateCommand<GridSearch> ShowAllCommand { get; }

        private void OnShowAllCommand(GridSearch obj)
        {
            try
            {
                try
                {
                    bool expand = false;
                    bool.TryParse(obj.SearchString, out expand);

                    foreach (var item in obj.Grids)
                    {
                        if (null == item) continue;
                        if (!expand)
                            item?.CollapseAllGroups();
                        else
                            item?.ExpandAllGroups();
                    }
                }
                catch (Exception exception)
                {
                    _exceptionService.RaiseException(exception);
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnSearchGridsCommand(GridSearch obj)
        {
            try
            {
                foreach (var item in obj.Grids)
                {
                    if (null == item) continue;
                    var searchBytextCommand = RadGridViewCommands.SearchByText as RoutedUICommand;
                    searchBytextCommand.Execute(obj.SearchString, item);

                    if (string.IsNullOrEmpty(obj.SearchString))
                        item?.CollapseAllGroups();
                    else
                        item?.ExpandAllGroups();
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnCompanySelectedEvent(Company obj)
        {
            try
            {
                RefreshConstructionSites();
                RefreshHome();
                RefreshPlaned();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private CollectionViewSource _sites;

        public CollectionViewSource SiteEmployees
        {
            get { return _sites; }
            set { SetProperty(ref _sites, value); }
        }

        private ObservableCollection<EmployeeDepature> _home;

        public ObservableCollection<EmployeeDepature> HomeEmployees
        {
            get { return _home; }
            set { SetProperty(ref _home, value); }
        }

        private CollectionViewSource _plnaed;

        public CollectionViewSource PlanedEmployees
        {
            get { return _plnaed; }
            set { SetProperty(ref _plnaed, value); }
        }
        
        private void RefreshConstructionSites()
        {
            try
            {
                SiteEmployees = null;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<ConstructionSiteOverview>, object>>())
                {

                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(true), "csite/listoverview").ToString(), _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            SiteEmployees = new CollectionViewSource() { Source = e };
                            SiteEmployees.GroupDescriptions.Add(new PropertyGroupDescription("SiteName"));

                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshHome()
        {
            try
            {
                HomeEmployees = null;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, object>>())
                {

                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(), "employees/listhome2").ToString(), _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            
                            HomeEmployees = new ObservableCollection<EmployeeDepature>(e);
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshPlaned()
        {
            try
            {
                PlanedEmployees = null;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<ConstructionSiteOverview>, object>>())
                {

                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(), "departures/listpladed").ToString(), _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            PlanedEmployees = new CollectionViewSource() { Source = e };
                            PlanedEmployees.GroupDescriptions.Add(new PropertyGroupDescription("SiteName"));
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
            
            base.OnNavigatedTo(navigationContext);
            RefreshConstructionSites();
            RefreshHome();
            RefreshPlaned();
        }
    }

}
