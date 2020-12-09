using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace Ism.Employees.Commands
{
    public class EmployeeCommands : IEmployeeCommands
    {

        private CompositeCommand _saveCommand = new CompositeCommand();

        public CompositeCommand SaveCommand
        {
            get
            {
                return _saveCommand;
            }
        }
    }
}
