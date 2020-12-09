using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Model;

namespace Ism.Document.ViewModels
{
    public class FileAddInteraction : WindowAwareConfirmation
    {
        public FileAddInteraction()
        {
            Files = null;
        }

        public List<File> Files { get; set; }

        public Infrastructure.Model.Document Document { get; set; }

        public override bool CanExecuteMaximizeCommand(object window)
        {
            return false;
        }

        public override bool CanExecuteMinimizeCommand(object window)
        {
            return false;
        }

        public override bool CanExecuteRestoreCommand(object window)
        {
            return false;
        }
    }
}
