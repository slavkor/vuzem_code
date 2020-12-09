using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Model;

namespace Ism.Document.ViewModels
{
    public  class DocumentTypeInteraction: WindowAwareConfirmation
    {
        public DocumentType DocumentType  { get; set; }
        public Action SaveCallBack { get; set; }
        public EditMode EditMode { get; set; }
        public override void OnCloseCommandByEscape(object window)
        {
            OnCloseCommand(window);
        }

        public override void OnCloseCommand(object window)
        {
            SaveCallBack?.Invoke();
            base.OnCloseCommand(window);
        }
    }
}
