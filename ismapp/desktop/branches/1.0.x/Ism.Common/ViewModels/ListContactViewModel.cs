using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Ism.Common.ViewModels
{
    class ListContactViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private ListInteraction<Contact> _notification;
        private List<Contact> _contacts;
        private Contact _selected;
        private bool _isSelect;

        public ListContactViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                SelectCommand = new DelegateCommand<Contact>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public List<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                SetProperty(ref _contacts, value);

            }
        }
        public DelegateCommand<Contact> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public Contact Selected
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
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
                var notification = value as ListInteraction<Contact>;
                
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

                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Contact>;
                Header = navigation.Header;

                _notification = navigation.EditInteraction as ListInteraction<Contact>;
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
                _notification.ListEventArgs?.DataProvider?.Invoke(OnDataProviderCallback);


            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnDataProviderCallback(List<Contact> obj)
        {
            try
            {
                Contacts = obj;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
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
        private void OnSelectCommand(Contact obj)
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
                Contacts = null;

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
