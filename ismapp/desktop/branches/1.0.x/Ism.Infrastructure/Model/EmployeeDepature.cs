using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Ism.Infrastructure.Model
{
    public class EmployeeDepature : Employee
    {
        private List<OccupancyEx> _occupancies;
        private List<DepartureEx> _departures;

        private OccupancyEx _lastOccupancy;

        //[JsonProperty(PropertyName = "occupancies", NullValueHandling = NullValueHandling.Ignore)]
        //public List<OccupancyEx> Occupancies
        //{
        //    get
        //    {
        //        return _occupancies;
        //    }
        //    set
        //    {
        //        SetProperty(ref _occupancies, value);
        //        PropertyDeletegate?.Invoke(this);
        //    }
        //}

        //[JsonProperty(PropertyName = "departures", NullValueHandling = NullValueHandling.Ignore)]
        //public List<DepartureEx> Departures
        //{
        //    get
        //    {
        //        return _departures;
        //    }
        //    set
        //    {
        //        SetProperty(ref _departures, value);
        //        PropertyDeletegate?.Invoke(this);
        //    }
        //}


        [JsonProperty(PropertyName = "lastoccupancy", NullValueHandling = NullValueHandling.Ignore)]
        public OccupancyEx CurrentOccupancy
        {
            get
            {
                return _lastOccupancy;
            }
            set
            {
                SetProperty(ref _lastOccupancy, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        //[JsonIgnore]
        //public List<DepartureEx> PlanedDepartures => _departures.Where(d => d.Status == 0).OrderBy(d => d.DepartTime).ToList();

        //[JsonIgnore]
        //public OccupancyEx CurrentOccupancy => Occupancies?.Where(o => o.Active==1)?.FirstOrDefault();
        //[JsonIgnore]
        //public Project Project => Occupancies?.Where(o => o.Active == 1).FirstOrDefault().Project;
    }
}
