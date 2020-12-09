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
using Ism.Construction.Events;

namespace Ism.Construction.ViewModels
{
    class ConstructionSitesListStatsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly IEmployeeService _employeeService;
        public ConstructionSitesListStatsViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService, IEmployeeService employeeService)
        {
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));


            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            _employeeService = employeeService;
        }

        private DateTime _start;
        public DateTime Start
        {
            get { return _start; }
            set { SetProperty(ref _start, value); }
        }
        private DateTime _vstart;
        public DateTime VStart
        {
            get { return _vstart; }
            set { SetProperty(ref _vstart, value); }
        }
        private DateTime _sstart;
        public DateTime SStart
        {
            get { return _sstart; }
            set { SetProperty(ref _sstart, value); }
        }
        private DateTime _end;
        public DateTime End
        {
            get { return _end; }
            set { SetProperty(ref _end, value); }
        }
        private DateTime _vend;
        public DateTime VEnd
        {
            get { return _vend; }
            set { SetProperty(ref _vend, value); }
        }
        private DateTime _send;
        public DateTime SEnd
        {
            get { return _send; }
            set { SetProperty(ref _send, value); }
        }

        private List<Project> _projects;
        public List<Project> Projects
        {
            get { return _projects; }
            set { SetProperty(ref _projects, value); }
        }

        private int _inProgressCnt;

        public int InProgressCnt
        {
            get { return _inProgressCnt; }
            set { SetProperty( ref _inProgressCnt ,value); }
        }

        private int _planedCnt;

        public int PlannedCnt
        {
            get { return _planedCnt; }
            set { SetProperty(ref _planedCnt ,value); }
        }

        private List<ProjectDateInfo> _dates;

        public List<ProjectDateInfo> Dates
        {
            get { return _dates; }
            set { SetProperty(ref _dates, value); }
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            Reset();

            var list =  _employeeService.GetAllActiveEmployees(_securityService.GetCurrentCompany());

            int allEmps = list.Sum(l => l.Count);

            Start = (DateTime)navigationContext.Parameters["start"];
            End = (DateTime)navigationContext.Parameters["end"];

            VStart = (DateTime)navigationContext.Parameters["vstart"];
            VEnd = (DateTime)navigationContext.Parameters["vend"];

            SStart = VEnd.AddDays(-(VEnd.Subtract(VStart).Days / 2));
            SEnd = SStart.AddDays(7);

            Projects = navigationContext.Parameters["projects"] as List<Project>;


            if (null == Projects) return; 

            PlannedCnt = Projects.Where(p => p.ProjectState == ProjectState.Planned).Sum(p => p.EstimatedWorkers);
            InProgressCnt = Projects.Where(p => p.ProjectState == ProjectState.InProgress).Sum(p => p.EstimatedWorkers);

            Dates = Projects.SelectMany(p => p.ProjectDays).GroupBy(d => d.Date).Select(g => new ProjectDateInfo(g.First().Date.AddHours(12), g.Sum(t => t.NoOfWorkers), allEmps)).OrderBy(o => o.Date).ToList();
        }


        public override bool KeepAlive => false;


        private void Reset()
        {
            try
            {
                Dates = null;
                Projects = null;
                Start = DateTime.Now;
                End = DateTime.Now;
                VStart = Start;
                VEnd = End;
                PlannedCnt = 0;
                InProgressCnt = 0;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
