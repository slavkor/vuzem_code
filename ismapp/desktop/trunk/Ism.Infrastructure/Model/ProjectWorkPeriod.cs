using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ism.Infrastructure.Validation;
using System.Collections.ObjectModel;

namespace Ism.Infrastructure.Model
{
    public class ProjectWorkPeriod: BaseModel, ISelectionAware
    {
        private DateTime _start;
        private DateTime _end;
        bool _selected;
        private ObservableCollection<WorkPlan> _workPlan;
        private string _description;

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Start
        {
            get { return _start; }
            set
            {
                SetProperty(ref _start, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("workplans", NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<WorkPlan> WorkPlans
        {
            get { return _workPlan; }
            set
            {
                SetProperty(ref _workPlan, value);
                PropertyDeletegate?.Invoke(this);
                Plan = _workPlan?.Sum(p =>p.Plan);
            }
        }

        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyyMMdd" })]
        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime End
        {
            get { return _end; }
            set
            {
                SetProperty(ref _end, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
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
        private int? _plan;
        
        [JsonIgnore]
        public int? Plan
        {
            get { return _plan; }
            set { SetProperty(ref _plan, value); }
        }

    }
}
