using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Interaction
{
    public class EditDetailInteraction: WindowAwareConfirmation
    {
        public Address Address { get; set; }
        public Contact Contact { get; set; }
        public Document Document { get; set; }
        public EditDetailInteractionEnum EditContext { get; set; }
    }
}
