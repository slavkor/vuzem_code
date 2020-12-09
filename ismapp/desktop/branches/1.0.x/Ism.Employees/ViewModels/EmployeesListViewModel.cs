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


namespace Ism.Employees.ViewModels
{
    public class EmployeesListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<EmployeeList> _employees;
        private ObservableCollection<EmployeeDepature> _employeesAway;
        private ObservableCollection<EmployeeDepature> _employeesHome;
        private EmployeeList _selectedEmployee;
        private IList<Document> _documents;
        private ListInteraction<Employee> _notification;
        private int _tabindex = 0;
        public EmployeesListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
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
                _eventAggregator.GetEvent<EmployeeAdded>().Subscribe(OnEmployeeAdded);
                _eventAggregator.GetEvent<EmployeeEdited>().Subscribe(OnEmployeeEdited);
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);

                DoubleClickCommand = new DelegateCommand<EmployeeList>(OnDoubleClickCommand);
                TabSelectionChangedCommand = new DelegateCommand<object>(OnTabSelectionChangedCommand);
                //_eventAggregator.GetEvent<ListEvent<Employee>>().Subscribe(OnListEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnTabSelectionChangedCommand(object obj)
        {
            try
            {
                var tabitem = obj as Telerik.Windows.Controls.RadTabItem;

                if (null == tabitem) return;

                int.TryParse(tabitem.Tag.ToString(), out _tabindex);

                RefreshEmployees();
                
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<object> TabSelectionChangedCommand { get; }
        private void OnListEvent(ListEventArgs<Employee> obj)
        {
            throw new NotImplementedException();
        }

        private void OnDoubleClickCommand(EmployeeList employee)
        {
            try
            {
                if(null == employee?.Employee) return;

                _eventAggregator.GetEvent<EditEvent<Employee>>().Publish(new EditEventArgs<Employee>() { EditMode = EditMode.Edit, EditObject = employee.Employee } );
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand<EmployeeList> DoubleClickCommand { get; }


        private void OnCompanySelectedEvent(Company obj)
        {
            try
            {
                RefreshEmployees();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public ObservableCollection<EmployeeList> Employees { get { return _employees; } set { SetProperty(ref _employees, value); } }
        public ObservableCollection<EmployeeDepature> EmployeesAway { get { return _employeesAway; } set { SetProperty(ref _employeesAway, value); } }
        public ObservableCollection<EmployeeDepature> EmployeesHome { get { return _employeesHome; } set { SetProperty(ref _employeesHome, value); } }

        public EmployeeList SelectedEmployee {
            get { return _selectedEmployee; }
            set {
                try
                {
                    SetProperty(ref _selectedEmployee, value);
                    _eventAggregator.GetEvent<SelectedEvent<Employee>>().Publish(new SelectedEventArgs<Employee>(_selectedEmployee?.Employee));
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                }
            }
        }

        public IList<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is ListInteraction<Employee>)) return;
                _notification = (ListInteraction<Employee>)value;
            }
        }
        public Action FinishInteraction { get; set; }
        #endregion

        


        private void OnEmployeeAdded(Employee employee)
        {
            try
            {
                _eventAggregator.GetEvent<SelectedEvent<Employee>>().Publish(null);
                RefreshEmployees();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEmployeeEdited(Employee employee)
        {
            try
            {
                _eventAggregator.GetEvent<SelectedEvent<Employee>>().Publish(null);
                RefreshEmployees();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshEmployees(bool global = false)
        {
            try
            {


                Employees = null;

                switch (_tabindex)
                {
                    case 0:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeList>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/list").ToString(), _securityService.GetCurrentUser().AccessToken,
                             (list) =>
                             {
                                 Employees = new ObservableCollection<EmployeeList>(list.OrderBy(emp => emp.Employee.LastName));
                             });
                        }
                        break;
                    case 1:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeList>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/list").ToString(), _securityService.GetCurrentUser().AccessToken,
                             (list) =>
                             {
                                 Employees = new ObservableCollection<EmployeeList>(list.Where(emp => emp.Employee.Loaner == null).OrderBy(emp => emp.Employee.LastName));
                             });
                        }
                        break;
                    case 2:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeList>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/list").ToString(), _securityService.GetCurrentUser().AccessToken,
                             (list) =>
                             {
                                 Employees = new ObservableCollection<EmployeeList>(list.Where(emp => emp.Employee.Loaner != null).OrderBy(emp => emp.Employee.LastName));
                             });
                        }
                        break;

                    case 3:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listaway").ToString(), _securityService.GetCurrentUser().AccessToken,
                             (list) =>
                             {
                                 EmployeesAway = new ObservableCollection<EmployeeDepature>(list.OrderBy(emp => emp.LastName));
                             });
                        }
                        break;
                    case 4:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listhome").ToString(), _securityService.GetCurrentUser().AccessToken,
                             (list) =>
                             {
                                 EmployeesHome= new ObservableCollection<EmployeeDepature>(list.OrderBy(emp => emp.LastName));
                             });
                        }
                        break;
                }


            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnFilter(object sender, FilterEventArgs e)
        {
            var emp = e.Item as EmployeeList;
            e.Accepted = emp.Employee.Name.Contains("a");
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedEmployee = null;
            base.OnNavigatedTo(navigationContext);
            RefreshEmployees();
        }

    }
}
