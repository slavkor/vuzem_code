using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Month : BaseModel
    {
        private int _value;
        private Year _year;
        private string _strRep;

        [JsonProperty("value")]
        public int Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("strrep")]
        public string StrRep
        {
            get { return _strRep; }
            set
            {
                SetProperty(ref _strRep, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("year")]
        public Year Year
        {
            get { return _year; }
            set
            {
                SetProperty(ref _year, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
