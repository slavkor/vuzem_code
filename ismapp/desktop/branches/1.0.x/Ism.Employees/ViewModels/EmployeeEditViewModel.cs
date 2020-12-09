using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using Ism.Infrastructure.Interaction;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;
using Ism.Employees.Commands;
using Prism;
using Ism.Infrastructure.Extensions;

namespace Ism.Employees.ViewModels
{
    public class EmployeeEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;
        private IAppCommands _appCommands;

        private EditInteraction<Employee> _notification;

        private Employee _employee;
        private EditMode _editMode;

        private bool _loaded;
        private ObservableCollection<Document> _documents;
        private ObservableCollection<Address> _addresses;
        private ObservableCollection<Contact> _contacts;
        private string _employeeImagePath;

        private List<MartialStatus> _martialStatuses;
        private MartialStatus _selectedMartialStatus;
        private ObservableCollection<Language> _languages;

        
        private CurrentWorkPeriod _currentWorkPeriod;
        private List<Language> DeleteSpokenList;
        private List<Language> AddSpokenList;

        private Action<List<Document>> _lastRefreshDocumentCallback;
        private Action<List<Document>> _lastRefreshOneDocumentCallback;
        private Action<List<Contact>> _lastRefreshContactCallback;
        private Action<List<Address>> _lastRefreshAddressCallback;

        private List<WorkPlace> _workPlaces;

        public EmployeeEditViewModel(ISettingsService settings, ISecurityService securityService, IAppCommands appCommands, IExceptionService exceptionService, ICommonService commonService)
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
            _commonService = commonService;
            try
            {
  

                ChangeEmployerCommand = new DelegateCommand(OnChangeEmployerCommand, () => (CurrentWorkPeriod == null || CurrentWorkPeriod.LastWorkedInCompany(_securityService.GetCurrentCompany())) && _editMode != EditMode.New);
                WorkPlaceCommand = new DelegateCommand(OnWorkPlaceCommand);
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);

                EmployeeImagePath = "/Ism.Infrastructure;component/Images/no-image.png";
                EmployeeChangeEmployerInteractionRequest = new InteractionRequest<EditInteraction<EmployeeChangeEmployer>>();
                SpokenLanguageChangeCommand = new DelegateCommand<Language>(OnSpokenLanguageChangeCommand, CanExecuteSpokenLanguageChangeCommand);

