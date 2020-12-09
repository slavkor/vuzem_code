using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Services
{
    public interface IExceptionService
    {
        void RaiseException(Exception exception);
    }
}
