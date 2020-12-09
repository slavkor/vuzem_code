using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class AddAddress<T> : BaseModel where T : BaseModel
    {
        private Address _address;
        private T _parent;

        public AddAddress() : this(null){}

        public AddAddress(Address address) : this(null, address){}

        public AddAddress(T parent, Address address)
        {
            Parent = parent;
            Address = address;

        }

        [JsonIgnore]
        public T Parent
        {
            get { return _parent; }
            private set
            {
                SetProperty(ref _parent, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("parentuuid")]
        public string ParentUuid
        {
            get { return Parent.UuId; }
            set
            {
                if(null == Parent) return;
                
                Parent.UuId = value;
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("address")]
        public Address Address
        {
            get { return _address; }
            set
            {
                SetProperty(ref _address, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
