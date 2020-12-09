using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;

namespace Ism.ViewModels
{
    public class ExceptionNotification: WindowAwareConfirmation
    {
        public event EventHandler CloseRequested;
        public Exception Exception { get; set; }

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

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
