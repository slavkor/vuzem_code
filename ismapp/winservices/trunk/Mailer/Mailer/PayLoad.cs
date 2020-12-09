using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer
{
    public class PayLoad
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
