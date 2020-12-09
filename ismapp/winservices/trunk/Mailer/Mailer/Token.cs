using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailer
{
    public class Token 
    {
        private string accessToken;
        private int expiresIn;
        private string tokenType;
        private string refreshToken;

        [JsonProperty("access_token")]
        public string AccessToken
        {
            get { return accessToken; }
            set
            {
                accessToken = value;
                JwtToken = new JwtSecurityToken(accessToken);

            }
        }

        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get { return expiresIn; }
            set
            {
                expiresIn = value;
            }
        }


        [JsonProperty("token_type")]
        public string Type
        {
            get { return tokenType; }
            set
            {
                tokenType = value;


            }
        }


        [JsonProperty("refresh_token")]
        public string RefreshToken
        {
            get { return refreshToken; }
            set
            {
                refreshToken = value;
            }
        }

        [JsonIgnore]
        public JwtSecurityToken JwtToken { get; set; }

        public string GetTokenId()
        {
            return JwtToken?.Claims.Where(c => c.Type == "jti").Select(c => c.Value).FirstOrDefault();
        }

        public string GetClaim(string claim)
        {
            return JwtToken?.Claims.Where(c => c.Type == claim).Select(c => c.Value).FirstOrDefault();
        }

    }
}
