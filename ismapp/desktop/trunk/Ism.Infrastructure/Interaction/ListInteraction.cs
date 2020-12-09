using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;

namespace Ism.Infrastructure.Interaction
{
    public class ListInteraction<T> : WindowAwareConfirmation, IInteraction<T>
    {
        public T InteractionObject { get; set; }
        public Action<T> SelectAction { get; set; }
        public Action<List<T>> SelectManyAction { get; set; }
        public ListEventArgs<T> ListEventArgs { get; set; }
    }
}
