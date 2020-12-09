using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Services
{
    public interface IDocumentService
    {
        void UploadUserReport(Report report);
    }
}
