using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ism.Infrastructure.Validation;


namespace Ism.Infrastructure.Model
{
    public class WorkPlan: BaseModel
    {
        private WorkPlace _workPlace;
        private int _plan;


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

        [JsonProperty("plan", NullValueHandling = NullValueHandling.Ignore)]
        public int Plan
        {
            get { return _plan; }
            set
            {
                SetProperty(ref _plan, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

    }
}
