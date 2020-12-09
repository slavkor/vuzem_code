using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class User : BaseModel
    {
        private Token _accessToken;
        private string _userName;
        private string _password;
        private IList<Scope> _scopes;

        [JsonProperty("username")]
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("password")]
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("scopes")]
        public IList<Scope> Scopes
        {
            get { return _scopes; }
            set
            {
                SetProperty(ref _scopes, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        public Token AccessToken
        {
            get { return _accessToken; }
            set
            {
                SetProperty(ref _accessToken, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
