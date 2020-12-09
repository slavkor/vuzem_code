using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class File : BaseModel
    {
        private string _name;
        private string _uniquename;
        private string _ext;
        private Language _language;

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "uniquename")]
        public string UniqueName
        {
            get { return _uniquename; }
            set
            {
                SetProperty(ref _uniquename, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "ext")]
        public string Extension
        {
            get { return _ext; }
            set
            {
                SetProperty(ref _ext, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "language", NullValueHandling = NullValueHandling.Ignore)]
        public Language Language
        {
            get { return _language; }
            set
            {
                SetProperty(ref _language, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public string FullName { get; set; }

        [JsonIgnore]
        public bool AddDummy { get; set; }
    }
}
