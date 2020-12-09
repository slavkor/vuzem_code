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
using Ism.Construction.Views;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;

namespace Ism.Construction
{
    [Module(ModuleName = "ConstructionModule", OnDemand = true)]
    [ModuleDependency("NavigationModule")]
    public class ConstructionModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly IExceptionService _exceptionService;
        public ConstructionModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IServiceLocator serviceLocator, IExceptionService exceptionService)
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
                _container.RegisterTypeForNavigation<NavConstruction>("NavConstruction");
                _container.RegisterTypeForNavigation<ConstructionSitesView>("ConstructionSitesView");
                _container.RegisterTypeForNavigation<ConstructionSiteOpt>("ConstructionSiteOpt");
                _container.RegisterTypeForNavigation<ConstructionSitesList>("ConstructionSitesList");
                _container.RegisterTypeForNavigation<ConstructionSiteEdit>("ConstructionSiteEdit");
                _container.RegisterTypeForNavigation<Projects>("Projects");
                _container.RegisterTypeForNavigation<ProjectEditView>("ProjectEditView");
                _container.RegisterTypeForNavigation<DepartturesArrivals>("DepartturesArrivals");
                _container.RegisterTypeForNavigation<ProjectSelectList>("ProjectSelectList");
                _container.RegisterTypeForNavigation<ForemanCounstructionSite>("ForemanCounstructionSite");
                _container.RegisterTypeForNavigation<ForemanProjects>("ForemanProjects");
                _container.RegisterTypeForNavigation<ForemanOptions>("ForemanOptions");
                _container.RegisterTypeForNavigation<WorkingHours>("WorkingHours");
                _container.RegisterTypeForNavigation<Ewr>("Ewr");
                _container.RegisterTypeForNavigation<EwrEdit>("EwrEdit");
                _container.RegisterTypeForNavigation<EwrList>("EwrList");
                _container.RegisterTypeForNavigation<EwrOptions>("EwrOptions");
                _container.RegisterTypeForNavigation<ConstructionSitesListStats>("ConstructionSitesListStats");
                

                //_regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.NavigaionRegion, "NavConstruction", NavigaionCallback);
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
