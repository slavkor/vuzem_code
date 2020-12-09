using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Prism.Commands;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;

namespace Ism.Security.ViewModels
{
    class NavUsersViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        public NavUsersViewModel(ISecurityService securityService, IExceptionService exceptionService)
        {

            _securityService = securityService;
            _exceptionService = exceptionService;
            UsersListCommand = new DelegateCommand(OnUsersListCommand, () => _securityService.HasPermission("admin"));
        }

        private void OnUsersListCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "UsersList", NavigaionCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand UsersListCommand { get; }

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
