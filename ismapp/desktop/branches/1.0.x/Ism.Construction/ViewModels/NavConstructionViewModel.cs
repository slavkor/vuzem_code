using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;

namespace Ism.Construction.ViewModels
{
    public class NavConstructionViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        public NavConstructionViewModel(ISecurityService securityService, IExceptionService exceptionService)
        {
            _securityService = securityService;
            _exceptionService = exceptionService;
            NavigateConstrucitonSitesCommand = new DelegateCommand(OnNavigateConstrucitonSitesCommand, () => _securityService.HasPermission("csite"));
            NavigateForemanConstrucitonSitesCommand = new DelegateCommand(OnNavigateForemanConstrucitonSitesCommand, () => _securityService.HasPermission("foreman"));
            _eventAggregator.GetEvent<EditEvent<ForemanConstructionSite>>().Subscribe(OnForemanConstructionSiteEvent);

        }

        private void OnForemanConstructionSiteEvent(EditEventArgs<ForemanConstructionSite> obj)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("sitedata", obj.EditObject);
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.MainContentRegion, "ForemanCounstructionSite", parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }



        #region public properies


        public DelegateCommand NavigateConstrucitonSitesCommand { get; }
        public DelegateCommand NavigateForemanConstrucitonSitesCommand { get; }

        #endregion

        #region private helper methods

        private void OnNavigateConstrucitonSitesCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.MainContentRegion, "ConstructionSitesView");
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnNavigateForemanConstrucitonSitesCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.MainContentRegion, "ForemanCounstructionSite");
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        #endregion
    }
}
