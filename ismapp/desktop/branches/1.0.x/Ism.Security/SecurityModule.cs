
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Security.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;

namespace Ism.Security
{
    [Module(ModuleName = "SecurityModule", OnDemand = false)]
    [ModuleDependency("SettingsModule")]
    //[ModuleDependency("NavigationModule")]
    [ModuleDependency("RepositoryModule")]
    public class SecurityModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IExceptionService _exceptionService;
        public SecurityModule(IRegionManager regionManager, IEventAggregator eventAggregator, IUnityContainer container, IExceptionService exceptionService)
        {
            if (null == regionManager)
                throw new ArgumentNullException(nameof(regionManager));

            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));

            if (null == container)
                throw new ArgumentNullException(nameof(container));

            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _container = container;
            _exceptionService = exceptionService;
        }

        public void Initialize()
        {
            try
            {
                _container.RegisterType(typeof(ISecurityService), typeof(Services.SecurityService));
                _container.RegisterTypeForNavigation<NavUsers>("NavUsers");
                _container.RegisterTypeForNavigation<UsersList>("UsersList");
                _container.RegisterTypeForNavigation<CompanyList>("CompanyList");
                _container.RegisterTypeForNavigation<ComanyChange>("ComanyChange");
                _regionManager.RegisterViewWithRegion(Infrastructure.RegionNames.CompanyLogoRegion, typeof(ComanyChange));
                _regionManager.RegisterViewWithRegion(Infrastructure.RegionNames.SysNavRegion, typeof(NavLogin));

                var a = _container.TryResolve<NavLogin>();
                var b = _container.TryResolve<NavUsers>();
                _eventAggregator.GetEvent<LogInEvent>().Publish(null);

            }

            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
