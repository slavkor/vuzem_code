using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class AddDocument<T>  :BaseModel where T : BaseModel 
    {
        private Document _document;
        private T _parent;

        public AddDocument(): this(null){}

        public AddDocument(Document document) :this(null, document){}

        public AddDocument(T parent, Document document)
        {
            Parent = parent;
            Document = document;

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
            get { return Parent?.UuId; }
            set
            {
                if (Parent != null) Parent.UuId = value;
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("document")]
        public Document Document
        {
            get { return _document; }
            set
            {
                SetProperty(ref _document, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
