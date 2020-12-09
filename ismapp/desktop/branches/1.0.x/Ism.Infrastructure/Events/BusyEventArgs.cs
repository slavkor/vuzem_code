using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Ism.Infrastructure.Events
{
    public class BusyEventArgs : BindableBase
    {
        private bool _busy;
        private string _notification;

        public BusyEventArgs()
        {
            Busy = false;
            Notification = string.Empty;
        }

        public bool Busy
        {
            get { return _busy; }
            set { SetProperty(ref _busy, value); }
        }

        public string Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value) ; }
        }
    }
}
