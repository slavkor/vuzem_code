using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ism.Infrastructure.Extensions;
using Telerik.Windows.Controls.GanttView;
using System.Collections;

namespace Ism.Infrastructure.Model
{
    public class ConstructionSiteList : BaseModel//, IGanttTask
    {
        private ConstructionSite _constructionSite;
        private IList<Project> _projects;
        private DateTime _start;
        private DateTime _end;

        public ConstructionSiteList()
        {
            

        }

        [JsonProperty("site")]
        public ConstructionSite Site
        {
            get { return _constructionSite; }
            set
            {
                SetProperty(ref _constructionSite, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("projects")]
        public IList<Project> Projects
        {
            get { return _projects; }
            set
            {
                SetProperty(ref _projects, value);
                PropertyDeletegate?.Invoke(this);
                Site.Projects = Projects;
                Start = null == Site.Projects || Site.Projects.Count == 0 ? DateTime.Now : (from p in Site.Projects select p.Start == null ? DateTime.Now.FirstDayOfMonth() : p.Start.Date).Min();
                End = null == Site.Projects || Site.Projects.Count == 0 ? DateTime.Now : (from p in Site.Projects select p.End == null ? DateTime.Now.LastDayOfMonth() : p.End.Date).Max().AddDays(1).AddSeconds(-1);
            }
        }
        [JsonIgnore]
        public DateTime Start
        {
            get
            {
                return _start;

            }
            set
            {
                SetProperty(ref _start, value);
                PropertyDeletegate?.Invoke(this);

            }
        }
        [JsonIgnore]
        public DateTime End
        {
            get
            {
                return _end;

            }
            set
            {

                SetProperty(ref _end, value);
                PropertyDeletegate?.Invoke(this);
            }

        }

        [JsonIgnore]
        public TimeSpan Duration
        {
            get
            {
                return End - Start;
            }
        }

        [JsonIgnore]
        public string GroupName
        {
            get
            {
                return Site?.Customer?.Name;
            }
        }

        [JsonIgnore]
        public int? EstimatedWorkers
        {
            get
            {
                
                return Site?.Projects?.Sum(p => p.EstimatedWorkers);
            }
        }

        [JsonIgnore]
        public ConstructionStieState ConstructionStieState
        {
            get
            {
                var projs = Site?.Projects;
                if (projs == null || projs.Count <= 0 ) return ConstructionStieState.Planned;

                if (projs.All(p => p.ProjectState == ProjectState.Planned)) return ConstructionStieState.Planned;
                if (projs.All(p => p.ProjectState == ProjectState.Closed)) return ConstructionStieState.Closed;

                return ConstructionStieState.InProgress;

            }
        }

        //public IEnumerable Dependencies
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        //TimeSpan IGanttTask.Duration
        //{
        //    get
        //    {
        //        return Duration;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public string Title
        //{
        //    get
        //    {
        //        return ConstructionSite.Name;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public string Description
        //{
        //    get
        //    {
        //        return ConstructionSite.Description;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public double Progress
        //{
        //    get
        //    {
        //        return 0;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public DateTime? Deadline
        //{
        //    get
        //    {
        //        return End;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public IList Resources
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public IEnumerable Children
        //{
        //    get
        //    {
        //        return Projects;
        //    }
        //}

        //public object SaveState()
        //{
        //    throw new NotImplementedException();
        //}

        //public void LoadState(object state)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
