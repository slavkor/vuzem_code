using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class ShiftsInternal : BaseModel, ISelectionAware
    {
        private Employee _employee;
        private Shift _shiftday;
        private Shift _shiftnight;
        private bool _isSelected;
        private List<Shift> _shifts;

        public Employee Employee
        {
            get { return _employee; }
            set
            {

                SetProperty(ref _employee, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty(@"shifts")]
        public List<Shift> Shifts
        {
            get { return _shifts; }
            set
            {

                SetProperty(ref _shifts, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonIgnore]

        public Shift DayShift
        {
            get { return _shiftday; }
            set
            {

                SetProperty(ref _shiftday, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]

        public Shift NightShift
        {
            get { return _shiftnight; }
            set
            {

                SetProperty(ref _shiftnight, value);
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
