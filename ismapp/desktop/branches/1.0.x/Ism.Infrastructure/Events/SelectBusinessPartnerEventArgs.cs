using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Events
{
    public class SelectBusinessPartnerEventArgs
    {
        public BusinessPartner Partner { get; set; }
        public BaseModel Parent { get; set; }
        public string Uri { get; set; }
        public Action<BusinessPartner> CallBack { get; set; }
    }
}
