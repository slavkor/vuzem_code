using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using System.Collections.ObjectModel;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure;
using Ism.Infrastructure.Repository;

namespace Ism.Departure.ViewModels
{
    class DepartureEditViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private EditInteraction<Infrastructure.Model.Departure> _notification;
        private Infrastructure.Model.Departure _departure;
        private ObservableCollection<EmployeeListItem> _employees;
        private ObservableCollection<CarListItem> _cars;
        private Employee _selectedEmployee;

        List<Car> _carList;
        List<Car> _addCarList;
        List<Car> _deleteCarList;
        List<Employee> _employeeList;
        List<Employee> _addEmployeeList;
        List<Employee> _deleteEmployeeList;

        public DepartureEditViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settingsService = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;

            try
            {
                OriginDestinationSelectRequest = new InteractionRequest<ListInteraction<IDepartureArrival>>();
                EmployeeSelectRequest = new InteractionRequest<ListInteraction<EmployeeDepature>>();
                SelectStartCommand = new DelegateCommand(OnSelectStartCommand);
                SelectDestinationCommand = new DelegateCommand(OnSelectDestinationCommand, CanExecuteSelectDestinationCommand);
                EmployeeSelectCommand = new DelegateCommand(OnEmployeeSelectCommand, CanExecuteSaveCommand);
                EmployeeRemoveCommand = new DelegateCommand<EmployeeListItem>(OnEmployeeRemoveCommand, (e) => { return Departure?.Origin != null && Departure?.Destination != null; });
                CarRemoveCommand = new DelegateCommand<CarListItem>(OnCarRemoveCommand, (e) => { return Departure?.Origin != null && Departure?.Destination != null; });
                EmployeeStateChangeCommand = new DelegateCommand<EmployeeListItem>(OnEmployeeStateChangeCommand);

                CarSelectCommand = new DelegateCommand(OnCarSelectCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);


            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }



        private bool CanExecuteSelectDestinationCommand()
        {
            return !_securityService.HasPermissionExcplicit("foreman");
        }

        #region public properties

        public ObservableCollection<EmployeeListItem> Employees
        {
            get { return _employees; }
            set
            {
                SetProperty(ref _employees, value);
            }
        }
        public ObservableCollection<CarListItem> Cars
        {
            get { return _cars; }
            set
            {
                SetProperty(ref _cars, value);
            }
        }

        public Employee SelectedEmployee
        {

            get
            {
                return _selectedEmployee;
            }
            set
            {
                SetProperty(ref _selectedEmployee, value);
            }
        }

        public Infrastructure.Model.Departure Departure
        {
            get
            {
                return _departure;
            }
            set
            {
                SetProperty(ref _departure, value);
            }
        }

        private string _start;
        private string _destination;
        public string Start { get { return _start; } set { SetProperty(ref _start, value); } }
        public string Destination { get { return _destination; } set { SetProperty(ref _destination, value); } }


        public InteractionRequest<ListInteraction<IDepartureArrival>> OriginDestinationSelectRequest { get; }
        public InteractionRequest<ListInteraction<EmployeeDepature>> EmployeeSelectRequest { get; }
        
        public EditMode EditMode { get; set; }

        #endregion

        #region commands
        public DelegateCommand SelectStartCommand { get; }
        public DelegateCommand SelectDestinationCommand { get; }

        public DelegateCommand EmployeeSelectCommand { get; }

        public DelegateCommand<EmployeeListItem> EmployeeRemoveCommand { get; }
        public DelegateCommand<EmployeeListItem> EmployeeStateChangeCommand { get; }

        public DelegateCommand<CarListItem> CarRemoveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CarSelectCommand { get; }

        public System.Globalization.CultureInfo Culture { get { return new System.Globalization.CultureInfo("sl-SI"); } }

        #endregion

