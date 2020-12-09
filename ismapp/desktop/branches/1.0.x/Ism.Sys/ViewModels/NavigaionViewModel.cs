using Ism.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Omu.ValueInjecter;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Extensions;
namespace Ism.Sys.ViewModels
{
    public class NavigaionViewModel : ViewModelBase
    {


        private IExceptionService _exceptionService;
        public NavigaionViewModel(IExceptionService exceptionService)
        {
            Items = new ObservableCollection<MenuEntry>();

            _eventAggregator.GetEvent<NavigationMenuEntryEvent>().Subscribe(OnNavigationMenuEntryEvent);
        }

        private ObservableCollection<MenuEntry> _items;
        
        public ObservableCollection<MenuEntry> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }


        private void OnNavigationMenuEntryEvent(NavigationMenuEntryEventArgs obj)
        {
            try
            {
                var entry = new MenuEntry(obj);

                if (entry.Parent == null)
                    Items.Add(entry);
                else
                {
                    if(Items.Where(i => i.Equals(entry.Parent)).FirstOrDefault() == null) Items.Add(entry.Parent);

                    Items.Where(i => i.Equals(entry.Parent)).FirstOrDefault()?.Items.Add(entry);
                }
                Items.Sort((x, y) => x.Importance.CompareTo(y.Importance));
            }

            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


    }



}
