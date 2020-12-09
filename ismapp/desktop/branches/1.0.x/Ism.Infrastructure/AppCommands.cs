using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure
{
    public class AppCommands: IAppCommands
    {
        private CompositeCommand loginCommand = new CompositeCommand(true);
        private CompositeCommand logoutCommand = new CompositeCommand(true);
        private CompositeCommand saveCommand = new CompositeCommand(true);

        public CompositeCommand LogInCommand
        {
            get
            {
                return loginCommand;
            }
        }

        public CompositeCommand LogOutCommand
        {
            get
            {
                return logoutCommand;
            }
        }

        public CompositeCommand SaveCommand
        {
            get
            {
                return saveCommand;
            }
        }
    }
}
