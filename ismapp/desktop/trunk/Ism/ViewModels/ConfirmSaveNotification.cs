using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;

namespace Ism.ViewModels
{
    public class ConfirmSaveNotification<T> : WindowAwareConfirmation
    {
        public  ConfirmSaveEventArgs<T> EventArgs { get; set; }
    }
}
