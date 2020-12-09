using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class AddContact<T> : BaseModel where T : BaseModel
    {
        private Contact _contact;
        private T _parent;
        public AddContact() : this(null) { }

        public AddContact(Contact contact) :this(null, contact) {}

        public AddContact(T parent, Contact contact)
        {
            Parent = parent;
            Contact = contact;

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

        [JsonProperty("contact")]
        public Contact Contact
        {
            get { return _contact; }
            set
            {
                SetProperty(ref _contact, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
