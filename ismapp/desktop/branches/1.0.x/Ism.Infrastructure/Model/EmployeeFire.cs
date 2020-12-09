using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class EmployeeFire: EmployeeHire
    {
        private Company _fireEmployer;
        private DateTime? _fireDate;

        [JsonProperty("fireemployer")]
        public Company FireEmployer
        {
            get { return _fireEmployer; }
            set
            {
                SetProperty(ref _fireEmployer, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("firedate")]
        public DateTime? FireDate
        {
            get { return _fireDate; }
            set
            {
                SetProperty(ref _fireDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
