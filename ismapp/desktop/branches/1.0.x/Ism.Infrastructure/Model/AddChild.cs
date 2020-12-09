using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class AddChild<T, TC> : BaseModel where T: BaseModel where TC: BaseModel
    {
        private TC _child;
        private T _parent;

        public AddChild() : this(null){ }

        public AddChild(T parent) : this(parent, null){ }

        public AddChild(T parent, TC child)
        {
            Parent = parent;
            Child = child;

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

        [JsonProperty("childpayload")]
        public TC Child
        {
            get { return _child; }
            set
            {
                SetProperty(ref _child, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
