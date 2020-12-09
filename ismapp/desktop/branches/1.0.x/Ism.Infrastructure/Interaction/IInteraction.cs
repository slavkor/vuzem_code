using Ism.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Interaction
{
    public interface IInteraction<T>
    {
        T InteractionObject { get; set; }
        Action<T> SelectAction { get; set; }
        Action<List<T>> SelectManyAction { get; set; }
        ListEventArgs<T> ListEventArgs { get; set; }
    }
}
