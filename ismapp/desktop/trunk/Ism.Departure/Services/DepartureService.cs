using Ism.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;
using Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Ism.Infrastructure.Repository;

namespace Ism.Departure.Services
{
    public class DepartureService : IDepartureService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISecurityService _security;
        private readonly ISettingsService _settings;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;
        
        public DepartureService(IEventAggregator eventAggregator, IServiceLocator serviceLocator, ISecurityService security, ISettingsService settings, IExceptionService exceptionService, ICommonService commonService)
        {
            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _security = security;
            _settings = settings;
            _commonService = commonService;
            _exceptionService = exceptionService;
        }

        public void AddInteralsiteDeparture(Project origin, Project destination, List<Employee> employees, DateTime departTime, Action callback)
        {
            try
            {
                Infrastructure.Model.Departure departure = new Infrastructure.Model.Departure() { Origin = origin, Destination = destination, DepartTime = departTime, EmployeesAdd = employees, Internal = true};

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Departure, Infrastructure.Model.Departure>>())
                {
                    var url = new Uri(_settings.GetApiServer(), "departures/addconfirm");
                    

                    repositroy.PostRequestAsync(url.ToString(), departure,
                        _security.GetCurrentToken(),
                        (d) =>
                        {
                            try
                            {
                                callback?.Invoke();
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }
        
    }
}
