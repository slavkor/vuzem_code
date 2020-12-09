using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class CurrentWorkPeriod: BaseModel
    {
        private WorkPeriod _current, _last;

        [JsonProperty("current", NullValueHandling = NullValueHandling.Ignore)]
        public WorkPeriod Current
        {
            get { return _current; }
            set
            {
                SetProperty(ref _current, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("last", NullValueHandling = NullValueHandling.Ignore)]
        public WorkPeriod Last
        {
            get { return _last; }
            set
            {
                SetProperty(ref _last, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public bool LastWorkedInCompany(Company company)
        {
            return company.ShortName.Equals(_current == null ? _last == null ? Guid.NewGuid().ToString() : _last.Company.ShortName : _current.Company.ShortName);
        }
    }
}
