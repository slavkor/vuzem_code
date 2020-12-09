
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using System.ComponentModel.Composition;
using Prism.Regions;
using Ism.Employees.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Employees.Commands;
using Ism.Infrastructure.Services;
using Ism.Employees.Services;

namespace Ism.Employees
{
    [Module(ModuleName = "EmployeesModule", OnDemand =true)]
    public class EmployeesModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        public EmployeesModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IExceptionService exceptionService)
        {
            if (null == regionManager)
                throw new ArgumentNullException(nameof(regionManager));
            if (null == container)
                throw new ArgumentNullException(nameof(container));
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));

            _regionManager = regionManager;
            _container = container;
            _eventAggregator = eventAggregator;
            _exceptionService = exceptionService;
        }
        public void Initialize()
        {
            try
            {
                _container.RegisterType(typeof(IEmployeeService), typeof(EmployeeService));

                _container.RegisterTypeForNavigation<Views.Employees>("Employees");
                _container.RegisterTypeForNavigation<EmployeesList>("EmployeesList");
                _container.RegisterTypeForNavigation<EmployeeEdit>("EmployeeEdit");
                _container.RegisterTypeForNavigation<EmployeesNavView>("EmployeesNavView");
                _container.RegisterTypeForNavigation<EmployeesOptions>("EmployeesOptions");
                _container.RegisterTypeForNavigation<EmployeeDocumentsToExpire>("EmployeeDocumentsToExpire");
                _container.RegisterTypeForNavigation<EmployeeEditOptions>("EmployeeEditOptions");
                _container.RegisterTypeForNavigation<VacSickLeaveView>("VacSickLeaveView");
                _container.RegisterTypeForNavigation<EmployeeHistoryView>("EmployeeHistoryView");

                _regionManager.RequestNavigate(Infrastructure.RegionNames.NavigaionRegion, "EmployeesNavView", NavigaionCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                var b = !navigationResult.Result;
                if (b != null && (bool)b)
                {
                    _exceptionService.RaiseException(navigationResult.Error);
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
