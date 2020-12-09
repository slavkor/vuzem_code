using Ism.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Interaction
{
    public class EditChidlInteraction<T, TC>: EditInteraction<T>
    {
        public TC ChildInteractionObject { get; set; }

        public new Action<T,TC, EditMode> SaveAction { get; set; }
        public new Action<T> DeleteAction { get; set; }
        public new Action<T> RefreshAction { get; set; }


        public override void OnCloseCommandByEscape(object window)
        {
            OnCloseCommand(window);
        }

        public override void OnCloseCommand(object window)
        {
            SaveAction?.Invoke(InteractionObject, ChildInteractionObject, EditMode);
            RefreshAction?.Invoke(InteractionObject);
            base.OnCloseCommand(window);
        }

        public new Action<Action<List<T>>> DataProvider { get; set; }
    }
}
