using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class ConstructionSiteOverview : BaseModel, ISelectionAware
    {

        private string sitename;

        [JsonProperty("sitename", NullValueHandling = NullValueHandling.Ignore)]
        public string SiteName
        {
            get { return $"{sitename}"; }
            set { SetProperty(ref sitename, value); PropertyDeletegate?.Invoke(this); }
        }
        private string partner;

        [JsonProperty("partner", NullValueHandling = NullValueHandling.Ignore)]
        public string Partner
        {
            get { return partner; }
            set { SetProperty(ref partner, value); PropertyDeletegate?.Invoke(this); }
        }
        private string siteid;

        [JsonProperty("siteid", NullValueHandling = NullValueHandling.Ignore)]
        public string SiteId
        {
            get { return siteid; }
            set { SetProperty(ref siteid, value); PropertyDeletegate?.Invoke(this); }
        }
        private string lastname;

        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName
        {
            get { return lastname; }
            set { SetProperty(ref lastname, value); PropertyDeletegate?.Invoke(this); }
        }

        private string employeename;
        [JsonProperty("employeename", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName
        {
            get { return employeename; }
            set { SetProperty(ref employeename, value); PropertyDeletegate?.Invoke(this); }
        }

        private string employeeid;
        [JsonProperty("employeeid", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeId
        {
            get { return employeeid; }
            set { SetProperty(ref employeeid, value); PropertyDeletegate?.Invoke(this); }
        }

        private IList<DepartureOverview> departures;
        [JsonProperty("departures", NullValueHandling = NullValueHandling.Ignore)]
        public IList<DepartureOverview> Departures
        {
            get { return departures; }
            set { SetProperty(ref departures, value); PropertyDeletegate?.Invoke(this); }
        }

        [JsonIgnore]
        public string PlanedDepartures
        {
            get
            {
                string deps = "";
                if (null == Departures) return deps;
                foreach (var item in Departures.OrderBy(i => i.DepartTime))
                {
                    deps += $"{item.DepartTime.ToShortDateString()} {item.Origin} -> {item.Destination} / ";
                }

                return deps;
            }
        }

        private bool _selected;
        public bool IsSelected { get { return _selected; } set { SetProperty(ref _selected, value); PropertyDeletegate?.Invoke(this); } }
    }
}
