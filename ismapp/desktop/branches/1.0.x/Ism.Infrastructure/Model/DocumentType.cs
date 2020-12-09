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


    }
}