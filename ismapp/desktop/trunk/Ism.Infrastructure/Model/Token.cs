using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Token : BaseModel
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
                SetProperty(ref accessToken, value);
                PropertyDeletegate?.Invoke(this);
                JwtToken = new JwtSecurityToken(accessToken);
 
            }
        }

        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get { return expiresIn; }
            set
            {
                SetProperty(ref expiresIn, value); PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("token_type")]
        public string Type
        {
            get { return tokenType; }
            set
            {
                SetProperty(ref tokenType, value); PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("refresh_token")]
        public string RefreshToken
        {
            get { return refreshToken; }
            set
            {
                SetProperty(ref refreshToken, value); PropertyDeletegate?.Invoke(this);
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

        [JsonIgnore]
        public IList<Scope> Scopes
        {
            get {
                IList<Scope> scopes = new List<Scope>();
                try
                {
                    foreach (var claim in JwtToken?.Claims.Where(c => c.Type == "scopes").Select(c => c))
                    {
                        //scopes.Add(JsonConvert.DeserializeObject<Scope>(claim.Value));
                        scopes.Add(new Scope() { Identifier = claim.Value });
                    }
                }
                catch (Exception)
                {
                    
                    scopes.Add(new Scope() { Identifier = "admin" });
                }
                return scopes;
            }
        }
    }
}
