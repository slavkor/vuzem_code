using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Events
{
    public class SelectedEventArgs<T> 
        where T: class 

    {
    public SelectedEventArgs() : this(null)
    {

    }

    public SelectedEventArgs(T payload)
    {
        SelectedData = payload;
    }

    public T SelectedData { get; set; }
    }
}
