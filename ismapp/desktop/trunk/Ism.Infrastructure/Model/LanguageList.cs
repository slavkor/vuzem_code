using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class LanguageList: BaseModel
    {
        private Language _language;

        [JsonProperty("language")]
        public Language Language
        {
            get { return _language; }
            set
            {

                SetProperty(ref _language, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
