using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ism.Infrastructure.Extensions;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.GanttView;
using System.Collections;
using Telerik.Windows.Controls;

namespace Ism.Infrastructure.Model
{
    public class Project : BaseModel, IDepartureArrival, ISelectionAware//, IGanttTask
    {
        private string _name;
        private string _description;
        private Day _start;
        private Day _end;
        private long _estimatedHours;
        private decimal _estimatedValue;
        private long _projectNumber;
        private string _externalNumber;
        private IList<Document> _documents;
        private IList<Contact> _contacts;
        private IList<Address> _addresses;
        private int _estimatedworkers;
        private Address _address;
        private bool _selected;

        private ConstructionSite _site;
        private ObservableCollection<ProjectWorkPeriod> _workperiods;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("start")]
        public Day Start
        {
            get { return _start; }
            set
            {
                SetProperty(ref _start, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("end")]
        public Day End
        {
            get { return _end; }
            set
            {
                SetProperty(ref _end, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("estimatedhours", NullValueHandling = NullValueHandling.Ignore)]
        public long EstimatedHours
        {
            get { return _estimatedHours; }
            set
            {
                SetProperty(ref _estimatedHours, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("estimatedvalue", NullValueHandling=NullValueHandling.Ignore)]
        public decimal EstimatedValue
        {
            get { return _estimatedValue; }
            set
            {
                SetProperty(ref _estimatedValue, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("estimatedworkers", NullValueHandling = NullValueHandling.Ignore)]
        public int EstimatedWorkers
        {
            get { return _estimatedworkers; }
            set
            {
                SetProperty(ref _estimatedworkers, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("projectnumber")]
        public long ProjectNumber
        {
            get { return _projectNumber; }
            set
            {
                SetProperty(ref _projectNumber, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("externalnumber")]
        public string ExternalNumber
        {
            get { return _externalNumber; }
            set
            {
                SetProperty(ref _externalNumber, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("contacts", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                SetProperty(ref _contacts, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("addresses", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty(ref _addresses, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("site", NullValueHandling = NullValueHandling.Ignore)]
        public ConstructionSite Site
        {
            get { return _site; }
            set
            {
                SetProperty(ref _site, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        
        [JsonProperty("workperiods", NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<ProjectWorkPeriod> WorkPeriods
        {
            get { return _workperiods; }
            set
            {
                SetProperty(ref _workperiods, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public ProjectState ProjectState
        {
            get
            {
                return (ProjectState)Status;
            }
            set
            {
                Status = (int)value;
            }
        }

        #region IDepartureArrival
        public string DepartureArrivalType
        {
            get
            {
                return "PROJECT";
            }

            set
            {
                //SetProperty(ref _departureArrivalType, value);
                //PropertyDeletegate?.Invoke(this);
            }
        }

        public Address Address
        {
            get
            {
                return _address; 
            }
            set
            {
                SetProperty(ref _address, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public string PointName
        {
            get
            {
                return Site == null ? Name : Site.Customer == null ? Site.Name + " - " + Name : Site.Customer.Name + " - " + Site.Name + " - " + Name;
            }
        }
        public bool IsOrigin
        {
            get
            {
                return false;
            }

            set
            {
                ;
            }
        }
        #endregion

        [JsonIgnore]
        public TimeSpan Duration => End.Date.AddDays(1).AddSeconds(-1)-Start.Date;

        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return _selected;
            }

            set
            {
                SetProperty(ref _selected, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public List<ProjectDateInfo> ProjectDays => (End.Date - Start.Date).ProjectDays(Start.Date, EstimatedWorkers);



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
        //        return Name;
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
        //        return End.Date;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //DateTime IDateRange.Start
        //{
        //    get
        //    {
        //        return Start.Date;
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //DateTime IDateRange.End
        //{
        //    get
        //    {
        //        return End.Date;
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
        //        return WorkPeriods;
        //    }
        //}

        public override string ToString()
        {
            return $"{Site?.Name} {Name}" ;
        }

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
