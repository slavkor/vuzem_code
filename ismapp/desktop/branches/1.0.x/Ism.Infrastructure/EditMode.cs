using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure
{
    public enum EditMode
    {
        Undefined = -1,
        New = 0,
        Edit = 1,
        Delete = 2,
        List = 3,
        Extend = 4, 
        Cancel = 5, 
        Bind = 6

    }
}
