using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class EmployeeEx : Employee
    {

        private DepartureState _departureState;

        [JsonProperty("departure", NullValueHandling= NullValueHandling.Ignore)]
        public DepartureState DepartureState
        {
            get { return _departureState; }
            set
            {
                SetProperty(ref _departureState, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }

    public class DepartureState: BaseModel
    {
        private int _state;
        private string _note;

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public int State
        {
            get { return _state; }
            set
            {
                SetProperty(ref _state, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string Note
        {
            get { return _note; }
            set
            {
                SetProperty(ref _note, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
