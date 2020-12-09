using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Repository;
using System.Collections.ObjectModel;
using Omu.ValueInjecter;
using System.Windows;
using Ism.Infrastructure.Ui;
using Prism.Commands;
using System.Threading;
using Ism.Construction.Events;

namespace Ism.Construction.ViewModels
{
    class WorkingHoursViewModel : ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IDepartureService _departures;

        private DateTime _selectedDate;
        private ForemanConstructionSite _siteData;
        private ObservableCollection<EmployeeShift> _employeeShifts;
        private ObservableCollection<ShiftDay> _shiftDays;
        private EmployeeShift _selectedEmployeeShift;
        private ObservableCollection<EmployeeShift> _selectedItems;

        public WorkingHoursViewModel(IExceptionService exceptionService, ISettingsService settingsService, ISecurityService securityService, IDepartureService departures)
        {
            _exceptionService = exceptionService;
            _settingsService = settingsService;
            _securityService = securityService;
            _departures = departures;
            ChangeProjectCommand = new DelegateCommand(OnChangeProjectCommand);
            HoursToAll = new DelegateCommand<IList<int>>(OnHoursToAll);
        }



        private void OnHoursToAll(IList<int> array)
        {
            try
            {
                int type = array[0];
                int hours = array[1];

                var list = EmployeeShifts.Select(e => e.Employee);

                if (EmployeeShifts.Any(e => e.IsSelected)) list = EmployeeShifts.Where(e => e.IsSelected).Select(e => e.Employee);

                var last = list.Last();


                foreach (var employee in list)
                {
                    var shift = new Shift() { UuId = Guid.NewGuid().ToString(), ShiftType = type, WorkDay = new Day(SelectedDate), Hours = hours };
   
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<object, Shift>>())
                    {
                        rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/{Project.UuId}/employee/{employee.UuId}/shiftadd").ToString(), shift, _securityService?.GetCurrentToken(), (l) => {

                            if (!ShiftDatesCollection.Contains(shift.WorkDay.Date))
                                ShiftDatesCollection.Add(shift.WorkDay.Date);

                            if (employee == last)
                                RefreshEmployees(SelectedDate);

                        }, "", false);
                    }
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<IList<int>> HoursToAll { get; }
        public ForemanConstructionSite SiteData
        {
            get
            {
                return _siteData;
            }
            set
            {
                SetProperty(ref _siteData, value);
                Project = SiteData.Projects.Where(p => p.IsSelected).FirstOrDefault();
            }
        }

        public Project Project { get; set;}

        public DelegateCommand ChangeProjectCommand { get; }

