using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Unity;
using Ism.Infrastructure.Repository;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;

namespace Ism.Repository
{
    [Module(ModuleName = "RepositoryModule", OnDemand = false)]
    public class RepositoryModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionSerice;
        public RepositoryModule(IUnityContainer container, IEventAggregator eventAggregator, IExceptionService exceptionService)
        {
            if (null == container)
                throw new ArgumentNullException(nameof(container));
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));

            _container = container;
            _eventAggregator = eventAggregator;
            _exceptionSerice = exceptionService;
        }
        public void Initialize()
        {
            try
            {
                _container.RegisterType(typeof(IRestRepository<,>), typeof(RestRepositroy<,>));
            }
            catch (Exception exc)
            {
                _exceptionSerice.RaiseException(exc);
            }
        }
    }
}
