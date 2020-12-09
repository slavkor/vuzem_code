
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Prism.Regions;
using Ism.Reports.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Prism.Events;
using Ism.Infrastructure.Events;

using Ism.Infrastructure.Services;

namespace Ism.Reports
{
    [Module(ModuleName = "ReportsModule", OnDemand = true)]
    public class ReportsModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        public ReportsModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IExceptionService exceptionService)
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
                //_container.RegisterType(typeof(IEmployeeCommands), typeof(EmployeeCommands));

                _container.RegisterTypeForNavigation<Views.Reports>("Reports");
                _container.RegisterTypeForNavigation<Views.ReportEdit>("ReportEdit");
                _container.RegisterTypeForNavigation<Views.ReportsList>("ReportsList");
                _container.RegisterTypeForNavigation<Views.ReportsNavView>("ReportsNavView");
                _container.RegisterTypeForNavigation<Views.ReportsOptions>("ReportsOptions");
                _container.RegisterTypeForNavigation<Views.ReportsContext>("ReportsContext");
                _container.RegisterTypeForNavigation<Views.ReportsUserBind>("ReportsUserBind");

                _regionManager.RequestNavigate(Infrastructure.RegionNames.NavigaionRegion, "ReportsNavView", NavigaionCallback);
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
