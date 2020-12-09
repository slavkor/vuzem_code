using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Sys.Services
{
    public class NavigationService : INavigationService
    {
        private List<MenuEntry> _paretns;

        public NavigationService()
        {
            _paretns = new List<MenuEntry>();

            _paretns.Add(new MenuEntry() { Title = "Sistem", Importance = 99 });
            _paretns.Add(new MenuEntry() { Title = "Zaposleni", Importance = 5 });
        }


        public IEnumerable<MenuEntry> GetParents()
        {
            return _paretns;
        }
    }
}
