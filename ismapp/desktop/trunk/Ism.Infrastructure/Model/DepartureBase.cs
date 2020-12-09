using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ism.Infrastructure.Model
{
    public class DepartureBase : BaseModel
    {
        private DateTime _departTime;
        private int _estimatedworkers;
        private bool _internal;

        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty(PropertyName = "departtime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DepartTime
        {
            get
            {
                return _departTime;
            }
            set
            {
                SetProperty(ref _departTime, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "internal", NullValueHandling = NullValueHandling.Ignore)]
        public bool Internal
        {
            get
            {
                return _internal;
            }
            set
            {
                SetProperty(ref _internal, value);
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
    }
}
