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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;

namespace Ism.Common.ViewModels
{
    public class EditAddressViewModel : ViewModelBase, IInteractionRequestAware
    {
        
        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        //private AddAddress<BaseModel> _address;
        //private AddressViewInteraction _notification;
        private Address _address;
        private EditInteraction<Address> _notification;

        private Uri _url;
        private List<AddressType> _addressTypes;
        private AddressType _selectedAddressType;
        private List<Address> _allAddresses;
        private bool _selectAddressVisible;
        public EditAddressViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                SelectAddressCommand = new DelegateCommand<Address>(OnSelectAddressCommand);
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveComand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private string _addressSearch;
        public string AddressSearch
        {
            get
            {
                return _addressSearch;
            }
            set
            {
                SetProperty(ref _addressSearch, value);

            }
        }

        public bool SelectAddressVisible {
            get
            {
                return _selectAddressVisible;
            }
            set
            {
                SetProperty(ref _selectAddressVisible, value);
            }
        }
        public Address SelectedAddress { get; set; }

        public List<AddressType> AddressTypes
        {
            get { return _addressTypes; }
            set { SetProperty(ref _addressTypes, value);}
        }

        public List<Address> AllAddresses
        {
            get
            {
                return _allAddresses;
            }
            set
            {
                SetProperty(ref _allAddresses, value);
            }
        }

        public AddressType SelectedAddressType
        {
            get { return _selectedAddressType; }
            set
            {
                SetProperty(ref _selectedAddressType, value);
                if(null != Address && Address.Type != SelectedAddressType?.Name) Address.Type = SelectedAddressType?.Name;
            }
        }

        public Address Address
        {
            get { return _address; }
            set
            {
                SetProperty(ref _address, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<Address> SelectAddressCommand { get; }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    var common = _serviceLocator.GetInstance<ICommonService>();

                    common.FetchShared<List<Address>>(@"/shrd/address/list", list =>
                    {
                        try
                        {
                            AddressSearch = null;
                            var notificaton = value as EditInteraction<Address>;
                            if (notificaton == null) return;

                            AddressTypes = common.GetAddressTypes();

                            Address = notificaton.EditMode == EditMode.Edit ? notificaton.InteractionObject : new Address() { UuId = Guid.NewGuid().ToString(), Type = "STALNI" };
                            Address.IsDirty = false;
                            Address.PropertyDeletegate = (model) =>
                            {
                                SaveCommand.RaiseCanExecuteChanged();
                            };

                            SelectedAddressType = AddressTypes.FirstOrDefault(a => a.Name == Address.Type);

                            SetProperty(ref _notification, notificaton);
                            SaveCommand.RaiseCanExecuteChanged();
                            AllAddresses = list;

                            SelectedAddress = null;
                            SelectAddressVisible = false;
                            SelectAddressVisible = notificaton.EditMode == EditMode.New;
                        }
                        catch (Exception exception)
                        {
                            _exceptionService.RaiseException(exception);
                        }
                    });

                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                    OnFinishInteraction();
                }
            }
        }

        public Action FinishInteraction { get; set; }

        
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

        }

        private bool CanExecuteSaveComand()
        {
            try
            {
                return Address != null && Address.IsDirty;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return false;
        }

        private void OnSaveCommand()
        {
            try
            {
                _notification.SaveAction?.Invoke(Address, SelectedAddress == null ? EditMode.New: EditMode.Bind);
                OnFinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                FinishInteraction?.Invoke();
            }
        }
        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnFinishInteraction()
        {
            try
            {
                Clear();
                FinishInteraction?.Invoke();
                NavigateBack();

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
                Address = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnSelectAddressCommand(Address obj)
        {
            try
            {
                Address = obj;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

    }
}
