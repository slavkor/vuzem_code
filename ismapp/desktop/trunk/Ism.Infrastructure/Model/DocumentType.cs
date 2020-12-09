using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class DocumentType : BaseModel
    {
        private string _name;
        private string _description;
        private string _notifictionMail;
        private bool _expirable;
        private bool _numbered;
        private bool _csitebound;
        private bool _countrybound;
        private bool _companybound;
        private bool _ewrbound;
        private bool _csiteprint;
        private bool _hqglobal;
        private bool _credit;
        private bool _debet;
        private bool _prolongable;
        private bool _promissorynote;

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "notificationmail", NullValueHandling = NullValueHandling.Ignore)]
        public string NotificationMail
        {
            get { return _notifictionMail; }
            set
            {
                SetProperty(ref _notifictionMail, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "expirable", NullValueHandling = NullValueHandling.Ignore)]
        public bool Expirable
        {
            get { return _expirable; }
            set
            {
                SetProperty(ref _expirable, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "numbered", NullValueHandling = NullValueHandling.Ignore)]
        public bool Numbered
        {
            get { return _numbered; }
            set
            {
                SetProperty(ref _numbered, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "csitebound", NullValueHandling = NullValueHandling.Ignore)]
        public bool CsiteBound
        {
            get { return _csitebound; }
            set
            {
                SetProperty(ref _csitebound, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "countrybound", NullValueHandling = NullValueHandling.Ignore)]
        public bool CountryBound
        {
            get { return _countrybound; }
            set
            {
                SetProperty(ref _countrybound, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "companybound", NullValueHandling = NullValueHandling.Ignore)]
        public bool CompanyBound
        {
            get { return _companybound; }
            set
            {
                SetProperty(ref _companybound, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "ewrbound", NullValueHandling = NullValueHandling.Ignore)]
        public bool EwrBound
        {
            get { return _ewrbound; }
            set
            {
                SetProperty(ref _ewrbound, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "csiteprint", NullValueHandling = NullValueHandling.Ignore)]
        public bool CsitePrint
        {
            get { return _csiteprint; }
            set
            {
                SetProperty(ref _csiteprint, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "hqglobal", NullValueHandling = NullValueHandling.Ignore)]
        public bool HqGlobal
        {
            get { return _hqglobal; }
            set
            {
                SetProperty(ref _hqglobal, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "credit", NullValueHandling = NullValueHandling.Ignore)]
        public bool Credit
        {
            get { return _credit; }
            set
            {
                SetProperty(ref _credit, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "debet", NullValueHandling = NullValueHandling.Ignore)]
        public bool Debet
        {
            get { return _debet; }
            set
            {
                SetProperty(ref _debet, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "prolongable", NullValueHandling = NullValueHandling.Ignore)]
        public bool Prolongable
        {
            get { return _prolongable; }
            set
            {
                SetProperty(ref _prolongable, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "promissorynote", NullValueHandling = NullValueHandling.Ignore)]
        public bool PromissoryNote
        {
            get { return _promissorynote; }
            set
            {
                SetProperty(ref _promissorynote, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


    }
}