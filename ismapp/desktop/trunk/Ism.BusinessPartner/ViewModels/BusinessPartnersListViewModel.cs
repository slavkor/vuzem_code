using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.BusinessPartners.Events;
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
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;

namespace Ism.BusinessPartners.ViewModels
{
    public class BusinessPartnersListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private ListInteraction<BusinessPartner> _notification;
        private List<BusinessPartner> _partners;
        private BusinessPartner _selectedPartner;
        private bool _isSelect;

        public BusinessPartnersListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                SelectCommand = new DelegateCommand<BusinessPartner>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public List<BusinessPartner> Partners
        {
            get { return _partners; }
            set
            {
                SetProperty(ref _partners, value);

            }
        }
        public DelegateCommand<BusinessPartner> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public BusinessPartner SelectedPartner
        {
            get { return _selectedPartner; }
            set
            {
                SetProperty(ref _selectedPartner, value);
                _eventAggregator.GetEvent<CurrentBusinessPartnerChange>().Publish(_selectedPartner);
            }
        }

        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                SetProperty(ref _isSelect, value);
            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as ListInteraction<BusinessPartner>;
                if (notification != null)
                {
                    _notification = notification;
                    RefreshPartners();
                    IsSelect = true;
                }
            }
        }

        public Action FinishInteraction { get; set; }

        
        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<BusinessPartner>;
                Header = navigation.Header;

                _notification = navigation.EditInteraction as ListInteraction<BusinessPartner>;
                RefreshPartners();
                IsSelect = false;
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
                Clear();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion

        private void RefreshPartners()
        {
            try
            {
                Partners = null;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<BusinessPartner>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), "partners/list").ToString(), _securityService.GetCurrentToken(),
                        list =>
                        {
                            Partners = list;
                            SelectedPartner = null;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnCancelCommand()
        {
            try
            {
                _notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnSelectCommand(BusinessPartner obj)
        {
            try
            {
                _notification.Confirmed = true;
                _notification.SelectAction?.Invoke(obj);
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void Clear()
        {
            try
            {
                Partners = null;

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
