using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class ListAddress : BaseModel
    {
        private IList<Address> _addresses;

        [JsonProperty("addresses", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty(ref _addresses, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