        public ObservableCollection<DateTime> ShiftDatesCollection { get; set; }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                SetProperty(ref _selectedDate, value);
                RefreshEmployees(_selectedDate);
                _eventAggregator.GetEvent<DateSelectedEvent>().Publish(SelectedDate);
            }
        }

        public ObservableCollection<EmployeeShift> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                {
                    _selectedItems = new ObservableCollection<EmployeeShift>();
                }
                return _selectedItems;
            }
            set
            {
                ;// SetProperty(ref _selectedItems, value);

            }


        }

        public ObservableCollection<EmployeeShift> EmployeeShifts
        {
            get { return _employeeShifts; }
            set
            {
                SetProperty(ref _employeeShifts, value);
            }
        }

        public ObservableCollection<ShiftDay> ShiftDays
        {
            get
            {
                return _shiftDays;
            }
            set
            {
                SetProperty(ref _shiftDays, value);
            }
        }
        public EmployeeShift SelectedEmployeeShift
        {
            get { return _selectedEmployeeShift; }
            set
            {
                SetProperty(ref _selectedEmployeeShift, value);
            }
        }



        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                SiteData = navigationContext.Parameters["sitedata"] as ForemanConstructionSite;

                RefreshShifts();
                SelectedDate = DateTime.Now;
                //SelectedItems = new ObservableCollection<EmployeeShift>();
                //SelectedItems.Clear();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion


        private void RefreshEmployees(DateTime day)
        {
            try
            {
                if (Project == null) return;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<EmployeeShift>, Day>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/{Project.UuId}/workdayshifts").ToString(), new Day(day), _securityService.GetCurrentToken(), (list) =>
                    {
                        try
                        {
                            list.ForEach(AddShiftHandler);
                            EmployeeShifts = new ObservableCollection<EmployeeShift>(list);
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

        private void AddShiftHandler(EmployeeShift obj)
        {
            try
            {

                if (null == obj.DayShift) obj.DayShift = new Shift() { UuId = Guid.NewGuid().ToString(), ShiftType = 0, WorkDay = new Day(SelectedDate) };
                if (null == obj.NightShift) obj.NightShift = new Shift() { UuId = Guid.NewGuid().ToString(), ShiftType = 1, WorkDay = new Day(SelectedDate) };


                obj.DayShift.PropertyDeletegate = ShiftEventHandler;
                obj.NightShift.PropertyDeletegate = ShiftEventHandler;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ShiftEventHandler(BaseModel model)
        {
            try
            {
                var shift = model as Shift;
                if (null == shift) return;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<object, Shift>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/{Project.UuId}/employee/{SelectedEmployeeShift.Employee.UuId}/shiftadd").ToString(), shift, _securityService?.GetCurrentToken(), (l) =>
                    {
                        if (ShiftDatesCollection.Contains(shift.WorkDay.Date)) return;
                        ShiftDatesCollection.Add(shift.WorkDay.Date);
                    }, "", false);
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshShifts()
        {
            if (Project == null) return;
            using (var rep = _serviceLocator.GetInstance<IRestRepository<List<ShiftDay>, Day>>())
            {
                rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/{Project.UuId}/allshifts").ToString(), null, _securityService.GetCurrentToken(), (list) =>
                {
                    try
                    {
                        if (list == null)
                            ShiftDays = new ObservableCollection<ShiftDay>();
                        else
                            ShiftDays = new ObservableCollection<ShiftDay>(list);

                        try
                        {
                            var res = Application.Current.Resources;
                            var k = res.Keys;

                            //var dayTemplateselector = (ShiftCalendarTemplateSelector)Application.Current.FindResource("ShiftCalendarTemplateSelector");

                            ShiftDatesCollection = new ObservableCollection<DateTime>(ShiftDays.Select(s => s.Date).ToList());
                            //dayTemplateselector.ShiftDays = ShiftDatesCollection;

                            _eventAggregator.GetEvent<SelectedEvent<ObservableCollection<DateTime>>>().Publish(new SelectedEventArgs<ObservableCollection<DateTime>> { SelectedData = ShiftDatesCollection });

                        }
                        catch (ResourceReferenceKeyNotFoundException ex)
                        {
                            int a = 0;
                        }


                    }
                    catch (Exception exc)
                    {
                        _exceptionService.RaiseException(exc);
                    }
                });
            }
        }

        private void OnChangeProjectCommand()
        {
            try
            {
                if(!EmployeeShifts.Any(e => e.IsSelected))
                {
                    return;
                }
                _eventAggregator.GetEvent<ListEvent<IDepartureArrival>>().Publish(new ListEventArgs<IDepartureArrival>() { SelectAction = OnListProjectCallback, DataProvider = OnProvideDataForProjectSelect });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnProvideDataForProjectSelect(Action<List<IDepartureArrival>> obj)
        {
            try
            {
                obj?.Invoke(SiteData.Projects.Where(p => !p.IsSelected).Select(p=>p as IDepartureArrival).ToList());
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnListProjectCallback(IDepartureArrival obj)
        {
            try
            {
                _departures.AddInteralsiteDeparture(Project, obj as Project, EmployeeShifts.Where(e => e.IsSelected).Select(e => e.Employee).ToList(), SelectedDate, OnAddDepartureCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnAddDepartureCallback()
        {
            try
            {
                RefreshEmployees(SelectedDate);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
