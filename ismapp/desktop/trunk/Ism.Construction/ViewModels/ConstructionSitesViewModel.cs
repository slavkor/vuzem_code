using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;

namespace Ism.Construction.ViewModels
{
    class ConstructionSitesViewModel: ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        public ConstructionSitesViewModel(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;

        }


        
  

        #region INavigationAware
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {

                NavigationParameters par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<BaseModel>()
                {
                    Header = "Možnosti",
                });

                _regionManager.RequestNavigate(Infrastructure.RegionNames.CSiteOptRegion, "ConstructionSiteOpt", par);


                par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<BaseModel>()
                {
                    Header = "Seznam gradbišč",
                });

                _regionManager.RequestNavigate(Infrastructure.RegionNames.CSiteRegion, "ConstructionSitesList", par);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion
    }
}
