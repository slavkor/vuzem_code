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

namespace Ism.Construction.ViewModels
{
    public class ConstructionSitesListViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly IEmployeeService _employeeService;
        private ObservableCollection<ConstructionSiteList> _constructionSites;
        private ConstructionSiteList _selected;
        private ObservableCollection<Project> _projects;


        public ConstructionSitesListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService, IEmployeeService employeeService)
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
                EditConstructionSiteCommand = new DelegateCommand<ConstructionSiteList>(OnEditConstructionSiteCommand, CanExecuteEditConstructionSiteCommand);
                SelectionChangedEventCommand = new DelegateCommand<object>(OnSelectionChangedEventCommand);
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
                RefreshConstructionSites(new Range(Date.FirstDayOfYear(), Date.LastDayOfYear()));
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private ObservableCollection<Project> _projectsInSelection;

        private ObservableCollection<IGanttTask> _tasks;
        public ObservableCollection<IGanttTask> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                SetProperty(ref _tasks, value);
            }
        }

        public ObservableCollection<Project> ProjectsInSelection
        {
            get { return _projectsInSelection; }
            set { SetProperty(ref _projectsInSelection, value); }
        }

        private ObservableCollection<WorkPlaceInfo> _workPlaceInfos;
        public ObservableCollection<WorkPlaceInfo> WorkPlaceInfos
        {
            get { return _workPlaceInfos; }
            set { SetProperty(ref _workPlaceInfos, value); }
        }

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set { SetProperty(ref _projects, value); }
        }

        private Telerik.Windows.Controls.IDateRange visibleRange;
        public Telerik.Windows.Controls.IDateRange VisibleRange
        {
            get { return visibleRange; }
            set
            {
                SetProperty(ref visibleRange, value);
            }
        }

        private Range LastRange { get; set; }


        private List<ProjectDateInfo> _dates;

        public List<ProjectDateInfo> Dates
        {
            get { return _dates; }
            set { SetProperty(ref _dates, value); }
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
                RefreshConstructionSites(new Range(Date.FirstDayOfYear(), Date.LastDayOfYear()));
            }
        }

        private bool CanExecuteEditConstructionSiteCommand(ConstructionSiteList site)
        {
            return site?.Site?.Company?.UuId == _securityService.GetCurrentCompany()?.UuId;
        }

        private void OnEditConstructionSiteCommand(ConstructionSiteList site)
        {
            try
            {
                if (null == site?.Site) return;
                _eventAggregator.GetEvent<EditEvent<ConstructionSite>>().Publish(new EditEventArgs<ConstructionSite>() { EditObject = site?.Site, EditMode = EditMode.Edit, RefreshAction = s => { RefreshConstructionSites(LastRange); } });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private DateTime _visibleStart;

        public DateTime VisibleStart
        {
            get { return _visibleStart; }
            set
            {
                SetProperty(ref _visibleStart, value);
                if (null == WorkPlaceInfos) return;
                foreach (var item in WorkPlaceInfos)
                {
                    item.VisibleStart = value;
                }
            }
        }

        private DateTime _visibleEnd;

        public DateTime VisibleEnd
        {
            get { return _visibleEnd; }
            set
            {
                SetProperty(ref _visibleEnd, value);
                if (null == WorkPlaceInfos) return;
                foreach (var item in WorkPlaceInfos)
                {
                    item.VisibleEnd = value;
                }
            }
        }

        private DateTime _selectionStart;

        public DateTime SelectionStart
        {
            get { return _selectionStart; }
            set
            {
                SetProperty(ref _selectionStart, value);
                if (null == WorkPlaceInfos) return;
                foreach (var item in WorkPlaceInfos)
                {
                    item.SelectionStart = value;
                }
            }
        }

        private DateTime _selectionEnd;

        public DateTime SelectionEnd
        {
            get { return _selectionEnd; }
            set
            {
                SetProperty(ref _selectionEnd, value);
                if (null == WorkPlaceInfos) return;

                foreach (var item in WorkPlaceInfos)
                {
                    item.SelectionEnd = value;
                }
            }
        }


        private DateTime _minDate;

        public DateTime MinDate
        {
            get { return _minDate; }
            set
            {
                SetProperty(ref _minDate, value);
                if (null == WorkPlaceInfos) return;
                foreach (var item in WorkPlaceInfos)
                {
                    item.MinDate = value;
                }
            }
        }

        private DateTime _maxDate;

        public DateTime MaxDate
        {
            get { return _maxDate; }
            set
            {
                SetProperty(ref _maxDate, value);
                if (null == WorkPlaceInfos) return;
                foreach (var item in WorkPlaceInfos)
                {
                    item.MaxDate = value;
                }
            }
        }
        public DelegateCommand<object> SelectionChangedEventCommand { get; }
        public DelegateCommand<ConstructionSiteList> EditConstructionSiteCommand { get; }


        public ObservableCollection<ConstructionSiteList> ConstructionSites { get { return _constructionSites; } set { SetProperty(ref _constructionSites, value); } }

        public ConstructionSiteList Selected
        {
            get { return _selected; }
            set
            {
                try
                {
                    SetProperty(ref _selected, value);
                    _eventAggregator.GetEvent<SelectedEvent<ConstructionSite>>().Publish(new SelectedEventArgs<ConstructionSite>(_selected?.Site));
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                }
            }
        }

        private void RefreshConstructionSites(Range range)
        {
            try
            {

                ConstructionSites = null;
                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<ConstructionSiteList>, Range>>())
                {

                    repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), "csite/list").ToString(), range, _securityService.GetCurrentUser().AccessToken,
                        (e) =>
                        {

                            Projects = new ObservableCollection<Project>(e.SelectMany(site => site.Site.Projects == null ? new List<Project>() : site.Site.Projects).ToList());

                            MinDate = range.From.Date.AddDays(-1);
                            MaxDate = range.To.Date.AddDays(1);

                            ConstructionSites = new ObservableCollection<ConstructionSiteList>(e);

                            Tasks = new ObservableCollection<IGanttTask>();
                            foreach (var item in ConstructionSites)
                            {
                                var task = new GanttTask(item.Start, item.End, item.Site.Name);
                                GanttTask lp = null;
                                foreach (var p in item.Projects.OrderBy(p => p.Start.Date))
                                {
                                    GanttTask pt = new GanttTask(p.Start.Date, p.End.Date, p.Site.Name);
                                    if (null != lp) pt.Dependencies.Add(new Dependency() { FromTask = lp});
                                    task.Children.Add(pt);
                                    lp = pt;
                                }

                                //for (int i = task.Children.Count; i > 0; i--)
                                //{
                                //    if (i == 1) continue;
                                //    ((GanttTask)task.Children[i]).Dependencies.Add(new Dependency() { FromTask = task.Children[i - 1] });
                                //}
                               

                                //var task = new Task() { Start = item.Start, End = item.End, Duration = item.Duration, Deadline = item.End, Description = item.Site.Description, Title = item.Site.Name };

                                Tasks.Add(task);
                            }

                            VisibleRange = new Telerik.Windows.Controls.Scheduling.DateRange(range.From.Date, range.To.Date);


                            VisibleStart = MinDate;
                            VisibleEnd = MaxDate;

                            SelectionStart = MaxDate.AddDays(-(MaxDate.Subtract(MinDate).Days / 2)).AddDays(-3);
                            SelectionEnd = SelectionStart.AddDays(7);

                            var listwps = _employeeService.GetAllActiveEmployees(_securityService.GetCurrentCompany());
                            int allEmps = listwps.Sum(l => l.Count);

                            Dates = null;
                            if (null == ConstructionSites) return;

                            var comparer = new PropertyComparer<WorkPlace>("UuId");
                            var list = Projects.SelectMany(p => p.WorkPeriods.SelectMany(wp => wp.WorkPlans.GroupBy(pp => pp.WorkPlace, comparer).Select(g => new WorkPlacePeriod() { Start = wp.Start, End = wp.End, WorkPlace = g.First().WorkPlace, NoOfWorkers = g.Sum(r => r.Plan) })));

                            var wps = list.GroupBy(l => l.WorkPlace, comparer).Select(g => g.First().WorkPlace);

                            WorkPlaceInfos = new ObservableCollection<WorkPlaceInfo>(list.GroupBy(l => l.WorkPlace, comparer).Select(g => new WorkPlaceInfo() {DateCallbackAction = OnDateCallbackAction, SelectionChangedEventCommand = SelectionChangedEventCommand, MinDate = MinDate, MaxDate = MaxDate, VisibleStart = VisibleStart, VisibleEnd = VisibleEnd, SelectionStart = SelectionStart, SelectionEnd = SelectionEnd, WorkPlace = g.First().WorkPlace, Dates = g.SelectMany(f => f.Days).GroupBy(d => d.Date).Select(gg => new WorkPlaceDateInfo(gg.First().Date.AddHours(12), gg.Sum(gs => gs.NoOfWorkers), listwps.Where(lw => lw.WorkPlace.UuId == gg.First().WorkPlace.UuId).FirstOrDefault()?.Count ?? default(int), gg.First()?.WorkPlace)).ToList() }));

                            var projects = ConstructionSites == null ? null : ConstructionSites.SelectMany(site => site.Site.Projects == null ? new List<Project>() : site.Site.Projects).ToList();
                            Dates = projects.SelectMany(p => p.ProjectDays).GroupBy(d => d.Date).Select(g => new ProjectDateInfo(g.First().Date.AddHours(12), g.Sum(t => t.NoOfWorkers), allEmps)).OrderBy(o => o.Date).ToList();

                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        public new event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void OnDateCallbackAction(string date, DateTime value)
        {
            try
            {
          
                switch (date)
                {
                    case nameof(MinDate):
                        _minDate = value;
                        NotifyPropertyChanged(date);
                        break;
                    case nameof(MaxDate):
                        _maxDate = value;
                        NotifyPropertyChanged(date);
                        break;
                    case nameof(VisibleStart):
                        _visibleStart = value;
                        NotifyPropertyChanged(date);
                        break;
                    case nameof(VisibleEnd):
                        _visibleEnd = value;
                        NotifyPropertyChanged(date);
                        break;
                    case nameof(SelectionStart):
                        _selectionStart = value;
                        NotifyPropertyChanged(date);
                        break;
                    case nameof(SelectionEnd):
                        _selectionEnd = value;
                        NotifyPropertyChanged(date);
                        break;
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
            Date = DateTime.Now.FirstDayOfYear();
        }


        private void OnSelectionChangedEventCommand(object obj)
        {
            try
            {
                if (null == ConstructionSites) return;

                var data = obj as Tuple<DateTime, DateTime>;
                if (null == data) return;

                ProjectsInSelection = new ObservableCollection<Project>(ConstructionSites == null ? null : ConstructionSites.SelectMany(site => site.Site.Projects == null ? new List<Project>() : site.Site.Projects.Where(p => p.Start.Date <= data.Item2 && data.Item1 <= p.End.Date)).ToList().OrderBy(p => p.Start.Date));

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);

            }
        }
    }

    internal class Task : IGanttTask
    {
        private DateTime? _deadLine;
        private string _description;
        private TimeSpan _duration;
        private DateTime _start, _end;
        private double _progress;
        private string _title;

        public Task(IEnumerable subTasks)
        {
            SubTasks = SubTasks;
        }

        public IEnumerable SubTasks { get; private set; }

        public IEnumerable Children
        {
            get
            {
                return SubTasks;
            }
        }

        public DateTime? Deadline
        {
            get
            {
                return _deadLine;
            }

            set
            {
                _deadLine = value;
            }
        }

        public IEnumerable Dependencies
        {
            get
            {
                return null;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                _duration = value;
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
                _end = value;
            }
        }

        public double Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
            }
        }

        public IList Resources
        {
            get
            {
                return null;
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
                _start = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        public void LoadState(object state)
        {
            //throw new NotImplementedException();
        }

        public object SaveState()
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
