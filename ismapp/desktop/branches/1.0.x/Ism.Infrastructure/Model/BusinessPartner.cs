using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class BusinessPartner: BaseModel
    {
        private IList<Address> _addresses;
        private IList<Document> _documents;
        private IList<Contact> _contacts;
        private string _name;
        private string _lastName;
        private string _shortname;
        [Required(ErrorMessage = "Obvezen vnos")]
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

        [JsonProperty("shortname")]
        public string ShortName
        {
            get { return _shortname; }
            set
            {
                SetProperty(ref _shortname, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("addresses", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty(ref _addresses, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("contacts", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                SetProperty(ref _contacts, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
