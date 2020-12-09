using Itenso.TimePeriod;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class WorkHistory : BaseModel
    {
        private string destination;
        [JsonProperty("destination")]
        public string Destination
        {
            get { return destination; }
            set { SetProperty(ref destination, value); }
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
            }
        }

        [JsonIgnore]
        public string Duration
        {
            get
            {
                DateDiff dateDiff = new DateDiff(from, to);
                return $"{dateDiff.ElapsedYears} let {dateDiff.ElapsedMonths} mesecv {dateDiff.ElapsedDays} dni";
            }
        }
    }
}
