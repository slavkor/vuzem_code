﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;
using Prism.Events;

namespace Ism.Infrastructure.Events
{
    public class EmployeeEditEvent : PubSubEvent<Employee>
    {
    }
}
