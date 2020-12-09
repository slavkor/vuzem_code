using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Ism.Infrastructure.Validation;
using Newtonsoft.Json;
namespace Ism.Infrastructure.Model
{
    
    public class Employee : BaseModel, ISelectionAware
    {
        private string _name;
        private string _lastName;
        private string _nickName;
        private DateTime _birthDay;
        private string _birthPlace;
        private string _emso;
        private string _taxNumber;
        private IList<Address>_addresses;
        private IList<Contact> _contacts;
        private IList<Document> _documents;
        private string _position;
        private string _iban;
        private string _personalIdNumber;

        private string _pictureUri;
        private string _insunumber;
        private string _email;
        private string _martialStatus;
        private string _nationality;
        private string _username;

        private bool _isSelected;
        private WorkPlace _workPlace;

        public Employee()
        {
            PictureUri = "/Ism.Infrastructure;component/Images/no-image.png";
        }

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
        [JsonProperty("nickname")]
        public string NickName
        {
            get { return _nickName; }
            set
            {
                SetProperty(ref _nickName, value);
                
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BirthDay
        {
            get { return _birthDay; }
            set
            {
                SetProperty(ref _birthDay, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("birthplace")]
        public string BirthPlace
        {
            get { return _birthPlace; }
            set
            {
                SetProperty(ref _birthPlace, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("emso")]
        public string Emso
        {
            get { return _emso; }
            set
            {
                SetProperty(ref _emso, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("taxnumber")]
        public string TaxNumber
        {
            get { return _taxNumber; }
            set
            {
                SetProperty(ref _taxNumber, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("personalidnumber")]
        public string PersonalIdNumber
        {
            get { return _personalIdNumber; }
            set
            {
                SetProperty(ref _personalIdNumber, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("iban")]
        public string Iban
        {
            get { return _iban; }
            set
            {
                SetProperty(ref _iban, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("position")]
        public string Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("insunumber")]
        public string InsuranceNumber
        {
            get { return _insunumber; }
            set
            {
                SetProperty(ref _insunumber, value);
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
        [JsonProperty("martialstatus")]
        public string MartialStatus
        {
            get { return _martialStatus; }
            set
            {
                SetProperty(ref _martialStatus, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("nationality")]
        public string Nationality
        {
            get { return _nationality; }
            set
            {
                SetProperty(ref _nationality, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("addresses", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty( ref _addresses, value);
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

        private BusinessPartner _loaner;
        [JsonProperty("loaner", NullValueHandling = NullValueHandling.Ignore)]
        public BusinessPartner Loaner
        {
            get
            {
                return _loaner;
            }
            set
            {
                SetProperty(ref _loaner, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public Company Employer { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("workplace", NullValueHandling = NullValueHandling.Ignore)]
        public WorkPlace WorkPlace
        {
            get { return _workPlace; }
            set
            {
                SetProperty(ref _workPlace, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        //private List<Occupancy> _occupancies;
        //[JsonProperty("occupancies", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Occupancy> Occupancies
        //{
        //    get { return _occupancies; }
        //    set
        //    {
        //        SetProperty(ref _occupancies, value);
        //        PropertyDeletegate?.Invoke(this);
        //    }
        //}


        [JsonIgnore]
        public string PictureUri
        {
            get { return _pictureUri; }
            set
            {
                SetProperty(ref _pictureUri, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                SetProperty(ref _isSelected, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        //[JsonIgnore]
        //public Project Project => Occupancies?.Where(o => o.Active == 1).FirstOrDefault().Project;
        public override string ToString()
        {
            return null == NickName ? $"{LastName} {Name}" : $"{LastName} {Name} aka {NickName}";
        }
    }
}
