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
using Ism.Document.Views;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;

namespace Ism.Document
{
    [Module(ModuleName = "DocumentModule", OnDemand = true)]
    [ModuleDependency("NavigationModule")]
    public class DocumentModule: IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly IExceptionService _exceptionService;
        public DocumentModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IServiceLocator serviceLocator, IExceptionService exceptionService)
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

                _container.RegisterType(typeof(IDocumentService), typeof(Services.DocumentService));
                _container.RegisterTypeForNavigation<NavDocument>("NavDocument");
                _container.RegisterTypeForNavigation<NavDocument>("NavDocument");
                _container.RegisterTypeForNavigation<Documents>("Documents");
                _container.RegisterTypeForNavigation<DocmentsExt>("DocmentsExt");
                _container.RegisterTypeForNavigation<DocumentType>("DocumentType");
                _container.RegisterTypeForNavigation<DocumentOneAdd>("DocumentOneAdd");
                
                var security = _serviceLocator.GetInstance<ISecurityService>();
                var user = security?.GetCurrentUser();

                //_regionManager.RequestNavigate(RegionNames.NavigaionRegion, "NavDocument", NavigaionCallback);
               

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
