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
                SettingsCommand = new DelegateCommand(OnSettingsCommand);
                
                
                SettingsInteractionRequest = new InteractionRequest<EditInteraction<object>>();

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
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
                    
                    _eventAggregator.GetEvent<SettingsEvent>().Publish();
                });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
   

    }
}
