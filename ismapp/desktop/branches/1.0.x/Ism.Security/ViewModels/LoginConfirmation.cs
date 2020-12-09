using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ism.Infrastructure;

namespace Ism.Security.ViewModels
{
    public class LoginConfirmation : WindowAwareConfirmation
    {
        public string UserName { get; set; }
        //public SecureString Password { get; set; }
        public string Password { get; set; }

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
