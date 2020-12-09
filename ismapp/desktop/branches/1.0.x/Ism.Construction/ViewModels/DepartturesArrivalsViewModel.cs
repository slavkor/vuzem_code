using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure;
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
using TimeLineTool;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Construction.ViewModels
{
    class DepartturesArrivalsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;


        public DepartturesArrivalsViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                var par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Odhodi" });
                _regionManager.RequestNavigate(RegionNames.CDeparturesRegion, "Departures", OnRequestNavigateCallBack, par);

                par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Prihodi" });
                _regionManager.RequestNavigate(RegionNames.CArrivalsRegion, "Arrivals", OnRequestNavigateCallBack, par);

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }

        }

        public override bool KeepAlive => false;

        private void OnRequestNavigateCallBack(NavigationResult obj)
        {
            int a = 0;
            //throw new NotImplementedException();
        }
    }

}

