using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
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
using Ism.Infrastructure.Interaction;

namespace Ism.BusinessPartners.ViewModels
{
    public class BusinessPartnerOverViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private BusinessPartner _businessPartner;
        private EditInteraction<BusinessPartner> _interaciton;

        public BusinessPartnerOverViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {

            if (null == settings)
                throw new ArgumentNullException(nameof(settings));


            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
            SelectBusinessPartner = new DelegateCommand(OnSelectBusinessPartner, () => BusinessPartner == null);
            RemoveBusinessPartner = new DelegateCommand(OnRemoveBusinessPartner, () => BusinessPartner != null);
            BusinessPartnerListRequest = new InteractionRequest<ListInteraction<BusinessPartner>>();
        }

        public DelegateCommand SelectBusinessPartner { get; }
        public DelegateCommand RemoveBusinessPartner { get; }

        public BusinessPartner BusinessPartner { get { return _businessPartner; } set { SetProperty(ref _businessPartner, value);
                SelectBusinessPartner.RaiseCanExecuteChanged();
                RemoveBusinessPartner.RaiseCanExecuteChanged();
            } }

        public InteractionRequest<ListInteraction<BusinessPartner>> BusinessPartnerListRequest { get; }
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<BusinessPartner>;
                Header = navigation.Header;
                _interaciton = navigation.EditInteraction;
                BusinessPartner = navigation.EditInteraction.InteractionObject;
                //int a = 0;
            }
            catch (Exception)
            {
            }
            //throw new NotImplementedException();
        }


        #region private helper methods

        private void OnSelectBusinessPartner()
        {
            try
            {
                BusinessPartnerListRequest.Raise(new ListInteraction<BusinessPartner>() { Title = "Izbira stranke", SelectAction = p => { BusinessPartner = p; _interaciton.SelectAction(p); } });
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnRemoveBusinessPartner()
        {
            try
            {
                BusinessPartner = null;
                _interaciton?.SelectAction?.Invoke(null);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        

        #endregion
    }
}
