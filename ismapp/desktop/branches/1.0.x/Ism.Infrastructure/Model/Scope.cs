using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Ism.Infrastructure.Model
{
    public class Scope : BaseModel, ISelectionAware
    {
        private bool _selected;
        private string _identifier;
        private string _description;


        [JsonProperty("identifier")]
        public string Identifier
        {
            get { return _identifier; }
            set
            {
                SetProperty(ref _identifier, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
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
    }
}
