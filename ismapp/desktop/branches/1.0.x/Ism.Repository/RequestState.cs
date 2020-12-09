using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;

namespace Ism.Repository
{
    public class RequestState
    {
        public HttpWebRequest Request { get; set; }
        public object CallbackAction { get; set; }
        public Action<Exception> ErrorCallbackAction { get; set; }
        public object PayLoad { get; set; }

        public BusyEventArgs BusyEventArgs { get; set; }
    }
}
