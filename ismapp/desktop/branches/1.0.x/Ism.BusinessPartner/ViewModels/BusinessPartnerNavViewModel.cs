using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;

namespace Ism.BusinessPartners.ViewModels
{
    public class BusinessPartnerNavViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        public BusinessPartnerNavViewModel(ISecurityService securityService, IExceptionService exceptionService)
        {
            
            try
            {
                _securityService = securityService;
                _exceptionService = exceptionService;
                NavigateBusinessPartners = new DelegateCommand(OnNavigateBusinessPartners, () => _securityService.HasPermission("bussinesspartner"));
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        public DelegateCommand NavigateBusinessPartners { get; }
        
        private void OnNavigateBusinessPartners()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "BusinessPartners", NavigaionCallback);
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
    }
}
