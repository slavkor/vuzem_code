
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
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Prism.Commands;

namespace Ism.Employees
{
    [Module(ModuleName = "EmployeesModule", OnDemand =true)]
    [ModuleDependency("NavigationModule")]
    public class EmployeesModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        private readonly INavigationService _navigationService;
        public EmployeesModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, IExceptionService exceptionService, INavigationService navigationService)
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
            _navigationService = navigationService;

            EmployeesList = new DelegateCommand(OnEmployeesList);
        }

        public DelegateCommand EmployeesList { get; }

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


                _eventAggregator.GetEvent<NavigationMenuEntryEvent>().Publish(new NavigationMenuEntryEventArgs()
                {
                    Parent = _navigationService.GetParents().Where(p => p.Title == "Zaposleni").FirstOrDefault(),
                    Title = "Seznam",
                    Command = EmployeesList,
                    ContentSouce = "pack://application:,,,/Ism.Infrastructure;component/Icons/png/user-3.png"
                });

                //_eventAggregator.GetEvent<NavigationMenuEntryEvent>().Publish(new NavigationMenuEntryEventArgs()
                //{
                //    Parent = _navigationService.GetParents().Where(p => p.Title == "Zaposleni").FirstOrDefault(),
                //    Title = "Potek dokumentov",
                //    Command = DocumentsToExpire
                //});

                //var a = _container.TryResolve<EmployeesNavView>();
                //var b = _container.TryResolve<EmployeesOptions>();
                //_regionManager.RequestNavigate(Infrastructure.RegionNames.NavigaionRegion, "EmployeesNavView", NavigaionCallback);
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

        private void OnEmployeesList()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Pregled zaposlenih" });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "EmployeesList", NavigaionCallback, parameters);

                parameters = new NavigationParameters();
                parameters.Add("context", "Employees.List");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                _regionManager.RequestNavigate(RegionNames.ReportsRegion, "ReportsContext", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ReportMetaDataProvider(string meta, Action<string> callback)
        {
            try
            {

                callback?.Invoke(meta);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

    }
}
