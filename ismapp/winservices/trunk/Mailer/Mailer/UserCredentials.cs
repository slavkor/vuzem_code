using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer
{
    public class UserCredentials
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        [JsonProperty("client_id")]
        public string ClinetId { get; set; }
        [JsonProperty("client_secret")]
        public string ClinetSecret { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string PassWord { get; set; }
    }
}
