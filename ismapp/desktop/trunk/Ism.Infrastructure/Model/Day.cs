using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Day : BaseModel
    {

        private int _value;
        private Month _month;
        private string _strRep;
        private DateTime _date;
        public Day() : this(DateTime.Now)
        {

        }

        public Day(DateTime date)
        {
            if (date == null)
                date = DateTime.Now;

            Value = date.Day;
            Month = new Month() { Value = date.Month, Year = new Year() { Value = date.Year } };
        }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("strrep", NullValueHandling=NullValueHandling.Ignore)]
        public string StrRep
        {
            get { return _strRep; }
            set {
                SetProperty(ref _strRep, value);

                if (_strRep.Length == 12)
                    Date = DateTime.ParseExact(_strRep, "yyyyMMddHHmmss", null);
                if (_strRep.Length == 6)
                    Date = DateTime.ParseExact(_strRep, "yyyyMMdd", null);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("month", NullValueHandling = NullValueHandling.Ignore)]
        public Month Month
        {
            get { return _month; }
            set {
                SetProperty(ref _month, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public DateTime Date
        {
            get
            {
                if(_date == DateTime.MinValue)
                    return new DateTime(Month.Year.Value, Month.Value, Value); 
                return _date; 
            }
            set {
                SetProperty(ref _date, value);
                //Value = value.Day;
                //Month = new Month() { Value = value.Month, Year = new Year() { Value = value.Year } };
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public static Day Now { get { return new Day(DateTime.Now); } }
    }
}
