using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using Ism.Infrastructure.Model;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public  class AddressType:BaseModel
    {
        private string _name;
        private string _descrioption;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _descrioption; }
            set
            {
                SetProperty(ref _descrioption, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
