using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class EmployeeDocumentToExpire : BaseModel
    {
        private Day _day;
        private Document _document;
        private Employee _employee;
        private DocumentType _type;

        //[JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty(PropertyName = "day", NullValueHandling = NullValueHandling.Ignore)]
        public Day Day
        {
            get { return _day; }
            set
            {
                SetProperty(ref _day, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "doc", NullValueHandling = NullValueHandling.Ignore)]
        public Document Document
        {
            get { return _document; }
            set {
                SetProperty(ref _document, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "e", NullValueHandling = NullValueHandling.Ignore)]
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                SetProperty(ref _employee, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "typ", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentType Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


    }

    public class E : BaseModel
    {
        private Employee _employee;
        [JsonProperty(PropertyName = "employee", NullValueHandling = NullValueHandling.Ignore)]
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                SetProperty(ref _employee, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

    }
}
