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
using System.Collections.ObjectModel;
namespace Ism.Common.ViewModels
{
    public class AddressesViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private EditInteraction<Address> _interaction;

        private ObservableCollection<Address> _addressList;
        private Address _selectedAddress;
        private Uri _url;

        public AddressesViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                AddressCommand = new DelegateCommand(OnAddressCommand);
                AddressCommandEdit = new DelegateCommand<Address>(OnAddressCommandEdit, CanExecuteAddressCommandEdit);
                AddressCommandDelete = new DelegateCommand<Address>(OnAddressCommandDelete, CanExecuteAddressCommandEdit);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public ObservableCollection<Address> AddressList
        {
            get
            {
                return _addressList;
            }
            set
            {
                SetProperty(ref _addressList, value);

            }
        }

        public Address SelectedAddress
        {
            get
            {
                return _selectedAddress;
            }
            set
            {
                SetProperty(ref _selectedAddress, value);
                AddressCommandEdit.RaiseCanExecuteChanged();
                AddressCommandDelete.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddressCommand { get; }
        public DelegateCommand<Address> AddressCommandEdit { get; }
        public DelegateCommand<Address> AddressCommandDelete { get; }

        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                _interaction = (navigationContext.Parameters["navigation"] as NavigationInteraction<Address>)?.EditInteraction;
                _interaction?.DataProvider?.Invoke(DataProviderCallback);

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        public override bool KeepAlive => false;

        private void DataProviderCallback(List<Address> address)
        {
            try
            {
                AddressList = null;
                if (null == address) return;

                AddressList = new ObservableCollection<Address>(address);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        #region Helper methods for Address edit


        #region adding new Address

        private void OnAddressCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<BaseModel, Address>>().Publish(new EditChildEventArgs<BaseModel, Address>() { EditObject = _interaction.InteractionObject, EditChildObject = null, EditMode = EditMode.Edit, EditChildMode = EditMode.New, SaveChildAction = OnAddAddress });

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
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() {  CallBackAction = OnConfirmAddAddressCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = obj, EditMode = editMode });

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmAddAddressCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                Address Address = args.PayLoad as Address;
                if (null == Address) return;
                _interaction.SaveAction.Invoke(Address, args.EditMode);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        #endregion


        #region updating existing Address

        private void OnAddressCommandEdit(Address obj)
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<BaseModel, Address>>().Publish(new EditChildEventArgs<BaseModel, Address>() { EditObject = _interaction.InteractionObject, EditChildObject = obj, EditMode = EditMode.Edit, EditChildMode = EditMode.Edit, SaveChildAction = OnUpdateAddress });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnUpdateAddress(Address obj, EditMode editMode)
        {
            try
            {
                if (obj == null || !obj.IsDirty) return;
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmUpdateAddressCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmUpdateAddressCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;
                Address Address = args.PayLoad as Address;
                if (null == Address) return;
                _interaction.SaveAction.Invoke(Address, EditMode.Edit);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        #endregion

        #region delete Address


        private void OnAddressCommandDelete(Address obj)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmAddressDelete, Title = "ALO", Content = "Želiš izbrisati kontakt zaposlenega?", PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        private void OnConfirmAddressDelete(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed)
                    return;

                Address Address = args.PayLoad as Address;
                if (null == Address) return;

                _interaction.SaveAction.Invoke(Address, EditMode.Delete);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion

        private bool CanExecuteAddressCommandEdit(Address arg)
        {
            return SelectedAddress != null && _interaction.EditMode != EditMode.New;
        }

        #endregion

    }
}
