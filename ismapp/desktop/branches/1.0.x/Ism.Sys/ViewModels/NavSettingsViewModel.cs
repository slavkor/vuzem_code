using System;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Ism.Sys.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Report = Ism.Infrastructure.Model.Report;
using Ism.Infrastructure.Mvvm;
using System.Linq;
using System.Threading;

namespace Ism.Sys.ViewModels
{
    public class NavSettingsViewModel : ViewModelBase
    {

        private readonly IExceptionService _exceptionService;
        public NavSettingsViewModel(IExceptionService exceptionService)
        {

            _exceptionService = exceptionService;

            try
            {

                _eventAggregator.GetEvent<ShowSettingsEvent>().Subscribe(OnShowSettingsEvent);
                SettingsCommand = new DelegateCommand(OnSettingsCommand);
                SettingsInteractionRequest = new InteractionRequest<EditInteraction<object>>();

                
                //_eventAggregator.GetEvent<NavigationMenuEntryEvent>().Publish(new NavigationMenuEntryEventArgs()
                //{
                //    Parent = navigationService.GetParents().Where(p => p.Title == "Sistem").FirstOrDefault(),
                //    Title = "Nastavitve",
                //    Command = SettingsCommand
                //});
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnShowSettingsEvent()
        {
            try
            {
                if (SettingsCommand.CanExecute()) SettingsCommand.Execute();
            }
            catch (Exception error)
            {
                _exceptionService.RaiseException(error);
            }
        }

        public InteractionRequest<EditInteraction<object>> SettingsInteractionRequest { get; }

        ~NavSettingsViewModel()
        {
            Dispose(false);
        }

        public DelegateCommand SettingsCommand { get; }

        

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                if (_regionManager != null)
                    _regionManager = null;
                if (_serviceLocator != null)
                    _serviceLocator = null;
                if (_eventAggregator != null)
                    _eventAggregator = null;

            }
            ////////// free native resources if there are any.  
            ////////if (nativeResource != IntPtr.Zero)
            ////////{
            ////////    Marshal.FreeHGlobal(nativeResource);
            ////////    nativeResource = IntPtr.Zero;
            ////////}
        }

        #endregion
        private void OnSettingsCommand()
        {
            try
            {
                SettingsInteractionRequest.Raise(new EditInteraction<object>() {Title = "Nastavitve"}, interaction =>
                {
                    if(!interaction.Confirmed) return;
                    
                    _eventAggregator.GetEvent<ApplySettingsEvent>().Publish();
                });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
   

    }
}
