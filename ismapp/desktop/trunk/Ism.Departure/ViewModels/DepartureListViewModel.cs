using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Mvvm;

using System.Collections;
using Ism.Departure.Events;

namespace Ism.Departure.ViewModels
{
    class DepartureListViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private DepartureList _currnet;
        private DateTime _start;
        private DateTime _end;

        private DateTime _visibleStart;
        private DateTime _visibleEnd;
        private ObservableCollection<DepartureList> _departures;
        private List<DepartureList> _list;

        private ObservableCollection<Project> _projects;

        private BaseModel _origin;
        private BaseModel _destination;

        private ObservableCollection<IDepartureArrival> _originiDeparture;

        public DepartureListViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;

            ConfirmEmployeeCommand = new DelegateCommand(OnConfirmEmployeeCommand);
            EditDepartureCommand = new DelegateCommand<DepartureList>(OnEditDepartureCommand, CanExecuteEditDepartureCommand);
            SelectionChangedCommand = new DelegateCommand<object>(OnSelectionChangedCommand);
        }

        private void OnSelectionChangedCommand(object obj)
        {
            try
            {
                var p = obj as Project;

                Departures = new ObservableCollection<DepartureList>(_list.Where(d => d.FromProject?.UuId == p.UuId || d.ToProject?.UuId == p.UuId));
                int a = 0;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<object> SelectionChangedCommand { get; }

        #region public properties


        public DepartureList CurrentSelected
        {
            get
            {
                return _currnet;
            }
            set
            {
                SetProperty(ref _currnet, value);
                _eventAggregator.GetEvent<SelectedEvent<DepartureList>>().Publish(new SelectedEventArgs<DepartureList>() { SelectedData = _currnet });
                if (CurrentSelected == null) return;
                OriginiDeparture = new ObservableCollection<IDepartureArrival>();
                var o = CurrentSelected?.Departure?.Origin as IDepartureArrival;
                var d = CurrentSelected?.Departure?.Destination as IDepartureArrival;
                o.IsOrigin = true;
                OriginiDeparture.Add(o);
                OriginiDeparture.Add(d);
            }
        }

        public BaseModel Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                SetProperty(ref _origin, value);
            }
        }

        public BaseModel Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                SetProperty(ref _destination, value);
            }
        }

        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                SetProperty(ref _start, value);
            }
        }

        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                SetProperty(ref _end, value);
            }
        }

        public DateTime VisibleStart
        {
            get
            {
                return _visibleStart;
            }
            set
            {
                SetProperty(ref _visibleStart, value);
            }
        }

        public DateTime VisibleEnd
        {
            get
            {
                return _visibleEnd;
            }
            set
            {
                SetProperty(ref _visibleEnd, value);
                _eventAggregator.GetEvent<DateRange>().Publish(new Range(_visibleStart, _visibleEnd));
            }
        }
        private DateTime _date;

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                SetProperty(ref _date, value);
                RefreshDepartures(new Range(Date.FirstDayOfMonth().AddMonths(-3), Date.LastDayOfMonth().AddMonths(3)));
            }
        }

        public ObservableCollection<DepartureList> Departures
        {
            get
            {
                return _departures;

            }
            set
            {
                SetProperty(ref _departures, value);
            }
        }

        public ObservableCollection<Project> Projects
        {
            get
            {
                return _projects;

            }
            set
            {
                SetProperty(ref _projects, value);
            }
        }
        #endregion


        #region commands

        public DelegateCommand<DepartureList> EditDepartureCommand { get; }
        public DelegateCommand ConfirmEmployeeCommand { get; }

        public ObservableCollection<IDepartureArrival> OriginiDeparture
        {
            get { return _originiDeparture; }
            set { SetProperty(ref _originiDeparture, value); }
        }

        #endregion

        #region ViewModelBase overrides
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                if (navigationContext.Parameters?["origin"] != null)
                    Origin = navigationContext.Parameters?["origin"] as BaseModel;
                if (navigationContext.Parameters?["destination"] != null)
                    Destination = navigationContext.Parameters?["destination"] as BaseModel;

                CurrentSelected = null;
                Date = DateTime.Now;
                //RefreshProjects();

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void RefreshProjects()
        {
            try
            {
                //using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Project>, string>>())
                //{

                //    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"project/list/{(int)ProjectState.InProgress}").ToString(), _securityService.GetCurrentUser().AccessToken,
                //        list =>
                //        {
                //            Projects = new ObservableCollection<Project>(list);
                //        });
                //}
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public override bool KeepAlive => false;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            CurrentSelected = null;

        }

        #endregion



        public void RefreshDepartures(Range range)
        {
            try
            {
                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<DepartureList>, Range>>())
                {
                    if (_securityService.HasPermissionExcplicit("foreman"))
                    {
                        range.Origin = Origin;
                        range.Destination = Destination;
                    }
                    repository.PostRequestAsync(new Uri(_settings.GetApiServer(), "departures/list").ToString(), range, _securityService.GetCurrentToken(), (list) =>
                    {
                        try
                        {
                            Start = range.From.Date;
                            End = range.To.Date.AddDays(1);

                            VisibleStart = Date.FirstDayOfMonth();
                            VisibleEnd = Date.LastDayOfMonth().AddDays(1);

                            CurrentSelected = null;
                            _list = list;
                            Departures = new ObservableCollection<DepartureList>(_list.OrderBy(d => d.Departure.DepartTime));
                            
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

        #region private methods

        #region commands

        private void OnConfirmEmployeeCommand()
        {
            try
            {
                throw new Exception("OnConfirmEmployeeCommand");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteEditDepartureCommand(DepartureList arg)
        {
            return true;
        }

        private void OnEditDepartureCommand(DepartureList obj)
        {
            try
            {
                _eventAggregator.GetEvent<EditEvent<DepartureList>>().Publish(new EditEventArgs<DepartureList>() { EditMode = EditMode.Edit, EditObject = obj });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        #endregion

        #endregion

    }

}

