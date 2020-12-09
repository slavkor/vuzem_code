using Ism.Infrastructure;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using Prism.Unity;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Prism.Interactivity.InteractionRequest;


namespace Ism.Reports.ViewModels
{
    class ReportsNavViewModel : ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        public ReportsNavViewModel(IExceptionService exceptionService)
        {
            try
            {
                _exceptionService = exceptionService;
                ReportsCommand = new DelegateCommand(OnReportsCommand);


            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand ReportsCommand { get; }


        private void OnReportsCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "Reports", NavigaionCallback);
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
