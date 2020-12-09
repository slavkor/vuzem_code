using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
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
    public class ListWorkPlacesViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;


        //private AddAddress<BaseModel> _address;
        //private AddressViewInteraction _notification;
        private Address _address;
        private ListInteractionEx<Address> _notification;
        private List<Address> _addresses;
        private bool _isSelect;
        private bool _isEdit;

        public ListWorkPlacesViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                AddCommand = new DelegateCommand(OnAddCommand);
                EditCommand = new DelegateCommand<Address>(OnEditCommand, CanExecuteEditCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                SelectCommand = new DelegateCommand<Address>(OnSelectCommand, CanExecuteSelectCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                SetProperty(ref _isSelect, value);
                IsEdit = !value;
            }
        }

        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                SetProperty(ref _isEdit, value);

            }
        }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand<Address> EditCommand { get; }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<Address> SelectCommand { get; }

        public List<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty(ref _addresses, value);

            }
        }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    var notificaton = value as ListInteractionEx<Address>;
                    if (notificaton == null) return;
                    SetProperty(ref _notification, notificaton);
                    RefreshAddresses();
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                    FinishInteraction();
                }
            }
        }

        public Action FinishInteraction { get; set; }

        

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        #endregion


        #region private helper methods

        private void OnAddCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditEvent<Address>>().Publish(new EditEventArgs<Address>() { EditObject = null, EditMode = EditMode.New, SaveAction = OnAddAddress });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnAddAddress(Address obj, EditMode editMode)
        {
            try
            {
                _notification?.AddAction?.Invoke(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        private void OnEditCommand(Address obj)
        {
            throw new NotImplementedException();
        }
        private bool CanExecuteEditCommand(Address arg)
        {
            throw new NotImplementedException();
        }

        private void RefreshAddresses()
        {
            try
            {
                if (null == _notification) return;
                //Addresses = _notification.DataProviderDelegate?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnSelectCommand(Address obj)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteSelectCommand(Address arg)
        {
            throw new NotImplementedException();
        }

        private void OnCancelCommand()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}

