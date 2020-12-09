using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Interaction
{
    public class EditInteraction<T> : ListInteraction<T>
    {
        public Action<T, EditMode> SaveAction { get; set; }
        public Action<T> DeleteAction { get; set; }
        public Action<T> RefreshAction { get; set; }
        public EditMode EditMode { get; set; }

        public EditEventArgs<T> EditEventArgs { get; set; }

        public override void OnCloseCommandByEscape(object window)
        {
            OnCloseCommand(window);
        }

        public override void OnCloseCommand(object window)
        {
            SaveAction?.Invoke(InteractionObject, EditMode);
            RefreshAction?.Invoke(InteractionObject);
            base.OnCloseCommand(window);
        }

        public Action<Action<List<T>>> DataProvider { get; set; }
    }


    public class EditDocumentInteraction : EditInteraction<Document>
    {
        public Action<Action<List<DocumentType>>> DocumentTypesProvider { get; set; }
    }
}
