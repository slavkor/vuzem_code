using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Events
{
    public class EditEventArgs<T>
    {
        public T EditObject { get; set; }
        public Action<T, EditMode> SaveAction { get; set; }
        public Action<T> RefreshAction { get; set; }
        public EditMode EditMode { get; set; }
    }

    public class EditDocumentEventArgs : EditEventArgs<Document>
    {
        public Action<Action<List<DocumentType>>> DocumentTypesProvider { get; set; }

    }
}
