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
    public class EwrViewModel : ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;

        public EwrViewModel(IExceptionService exceptionService, ISettingsService settingsService, ISecurityService securityService)
        {
            _exceptionService = exceptionService;
            _settingsService = settingsService;
            _securityService = securityService;
            _eventAggregator.GetEvent<SelectedEvent<ForemanConstructionSite>>().Subscribe(OnProjectSelectedEvent);
        }

        private void OnProjectSelectedEvent(SelectedEventArgs<ForemanConstructionSite> obj)
        {
            try
            {
                _regionManager.Regions.Remove(RegionNames.EwrDocumentsRegion);
                _regionManager.Regions.Remove(RegionNames.EwrExSiteManagerRegion);
                _regionManager.Regions.Remove(RegionNames.EwrReportsRegion);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #region INavigationAware
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                NavigationParameters par;
                var project = navigationContext.Parameters["project"] as Project;

                par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Dodatna dela", });
                par.Add("project", project);
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EwrOptRegion, "EwrOptions", par);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

        }
        public override bool KeepAlive => false;
        #endregion
    }
}
