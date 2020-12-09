using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class WorkPlace : BaseModel, ISelectionAware
    {
        string _code, _workPlace, _deworkPlace, _enworkPlace;
        bool _selected;

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code
        {
            get { return _code; }
            set
            {
                SetProperty(ref _code, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("workplace", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkPlaceName
        {
            get { return _workPlace; }
            set
            {
                SetProperty(ref _workPlace, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("deworkplace", NullValueHandling = NullValueHandling.Ignore)]
        public string DeWorkPlaceName
        {
            get { return _deworkPlace; }
            set
            {
                SetProperty(ref _deworkPlace, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("enworkplace", NullValueHandling = NullValueHandling.Ignore)]
        public string EnWorkPlaceName
        {
            get { return _enworkPlace; }
            set
            {
                SetProperty(ref _enworkPlace, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public bool IsSelected
        {
            get
            {
                return _selected;
            }

            set
            {
                SetProperty(ref _selected, value);
                PropertyDeletegate?.Invoke(this); 
            }
        }

        public override string ToString()
        {
            return WorkPlaceName;
        }
    }
}
