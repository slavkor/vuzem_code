using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Prism.Interactivity.InteractionRequest;

namespace Ism.ViewModels
{
    public class BusyIndicatorNotification: WindowAwareConfirmation
    {

        public event EventHandler CloseRequested;

        public BusyIndicatorNotification()
        {
            Title = "Please wait...";
        }
        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
       
        public override bool CanExecuteCloseCommand(object window)
        {
            return false;
        }
        public override bool CanExecuteMaximizeCommand(object window)
        {
            return false;
        }

        public override bool CanExecuteMinimizeCommand(object window)
        {
            return false;
        }

        public override bool CanExecuteRestoreCommand(object window)
        {
            return false;
        }
    }
}
