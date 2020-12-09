using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ism.Infrastructure.Model
{
    public class Company: BaseModel, IDepartureArrival
    {
        private string _name;
        private string _shortname;
        private string _taxNumber;
        private string _registrationNumber;
        private Document _logo;
        private string _logoPath;
        private BitmapImage _logoImage;

        private string _departureArrivalType;
        private Address _address;
        private string _color;


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

        [JsonProperty("regnumber")]
        public string RegistrationNumber
        {
            get { return _registrationNumber; }
            set
            {
                SetProperty(ref _registrationNumber, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("logo")]
        public Document Logo
        {
            get { return _logo; }
            set
            {
                SetProperty(ref _logo, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color
        {
            get { return _color; }
            set
            {
                SetProperty(ref _color, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public string LogoPath
        {
            get { return _logoPath; }
            set
            {
                SetProperty(ref _logoPath, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public BitmapImage LogoImage
        {
            get { return _logoImage; }
            set
            {
                SetProperty(ref _logoImage, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        #region IDepartureArrival
        public string DepartureArrivalType
        {
            get
            {
                return "COMPANY";
            }

            set
            {
                //SetProperty(ref _departureArrivalType, value);
                //PropertyDeletegate?.Invoke(this);
            }
        }

        public Address Address
        {
            get
            {
                return _address;
            }

            set
            {
                SetProperty(ref _address, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public string PointName
        {
            get
            {
                return Name;
            }
        }

        public bool IsOrigin
        {
            get
            {
                return false;
            }

            set
            {
                ;
            }
        }

        #endregion


        public override string ToString()
        {
            return ShortName;
        }
    }
}
