using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Common.ViewModels
{
    public class ContactViewInteraction : WindowAwareConfirmation
    {
        public Contact Contact { get; set; }

        public BaseModel Parent { get; set; }

        public string Uri { get; set; }

        public Action<AddContact<BaseModel>> CallBack { get; set; }
    }
}
