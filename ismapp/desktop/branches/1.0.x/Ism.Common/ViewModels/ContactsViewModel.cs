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
    public class ContactsViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private EditInteraction<Contact> _interaction;

        private ObservableCollection<Contact> _contacts;
        private Contact _selectedContact;
        
        public ContactsViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                ContactCommand = new DelegateCommand(OnContactCommand);
                ContactCommandEdit = new DelegateCommand<Contact>(OnContactCommandEdit, CanExecuteContactCommandEdit);
                ContactCommandDelete = new DelegateCommand<Contact>(OnContactCommandDelete, CanExecuteContactCommandEdit);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public ObservableCollection<Contact> Contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                SetProperty(ref _contacts, value);
                
            }
        }

        public Contact SelectedContact {
            get {
                return _selectedContact; }
            set
            {
                SetProperty(ref _selectedContact, value);
                ContactCommandEdit.RaiseCanExecuteChanged();
                ContactCommandDelete.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ContactCommand { get; }
        public DelegateCommand<Contact> ContactCommandEdit { get; }
        public DelegateCommand<Contact> ContactCommandDelete { get; }
        
        
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                _interaction = (navigationContext.Parameters["navigation"] as NavigationInteraction<Contact>)?.EditInteraction;
                _interaction?.DataProvider?.Invoke(DataProviderCallback);

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void DataProviderCallback(List<Contact> contacts)
        {
            try
            {
                Contacts = null;
                if (null == contacts) return;

                Contacts = new ObservableCollection<Contact>(contacts);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        #region Helper methods for Contact edit


        #region adding new contact

        private void OnContactCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<BaseModel, Contact>>().Publish(new EditChildEventArgs<BaseModel, Contact>() { EditObject = _interaction.InteractionObject, EditChildObject = null, EditMode = EditMode.Edit, EditChildMode = EditMode.New, SaveChildAction = OnAddContact });

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnAddContact(Contact contact, EditMode editMode)
        {
            try
            {
                if (null == contact) return;

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmAddContactCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = contact });

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmAddContactCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                Contact contact = args.PayLoad as Contact;
                if (null == contact) return;
                _interaction.SaveAction.Invoke(contact, EditMode.New);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #endregion


        #region updating existing contact

        private void OnContactCommandEdit(Contact obj)
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<BaseModel, Contact>>().Publish(new EditChildEventArgs<BaseModel, Contact>() { EditObject = _interaction.InteractionObject, EditChildObject = obj, EditMode = EditMode.Edit, EditChildMode = EditMode.Edit, SaveChildAction = OnUpdateContact });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnUpdateContact(Contact obj, EditMode editMode)
        {
            try
            {
                if (obj == null || !obj.IsDirty) return;
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmUpdateContactCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmUpdateContactCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;
                Contact contact = args.PayLoad as Contact;
                if (null == contact) return;
                _interaction.SaveAction.Invoke(contact, EditMode.Edit);
 
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        #endregion

        #region delete contact


        private void OnContactCommandDelete(Contact obj)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmContactDelete, Title = "ALO", Content = "Želiš izbrisati kontakt zaposlenega?", PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        private void OnConfirmContactDelete(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed)
                    return;

                Contact contact = args.PayLoad as Contact;
                if (null == contact) return;

                _interaction.SaveAction.Invoke(contact, EditMode.Delete);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion

        private bool CanExecuteContactCommandEdit(Contact arg)
        {
            return SelectedContact != null && _interaction.EditMode != EditMode.New;
        }

        #endregion

    }
}
