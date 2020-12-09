using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Ism.Infrastructure.Model
{
    public class Shift: BaseModel
    {
        private int _hours;
        private int _type;
        private Day _day;

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


        [JsonProperty(@"workday", NullValueHandling =NullValueHandling.Ignore)]
        public Day WorkDay
        {
            get { return _day; }
            set
            {
                SetProperty(ref _day, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


    }
}
