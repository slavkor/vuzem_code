using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Events
{
    public class EditChildEventArgs<T, TC>: EditEventArgs<T>
    {
        public TC EditChildObject { get; set; }
        public Action<TC, EditMode> SaveChildAction { get; set; }
        public EditMode EditChildMode { get; set; }
    }
}
