using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Events
{
    public class PrintEventAgrs
    {
        public IList<DocumentPrint> DownloadFiles { get; set; }
        public IList<DocumentPrint> PrintFiles { get; set; }
        public string PrintKey { get; set; }

    }
}
