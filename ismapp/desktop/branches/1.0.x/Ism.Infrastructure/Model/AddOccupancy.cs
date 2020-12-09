using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class AddOccupancy<T> : BaseModel where T : BaseModel
    {
        private Occupancy _occupancy;
        private T _parent;
        public AddOccupancy() : this(null) { }

        public AddOccupancy(Occupancy occupancy) :this(null, occupancy) { }

        public AddOccupancy(T parent, Occupancy occupancy)
        {
            Parent = parent;
            Occupancy = occupancy;

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
                if (null == Parent) return;

                Parent.UuId = value;
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("occupancy")]
        public Occupancy Occupancy
        {
            get { return _occupancy; }
            set
            {
                SetProperty(ref _occupancy, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
