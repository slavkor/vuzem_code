using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class ApiServerException : Exception
    {
        [JsonProperty("error")]
        public string ServerError{ get; set; }
        [JsonProperty("message")]
        public string ServerMessage { get; set; }
        [JsonProperty("hint")]
        public string Hint { get; set; }

    }
}
