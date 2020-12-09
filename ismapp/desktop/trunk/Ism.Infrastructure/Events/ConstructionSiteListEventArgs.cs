using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Events
{
    public class ConstructionSiteListEventArgs
    {
        public Action<ConstructionSite> SelectCallBack { get; set; }
    }
}
