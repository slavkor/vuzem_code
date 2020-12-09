using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ism.Infrastructure.Events
{
    public class NavigationMenuEntryEventArgs
    {
        public int Importance { get; set; }
        public string Title { get; set; }
        public ICommand Command { get; set; }
        public MenuEntry Parent{ get; set; }
        public string ContentSouce { get; set; }
    }
}
