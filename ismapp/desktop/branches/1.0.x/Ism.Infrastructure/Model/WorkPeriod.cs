using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ism.Infrastructure.Validation;

namespace Ism.Infrastructure.Model
{
    public class WorkPeriod: BaseModel
    {
        private DateTime? _start;
        private DateTime? _end;
        private Company _company;

        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Start
        {
            get { return _start; }
            set
            {
                SetProperty(ref _start, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? End
        {
            get { return _end; }
            set
            {
                SetProperty(ref _end, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public Company Company
        {
            get { return _company; }
            set
            {
                SetProperty(ref _company, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


    }
}
