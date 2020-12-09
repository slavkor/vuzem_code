using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Occupancy : BaseModel
    {
        private string _type;
        private Day _start;
        private Day _end;


        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value); PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public Day Start
        {
            get { return _start; }
            set
            {
                SetProperty(ref _start, value); PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public Day End
        {
            get { return _end; }
            set
            {
                SetProperty(ref _end, value); PropertyDeletegate?.Invoke(this);
            }
        }

        private Project _project;
        [JsonProperty("project", NullValueHandling = NullValueHandling.Ignore)]
        public Project Project
        {
            get { return _project; }
            set
            {
                SetProperty(ref _project, value); PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
