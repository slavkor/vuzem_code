using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Ism.Infrastructure.Events
{
    public class ConfirmSaveEvent<T>: PubSubEvent<ConfirmSaveEventArgs<T>>
    {
    }
}
