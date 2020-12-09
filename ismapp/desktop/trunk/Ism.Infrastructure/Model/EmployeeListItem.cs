using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class EmployeeListItem
    {
        public EmployeeEx Employee { get; set; }

        public DelegateCommand AddEmployee { get; set; }
        public DelegateCommand<EmployeeListItem> RemoveEmployee { get; set; }
        public DelegateCommand<EmployeeListItem> StateChangeCommand { get; set; }
        public DelegateCommand<EmployeeListItem> DriverCommand { get; set; }
        public DelegateCommand<EmployeeListItem> PassengerCommand { get; set; }
        public bool IsAddItem { get; set; }
    }
}
