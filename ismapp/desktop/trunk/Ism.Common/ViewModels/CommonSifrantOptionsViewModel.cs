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
    class CommonSifrantOptionsViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;


        private AddContact<BaseModel> _contact;
        private ContactViewInteraction _notification;
        private Uri _baseUri;
        private List<Language> _languages;

        public CommonSifrantOptionsViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
            try
            {
                LanguagesCommand = new DelegateCommand(OnLanguagesCommand, CanExectuteLanguagesCommand);
                WorkPlacesCommand = new DelegateCommand(OnWorkPlacesCommand, CanExecuteWorkPlacesCommand);
                
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }



        public DelegateCommand LanguagesCommand { get; }
        public DelegateCommand WorkPlacesCommand { get; }

        

        #region private methods


        private bool CanExecuteWorkPlacesCommand()
        {
            return true;
        }

        private void OnWorkPlacesCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.ComonSifrantRegion, "WorkPlaces");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExectuteLanguagesCommand()
        {
            return true;
        }

        private void OnLanguagesCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.ComonSifrantRegion, "Languages");

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    
        #endregion
    }
}
