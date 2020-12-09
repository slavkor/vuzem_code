using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Interaction
{
    public class NavigationInteraction<T> : INavigationInteraction
    {
        public string Header { get; set; }
        public EditInteraction<T> EditInteraction { get; set; }
    }
}
