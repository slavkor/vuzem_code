using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Interaction
{
    public class NavigationChildInteraction<T, TC> : WindowAwareConfirmation, INavigationInteraction
    {
        public string Header { get; set; }

        public EditChidlInteraction<T, TC> EditChildInteraction { get; set; }
    }
}
