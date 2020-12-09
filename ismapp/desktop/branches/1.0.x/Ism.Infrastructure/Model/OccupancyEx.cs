using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class OccupancyEx: Occupancy
    {
        private Company _company;
        private Project _project;

        [JsonProperty("company", NullValueHandling =NullValueHandling.Ignore)]
        public Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                SetProperty(ref _company, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("project", NullValueHandling = NullValueHandling.Ignore)]
        public Project Project
        {
            get
            {
                return _project;
            }
            set
            {
                SetProperty(ref _project, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public string Name => Company == null ? Project.PointName : Company.PointName;

        [JsonIgnore]
        public double TotalDays => ((End == null ? new Day(DateTime.Now).Date : End.Date) - (Start == null ? new Day(DateTime.Now).Date : Start.Date)).TotalDays;
    }
}
