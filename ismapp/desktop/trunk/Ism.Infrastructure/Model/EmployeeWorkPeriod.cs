using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class EmployeeWorkPeriod : BaseModel
    {
        private IList<WorkPeriod> _workPeriods;
        private Employee _employee;

        [JsonProperty("employee", NullValueHandling = NullValueHandling.Ignore)]
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                SetProperty(ref _employee, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("periods", NullValueHandling = NullValueHandling.Ignore)]
        public IList<WorkPeriod> WorkPeriods
        {
            get { return _workPeriods; }
            set
            {
                SetProperty(ref _workPeriods, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
