using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;
using Ism.Employees.Commands;

namespace Ism.Employees.ViewModels
{
    class EmployeeEditOptionsViewModel : ViewModelBase
    {


        private NavigationContext _navigationContext;
        private IAppCommands _appCommands;
        private readonly IExceptionService _exceptionService;

        public EmployeeEditOptionsViewModel(IAppCommands appCommands, IExceptionService exceptionService)
        {
            try
            {
                AppCommands = appCommands;
                //SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                _exceptionService = exceptionService;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public IAppCommands AppCommands
        {
            get
            {
                return _appCommands;
            }
            set
            {
                SetProperty(ref _appCommands, value);
            }
        }

        private void OnCancelCommand()
        {
            try
            {
                _navigationContext.NavigationService.Journal.GoBack();

                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Pregled zaposlenih" });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesRegion, "EmployeesList", NavigaionCallback, parameters);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        public override bool KeepAlive => false;

        private void NavigaionCallback(NavigationResult obj)
        {
            //throw new NotImplementedException();
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _navigationContext = navigationContext;
            
        }
    }
}
