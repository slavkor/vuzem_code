using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure
{
    public class ListViewNotification<T> : WindowAwareConfirmation
    {
        public Action<T> ListViewNotificationCallback { get; set; }
        public T Data { get; set; }

        public Uri ApiAddress { get; set; }
    }
}
