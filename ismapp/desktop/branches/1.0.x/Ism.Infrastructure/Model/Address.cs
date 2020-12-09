using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Address : BaseModel
    {
        private string _line1;
        private string _line2;
        private string _city;
        private string _state;
        private string _zip;
        private string _country;
        private string _type;



        [JsonProperty("line1")]
        public string Line1
        {
            get { return _line1; }
            set
            {
                SetProperty(ref _line1, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("line2")]
        public string Line2
        {
            get { return _line2; }
            set
            {
                SetProperty(ref _line2, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("city")]
        public string City
        {
            get { return _city; }
            set
            {
                SetProperty(ref _city, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("state")]
        public string State
        {
            get { return _state; }
            set
            {
                SetProperty(ref _state, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("zip")]
        public string Zip
        {
            get { return _zip; }
            set
            {
                SetProperty(ref _zip, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("country")]
        public string Country
        {
            get { return _country; }
            set
            {
                SetProperty(ref _country, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("type")]
        public string Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public string PrettyPrint => $"{Line1} {Line2}, {State}, {Zip} {City}, {Country}";
    }
}
