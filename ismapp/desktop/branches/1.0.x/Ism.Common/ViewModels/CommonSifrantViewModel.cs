using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ism.Common.Views;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Common.ViewModels
{
    public class CommonSifrantViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;


        public CommonSifrantViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
        }


        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Možnosti" });

                _regionManager.RequestNavigate(Infrastructure.RegionNames.ComonSifrantOptRegion, "CommonSifrantOptions", parameters);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        #endregion
    }
}
