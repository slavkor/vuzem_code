using Ism.Infrastructure.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Events
{
    public class EmployeeEdited : PubSubEvent<Employee>
    {
    }
}