                CanSave = false;
            }
            catch (Exception exc)
            {
               _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }

        private void OnWorkPlaceCommand()
        {
            try
            {
                _eventAggregator.GetEvent<ListEvent<WorkPlace>>().Publish(new ListEventArgs<WorkPlace>() { SelectAction = OnWorkPlaceSelcted });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnWorkPlaceSelcted(WorkPlace workplace)
        {
            try
            {
                Employee.WorkPlace = workplace;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        #region public properties

        private bool _canSave;

        public List<WorkPlace> WorkPlaces
        {
            get
            {
                return _workPlaces;
            }
            set
            {
                SetProperty(ref _workPlaces, value);
            }
        }

        private WorkPlace _selectedWorkPlace;
        public WorkPlace SelectedWorkPlace
        {
            get
            {
                return _selectedWorkPlace;
            }
            set
            {
                SetProperty(ref _selectedWorkPlace, value);
                Employee.WorkPlace = value;
            }
        }

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
                ChangeEmployerCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Language> SpokenLanguages { get; set; }

        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }
        public List<MartialStatus> MartialStatuses
        {
            get { return _martialStatuses; }
            set{ SetProperty(ref _martialStatuses, value);}
        }

        public MartialStatus SelectedMartialStatus
        {
            get { return _selectedMartialStatus; }
            set
            {
                SetProperty(ref _selectedMartialStatus, value);
                if (Employee != null && Employee.MartialStatus != SelectedMartialStatus?.Name)
                    Employee.MartialStatus = SelectedMartialStatus?.Name;
            }
        }

        public DelegateCommand<Language> SpokenLanguageChangeCommand { get;  }

        public DelegateCommand ChangeEmployerCommand { get; }
        public DelegateCommand WorkPlaceCommand { get; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; }

        public Employee Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
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
        public InteractionRequest<EditInteraction<EmployeeChangeEmployer>> EmployeeChangeEmployerInteractionRequest { get; }

        public string EmployeeImagePath
        {
            get { return _employeeImagePath; }
            set
            {
                SetProperty(ref _employeeImagePath, value);
                
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

                if (!(value is EditInteraction<Employee>)) return;
                _notification = (EditInteraction<Employee>)value;
                _notification.SaveAction = SaveAction;
                EditMode = _notification.EditMode;
                LoadEmployeeData(_notification.EditMode == EditMode.New ? new Employee() : _notification.InteractionObject);
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

            var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Employee>;

            if (!(navigation.EditInteraction is EditInteraction<Employee>)) return;
            _notification = navigation.EditInteraction;
            _notification.SaveAction = SaveAction;
            EditMode = _notification.EditMode;
            WorkPlaces = _commonService.GetWorkPlaces();

            LoadEmployeeData(_notification.EditMode == EditMode.New ? new Employee() { UuId = Guid.NewGuid().ToString()} : _notification.InteractionObject);

            SelectedWorkPlace = WorkPlaces.Where(w => w.UuId == Employee?.WorkPlace?.UuId).FirstOrDefault();

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


            
            par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Contact>()
            {
                Header = "Možnosti",
                EditInteraction = new EditInteraction<Contact>()
                {
                    Title = "Možnosti",
                    EditMode = _editMode
                }
            });

            _regionManager.RequestNavigate(Infrastructure.RegionNames.EmployeesOptRegion, "EmployeesOptions", OnRequestNavigateCallBack, par);


            par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Document>()
            {
                Header = "Dokumenti",
                EditInteraction = new EditDocumentInteraction()
                {
                    Title = "Dokumenti",
                    SaveAction = OnSaveDocumentCallbackAction,
                    DataProvider = callback => { RefreshOneDocument(callback); },
                    EditMode = _editMode
                }
            });
            _regionManager.RequestNavigate(RegionNames.OneDocumentRegion, "OneDocument", OnRequestNavigateCallBack, par);


            par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<BusinessPartner>() { Header = "Zunanji delodajalec", EditInteraction = new EditInteraction<BusinessPartner>() { Title = "Stranka", SelectAction = OnSelectOnSelectExternalEmployerCallback, InteractionObject = Employee.Loaner} });
            _regionManager.RequestNavigate(RegionNames.ExternalEmployerRegion, "BusinessPartnerOverView", OnRequestNavigateCallBack, par);

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
                if(!_loaded)
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
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(Employee, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "employees/addContact").ToString(), addContact, (e) =>
                                    {
                                        RefreshContacts(_lastRefreshContactCallback);
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddContact<BaseModel>>>())
                                {
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(Employee, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/contact/update").ToString(),
                                      addContact, (e) =>
                                      {
                                          RefreshContacts(_lastRefreshContactCallback);
                                      });
                                }
                                break;
                            case EditMode.Delete:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Contact, Contact>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/contact/delete").ToString(), contact,
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
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(Employee, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "employees/addAddress").ToString(), updateAddress, (e) =>
                                    {
                                        RefreshAddresses(_lastRefreshAddressCallback);
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                if (null == address) return;
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                                {
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(Employee, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(
                                        new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/address/update").ToString(),
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
                                    var url = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/address/delete");

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
                            case EditMode.Cancel:
                            case EditMode.Extend:
                                AddDocument<Employee> addDocument = new AddDocument<Employee>(Employee, document);
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, AddDocument<Employee>>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "employees/addDocument").ToString(), addDocument, (e) =>
                                    {
                                        try
                                        {
                                            RefreshDocuments(_lastRefreshDocumentCallback);
                                            if (document.Type.Name == "SLIKA") DownloadEmployeeImage(Employee);
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
                                if (document.Type.Name == "SLIKA") DownloadEmployeeImage(Employee);
                                break;
                            case EditMode.Delete:
                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Document, Document>>())
                                {
                                    var url = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/document/delete");
                                    repositroy.PostRequestAsync(url.ToString(), document, _securityService.GetCurrentUser().AccessToken, (e) => {

                                        try
                                        {
                                            RefreshDocuments(_lastRefreshDocumentCallback);
                                            if (document.Type.Name == "SLIKA") DownloadEmployeeImage(Employee);
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

        private void RefreshMartialStatuses()
        {
            try
            {
                var common = _serviceLocator.GetInstance<ICommonService>();
                MartialStatuses = common.GetMartilaStatuses();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void RefresLanguages()
        {
            try
            {
                var common = _serviceLocator.GetInstance<ICommonService>();
                Languages = new ObservableCollection<Language>(common.GetLanguages());

                SpokenLanguages = new ObservableCollection<Language>();

                if (string.IsNullOrEmpty(Employee.UuId)) return;


                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Language>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/spokenlang").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (
                            e) =>
                        {
                            if (e?.Count == 0) return;

                            Languages.ForEach(item => { item.IsSelected = e.Any(l => l.UuId == item.UuId); });
                            //Languages.

                            

                            e.ForEach(l => Languages.FirstOrDefault(g => g.UuId == l.UuId).IsSelected = true);

                            SpokenLanguages.AddRange(e);
                        });
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
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/address").ToString(),
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

                using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/contact").ToString(),
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
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/document").ToString(),
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

        private void RefreshOneDocument(Action<List<Document>> callback)
        {
            try
            {
                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/document").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (e) =>
                        {

                            callback.Invoke(e == null ? null : e.Documents == null ? null : e.Documents.ToList());
                            _lastRefreshOneDocumentCallback = callback;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveAction(Employee obj, EditMode editMode)
        {
            try
            {
                SaveEmployee(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveEmployee(Employee obj, bool finish = true)
        {
            try
            {
                if (!Employee.IsDirty || !SaveCommand.CanExecute())
                {
                    if (finish) OnFinishInteraction();
                    return;
                }
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() {CallBackAction = OnConfirmSaveEmployeeCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish ,PayLoad = obj});
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnConfirmSaveEmployeeCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {

                if (!confirmed || !Employee.IsDirty)
                {
                    if(args.FinishUp) OnFinishInteraction();
                    return;
                }

                Employee employee = args.PayLoad as Employee;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Employee, Employee>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_settings.GetApiServer(), "employees/add")
                        : new Uri(_settings.GetApiServer(), "employees/update");

                    repositroy.PostRequestAsync(url.ToString(), employee,
                        _securityService.GetCurrentUser().AccessToken,
                        (e) =>
                        {
                            if (_notification.EditMode == EditMode.New)
                            {

                                Employee = e;
                                Employee.Loaner = employee.Loaner;
                                OnEmployerSelectCallback(_securityService.GetCurrentCompany());
                            }
                            else
                            {
                                if (null != SelectedWorkPlace)
                                {
                                    using (var rep = _serviceLocator.GetInstance<IRestRepository<object, WorkPlace>>())
                                    {
                                        Uri uri = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/workplace");
                                        rep.PostRequestAsync(
                                            uri.ToString(),
                                            SelectedWorkPlace, (ee) => { });
                                    }
                                }

                                if (args.FinishUp) OnFinishInteraction();
                            }
                                
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

                if(null != Documents)
                {
                    // ADD DOCUMENTS
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, AddDocument<Employee>>>())
                    {
                        foreach (var document in Documents)
                        {

                            AddDocument<Employee> addDocument = new AddDocument<Employee>(Employee, document);
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "employees/addDocument").ToString(), addDocument, (e) =>
                            {
                            });
                        }
                    }
                }

                if(null != Addresses)
                {
                    // ADD ADDRESSES
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                    {

                        foreach (var address in Addresses)
                        {
                            if (null == address) continue;
                            AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(Employee, address) { UuId = address.UuId };
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "employees/addAddress").ToString(), updateAddress, (e) =>
                            {
                                //RefreshEmployeeAddresses();
                            });

                        }
                    }

                }
                
                if(null != Contacts)
                {

                    // ADD CONTACTS
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddContact<BaseModel>>>())
                    {
                        foreach (var contact in Contacts)
                        {
                            if (null == contact) continue;
                            AddContact<BaseModel> updateContact = new AddContact<BaseModel>(Employee, contact) { UuId = contact.UuId };
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "employees/addContact").ToString(), updateContact, (e) =>
                            {
                                //RefreshEmployeeContacts();
                            });
                        }
                    }
                }


                // ADD LANGUAGES
                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, Language>>())
                {
                    if(null != DeleteSpokenList)
                    {
                        foreach (var language in DeleteSpokenList)
                        {
                            Uri uri = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/spokenlang/delete");
                            rep.PostRequestAsync(uri.ToString(), language, (e) => { });
                        }
                    }

                    if(null != AddSpokenList)
                    {
                        foreach (var language in AddSpokenList)
                        {
                            Uri uri = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/spokenlang/add");
                            rep.PostRequestAsync(uri.ToString(), language, (e) => { });
                        }
                    }
                }


                if (null != Employee.Loaner)
                {
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<object, BusinessPartner>>())
                    {
                        Uri uri = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/externalemployer");
                        rep.PostRequestAsync(
                            uri.ToString(),
                            Employee.Loaner, (e) => { });
                    }
                }

                if (null != SelectedWorkPlace)
                {
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<object, WorkPlace>>())
                    {
                        Uri uri = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/workplace");
                        rep.PostRequestAsync(
                            uri.ToString(),
                            SelectedWorkPlace, (ee) => { });
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
        private void LoadEmployeeData(Employee employee)
        {
            try
            {
                Employee = employee;
                Employee.Errors.ValidateProperties();
                Employee.ErrorsChanged += (sender, args) =>
                {
    
                    CanSave = true;
                    SaveCommand.RaiseCanExecuteChanged();
                };

                Employee.PropertyDeletegate = OnPropertyChange;

                RefreshMartialStatuses();
                RefresLanguages();
                RefreshWorkPeriod();
                SelectedMartialStatus = MartialStatuses.FirstOrDefault(s => s.Name == Employee.MartialStatus);

                Employee.IsDirty = false;
                DownloadEmployeeImage(Employee);
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
                SaveEmployee(Employee);
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
            return Employee != null && Employee.IsDirty && !Employee.HasErrors;
        }
        private void OnChangeEmployerCommand()
        {
            try
            {
                _eventAggregator.GetEvent<ListEvent<Company>>().Publish(new ListEventArgs<Company>() { SelectAction = OnEmployerSelectCallback });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                throw;
            }
        }

        private void RefreshWorkPeriod()
        {
            try
            {
                if(null == Employee?.UuId) return;
                

                using (var rep = _serviceLocator.GetInstance<IRestRepository<CurrentWorkPeriod, object>>())
                {
                    var url = new Uri(_settings.GetApiServer(true), $"employees/{Employee.UuId}/workperiods/current");
                    rep.GetRequestAsync(url.ToString(), _securityService.GetCurrentUser().AccessToken,
                        currentwork =>
                        {
                            CurrentWorkPeriod = currentwork;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private bool CanExecuteSpokenLanguageChangeCommand(Language arg)
        {
            return _loaded;
        }
        private void OnSpokenLanguageChangeCommand(Language language)
        {
            try
            {
                if (language == null) return;
                if (Employee == null) return;

                switch (_editMode)
                {
                    case EditMode.New:
                        if (DeleteSpokenList == null) DeleteSpokenList = new List<Language>();
                        if (AddSpokenList == null) AddSpokenList = new List<Language>();

                        if (language.IsSelected)
                            if (!AddSpokenList.Any(l => l.Lang == language.Lang)) AddSpokenList.Add(language);
                        else
                            if (!DeleteSpokenList.Any(l => l.Lang == language.Lang)) DeleteSpokenList.Add(language);

                        break;
                    default:
                        Uri uri = language.IsSelected
                            ? new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/spokenlang/add")
                            : new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/spokenlang/delete");

                        using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, Language>>())
                        {
                            rep.PostRequestAsync(
                                uri.ToString(),
                                language, (e) => { });
                        }
                        break;
                }

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnEmployerSelectCallback(Company obj)
        {
            try
            {
                if (null == obj) return;
                

                var employee = new EmployeeChangeEmployer()
                    {
                        Employee = Employee,
                        CompanyFire = _securityService.GetCurrentCompany(),
                        CompanyHire = obj
                    };

                using (var rep = _serviceLocator.GetInstance<IRestRepository<CurrentWorkPeriod, object>>())
                {
                    var url = new Uri(_settings.GetApiServer(), $"employees/{employee.Employee.UuId}/workperiods/current");
                    rep.GetRequestAsync(url.ToString(), _securityService.GetCurrentUser().AccessToken,
                        currentwork =>
                        {
                            employee.CurentWorkPeriod = currentwork;
                            
                            //var allCompanies = _securityService.GetAllCompanies();
                            EmployeeChangeEmployerInteractionRequest.Raise(new EditInteraction<EmployeeChangeEmployer>() { Title = employee.CompanyFire.ShortName == employee.CompanyHire.ShortName ? "Delodajalec za" : "Menjava delodajalca za", TitleExtendet = Employee.ToString(), InteractionObject = employee, EditMode = employee.CompanyFire.ShortName == employee.CompanyHire.ShortName ? EditMode.Edit : EditMode.New }, OnEmployeeChangeEmployerInteractionRequest);
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnEmployeeChangeEmployerInteractionRequest(EditInteraction<EmployeeChangeEmployer> interaction)
        {
            try
            {
                if (!interaction.Confirmed) return;
                if (!CheckEmployeeHireAndFireData(interaction.InteractionObject)) return;

                var emp = new EmployeeFire() { HireEmployer = interaction.InteractionObject.CompanyHire, FireEmployer = interaction.InteractionObject.CompanyFire };
                if (interaction.InteractionObject.HireDate.HasValue) emp.HireDate = interaction.InteractionObject.HireDate.Value;
                if (interaction.InteractionObject.FireDate.HasValue) emp.FireDate = interaction.InteractionObject.FireDate.Value;
                emp.InjectFrom(interaction.InteractionObject.Employee);

                switch (interaction.EditMode)
                {
                    case EditMode.New: // trenutni delodajalec je drugačen od izbranega (menjava delodajalca)


                        using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, EmployeeHire>>())
                        {
                            var url = new Uri(_settings.GetApiServer(emp.FireEmployer.UuId), "employees/fire");
                            rep.PostRequestAsync(url.ToString(), emp,
                                _securityService.GetCurrentUser().AccessToken,
                                (e) =>
                                {

                                    using (var reph = _serviceLocator.GetInstance<IRestRepository<Employee, EmployeeHire>>())
                                    {
                                        var urlh = new Uri(_settings.GetApiServer(emp.HireEmployer.UuId), "employees/hire");
                                        reph.PostRequestAsync(urlh.ToString(), emp,
                                            _securityService.GetCurrentUser().AccessToken,
                                            (eh) =>
                                            {
                                                OnFinishInteraction();
                                            });
                                    }
                                });
                        }
                        
                        break;
                    case EditMode.Edit: // trenutni delodajalec je enak izbranemu (urejanje datuma prijave odjave)

                        using (var reph = _serviceLocator.GetInstance<IRestRepository<Employee, EmployeeHire>>())
                        {
                            var urlh = new Uri(_settings.GetApiServer(emp.HireEmployer.UuId), "employees/hire");
                            reph.PostRequestAsync(urlh.ToString(), emp,
                                _securityService.GetCurrentUser().AccessToken,
                                (eh) =>
                                {
                                    if (emp.FireDate != null)
                                    {

                                        using (var repf = _serviceLocator.GetInstance<IRestRepository<Employee, EmployeeHire>>())
                                        {
                                            var urlf = new Uri(_settings.GetApiServer(emp.HireEmployer.UuId), "employees/fire");
                                            repf.PostRequestAsync(urlf.ToString(), emp,
                                                _securityService.GetCurrentUser().AccessToken,
                                                (ef) =>
                                                {
                                                    OnFinishInteraction();
                                                });
                                        }
                                    }
                                    else
                                    {
                                        SaveRelatedData();
                                        OnFinishInteraction();
                                    }
                                });
                        }
                        //SetEmployer(interaction.InteractionObject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            finally
            {
                //OnFinishInteraction();
            }
        }
        private bool CheckEmployeeHireAndFireData(EmployeeChangeEmployer data)
        {
            try
            {
                return true;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<EmployeeList, object>>())
                {
                    if (data.CompanyHire.ShortName == data.CompanyFire?.ShortName)
                    {
                        var url = new Uri(_settings.GetApiServer(data.CompanyHire.UuId),
                            $"employees/{data.Employee.UuId}/workperiods/current");
                        var periodh = rep.GetRequest(url.ToString(), _securityService.GetCurrentUser().AccessToken);

                        if (data.CurentWorkPeriod != null && data.CurentWorkPeriod.Current.Company.ShortName != data.CompanyHire.ShortName)
                        {
                            _eventAggregator.GetEvent<RaiseException>()
                                .Publish(new Exception(
                                    $"Zaposleni {data.Employee} je trenutno prijavljen na podjetju {data.CurentWorkPeriod.Current.Company}! Zamenjaj podjetje. "));
                        }

                        if (periodh == null) return true;
                        if (periodh.WorkPeriod.End > data.HireDate)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (null != data.CompanyFire)
                        {

                            var url = new Uri(_settings.GetApiServer(data.CompanyHire.UuId),
                                $"employees/{data.Employee.UuId}/workperiods/current");
                            var periodh = rep.GetRequest(url.ToString(), _securityService.GetCurrentUser().AccessToken);
                            if (periodh != null && periodh.WorkPeriod.End > data.HireDate)
                            {
                                _exceptionService.RaiseException(new Exception($"Zaposleni {data.Employee} je bil na podjetju {periodh.Company} odjavlen na dan {periodh.WorkPeriod.End}.\n Zato ga ni možno prijaviti na dan {data.HireDate}"));
                                return false;
                            }

                            url = new Uri(_settings.GetApiServer(data.CompanyFire.UuId),
                                $"employees/{data.Employee.UuId}/workperiods/current");
                            var periodf = rep.GetRequest(url.ToString(), _securityService.GetCurrentUser().AccessToken);

                            if (periodf.WorkPeriod.Active == 0 && periodf.WorkPeriod.Start > data.HireDate)
                            {
                                _exceptionService.RaiseException(new Exception($"Zaposleni {data.Employee} je bil na podjetju {periodf.Company} prijavljen na dan {periodf.WorkPeriod.Start}.\n Zato ga ni možno prijaviti na dan {data.HireDate}"));
                                return false;
                            }
                        }
                        else
                        {
                            var url = new Uri(_settings.GetApiServer(data.CompanyHire.UuId),
                                $"employees/{data.Employee.UuId}/workperiods/current");
                            var periodh = rep.GetRequest(url.ToString(), _securityService.GetCurrentUser().AccessToken);
                            if (periodh.WorkPeriod.End > data.HireDate) return false;

                        }

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }
        private void DownloadEmployeeImage(Employee obj)
        {
            try
            {
                if(obj?.UuId == null) return;

                EmployeeImagePath = "/Ism.Infrastructure;component/Images/no-image.png";

                var commons = _serviceLocator.GetInstance<ICommonService>();

                using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{obj.UuId}/document/{commons.GetDocumentTypes().FirstOrDefault(t=> t.Name == "SLIKA")?.Name}").ToString(), _securityService.GetCurrentUser().AccessToken, employee =>
                        {
                            var doc = employee?.Documents?.FirstOrDefault();

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
                                    EmployeeImagePath = fileName;
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
                EmployeeImagePath = "/Ism.Infrastructure;component/Images/no-image.png";
                Employee = null;
                Addresses = null;
                Contacts = null;
                Documents = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        private void OnSelectOnSelectExternalEmployerCallback(BusinessPartner obj)
        {
            try
            {
                Employee.Loaner = obj;

                switch (_editMode)
                {
                    case EditMode.New:
                        Employee.Loaner = obj;

                        break;
                    default:
                        Uri uri = new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/externalemployer");
                        using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, BusinessPartner>>())
                        {
                            rep.PostRequestAsync(
                                uri.ToString(),
                                obj, (e) => { });
                        }
                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
