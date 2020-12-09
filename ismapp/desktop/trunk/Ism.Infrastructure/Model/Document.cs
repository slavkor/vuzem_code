using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class Document : BaseModel, ISelectionAware
    {
        private DocumentType _type;
        private string _name;
        private Day _validFrom;
        private Day _validTo;
        private Day _docDate;
        private string _description;
        private IList<File> _files;
        private string _documentnumber;
        private Document _next;
        private bool _isSelected;
        private double _amount;
        private double _amountwot;
        private double _amountwt;
        private DateTime? _expireDate;

        public Document()
        {
            UuId = Guid.NewGuid().ToString();
        }

        [JsonProperty(PropertyName = "type")]
        public DocumentType Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type,value);
                PropertyDeletegate?.Invoke(this);
            }
        }

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

        [JsonProperty(PropertyName = "validfrom", NullValueHandling = NullValueHandling.Ignore)]
        public Day ValidFrom
        {
            get { return _validFrom; }
            set
            {
                SetProperty(ref _validFrom, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "validto", NullValueHandling = NullValueHandling.Ignore)]
        public Day ValidTo
        {
            get { return _validTo; }
            set
            {
                SetProperty(ref _validTo, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "expiredate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpireDate
        {
            get { return _expireDate; }
            set
            {
                SetProperty(ref _expireDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "docdate", NullValueHandling = NullValueHandling.Ignore)]
        public Day DocDate
        {
            get
            {
                if (null == _docDate) DocDate = Day.Now;
                return _docDate; //?? Day.Now;
            }
            set
            {
                SetProperty(ref _docDate, value);
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


        [JsonProperty(PropertyName = "documentnumber", NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentNumber
        {
            get { return _documentnumber; }
            set
            {
                SetProperty(ref _documentnumber, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "amount", NullValueHandling = NullValueHandling.Ignore)]
        public double Amount
        {
            get { return _amount; }
            set
            {
                SetProperty(ref _amount, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "amountwot", NullValueHandling = NullValueHandling.Ignore)]
        public double AmountWoT
        {
            get { return _amountwot; }
            set
            {
                SetProperty(ref _amountwot, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "amountwt", NullValueHandling = NullValueHandling.Ignore)]
        public double AmountWT
        {
            get { return _amountwt; }
            set
            {
                SetProperty(ref _amountwt, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "next", NullValueHandling = NullValueHandling.Ignore)]
        public Document Next
        {
            get { return _next; }
            set
            {
                SetProperty(ref _next, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "files")]
        public IList<File> Files
        {
            get { return _files; }
            set
            {
                SetProperty(ref _files, value);
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

        public override string ToString()
        {
            return $"{Name} ({Type?.Name})";
        }
    }
}
