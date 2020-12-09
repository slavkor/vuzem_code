using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Ism.Infrastructure.Model
{
    public class Contact : BaseModel
    {
        private string _name;
        private string _lastName;
        private string _phone;
        private string _mobilePhone;
        private string _organizationPhone;
        private string _organizationMobilePhone;
        private string _description;
        private string _email;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("lastname")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("phone")]
        public string Phone
        {
            get { return _phone; }
            set
            {
                SetProperty(ref _phone, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("mobilephone")]
        public string MobilePhone
        {
            get { return _mobilePhone; }
            set
            {
                SetProperty(ref _mobilePhone, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("organizationphone")]
        public string OrganizationPhone
        {
            get { return _organizationPhone; }
            set
            {
                SetProperty(ref _organizationPhone, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("organizationmobilephone")]
        public string OrganizationMobilePhone
        {
            get { return _organizationMobilePhone; }
            set
            {
                SetProperty(ref _organizationMobilePhone, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("email")]
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public override string ToString()
        {
            return $"{LastName} {Name}";
        }
    }
}
