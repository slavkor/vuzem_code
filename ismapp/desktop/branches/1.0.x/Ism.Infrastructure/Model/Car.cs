using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Car : BaseModel, ISelectionAware
    {
        private bool _isSelected;

        private string _model;
        private string _make;
        private string _registration;
        private int _seats;
        private bool _towhitch;
        private Company _company;

        [JsonProperty("make")]
        public string Make
        {
            get
            {
                return _make;
            }

            set
            {
                SetProperty(ref _make, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("model")]
        public string Model
        {
            get
            {
                return _model;
            }

            set
            {
                SetProperty(ref _model, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("registration")]
        public string Registration
        {
            get
            {
                return _registration;
            }

            set
            {
                SetProperty(ref _registration, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("seats")]
        public int Seats
        {
            get
            {
                return _seats;
            }

            set
            {
                SetProperty(ref _seats, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("towhitch")]
        public bool TowHitch
        {
            get
            {
                return _towhitch;
            }

            set
            {
                SetProperty(ref _towhitch, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public Company Company
        {
            get { return _company; }
            set
            {
                SetProperty(ref _company, value);
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

    }
}
