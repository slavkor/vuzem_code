
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Prism.Regions;
using Ism.TravelOrder.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;

namespace Ism.TravelOrder
{
    [Module(ModuleName = "TravelOrderModule", OnDemand = true)]
    [ModuleDependency("NavigationModule")]
    public class TravelOrderModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        public TravelOrderModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IExceptionService exceptionService)
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
                _container.RegisterTypeForNavigation<CarEdit>("CarEdit");
                _container.RegisterTypeForNavigation<CarsList>("CarsList");
                _container.RegisterTypeForNavigation<CarsSelectList>("CarsSelectList");
                _container.RegisterTypeForNavigation<TravelOrderMainOptions>("TravelOrderMainOptions");
                _container.RegisterTypeForNavigation<CarsOptions>("CarsOptions");
                _container.RegisterTypeForNavigation<Cars>("Cars");

                //_regionManager.RequestNavigate(RegionNames.NavigaionRegion, "TravelOrderMainOptions", NavigaionCallback);
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
