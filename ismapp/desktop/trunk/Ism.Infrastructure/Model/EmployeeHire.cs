using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class EmployeeHire : Employee
    {
        private Company _hireEmployer;
        private DateTime _hireDate;

        [JsonProperty("hireemployer")]
        public Company HireEmployer
        {
            get { return _hireEmployer; }
            set
            {
                SetProperty(ref _hireEmployer, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("hiredate")]
        public DateTime HireDate
        {
            get { return _hireDate; }
            set
            {
                SetProperty(ref _hireDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
