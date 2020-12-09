using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Employees.Commands
{
    public interface IEmployeeCommands
    {
        CompositeCommand SaveCommand { get; }
    }
}
