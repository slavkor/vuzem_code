using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class DepartureOverview: DepartureBase
    {
        private string _origin;
        private string _destination;



        [JsonProperty(PropertyName = "origin", NullValueHandling = NullValueHandling.Ignore)]
        public string Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                SetProperty(ref _origin, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "destination", NullValueHandling = NullValueHandling.Ignore)]
        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                SetProperty(ref _destination, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        
    }
}
