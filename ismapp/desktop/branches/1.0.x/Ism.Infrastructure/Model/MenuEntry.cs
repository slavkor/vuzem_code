using Ism.Infrastructure.Events;
using Ism.Infrastructure.Mvvm;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ism.Infrastructure.Model
{
    public class MenuEntry : ViewModelBase
    {
        public int Importance { get; set; }
        public ICommand Command { get; set; }
        public string Title { get; set; }
        public MenuEntry Parent { get; set; }
        private ObservableCollection<MenuEntry> _items;

        public MenuEntry() : this(null) { Items = new ObservableCollection<MenuEntry>(); }

        public MenuEntry(NavigationMenuEntryEventArgs menuEntryArgs)
        {
            if (menuEntryArgs == null) return;
            this.InjectFrom(menuEntryArgs);


        }

        public ObservableCollection<MenuEntry> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
    }
}
