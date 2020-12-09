using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Ism.ViewModels
{
    public class ExceptionViewModel : BindableBase, IInteractionRequestAware
    {

        public ExceptionViewModel()
        {

        }

        private ExceptionNotification _notification;
        private Exception _exception;

        public Action FinishInteraction
        {
            get;
            set;
        }

        public Exception Exception
        {
            get { return _exception; }
            set
            {
                SetProperty(ref _exception, value);
            }
        }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is ExceptionNotification)) return;

                _notification = (ExceptionNotification)value;
                Exception = _notification.Exception;
                _notification.CloseRequested += _notification_CloseRequested;
                RaisePropertyChanged(nameof(Notification));
            }
        }

        private void _notification_CloseRequested(object sender, EventArgs e)
        {
            FinishInteraction?.Invoke();
            _notification.CloseRequested -= _notification_CloseRequested;
        }
    }
}
