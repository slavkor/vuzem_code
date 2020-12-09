using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
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
    public class BusinessPartnersViewModel: ViewModelBase
    {

        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private List<BusinessPartner> _partners;
        private Uri _baseUri;


        public BusinessPartnersViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                //_eventAggregator.GetEvent<SelectBusinessPartnerEvent>().Subscribe(OnSelectBusinessPartnerEvent);
                //BusinessPartnerEditRequest = new InteractionRequest<BusinessPartnerEditInteraction>();
                //BusinessPartnerSelectRequest = new InteractionRequest<BusinessPartnerListInteraction>();
                BusinessPartnerEditCommand = new DelegateCommand<BusinessPartner>(OnBusinessPartnerEditCommand);
                _baseUri = _settingsService.GetApiServer();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }


        //public InteractionRequest<BusinessPartnerEditInteraction> BusinessPartnerEditRequest { get; }
        //public InteractionRequest<BusinessPartnerListInteraction> BusinessPartnerSelectRequest { get; }

        public DelegateCommand<BusinessPartner> BusinessPartnerEditCommand { get; }

        

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BusinessPartner>() { Header = "Možnosti" });

                _regionManager.RequestNavigate(Infrastructure.RegionNames.PartnersOptRegion, "BusinessPartnerOptions", NavigaionCallback, parameters);
                

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
                parameters.Add("navigation", new NavigationInteraction<BusinessPartner>() { Header = "Urejanje poslovnega partnerja", EditInteraction = new EditInteraction<BusinessPartner>() { Title = "Dodajanje novega zaposlenega", InteractionObject = obj, EditMode = EditMode.New } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.PartnersRegion, "BusinessPartnerEdit", NavigaionCallback, parameters);

                //BusinessPartnerEditRequest.Raise(
                //    null == obj
                //        ? new BusinessPartnerEditInteraction() {Title = "Dodajanje novega poslovnega partnerja", Partner = new BusinessPartner(), Mode = EditMode.New}
                //        : new BusinessPartnerEditInteraction() {Title = "Urejanje poslovnega partnerja ",TitleExtendet = $"{obj.Name} {obj.LastName}",Partner = obj, Mode = EditMode.Edit},
                //    OnBusinessPartnerEditRequestCallback);
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
                _exceptionService.RaiseException(exception);
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

        //private void OnSelectBusinessPartnerEvent(SelectBusinessPartnerEventArgs obj)
        //{
        //    try
        //    {
        //        BusinessPartnerSelectRequest.Raise(new BusinessPartnerListInteraction() {Title = "Izbira poslovnega partnerja", Partner = obj.Partner, CallBack = obj.CallBack}, OnBusinessPartnerSelectRequestCallback);
        //    }
        //    catch (Exception e)
        //    {
        //        _exceptionService.RaiseException(e);
        //    }
        //}

        //private void OnBusinessPartnerSelectRequestCallback(BusinessPartnerListInteraction obj)
        //{
        //    try
        //    {
        //        if(!obj.Confirmed)
        //            return;

        //        obj.CallBack?.Invoke(obj.Partner);
        //    }
        //    catch (Exception e)
        //    {
        //        _exceptionService.RaiseException(e);
        //    }
        //}
    }
}
