using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public interface ISelectionAware
    {
        [JsonIgnore]
        bool IsSelected { get; set; }
    }
}
