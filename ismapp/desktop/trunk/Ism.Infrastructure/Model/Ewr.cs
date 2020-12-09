using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Ewr : BaseModel
    {

        private long _number;
        private string _externalnumber;
        private string _description;
        private int _hours;
        private double _materialcosts;

        private Employee _sitemanager;
        private Contact _exsitemanager;
        private Project _project;
        private List<Employee> _employees;
        private List<EwrWorker> _workers;
        private DateTime _date;

        [JsonProperty("number")]
        public long Number
        {
            get { return _number; }
            set
            {
                SetProperty(ref _number, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("externalnumber")]
        public string ExternalNumber
        {
            get { return _externalnumber; }
            set
            {
                SetProperty(ref _externalnumber, value);
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
        [JsonProperty("hours")]
        public int Hours
        {
            get { return _hours; }
            set
            {
                SetProperty(ref _hours, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("materialcosts")]
        public double MaterialCosts
        {
            get { return _materialcosts; }
            set
            {
                SetProperty(ref _materialcosts, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        public IList<Document> Documents { get; set; }

        [JsonProperty("project", NullValueHandling = NullValueHandling.Ignore)]
        public Project Project { get { return _project; } set { SetProperty(ref _project, value); PropertyDeletegate?.Invoke(this); } }


        [JsonProperty("sitemanager", NullValueHandling = NullValueHandling.Ignore)]
        public Employee SiteManager { get { return _sitemanager; } set { SetProperty(ref _sitemanager, value); PropertyDeletegate?.Invoke(this); } }

        [JsonProperty("exsitemanager", NullValueHandling = NullValueHandling.Ignore)]
        public Contact ExSiteManager { get { return _exsitemanager; } set { SetProperty(ref _exsitemanager, value); PropertyDeletegate?.Invoke(this); } }

        //[JsonProperty("employees", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Employee> Employees { get { return _employees; } set { SetProperty(ref _employees, value); PropertyDeletegate?.Invoke(this); } }
        [JsonProperty("workers", NullValueHandling = NullValueHandling.Ignore)]
        public List<EwrWorker> Workers { get { return _workers; } set { SetProperty(ref _workers, value); PropertyDeletegate?.Invoke(this); } }

        [JsonConverter(typeof(DateTimeConverter), "yyyyMMdd")]
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get { return _date; } set { SetProperty(ref _date, value); PropertyDeletegate?.Invoke(this); } }
    }
}
