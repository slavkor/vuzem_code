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
    class WorkPlacesViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private List<WorkPlace> _workPlaces;

        public WorkPlacesViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                AddWorkPlaceCommand = new DelegateCommand(OnAddWorkPlaceCommand);
                WorkPlaceInteractionRequest = new InteractionRequest<EditInteraction<WorkPlace>>();

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand AddWorkPlaceCommand { get; }

        public InteractionRequest<EditInteraction<WorkPlace>> WorkPlaceInteractionRequest { get; }

        public List<WorkPlace> WorkPlaces
        {
            get { return _workPlaces; }
            set
            {
                SetProperty(ref _workPlaces, value);

            }
        }


        private void OnAddWorkPlaceCommand()
        {
            try
            {
                WorkPlaceInteractionRequest.Raise(new EditInteraction<WorkPlace>() { Title = "Dodajanje delovnega mesta" }, OnWokrPlaceInteractionRequestCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnWokrPlaceInteractionRequestCallback(EditInteraction<WorkPlace> obj)
        {
            try
            {
                RefreshWorkPlaces();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void RefreshWorkPlaces()
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<WorkPlace>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(true), "shrd/workplace/list").ToString(), _securityService.GetCurrentUser().AccessToken,
                        list =>
                        {
                            WorkPlaces = list;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                RefreshWorkPlaces();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        #endregion
    }
}
