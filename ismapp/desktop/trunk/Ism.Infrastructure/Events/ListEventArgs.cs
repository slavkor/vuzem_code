using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Events
{
    public class ListEventArgs<T>
    {
        public Action<Action<List<T>>> DataProvider { get; set; }
        public Action<T> SelectAction { get; set; }
        public Action<List<T>> SelectManyAction { get; set; }
        public bool RememberSelection { get; set; }
        public bool ForceListSelection { get; set; }
    }
}
