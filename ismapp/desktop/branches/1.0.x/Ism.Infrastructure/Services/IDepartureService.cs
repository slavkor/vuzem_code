using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Services
{
    public interface IDepartureService
    {
        void AddInteralsiteDeparture(Project origin, Project destination, List<Employee> employees, DateTime departTime, Action callback);
    }
}
