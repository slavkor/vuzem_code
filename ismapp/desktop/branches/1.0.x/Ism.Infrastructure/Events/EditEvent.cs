using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Events
{
    public class EditEvent<T> : PubSubEvent<EditEventArgs<T>>
    {
    }

    public class EditDocumentEvent : PubSubEvent<EditDocumentEventArgs>
    {
    }
}
