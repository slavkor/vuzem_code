using Ism.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;
using Microsoft.Practices.ServiceLocation;
using Ism.Infrastructure.Repository;

namespace Ism.Employees.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IExceptionService _exceptionService;
        private readonly ISettingsService _settingsService;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISecurityService _securityService;

        public EmployeeService(IExceptionService exceptionService, ISettingsService settingsService, IServiceLocator serviceLocator, ISecurityService securityService)
        {
            _exceptionService = exceptionService;
            _settingsService = settingsService;
            _serviceLocator = serviceLocator;
            _securityService = securityService;
        }
        public List<WokForceStat> GetAllActiveEmployees()
        {
            return this.GetAllActiveEmployees(null);
        }

        public List<WokForceStat> GetAllActiveEmployees(Company company)
        {
            List<WokForceStat> list = null;
            try
            {
                var url = company == null ? _settingsService.GetApiServer(true) : _settingsService.GetApiServer();
                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<WokForceStat>, object>>())
                {
                    list = repository.GetRequest(new Uri(_settingsService.GetApiServer(), "employees/activecount").ToString(),_securityService.GetCurrentToken());
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            return list;
        }
    }
}
