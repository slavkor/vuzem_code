using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Interaction
{
    public class ReportInteraction<T>: EditInteraction<T>
    {
        public ReportEventArgs ReportEventArgs { get; set; }

        public override void OnCloseCommandByEscape(object window)
        {
            OnCloseCommand(window);
        }
    }
}