        #region ViewModelBase overrides
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);


            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        public override bool KeepAlive => false;


        #endregion

        #region IInteractionRequestAware
        public INotification Notification
        {
            get
            {
                return _notification;
            }
            set
            {

                var n = value as EditInteraction<Infrastructure.Model.Departure>;
                if (n == null) return;
                SetProperty(ref _notification, n);

                EditMode = _notification.EditMode;
                Departure = _notification.EditMode == EditMode.New ? new Infrastructure.Model.Departure() {DepartTime = DateTime.Now, Origin = _securityService.GetCurrentCompany() } : _notification.InteractionObject;

                if (_securityService.HasPermissionExcplicit("foreman"))
                {
                    if(EditMode == EditMode.New) Departure.Origin = null;
                    Departure.Destination = _securityService.GetCurrentCompany();
                    Destination = Departure.Destination.PointName;
                }

                Departure.PropertyDeletegate = (model) => { RaiseCanExecuteChanged(); };
                _employeeList = new List<Employee>();
                _addEmployeeList = new List<Employee>();
                _deleteEmployeeList = new List<Employee>();

                _carList = new List<Car>();
                _addCarList = new List<Car>();
                _deleteCarList = new List<Car>();

                switch (EditMode)
                {
                    case EditMode.New:
                        Employees = new ObservableCollection<EmployeeListItem>();
                        Employees.Add(new EmployeeListItem() { IsAddItem = true, AddEmployee = EmployeeSelectCommand });
                        Cars = new ObservableCollection<CarListItem>();
                        Cars.Add(new CarListItem() { IsAddItem = true, AddCar = CarSelectCommand});

                        Start = "";
                        Destination = Departure?.Destination?.PointName;
                        break;
                    case EditMode.Edit:

                        RefreshDepartureEmployees();
                        RefreshDepartureCars();
                        
                        Start = Departure.Origin.PointName;
                        Destination = Departure.Destination.PointName;
                        break;
                }



            }
        }



        public Action FinishInteraction{ get; set; }

        #endregion

        #region private methods


        #region commnads

        
        private void OnSelectStartCommand()
        {
            try
            {
                var interaction = new ListInteraction<IDepartureArrival>() { Title = "Izbira starta", SelectAction = OnStartSelect, InteractionObject = Departure.Destination};
                OriginDestinationSelectRequest.Raise(interaction);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnStartSelect(IDepartureArrival obj)
        {
            try
            {
                Departure.Origin = obj;
                Start = Departure.Origin.PointName;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                throw;
            }
        }

        private void OnSelectDestinationCommand()
        {
            try
            {
                var interaction = new ListInteraction<IDepartureArrival>() { Title = "Izbira cilja", SelectAction = OnDestinationSelect, InteractionObject = Departure.Origin };
                OriginDestinationSelectRequest.Raise(interaction);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDestinationSelect(IDepartureArrival obj)
        {
            try
            {
                Departure.Destination = obj;
                Destination = Departure.Destination.PointName;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                throw;
            }
        }

        private void OnEmployeeSelectCommand()
        {
            try
            {

                switch (EditMode)
                {
                    case EditMode.New:
                        if (SaveCommand.CanExecute())
                            SaveDeparture(Departure, false, RaiseSelectEmployeesInteraction);
                        //OnConfirmSaveDepartureCallback(true, new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = null, AfterSaveAction = RaiseSelectEmployeesInteraction, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = false, PayLoad = Departure });
                        break;
                    case EditMode.Edit:
                    default:
                        var interaction = new ListInteraction<EmployeeDepature>() { Title = "Izbira zaposlenih", SelectManyAction = OnSelectEmployees2 };
                        interaction.ListEventArgs = new ListEventArgs<EmployeeDepature>() { DataProvider = RefreshEmployees };
                        EmployeeSelectRequest.Raise(interaction);

                        break;
                }





            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RaiseSelectEmployeesInteraction(bool saved, ConfirmSaveEventArgs<BaseModel> args )
        {
            try
            {
                var interaction = new ListInteraction<EmployeeDepature>() { Title = "Izbira zaposlenih", SelectManyAction = OnSelectEmployees2 };
                interaction.ListEventArgs = new ListEventArgs<EmployeeDepature>() { DataProvider = RefreshEmployees };
                EmployeeSelectRequest.Raise(interaction);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }


        private void OnSelectEmployees2(List<EmployeeDepature> list)
        {
            try
            {

                var emps = list.Select(e => e as Employee).ToList();

                var last = emps?.LastOrDefault();

                using (var repository = _serviceLocator.GetInstance<IRestRepository<Employee, Employee>>())
                {
                    foreach (var item in emps)
                    {
                        if (Employees.Any(e => e?.Employee?.UuId == item?.UuId)) continue;
                        repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"departures/{Departure.UuId}/employee/add").ToString(), item as Employee, _securityService.GetCurrentUser().AccessToken, (e) => {

                            try
                            {
                                if (item.Equals(last))
                                {
                                    RefreshDepartureEmployees();
                                }

                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                            
                        });
                    }
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshDepartureEmployees()
        {
            try
            {

                using (var repository = _serviceLocator.GetInstance<IRestRepository<IList<EmployeeEx>, object>>())
                {

                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"departures/{Departure.UuId}/employees").ToString(), _securityService.GetCurrentUser().AccessToken, (list) =>
                    {
                        try
                        {
                            Employees = new ObservableCollection<EmployeeListItem>(list.Select(e => new EmployeeListItem() { Employee = e, RemoveEmployee = EmployeeRemoveCommand, StateChangeCommand = EmployeeStateChangeCommand }));
                            Employees.Add(new EmployeeListItem() { IsAddItem = true, AddEmployee = EmployeeSelectCommand });
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

        private void RefreshDepartureCars()
        {
            try
            {
                using (var repository = _serviceLocator.GetInstance<IRestRepository<IList<Car>, object>>())
                {

                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"departures/{Departure.UuId}/cars").ToString(), _securityService.GetCurrentUser().AccessToken, (list) =>
                    {
                        try
                        {
                            Cars = new ObservableCollection<CarListItem>(list.Select(c => new CarListItem() { Car = c, RemoveCar = CarRemoveCommand }));
                            Cars.Add(new CarListItem() { IsAddItem = true, AddCar = CarSelectCommand });
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
        private void OnCarSelectCommand()
        {
            try
            {
                switch (EditMode)
                {
                    case EditMode.New:
                        if (SaveCommand.CanExecute())
                            SaveDeparture(Departure, false, RaiseSelectCarInteraction);
                        break;
                    case EditMode.Edit:
                    default:
                        RaiseSelectCarInteraction(true, null);
                        break;
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RaiseSelectCarInteraction(bool saved, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                _eventAggregator.GetEvent<ListEvent<CarList>>().Publish(new ListEventArgs<CarList>() { SelectManyAction = OnSelectCars, DataProvider = RefreshCars });
                
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshCars(Action<List<CarList>> callback)
        {
            try
            {
                if (Departure?.Origin == null) return;
                switch (Departure.Origin.DepartureArrivalType)
                {
                    case "COMPANY":
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<CarList>, string>>())
                        {
                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(), "cars/list").ToString(),
                                _securityService.GetCurrentUser().AccessToken,
                                (list) =>
                                {
                                    callback.Invoke(list);
                                });
                        }
                        break;
                    case "PROJECT":
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<CarList>, string>>())
                        {

                            var p = Departure.Origin as Project;


                            //repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(false), $"project/{Departure.Origin.UuId}/car/list").ToString(),
                            //    _securityService.GetCurrentUser().AccessToken,
                            //    (list) =>
                            //    {
                            //        callback.Invoke(list);
                            //    });
                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(false), $"csite/{p.Site.UuId}/car/list").ToString(),
                                _securityService.GetCurrentUser().AccessToken,
                                (list) =>
                                {
                                    callback.Invoke(list);
                                });
                        }
                        break;
                }
                //callback.Invoke(null);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }


        private void RefreshEmployees(Action<List<EmployeeDepature>> callback)
        {
            try
            {
                if (Departure?.Origin == null) return;
                switch (Departure.Origin.DepartureArrivalType)
                {
                    case "COMPANY":
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                        {
                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(true), "employees/listactive").ToString(),
                                _securityService.GetCurrentUser().AccessToken,
                                (list) =>
                                {
                                    callback?.Invoke(list);
                                });
                        }
                        break;
                    case "PROJECT":
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                        {
                            var p = Departure.Origin as Project;

                            //repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(false), $"project/{Departure.Origin.UuId}/employee/list").ToString(),
                            //    _securityService.GetCurrentUser().AccessToken,
                            //    (list) =>
                            //    {
                            //        callback?.Invoke(list);
                            //    });
                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(false), $"csite/{p.Site.UuId}/employee/list").ToString(),
                                _securityService.GetCurrentUser().AccessToken,
                                (list) =>
                                {
                                    callback?.Invoke(list);
                                });
                        }
                        break;
                }

                //callback.Invoke(null);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        //private void OnSelectEmployees(List<EmployeeList> list)
        //{
        //    try
        //    {
        //        var emps = list.Select(e => e.Employee).ToList();

        //        var comparer = new PropertyComparer<Employee>("UuId");
        //        _addEmployeeList.AddRange(emps.Except(_employeeList, comparer).ToList());

        //        _employeeList.AddRange(emps);
        //        var distict = _employeeList.Distinct(comparer);
        //        //Employees = new ObservableCollection<Employee>(distict);
        //        RaiseCanExecuteChanged();
        //    }
        //    catch (Exception exc)
        //    {
        //        _exceptionService.RaiseException(exc);
        //    }
        //}

        private void OnSelectCars(List<CarList> list)
        {
            try
            {
    
                var cars = list.Select(e => e.Car).ToList();

                var last = cars?.LastOrDefault();

                using (var repository = _serviceLocator.GetInstance<IRestRepository<Car, Car>>())
                {
                    foreach (var item in cars)
                    {
                        if (Cars.Any(e => e?.Car?.UuId == item?.UuId)) continue;
                        repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"departures/{Departure.UuId}/car/add").ToString(), item as Car, _securityService.GetCurrentUser().AccessToken, (c) => {

                            try
                            {
                                if (item.Equals(last))
                                {
                                    RefreshDepartureCars();
                                }
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                    }
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteSaveCommand()
        {
            return Departure?.Origin != null && Departure?.Destination != null;
        }
        private void OnSaveCommand()
        {
            try
            {
                SaveDeparture(Departure);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction(true);
                Clear();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void OnEmployeeRemoveCommand(EmployeeListItem  employee)
        {
            try
            {
                if (null == employee) return;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<Employee, Employee>>())
                {
                        repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"departures/{Departure.UuId}/employee/remove").ToString(), employee.Employee as Employee, _securityService.GetCurrentUser().AccessToken, (e) => {
                            try
                            {
                                RefreshDepartureEmployees();
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
        #endregion
        private void OnCarRemoveCommand(CarListItem obj)
        {
            if (null == obj) return;

            using (var repository = _serviceLocator.GetInstance<IRestRepository<Car, Car>>())
            {
                repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"departures/{Departure.UuId}/car/remove").ToString(), obj.Car as Car, _securityService.GetCurrentUser().AccessToken, (e) => {
                    try
                    {
                        RefreshDepartureCars();
                    }
                    catch (Exception exc)
                    {
                        _exceptionService.RaiseException(exc);
                    }
                });
            }
        }

        private void OnFinishInteraction(bool confirmed = false)
        {
            try
            {
                _notification.Confirmed = confirmed;
                FinishInteraction?.Invoke();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void SaveAction(Infrastructure.Model.Departure obj, EditMode editMode)
        {
            try
            {
                SaveDeparture(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void SaveDeparture(Infrastructure.Model.Departure obj, bool finish = true, Action<bool, ConfirmSaveEventArgs<BaseModel>> afterSaveAction = null)
        {
            try
            {
                if (!Departure.IsDirty || !SaveCommand.CanExecute())
                {
                    if (finish) OnFinishInteraction(true);
                    return;
                }
                Departure.EmployeesAdd = _addEmployeeList;
                Departure.EmployeesRemove = _deleteEmployeeList;
                Departure.CarsRemove = _deleteCarList;
                Departure.CarsAdd = _addCarList;

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveDepartureCallback, AfterSaveAction = afterSaveAction, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish, PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmSaveDepartureCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {

                if (!confirmed || !Departure.IsDirty)
                {
                    if (args.FinishUp) OnFinishInteraction(true);
                    return;
                }

                Infrastructure.Model.Departure departure = args.PayLoad as Infrastructure.Model.Departure;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Departure, Infrastructure.Model.Departure>>())
                {
                    var url = EditMode == EditMode.New
                        ? new Uri(_settingsService.GetApiServer(), "departures/add")
                        : new Uri(_settingsService.GetApiServer(), "departures/update");

                    repositroy.PostRequestAsync(url.ToString(), departure,
                        _securityService.GetCurrentUser().AccessToken,
                        (d) =>
                        {
                            try
                            {
                                if (args.FinishUp) OnFinishInteraction(true);

                                Departure.IsDirty = false;
                                EditMode = EditMode.Edit;
                                RaiseCanExecuteChanged();
                                args?.AfterSaveAction?.Invoke(true, args);
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                OnFinishInteraction();
            }
        }
        
        private void RaiseCanExecuteChanged()
        {
            try
            {
                SaveCommand.RaiseCanExecuteChanged();
                EmployeeSelectCommand.RaiseCanExecuteChanged();
                EmployeeRemoveCommand.RaiseCanExecuteChanged();
                CarSelectCommand.RaiseCanExecuteChanged();
                CarRemoveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void Clear()
        {
            try
            {
                Departure = null;
                Employees = null;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void OnEmployeeStateChangeCommand(EmployeeListItem obj)
        {
            try
            {
                int a = 0;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        #endregion
    }

}

