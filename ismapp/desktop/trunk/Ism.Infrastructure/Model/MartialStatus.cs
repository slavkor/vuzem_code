using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class MartialStatus: BaseModel
    {
        private string _name;
        private string _status;

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

        //[JsonProperty("status")]
        //public string Status
        //{
        //    get { return _status; }
        //    set
        //    {
        //        SetProperty(ref _status, value);
        //        PropertyDeletegate?.Invoke(this);
        //    }
        //}
    }
}
