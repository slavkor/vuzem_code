using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Ism.Arrival.Views;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
namespace Ism.Arrival
{
    [Module(ModuleName = "ArrivalModule", OnDemand = true)]
    public class ArrivalModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly IExceptionService _exceptionService;
        public ArrivalModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IServiceLocator serviceLocator, IExceptionService exceptionService)
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
            _serviceLocator = serviceLocator;
            _exceptionService = exceptionService;
        }
        public void Initialize()
        {
            try
            {
                //_container.RegisterTypeForNavigation<Arrivals>("Arrivals");
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
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