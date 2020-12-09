using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.BusinessPartners.Views;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace Ism.BusinessPartners
{
    [Module(ModuleName = "BusinessPartnersModule", OnDemand = true)]
    [ModuleDependency("NavigationModule")]
    public class BusinessPartnerModule : IModule, IDisposable
    {
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private IServiceLocator _serviceLocator;
        private IEventAggregator _eventAggregator;

        private readonly IExceptionService _exceptionService;

        public BusinessPartnerModule(IRegionManager regionManager, IUnityContainer container, IServiceLocator serviceLocator, IEventAggregator eventAggregator, IExceptionService exceptionService)
        {
            if (null == regionManager)
                throw new ArgumentNullException(nameof(regionManager));
            if (null == container)
                throw new ArgumentNullException(nameof(container));
            if (null == serviceLocator)
                throw new ArgumentNullException(nameof(serviceLocator));
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));

            _regionManager = regionManager;
            _container = container;
            _serviceLocator = serviceLocator;
            _eventAggregator = eventAggregator;
            _exceptionService = exceptionService;
        }

        ~BusinessPartnerModule() { Dispose(false); }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            // free managed resources  
            _regionManager = null;
            _serviceLocator = null;
            _serviceLocator = null;
            _eventAggregator = null;
            ////////// free native resources if there are any.  
            ////////if (nativeResource != IntPtr.Zero)
            ////////{
            ////////    Marshal.FreeHGlobal(nativeResource);
            ////////    nativeResource = IntPtr.Zero;
            ////////}
        }

        #endregion


        public void Initialize()
        {
            try
            {
                _container.RegisterTypeForNavigation<BusinessPartnerNavView>("BusinessPartnerNavView");
                _container.RegisterTypeForNavigation<Views.BusinessPartners>("BusinessPartners");
                _container.RegisterTypeForNavigation<BusinessPartnersList>("BusinessPartnersList");
                _container.RegisterTypeForNavigation<BusinessPartnersSelectList>("BusinessPartnersSelectList");
                _container.RegisterTypeForNavigation<BusinessPartnerOptions>("BusinessPartnerOptions");
                _container.RegisterTypeForNavigation<BusinessPartnerOverView>("BusinessPartnerOverView");
                _container.RegisterTypeForNavigation<BusinessPartnerEdit>("BusinessPartnerEdit");

                //_regionManager.RequestNavigate(Infrastructure.RegionNames.NavigaionRegion, "BusinessPartnerNavView", NavigaionCallback);
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


