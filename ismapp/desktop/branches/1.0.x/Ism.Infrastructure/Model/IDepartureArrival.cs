using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public interface IDepartureArrival : IBaseModel
    {
        [JsonProperty("departtype")]
        string DepartureArrivalType { get; set; }

        [JsonProperty("address")]
        Address Address { get; set; }

        [JsonIgnore]
        string PointName { get; }
        [JsonIgnore]
        bool IsOrigin { get; set; }
    }

    public interface IOriginDestination : IDepartureArrival
    {

    }
}
