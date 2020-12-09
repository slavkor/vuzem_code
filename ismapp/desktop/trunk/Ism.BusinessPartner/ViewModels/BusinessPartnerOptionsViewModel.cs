using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.BusinessPartners.Events;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;

namespace Ism.BusinessPartners.ViewModels
{
    class BusinessPartnerOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private Uri _baseUri;
        private BusinessPartner _currentBusinessPartner;


        public BusinessPartnerOptionsViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {


            try
            {
                _settingsService = settingsService;
                _securityService = securityService;
                _exceptionService = exceptionService;

                _eventAggregator.GetEvent<CurrentBusinessPartnerChange>().Subscribe(OnCurrentBusinessPartnerChange);
                BusinessPartnerEditRequest = new InteractionRequest<EditInteraction<BusinessPartner>>();
                BusinessPartnerEditCommand = new DelegateCommand<BusinessPartner>(OnBusinessPartnerEditCommand);
                BusinessPartnerListCommand = new DelegateCommand(OnBusinessPartnerListCommand);
                _baseUri = _settingsService.GetApiServer();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnBusinessPartnerListCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BusinessPartner>() { Header = "Seznam poslovnih partnerjev", EditInteraction = new EditInteraction<BusinessPartner>() { Title = "Dodajanje novega zaposlenega", InteractionObject = null, EditMode = EditMode.New } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.PartnersRegion, "BusinessPartnersList", NavigaionCallback, parameters);


            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        public InteractionRequest<EditInteraction<BusinessPartner>> BusinessPartnerEditRequest { get; }
        
        public DelegateCommand<BusinessPartner> BusinessPartnerEditCommand { get; }

        public DelegateCommand BusinessPartnerListCommand { get; }

        public BusinessPartner CurrentBusinessPartner
        {
            get { return _currentBusinessPartner; }
            set
            {
                SetProperty(ref _currentBusinessPartner, value);
                
            }
        }

        


        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                CurrentBusinessPartner = null;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }



        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedFrom(navigationContext);
                CurrentBusinessPartner = null;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #endregion

        private void OnBusinessPartnerEditCommand(BusinessPartner obj)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BusinessPartner>() { Header = "Urejanje poslovnega partnerja", EditInteraction = new EditInteraction<BusinessPartner>() { Title = "Dodajanje novega zaposlenega", InteractionObject = obj, EditMode = obj == null ?  EditMode.New : EditMode.Edit } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.PartnersRegion, "BusinessPartnerEdit", NavigaionCallback, parameters);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void NavigaionCallback(NavigationResult obj)
        {
            try
            {

            }
            catch (Exception exception)
            {
            }
        }

        //private void OnBusinessPartnerEditRequestCallback(BusinessPartnerEditInteraction obj)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception e)
        //    {
        //        _exceptionService.RaiseException(e);
        //    }
        //}

        private void OnCurrentBusinessPartnerChange(BusinessPartner obj)
        {
            try
            {
                CurrentBusinessPartner = obj;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}
