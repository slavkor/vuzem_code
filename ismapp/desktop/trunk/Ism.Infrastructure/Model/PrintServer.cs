using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class PrintServer: BaseModel
    {
        private Uri _serverUri;
        private string _user;
        private string _password;

        public Uri ServerUri
        {
            get { return _serverUri; }
            set
            {
                SetProperty(ref _serverUri ,value); 
                PropertyDeletegate?.Invoke(this);
            }
        }

        public string User
        {
            get { return _user; }
            set
            {
                SetProperty( ref _user ,value);
                PropertyDeletegate?.Invoke(this);

            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty( ref _password ,value);
                PropertyDeletegate?.Invoke(this);

            }
        }
    }
}
