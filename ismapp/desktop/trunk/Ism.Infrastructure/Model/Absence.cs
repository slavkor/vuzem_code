using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Absence : BaseModel
    {
        private string description;
        [JsonProperty("description")]
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private DateTime from;
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("from")]
        public DateTime From
        {
            get { return from; }
            set
            {
                SetProperty(ref from, value);
                Duration = (int)(To-From).TotalDays;
            }
        }


        private DateTime to;
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("to")]
        public DateTime To
        {
            get { return to; }
            set
            {
                SetProperty(ref to, value);
                Duration = (int)(To - From).TotalDays;
            }
        }

        private int duration;
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public int Duration
        {
            get { return duration; }
            set { SetProperty(ref duration, value); }
        }

        private string type;
        [JsonProperty("type")]
        [Required]
        public string Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        
        //private AbsenceType atype;
        //[JsonIgnore]
        //public AbsenceType Atype
        //{
        //    get { return atype; }
        //    set
        //    {
        //        SetProperty(ref atype, value);
        //        Type = atype.Type;
        //    }
        //}


    }
}
