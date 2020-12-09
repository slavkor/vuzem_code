using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class WokForceStat : BaseModel
    {
        private WorkPlace _workPlace;
        private int _count;

        [JsonProperty("workplace", NullValueHandling = NullValueHandling.Ignore)]
        public WorkPlace WorkPlace
        {
            get { return _workPlace; }
            set
            {
                SetProperty(ref _workPlace, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count
        {
            get { return _count; }
            set
            {
                SetProperty(ref _count, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
