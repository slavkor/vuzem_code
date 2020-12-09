using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class CarList : BaseModel
    {
        [JsonProperty("car", NullValueHandling = NullValueHandling.Ignore)]
        public Car Car { get; set; }

        [JsonProperty("project", NullValueHandling = NullValueHandling.Ignore)]
        public Project Project { get; set; }

        [JsonProperty("site", NullValueHandling = NullValueHandling.Ignore)]
        public ConstructionSite ConstructionSite { get; set; }

    }
}
