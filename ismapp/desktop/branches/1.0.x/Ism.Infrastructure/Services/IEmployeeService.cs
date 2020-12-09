using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Services
{
    public interface IEmployeeService
    {
        List<WokForceStat> GetAllActiveEmployees();
        List<WokForceStat> GetAllActiveEmployees(Company company);
    }
}
