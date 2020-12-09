using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Commands;
using Ism.Infrastructure;
using Microsoft.Practices.Unity;
using Ism.Employees.Views;
using Prism.Events;
using Ism.Infrastructure.Events;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;

namespace Ism.Employees.ViewModels
{
    public class EmployeesViewModel: ViewModelBase
    {
        private readonly IExceptionService _exceptionService;

        public EmployeesViewModel(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
        }

        

        #region INavigationAware
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                NavigationInteraction<BaseModel> nav = new NavigationInteraction<BaseModel>() { Header = "Možnosti" };
                NavigationParameters prameters = new NavigationParameters();
                prameters.Add("navigation", nav);

                _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesOptRegion, "EmployeesOptions", prameters);
                //_regionManager.RequestNavigate(RegionNames.EmployeesRegion, "EmployeesList");
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #endregion
    }
}
