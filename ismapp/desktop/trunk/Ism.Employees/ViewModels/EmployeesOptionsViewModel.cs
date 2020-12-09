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
using System.Xml.Linq;

namespace Ism.Employees.ViewModels
{
    public class EmployeesOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _security;
        private readonly IExceptionService _exceptionService;
        private Employee _currentEmployee;
        private string _lastNavigated = "";

        public EmployeesOptionsViewModel(ISettingsService settings, ISecurityService security, IExceptionService exceptionService)
        {
            try
            {
           

                EmployeesList = new DelegateCommand(OnEmployeesList);
                EmployeeAdd = new DelegateCommand(OnEmployeeAdd);
                EmployeeEdit = new DelegateCommand<Employee>(OnEmployeeEdit,  (e) => CurrentEmployee != null  /*&& CurrentEmployee.WorkPeriod.Active == 1*/ );
                DocumentsToExpire = new DelegateCommand(OnDocumentsToExpire);
                _eventAggregator.GetEvent<SelectedEvent<Employee>>().Subscribe(OnEmployeeSelected);
                _settings = settings;
                _security = security;
                _exceptionService = exceptionService;
                _eventAggregator.GetEvent<EditEvent<Employee>>().Subscribe(OnEmployeeEditEvent);
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
                OnEmployeesList();
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
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEmployeeEditEvent(EditEventArgs<Employee> args)
        {
            try
            {
                if(EmployeeEdit.CanExecute(args.EditObject)) EmployeeEdit.Execute(args.EditObject);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand EmployeesList { get; }
        public DelegateCommand DocumentsToExpire { get; }
        public DelegateCommand EmployeeAdd { get; }
        public DelegateCommand<Employee> EmployeeEdit { get; }

        //public DelegateCommand<Employee> EmployeeDelete { get; }

        public Employee CurrentEmployee
        {
            get { return _currentEmployee; }
            set
            {
                SetProperty(ref _currentEmployee, value);
                RaiseCanExecuteChanged();
            }
        }
        

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                CurrentEmployee = null;
                _lastNavigated = navigationResult.Context.Uri.OriginalString;
                var b = !navigationResult.Result;
                if (b != null && (bool) b)
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

        private void OnEmployeesList()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Pregled zaposlenih" });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesRegion, "EmployeesList", NavigaionCallback, parameters);


                parameters = new NavigationParameters();
                parameters.Add("context", "Employees.List");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                _regionManager.RequestNavigate(RegionNames.ReportsRegion, "ReportsContext", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ReportMetaDataProvider(string meta, Action<string> callback)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(meta);

                if (CurrentEmployee != null)
                {

                    var ctx = xdoc.Element("root").Element("contextparams");
                    if (null == ctx) xdoc.Element("root").Add(new XElement("contextparams", new XElement("employeeId", CurrentEmployee.UuId)));

                    var element = xdoc.Element("root").Element("contextparams").Element("employeeId");
                    if (null == element) xdoc.Element("root").Element("contextparams").Add(new XElement("employeeId", CurrentEmployee.UuId));
                    else element.Value = CurrentEmployee.UuId;
                }

                callback?.Invoke(xdoc.ToString());

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        private void OnEmployeeAdd()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Employee>() { Header = "Dodajanje zaposlenega", EditInteraction = new EditInteraction<Employee>() { Title = "Dodajanje novega zaposlenega", InteractionObject = null, EditMode = EditMode.New } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesRegion, "EmployeeEdit", NavigaionCallback, parameters);

                _regionManager.Regions[RegionNames.ReportsRegion].RemoveAll();

                //parameters = new NavigationParameters();
                //parameters.Add("module", "Employee.Add");
                //_regionManager.RequestNavigate(RegionNames.ReportsRegion, "ReportsContext", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEmployeeEdit(Employee employee)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Employee>() { Header = "Urejanje zaposlenega", EditInteraction = new EditInteraction<Employee>() { Title = "Urejanje zaposlenega", TitleExtendet = $"{employee.LastName} {employee.Name}", InteractionObject = employee, EditMode = EditMode.Edit } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesRegion, "EmployeeEdit", NavigaionCallback, parameters);

                _regionManager.Regions[RegionNames.ReportsRegion].RemoveAll();

                //    parameters = new NavigationParameters();
                //    parameters.Add("module", "Employee.Edit");
                //    _regionManager.RequestNavigate(RegionNames.ReportsRegion, "ReportsContext", NavigaionCallback, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        void EmployeeEditRequestCallback(EditInteraction<Employee> request)
        {
            try
            {
                if(request.EditMode == EditMode.Edit)
                    _eventAggregator.GetEvent<EmployeeEdited>().Publish(request.InteractionObject);
                else
                    _eventAggregator.GetEvent<EmployeeAdded>().Publish(request.InteractionObject);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnEmployeeSelected(SelectedEventArgs<Employee> args)
        {
            try
            {
                CurrentEmployee = null;
                if(args?.SelectedData == null) return;
                CurrentEmployee = args.SelectedData;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        //private void OnEmployeeDelete(Employee obj)
        //{
        //    try
        //    {
        //        //_eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmEmployeeDelete, Title = "ALO", Content = "Želiš izbrisati zaposlenega?", PayLoad = obj });
        //        _eventAggregator.GetEvent<ReportEvent>().Publish(new ReportEventArgs() { Report = new Report() { ReportPath = "reports/ISM/employee_list_001.pdf" } });
        //    }
        //    catch (Exception e)
        //    {
        //        _exceptionService.RaiseException(e);
        //    }
        //}

        private void OnConfirmEmployeeDelete(bool confirmed, BaseModel payLoad)
        {
            try
            {
                if(!confirmed)
                    return;

                Employee employee = payLoad as Employee;
                if(null == employee) return;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Employee, Employee>>())
                {
                    var url = new Uri(_settings.GetApiServer(), "employees/delete");

                    repositroy.PostRequestAsync(url.ToString(), employee,
                        _security.GetCurrentToken(),
                        (e) =>
                        {
                            OnEmployeesList();
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnDocumentsToExpire()
        {
            try
            {
                CurrentEmployee = null;
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesRegion, "EmployeeDocumentsToExpire", NavigaionCallback);

                var parameters = new NavigationParameters();
                parameters.Add("context", "Employees.Documents.Expire");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                _regionManager.RequestNavigate(RegionNames.ReportsRegion, "ReportsContext", NavigaionCallback, parameters);
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
                EmployeesList.RaiseCanExecuteChanged();
                EmployeeEdit.RaiseCanExecuteChanged(); 
                //EmployeeDelete.RaiseCanExecuteChanged(); 
                DocumentsToExpire.RaiseCanExecuteChanged(); 
                EmployeeAdd.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

    }
}
