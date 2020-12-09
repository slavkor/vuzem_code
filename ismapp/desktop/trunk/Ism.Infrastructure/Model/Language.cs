using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class Language : BaseModel, ISelectionAware
    {
        private string _language;
        private string _alpha2;
        private string _alpha3;
        private bool _isSelected;

        [JsonProperty("language")]
        public string Lang
        {
            get { return _language; }
            set
            {
                SetProperty(ref _language, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("code2")]
        public string Alpha2
        {
            get { return _alpha2; }
            set
            {
                SetProperty(ref _alpha2, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("code3")]
        public string Alpha3
        {
            get { return _alpha3; }
            set
            {
                SetProperty(ref _alpha3, value);
                PropertyDeletegate?.Invoke(this);
            }
        }



        //[JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        /*
    language	"Slovenian"
    code2	"sl"
    code3	"slv"
    code	null
    lcid_dec	1060
    lcid_hex	"424"
    lcid_str	"sl"
    locale	"Slovenian"
    codepage	"1250"        
            */
    }
}
