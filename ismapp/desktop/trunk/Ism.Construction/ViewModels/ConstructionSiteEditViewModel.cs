using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
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
using TimeLineTool;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Construction.ViewModels
{
    public class ConstructionSiteEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private EditInteraction<ConstructionSite> _notification;

        private ConstructionSite _constructionSite;
        private EditMode _editMode;

        private bool _loaded;
        private ObservableCollection<Document> _documents;
        private ObservableCollection<Address> _addresses;
        private ObservableCollection<Contact> _contacts;

        private NavigationContext _navigationContext;
        private Action<List<Document>> _lastDocumentRefreshCallback;
        private Action<List<Project>> _lastProjectRefreshCallback;
        private Action<List<Contact>> _lastRefreshContactCallback;
        private Action<List<Address>> _lastRefreshAddressCallback;

        private List<Project> AddProjects;

        public ConstructionSiteEditViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {


                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                WorkHoursAndEwrCommand = new DelegateCommand(OnWorkHoursAndEwrCommand);
                _eventAggregator.GetEvent<Events.NavigateBpEvent>().Subscribe(() => {
                    _regionManager.RequestNavigate(RegionNames.CsiteBpRegion, "BusinessPartnerOverView", OnRequestNavigateCallBack);
                });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }

        public bool ProjectsEnabled{get;set;}
        private void OnWorkHoursAndEwrCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditEvent<ForemanConstructionSite>>().Publish(new EditEventArgs<ForemanConstructionSite>() { EditObject = new ForemanConstructionSite() { ConstructionSite = ConstructionSite, Projects = Projects } });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #region public properties

        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                SetProperty(ref _contacts, value);

            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public ConstructionSite ConstructionSite
        {
            get { return _constructionSite; }
            set { SetProperty(ref _constructionSite, value); }
        }
        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }
        public DelegateCommand DocumentCommand { get; }
        public DelegateCommand<Document> DocumentCommandEdit { get; }
        public DelegateCommand<Document> DocumentCommandDelete { get; }

        public DelegateCommand WorkHoursAndEwrCommand { get; }

        public ObservableCollection<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

            }
        }

        public List<Project> Projects{ get; set; }
        #endregion


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
        
        private void OnPropertyChange(BaseModel model)
        {
            try
            {
                if (!_loaded)
                    return;

                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnSaveProjectCallbackAction(Project project, EditMode editMode)
        {
            switch (EditMode)
            {
                case EditMode.New:
                    switch (editMode)
                    {
                        case EditMode.New:
                            //AddProjects.Add(project);
                            break;
                        case EditMode.Edit:

                            break;
                    }

                    break;
                case EditMode.Edit:
                    switch (editMode)
                    {
                        case EditMode.New:
                            using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, Project>>())
                            {
                                rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/project/add").ToString(), project, (e) =>
                                {
                                    RefreshProjects(_lastProjectRefreshCallback);
                                });
                            }
                            break;
                        case EditMode.Edit:
                            using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, Project>>())
                            {
                                rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/project/update").ToString(), project, (e) =>
                                {
                                    RefreshProjects(_lastProjectRefreshCallback);
                                });
                            }
                            break;
                    }
                    break;
            }
        }

        private void RefreshDocuments(Action<List<Document>> callback)
        {
            try
            {
                if (ConstructionSite?.UuId == null) return;
                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/document").ToString(),
                        _securityService.GetCurrentToken(), (
                            e) =>
                        {
                            callback.Invoke(e == null? null: e.Documents.ToList());
                            _lastDocumentRefreshCallback = callback;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveAction(ConstructionSite obj, EditMode editMode)
        {
            try
            {
                SaveConstructionSite(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveConstructionSite(ConstructionSite obj, bool finish = true)
        {
            try
            {
                if (!ConstructionSite.IsDirty)
                {
                    if (finish) OnFinishInteraction();
                    return;
                }

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveConstructionSiteCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish, PayLoad = obj });

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnConfirmSaveConstructionSiteCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed || !ConstructionSite.IsDirty)
                {
                    if (args.FinishUp) OnFinishInteraction();
                    return;
                }

                var construction = args.PayLoad as ConstructionSite;

                switch (_notification.EditMode)
                {
                    case EditMode.New:
                        AddConstructionSite(construction);
                        break;
                    case EditMode.Edit:
                        UpdateConstructionSite(construction);
                        break;
                    case EditMode.List:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                OnFinishInteraction();
            }
        }

        private void AddConstructionSite(ConstructionSite site)
        {
            try
            {
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, ConstructionSite>>())
                {

                    var url = new Uri(_settings.GetApiServer(), "csite/add");

                    repositroy.PostRequestAsync(url.ToString(), site,
                        _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            try
                            {
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, Project>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/project/add").ToString(), new Project() { UuId = Guid.NewGuid().ToString(), ProjectNumber = DateTime.Now.ToProjectNumber(), Start = new Day(DateTime.Now), End = new Day(DateTime.Now.AddDays(7)) }, (l) => 
                                    {
                                        EditMode = EditMode.Edit;
                                        try
                                        {
                                            var par = new NavigationParameters();
                                            par.Add("navigation", new NavigationInteraction<Project>()
                                            {
                                                Header = "Projekti",
                                                EditInteraction = new EditInteraction<Project>()
                                                {
                                                    Title = "Projekti",
                                                    SaveAction = OnSaveProjectCallbackAction,
                                                    DataProvider = callback => { RefreshProjects(callback); },
                                                    EditMode = _notification.EditMode,

                                                }
                                            });
                                            par.Add("sitedata", ConstructionSite);
                                            _regionManager.RequestNavigate(RegionNames.CProjectsRegion, "Projects", OnRequestNavigateCallBack, par);
                                        }
                                        catch (Exception exc)
                                        {
                                            _exceptionService.RaiseException(exc);
                                        }

                                    });
                                }
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }

                            ConstructionSite = e;
                            _notification.SaveAction.Invoke(e, _notification.EditMode);
                            //SaveRelatedData();
                        });
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void UpdateConstructionSite(ConstructionSite site)
        {
            try
            {
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, ConstructionSite>>())
                {
                    var url = new Uri(_settings.GetApiServer(), "csite/update");

                    repositroy.PostRequestAsync(url.ToString(), site,
                        _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            OnFinishInteraction();
                        });
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
                NavigateBack();

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void LoadConstructionSiteData(ConstructionSite site)
        {
            try
            {
    

                ConstructionSite = site;
                ConstructionSite.Errors.ValidateProperties();
                ConstructionSite.ErrorsChanged += (sender, args) =>
                {
                    DocumentCommand.RaiseCanExecuteChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                };
                ConstructionSite.PropertyDeletegate = OnPropertyChange;

                ConstructionSite.IsDirty = false;
                DownloadConstructionSiteImage(ConstructionSite);
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
                SaveConstructionSite(ConstructionSite);
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
            return ConstructionSite != null && ConstructionSite.IsDirty && !ConstructionSite.HasErrors;
        }
        private void RefreshProjects(Action<List<Project>> callback)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Project>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/project/list").ToString(),
                        _securityService.GetCurrentToken(), (list) =>
                        {
                            try
                            {
                                Projects = list;
                                callback.Invoke(null == list ? null : list);
                                _lastProjectRefreshCallback = callback;
                            }
                            catch (Exception exception)
                            {
                                _exceptionService.RaiseException(exception);
                            }
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

                using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/contact").ToString(),
                        _securityService.GetCurrentToken(), (
                            e) =>
                        {
                            try
                            {
                                callback.Invoke(e == null ? null : e.Contacts == null ? null : e.Contacts.ToList());
                                _lastRefreshContactCallback = callback;
                            }
                            catch (Exception exception)
                            {
                                _exceptionService.RaiseException(exception);
                            }
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void DownloadConstructionSiteImage(ConstructionSite obj)
        {
            try
            {
                return;

                if (obj?.UuId == null) return;

                //ConstructionSiteImagePath = "/Ism.Infrastructure;component/Images/no-image.png";

                var commons = _serviceLocator.GetInstance<ICommonService>();

                using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"ConstructionSites/{obj.UuId}/document/{commons.GetDocumentTypes().FirstOrDefault(t => t.Name == "SLIKA")?.Name}").ToString(), _securityService.GetCurrentToken(), ConstructionSite =>
                        {
                            //var doc = ConstructionSite?.Documents?.FirstOrDefault();

                            //using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                            //{
                            //    var file = doc?.Files?.FirstOrDefault();
                            //    if (null == file) return;

                            //    string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_{file.UniqueName}_{file.Name}");
                            //    var url = new Uri(_settings.GetApiServer(false), $"documents/{doc.UuId}/files/{file.UuId}");
                            //    repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentUser(), null, inputStream =>
                            //    {
                            //        using (inputStream)
                            //        {
                            //            using (var outputStream = System.IO.File.OpenWrite(fileName))
                            //            {
                            //                inputStream.CopyTo(outputStream);
                            //            }
                            //        }
                            //        ConstructionSiteImagePath = fileName;
                            //    }, "Pridobivam sliko zaposlenega...", false);
                            //}
                        }
                    );
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        public override void   OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            _loaded = false;
            Clear();
            base.OnNavigatedTo(navigationContext);

            var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<ConstructionSite>;

            if (!(navigation.EditInteraction is EditInteraction<ConstructionSite>)) return;
            _notification = navigation.EditInteraction;

            EditMode = _notification.EditMode;
            LoadConstructionSiteData(_notification.EditMode == EditMode.New ? new ConstructionSite() { UuId = Guid.NewGuid().ToString()} : _notification.InteractionObject);

            NavigationParameters par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<BusinessPartner>() { Header = "Stranka", EditInteraction = new EditInteraction<BusinessPartner>() {Title= "Stranka", SelectAction = OnSelectCustomerCallback, InteractionObject = ConstructionSite.Customer } });

            _regionManager.RequestNavigate(RegionNames.CsiteBpRegion, "BusinessPartnerOverView", OnRequestNavigateCallBack, par);


            par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Document>()
            {
                Header = "Dokumenti",
                EditInteraction = new EditDocumentInteraction()
                {
                    Title = "Dokumenti",
                    SaveAction = OnSaveDocumentCallback,
                    DataProvider = callback => { RefreshDocuments(callback); },
                    EditMode = _notification.EditMode
                }
            });
            _regionManager.RequestNavigate(RegionNames.CDocumentsRegion, "DocmentsExt", OnRequestNavigateCallBack, par);

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
            _regionManager.RequestNavigate(RegionNames.CContactsRegion, "Contacts", OnRequestNavigateCallBack, par);

            if(EditMode == EditMode.Edit) { 
                par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<Project>()
                {
                    Header = "Projekti",
                    EditInteraction = new EditInteraction<Project>()
                    {
                        Title = "Projekti",
                        SaveAction = OnSaveProjectCallbackAction,
                        DataProvider = callback => { RefreshProjects(callback); },
                        EditMode = _notification.EditMode,

                    }
                });
                par.Add("sitedata", ConstructionSite);
                _regionManager.RequestNavigate(RegionNames.CProjectsRegion, "Projects", OnRequestNavigateCallBack, par);
            }

            _loaded = true;
        }

        public override bool KeepAlive => false;

        private void OnSaveDocumentCallback(Document document, EditMode editMode)
        {

            try
            {
                switch (_editMode)
                {
                    case EditMode.New:

                        if (null == Documents) Documents = new ObservableCollection<Document>();
                        Documents.Add(document);
                        _lastDocumentRefreshCallback.Invoke(Documents.ToList());
                        break;
                    default:
                        switch (editMode)
                        {
                            case EditMode.New:
                                AddDocument<ConstructionSite> addDocument = new AddDocument<ConstructionSite>(ConstructionSite, document);
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, AddDocument<ConstructionSite>>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "csite/addDocument").ToString(), addDocument, (e) =>
                                    {
                                        try
                                        {
                                            RefreshDocuments(_lastDocumentRefreshCallback);
                                        }
                                        catch (Exception exception)
                                        {
                                            _exceptionService.RaiseException(exception);
                                        }
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                RefreshDocuments(_lastDocumentRefreshCallback);
                                break;
                            case EditMode.Delete:
                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Document, Document>>())
                                {
                                    var url = new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/document/delete");
                                    repositroy.PostRequestAsync(url.ToString(), document, _securityService.GetCurrentToken(), (e) =>
                                    {
                                        try
                                        {
                                            RefreshDocuments(_lastDocumentRefreshCallback);
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
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(ConstructionSite, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "csite/addContact").ToString(), addContact, (e) =>
                                    {
                                        try
                                        {
                                            RefreshContacts(_lastRefreshContactCallback);
                                        }
                                        catch (Exception exception)
                                        {
                                            _exceptionService.RaiseException(exception);
                                        }
                                        
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddContact<BaseModel>>>())
                                {
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(ConstructionSite, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/contact/update").ToString(),
                                      addContact, (e) =>
                                      {
                                          try
                                          {
                                              RefreshContacts(_lastRefreshContactCallback);
                                          }
                                          catch (Exception exception)
                                          {
                                              _exceptionService.RaiseException(exception);
                                          }
                                          
                                      });
                                }
                                break;
                            case EditMode.Delete:
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Contact, Contact>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/contact/delete").ToString(), contact,
                                   _securityService.GetCurrentToken(),
                                   (e) =>
                                   {
                                       try
                                       {
                                           RefreshContacts(_lastRefreshContactCallback);
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
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
        private void SaveRelatedData()
        {
            try
            {
                if (_editMode != EditMode.New) return;

                //if(null != AddProjects)
                //{
                    
                //    using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, Project>>())
                //    {
                //        foreach (var project in AddProjects)
                //        {
                //            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"csite/{ConstructionSite.UuId}/project/add").ToString(), project, (e) =>
                //            {
                //                //RefreshProjects(_lastProjectRefreshCallback);
                //            });
                //        }
                //    }
                //}


                if (null != Documents)
                {
                    // ADD DOCUMENTS
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<ConstructionSite, AddDocument<ConstructionSite>>>())
                    {
                        foreach (var document in Documents)
                        {
                            AddDocument<ConstructionSite> addDocument = new AddDocument<ConstructionSite>(ConstructionSite, document);
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "csite/addDocument").ToString(), addDocument, (e) =>
                            { });
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
                            AddContact<BaseModel> updateContact = new AddContact<BaseModel>(ConstructionSite, contact) { UuId = contact.UuId };
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "csite/addContact").ToString(), updateContact, (e) =>
                            {

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

        private void OnSelectCustomerCallback(BusinessPartner partner)
        {
            try
            {
                ConstructionSite.Customer = partner;



            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void Clear()
        {
            try
            {
                ConstructionSite = null;
                Documents = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }

        }
    }
    
}

