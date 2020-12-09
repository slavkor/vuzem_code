using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Common.Views;
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

namespace Ism.Common
{
    [Module(ModuleName = "CommonModule", OnDemand = true)]
    [ModuleDependency("NavigationModule")]
    public class CommonModule : IModule, IDisposable
    {
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private IServiceLocator _serviceLocator;
        private IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        public CommonModule(IRegionManager regionManager, IUnityContainer container, IServiceLocator serviceLocator, IEventAggregator eventAggregator, IExceptionService exceptionService)
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

        ~CommonModule() { Dispose(false); }

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
                _container.RegisterType(typeof(ICommonService), typeof(Services.CommonService));
                _container.RegisterType<CommonSifrantOptions>();

                _container.RegisterTypeForNavigation<CommonNav>("CommonNav");
                _container.RegisterTypeForNavigation<EditAddressView>("EditAddressView");
                _container.RegisterTypeForNavigation<CommonSifrant>("CommonSifrant");
                _container.RegisterTypeForNavigation<ListAddressView>("ListAddressView");
                _container.RegisterTypeForNavigation<EditWorkPlace>("EditWorkPlace");
                _container.RegisterTypeForNavigation<Contacts>("Contacts");
                _container.RegisterTypeForNavigation<Addresses>("Addresses");
                _container.RegisterTypeForNavigation<OneContact>("OneContact");
                _container.RegisterTypeForNavigation<CommonSifrantOptions>("CommonSifrantOptions");
                _container.RegisterTypeForNavigation<Languages>("Languages");
                _container.RegisterTypeForNavigation<WorkPlaces>("WorkPlaces");
                _container.RegisterTypeForNavigation<WorkPlacesList>("WorkPlacesList");

                
                //_regionManager.RequestNavigate(RegionNames.NavigaionRegion, "CommonNav", NavigaionCallback);
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

