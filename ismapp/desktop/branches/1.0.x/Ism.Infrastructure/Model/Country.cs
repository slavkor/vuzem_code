using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class Country: BaseModel
    {
        private string _name;
        private string _iso;

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

        [JsonProperty("iso")]
        public string Iso
        {
            get { return _iso; }
            set
            {
                SetProperty(ref _iso, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
