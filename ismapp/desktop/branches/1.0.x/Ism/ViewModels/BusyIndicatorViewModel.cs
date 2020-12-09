using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Ism.ViewModels
{
    public class BusyIndicatorViewModel : BindableBase, IInteractionRequestAware
    {
        private BusyIndicatorNotification _notification;

        public Action FinishInteraction
        {
            get;
            set;
        }

        public INotification Notification
        {
            get { return _notification; }

            set
            {
                if (value is BusyIndicatorNotification)
                {
                    _notification = (BusyIndicatorNotification)value;
                    _notification.CloseRequested += _notification_CloseRequested;
                    RaisePropertyChanged(nameof(Notification));
                }
            }
        }

        private void _notification_CloseRequested(object sender, EventArgs e)
        {
            FinishInteraction();
            _notification.CloseRequested -= _notification_CloseRequested;
        }
    }
}
