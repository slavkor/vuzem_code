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
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;
using System.Collections.ObjectModel;
using System.IO;

namespace Ism.BusinessPartners.ViewModels
{
    public class BusinessPartnerEditViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private IAppCommands _appCommands;

        private EditInteraction<BusinessPartner> _notification;

        private BusinessPartner _partner;
        private EditMode _editMode;

        private bool _loaded;
        private ObservableCollection<Document> _documents;
        private ObservableCollection<Address> _addresses;
        private ObservableCollection<Contact> _contacts;
        private string _partnerImagePath;

        private List<MartialStatus> _martialStatuses;
        private MartialStatus _selectedMartialStatus;
        private List<Language> _languages;
        private CurrentWorkPeriod _currentWorkPeriod;
        private List<Language> DeleteSpokenList;
        private List<Language> AddSpokenList;

        private Action<List<Document>> _lastRefreshDocumentCallback;
        private Action<List<Contact>> _lastRefreshContactCallback;
        private Action<List<Address>> _lastRefreshAddressCallback;

        public BusinessPartnerEditViewModel(ISettingsService settings, ISecurityService securityService, IAppCommands appCommands, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            if (null == appCommands)
                throw new ArgumentNullException(nameof(appCommands));

            _settings = settings;
            _securityService = securityService;
            _appCommands = appCommands;
            _exceptionService = exceptionService;

            try
            {
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                PartnerImagePath = "/Ism.Infrastructure;component/Images/no-image.png";
                CanSave = false;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }

        #region public properties

        private bool _canSave;


        public bool CanSave
        {
            get
            {
                return _canSave;
            }
            set
            {
                SetProperty(ref _canSave, value);
            }
        }

        public CurrentWorkPeriod CurrentWorkPeriod
        {
            get { return _currentWorkPeriod; }
            set
            {
                SetProperty(ref _currentWorkPeriod, value);
            }
        }

        public List<Language> SpokenLanguages { get; set; }

        public List<Language> Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }

