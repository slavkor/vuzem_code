using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class EmployeeOverview : Employee
    {

        private IList<DepartureOverview> _departures;

        [JsonProperty("departures", NullValueHandling = NullValueHandling.Ignore)]
        public IList<DepartureOverview> Departures
        {
            get
            {
                return _departures;
            }

            set
            {
                SetProperty(ref _departures, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        private DepartureOverview _departure;

        [JsonProperty("departure", NullValueHandling = NullValueHandling.Ignore)]
        public DepartureOverview Departure
        {
            get
            {
                return _departure;
            }

            set
            {
                SetProperty(ref _departure, value);
                PropertyDeletegate?.Invoke(this);
            }
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
                    deps += $"{item.DepartTime.ToShortDateString()} v {item.Destination}, ";
                }

                return deps;
            }
        }

        private string _sitename;
        [JsonIgnore]
        public string SiteName
        {
            get
            {
                return _sitename;
            }
            set
            {
                SetProperty(ref _sitename, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

    }
}
