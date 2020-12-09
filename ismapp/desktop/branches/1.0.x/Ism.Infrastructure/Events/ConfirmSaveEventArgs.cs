using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Events
{
    public class ConfirmSaveEventArgs<T>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool FinishUp { get; set; }
        public Action<bool, ConfirmSaveEventArgs<T>> CallBackAction { get; set; }

        public Action<bool, ConfirmSaveEventArgs<T>> AfterSaveAction { get; set; }
        public T PayLoad { get; set; }

        public EditMode EditMode { get; set; }

    }
}