        public List<MartialStatus> MartialStatuses
        {
            get { return _martialStatuses; }
            set { SetProperty(ref _martialStatuses, value); }
        }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; }
        public BusinessPartner BusinessPartner
        {
            get { return _partner; }
            set { SetProperty(ref _partner, value); }
        }

        public ObservableCollection<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty(ref _addresses, value);
            }
        }
        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                SetProperty(ref _contacts, value);

            }
        }
        public ObservableCollection<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

            }
        }

        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public string PartnerImagePath
        {
            get { return _partnerImagePath; }
            set
            {
                SetProperty(ref _partnerImagePath, value);

            }
        }

        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                _loaded = false;

                if (!(value is EditInteraction<BusinessPartner>)) return;
                _notification = (EditInteraction<BusinessPartner>)value;
                _notification.SaveAction = SaveAction;
                EditMode = _notification.EditMode;
                LoadPartnerData(_notification.EditMode == EditMode.New ? new BusinessPartner() : _notification.InteractionObject);
                _loaded = true;
            }
        }
        public Action FinishInteraction { get; set; }


        #endregion


        #region VieModelBase overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {

            _loaded = false;
            Clear();
            base.OnNavigatedTo(navigationContext);

            var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<BusinessPartner>;

            if (!(navigation.EditInteraction is EditInteraction<BusinessPartner>)) return;
            _notification = navigation.EditInteraction;
            _notification.SaveAction = SaveAction;
            EditMode = _notification.EditMode;
            LoadPartnerData(_notification.EditMode == EditMode.New ? new BusinessPartner() { UuId = Guid.NewGuid().ToString() } : _notification.InteractionObject);


            var par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Document>()
            {
                Header = "Dokumenti",
                EditInteraction = new EditDocumentInteraction()
                {
                    Title = "Dokumenti",
                    SaveAction = OnSaveDocumentCallbackAction,
                    DataProvider = callback => { RefreshDocuments(callback); },
                    EditMode = _editMode
                }
            });
            _regionManager.RequestNavigate(RegionNames.DocumentsRegion, "DocmentsExt", OnRequestNavigateCallBack, par);

            par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Contact>()
            {
                Header = "Kontakti",
                EditInteraction = new EditInteraction<Contact>()
                {
                    Title = "Kontakti",
                    SaveAction = OnSaveContactCallbackAction,
                    DataProvider = callback => { RefreshContacts(callback); },
                    EditMode = _editMode
                }
            });
            _regionManager.RequestNavigate(RegionNames.ContactsRegion, "Contacts", OnRequestNavigateCallBack, par);


            par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Address>()
            {
                Header = "Naslovi",
                EditInteraction = new EditInteraction<Address>()
                {
                    Title = "Naslovi",
                    SaveAction = OnSaveAddressCallbackAction,
                    DataProvider = callback => { RefreshAddresses(callback); },
                    EditMode = _editMode
                }
            });
            _regionManager.RequestNavigate(RegionNames.AddressRegion, "Addresses", OnRequestNavigateCallBack, par);

            _loaded = true;

        }

        public override bool KeepAlive => false;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            _appCommands.SaveCommand.UnregisterCommand(SaveCommand);
        }


        private void OnRequestNavigateCallBack(NavigationResult obj)
        {
            try
            {

                int a = 0;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        #endregion


        private void OnPropertyChange(BaseModel model)
        {
            try
            {
                if (!_loaded)
                    return;

                CanSave = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnSaveContactCallbackAction(Contact contact, EditMode editMode)
        {
            try
            {
                switch (_editMode)
                {
                    case EditMode.New:
                        if (Contacts == null) Contacts = new ObservableCollection<Contact>();
                        Contacts.Add(contact);
                        _lastRefreshContactCallback.Invoke(Contacts.ToList());
                        break;
                    default:
                        switch (editMode)
                        {
                            case EditMode.New:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddContact<BaseModel>>>())
                                {
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(BusinessPartner, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "partners/addContact").ToString(), addContact, (e) =>
                                    {
                                        RefreshContacts(_lastRefreshContactCallback);
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddContact<BaseModel>>>())
                                {
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(BusinessPartner, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/contact/update").ToString(),
                                      addContact, (e) =>
                                      {
                                          RefreshContacts(_lastRefreshContactCallback);
                                      });
                                }
                                break;
                            case EditMode.Delete:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Contact, Contact>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/contact/delete").ToString(), contact,
                                   _securityService.GetCurrentUser().AccessToken,
                                   (e) =>
                                   {
                                       RefreshContacts(_lastRefreshContactCallback);
                                   });
                                }
                                break;

                        }
                        break;

                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
        private void OnSaveAddressCallbackAction(Address address, EditMode editMode)
        {
            try
            {
                switch (_editMode)
                {
                    case EditMode.New:
                        if (Addresses == null) Addresses = new ObservableCollection<Address>();
                        Addresses.Add(address);
                        _lastRefreshAddressCallback.Invoke(Addresses.ToList());
                        break;
                    default:
                        switch (editMode)
                        {
                            case EditMode.New:
                                if (null == address) return;
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                                {
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(BusinessPartner, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "partners/addAddress").ToString(), updateAddress, (e) =>
                                    {
                                        RefreshAddresses(_lastRefreshAddressCallback);
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                if (null == address) return;
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                                {
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(BusinessPartner, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(
                                        new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/address/update").ToString(),
                                        updateAddress, (e) =>
                                        {
                                            RefreshAddresses(_lastRefreshAddressCallback);
                                        });
                                }
                                break;
                            case EditMode.Delete:
                                if (null == address) return;

                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Address, Address>>())
                                {
                                    var url = new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/address/delete");

                                    repositroy.PostRequestAsync(url.ToString(), address,
                                        _securityService.GetCurrentUser().AccessToken,
                                        (e) =>
                                        {
                                            RefreshAddresses(_lastRefreshAddressCallback);
                                        });
                                }
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnSaveDocumentCallbackAction(Document document, EditMode editMode)
        {
            try
            {
                switch (_editMode)
                {
                    case EditMode.New:

                        if (null == Documents) Documents = new ObservableCollection<Document>();
                        Documents.Add(document);
                        _lastRefreshDocumentCallback.Invoke(Documents.ToList());
                        break;
                    default:
                        switch (editMode)
                        {
                            case EditMode.New:
                                AddDocument<BusinessPartner> addDocument = new AddDocument<BusinessPartner>(BusinessPartner, document);
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, AddDocument<BusinessPartner>>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "partners/addDocument").ToString(), addDocument, (e) =>
                                    {
                                        try
                                        {
                                            RefreshDocuments(_lastRefreshDocumentCallback);
                                            if (document.Type.Name == "LOGO") DownloadPartnerImage(BusinessPartner);
                                        }
                                        catch (Exception exception)
                                        {
                                            _exceptionService.RaiseException(exception);
                                        }

                                    });
                                }
                                break;
                            case EditMode.Edit:
                                RefreshDocuments(_lastRefreshDocumentCallback);
                                if (document.Type.Name == "LOGO") DownloadPartnerImage(BusinessPartner);
                                break;
                            case EditMode.Delete:
                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Document, Document>>())
                                {
                                    var url = new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/document/delete");
                                    repositroy.PostRequestAsync(url.ToString(), document, _securityService.GetCurrentUser().AccessToken, (e) => {

                                        try
                                        {
                                            RefreshDocuments(_lastRefreshDocumentCallback);
                                            if (document.Type.Name == "LOGO") DownloadPartnerImage(BusinessPartner);
                                        }
                                        catch (Exception exception)
                                        {
                                            _exceptionService.RaiseException(exception);
                                        }

                                    });
                                }
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void RefreshAddresses(Action<List<Address>> callback)
        {
            try
            {

                Addresses = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/address").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (
                            e) =>
                        {
                            callback.Invoke(e == null ? null : e.Addresses == null ? null : e.Addresses.ToList());
                            _lastRefreshAddressCallback = callback;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void RefreshContacts(Action<List<Contact>> callback)
        {
            try
            {
                Contacts = null;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/contact").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (
                            e) =>
                        {
                            callback.Invoke(e == null ? null : e.Contacts == null ? null : e.Contacts.ToList());
                            _lastRefreshContactCallback = callback;

                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void RefreshDocuments(Action<List<Document>> callback)
        {
            try
            {
                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"partners/{BusinessPartner.UuId}/document").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (e) =>
                        {
                            callback.Invoke(e == null ? null : e.Documents == null ? null : e.Documents.ToList());
                            _lastRefreshDocumentCallback = callback;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveAction(BusinessPartner obj, EditMode editMode)
        {
            try
            {
                SavePartner(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SavePartner(BusinessPartner obj, bool finish = true)
        {
            try
            {
                if (!BusinessPartner.IsDirty || !SaveCommand.CanExecute())
                {
                    if (finish) OnFinishInteraction();
                    return;
                }
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSavePartnerCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish, PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnConfirmSavePartnerCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {

                if (!confirmed || !BusinessPartner.IsDirty)
                {
                    if (args.FinishUp) OnFinishInteraction();
                    return;
                }

                BusinessPartner partner = args.PayLoad as BusinessPartner;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, BusinessPartner>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_settings.GetApiServer(), "partners/add")
                        : new Uri(_settings.GetApiServer(), "partners/update");

                    repositroy.PostRequestAsync(url.ToString(), partner,
                        _securityService.GetCurrentUser().AccessToken,
                        (e) =>
                        {
                            if (_notification.EditMode == EditMode.New)
                            {
                                BusinessPartner = e;
                            }
                            else
                                if (args.FinishUp) OnFinishInteraction();
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                OnFinishInteraction();
            }
        }
        private void SaveRelatedData()
        {
            try
            {
                if (_editMode != EditMode.New) return;

                if (null != Documents)
                {
                    // ADD DOCUMENTS
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, AddDocument<BusinessPartner>>>())
                    {
                        foreach (var document in Documents)
                        {

                            AddDocument<BusinessPartner> addDocument = new AddDocument<BusinessPartner>(BusinessPartner, document);
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "partners/addDocument").ToString(), addDocument, (e) =>
                            {
                            });
                        }
                    }
                }

                if (null != Addresses)
                {
                    // ADD ADDRESSES
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                    {

                        foreach (var address in Addresses)
                        {
                            if (null == address) continue;
                            AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(BusinessPartner, address) { UuId = address.UuId };
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "partners/addAddress").ToString(), updateAddress, (e) =>
                            {
                                //RefreshPartnerAddresses();
                            });

                        }
                    }

                }

                if (null != Contacts)
                {

                    // ADD CONTACTS
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddContact<BaseModel>>>())
                    {
                        foreach (var contact in Contacts)
                        {
                            if (null == contact) continue;
                            AddContact<BaseModel> updateContact = new AddContact<BaseModel>(BusinessPartner, contact) { UuId = contact.UuId };
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "partners/addContact").ToString(), updateContact, (e) =>
                            {
                                //RefreshPartnerContacts();
                            });
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
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
        private void LoadPartnerData(BusinessPartner partner)
        {
            try
            {
                BusinessPartner = partner;
                BusinessPartner.Errors.ValidateProperties();
                BusinessPartner.ErrorsChanged += (sender, args) =>
                {

                    CanSave = true;
                    SaveCommand.RaiseCanExecuteChanged();
                };

                BusinessPartner.PropertyDeletegate = OnPropertyChange;
                BusinessPartner.IsDirty = false;
                DownloadPartnerImage(BusinessPartner);
                CanSave = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnSaveCommand()
        {
            try
            {
                SavePartner(BusinessPartner);
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
                OnFinishInteraction();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private bool CanExecuteSaveCommand()
        {
            return BusinessPartner != null && BusinessPartner.IsDirty && !BusinessPartner.HasErrors;
        }
        private void DownloadPartnerImage(BusinessPartner obj)
        {
            try
            {
                if (obj?.UuId == null) return;

                PartnerImagePath = "/Ism.Infrastructure;component/Images/no-image.png";

                var commons = _serviceLocator.GetInstance<ICommonService>();

                using (var rep = _serviceLocator.GetInstance<IRestRepository<BusinessPartner, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"partners/{obj.UuId}/document/{commons.GetDocumentTypes().FirstOrDefault(t => t.Name == "LOGO")?.Name}").ToString(), _securityService.GetCurrentUser().AccessToken, BusinessPartner =>
                    {
                        var doc = BusinessPartner?.Documents?.FirstOrDefault();

                        using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                        {
                            var file = doc?.Files?.FirstOrDefault();
                            if (null == file) return;

                            string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_{file.UniqueName}_{file.Name}");
                            var url = new Uri(_settings.GetApiServer(false), $"documents/{doc.UuId}/files/{file.UuId}");
                            repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentUser(), null, inputStream =>
                            {
                                using (inputStream)
                                {
                                    using (var outputStream = System.IO.File.OpenWrite(fileName))
                                    {
                                        inputStream.CopyTo(outputStream);
                                    }
                                }
                                PartnerImagePath = fileName;
                            }, "Pridobivam sliko zaposlenega...", false);
                        }
                    }
                    );
                }
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
                PartnerImagePath = "/Ism.Infrastructure;component/Images/no-image.png";
                BusinessPartner = null;
                Addresses = null;
                Contacts = null;
                Documents = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
