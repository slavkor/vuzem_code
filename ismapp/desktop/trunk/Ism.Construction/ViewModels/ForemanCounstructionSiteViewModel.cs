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
using Ism.Infrastructure.Repository;

namespace Ism.Construction.ViewModels
{
    public class ForemanCounstructionSiteViewModel : ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;

        public ForemanCounstructionSiteViewModel(IExceptionService exceptionService, ISettingsService settingsService, ISecurityService securityService)
        {
            _exceptionService = exceptionService;
            _settingsService = settingsService;
            _securityService = securityService;

        }

        #region INavigationAware
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                NavigationParameters par;
                var sitedata = navigationContext.Parameters["sitedata"] as ForemanConstructionSite;

                if(null == sitedata)
                {
                    if (_securityService.GetCurrentEmployee() == null) return;

                    using (var rep = _serviceLocator.GetInstance<IRestRepository<ForemanConstructionSite, object>>())
                    {
                        rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"csite/byemployee/{_securityService.GetCurrentEmployee().UuId}").ToString(), _securityService.GetCurrentToken(), (site) =>
                        {
                            try
                            {
                                if (site == null) return;
                                par = new NavigationParameters();
                                par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Projekti", });
                                par.Add("sitedata", site);
                                _regionManager.RequestNavigate(Infrastructure.RegionNames.ForemanProjectsRegion, "ForemanProjects", par);

                                par = new NavigationParameters();
                                par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Možnosti", });
                                par.Add("sitedata", site);
                                _regionManager.RequestNavigate(Infrastructure.RegionNames.ForemanOptRegion, "ForemanOptions", par);
                                
                                _regionManager.Regions[Infrastructure.RegionNames.ForemanCSiteRegion].RemoveAll();
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                    }
                }
                else
                {
                    par = new NavigationParameters();
                    par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Projekti", });
                    par.Add("sitedata", sitedata);
                    _regionManager.RequestNavigate(Infrastructure.RegionNames.ForemanProjectsRegion, "ForemanProjects", par);

                    par = new NavigationParameters();
                    par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Možnosti", });
                    par.Add("sitedata", sitedata);
                    _regionManager.RequestNavigate(Infrastructure.RegionNames.ForemanOptRegion, "ForemanOptions", par);

                    _regionManager.Regions[Infrastructure.RegionNames.ForemanCSiteRegion].RemoveAll();
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion
    }
}
