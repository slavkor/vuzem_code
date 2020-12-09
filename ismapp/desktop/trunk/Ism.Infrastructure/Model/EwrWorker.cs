using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class EwrWorker: BaseModel
    {

        private Employee _worker;
        private string _workPlace;

        [JsonProperty("workplace", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkPlace { get { return _workPlace; } set { SetProperty(ref _workPlace, value); PropertyDeletegate?.Invoke(this); } }

        [JsonProperty("worker", NullValueHandling = NullValueHandling.Ignore)]
        public Employee Worker { get { return _worker; } set { SetProperty(ref _worker, value); PropertyDeletegate?.Invoke(this); } }

        [JsonIgnore]
        public string WorkerId { get { return Worker.UuId; } }

        public override string ToString()
        {
            return $"{Worker.LastName} {Worker.Name}";
        }
    }
}
