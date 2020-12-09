using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class ShiftDay : BaseModel
    {
        private int _hours;
        private int _type;
        private DateTime _date;

        [JsonProperty(@"shifttype")]
        public int ShiftType
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(@"hours")]
        public int Hours
        {
            get { return _hours; }
            set
            {
                SetProperty(ref _hours, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonConverter(typeof(DateTimeConverter), "yyyyMMdd")]
        [JsonProperty("strrep", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date
        {
            get { return _date; }
            set
            {
                SetProperty(ref _date, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

    }
}
