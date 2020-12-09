using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Extensions;
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
using Ism.Infrastructure.Mvvm;
using System.Collections.ObjectModel;

namespace Ism.Construction.ViewModels
{
    public class ProjectEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private Project _Project;
        private EditMode _editMode;

        private bool _loaded;
        private NavigationContext _navigationContext;
        private EditInteraction<Project> _interaction;

        private ObservableCollection<Document> _documents;
        private ObservableCollection<Address> _addresses;
        private ObservableCollection<Contact> _contacts;

        private Action<List<Document>> _lastDocumentRefreshCallback;
        private Action<List<Contact>> _lastRefreshContactCallback;
        private Action<List<Address>> _lastRefreshAddressCallback;

        public ProjectEditViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
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
                //WorkingHoursCommand = new DelegateCommand(OnWorkingHoursCommand, CanExecuteWorkingHoursCommand);
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                AddWorkPeriodCommand = new DelegateCommand(OnAddWorkPeriodCommand, CanExecuteAddWorkPeriodCommand);
                DeleteWorkPeriodCommand = new DelegateCommand<ProjectWorkPeriod>(OnDeleteWorkPeriodCommand, CanExecuteDeleteWorkPeriodCommand);

                EditWorkPeriodInteraction = new InteractionRequest<NavigationChildInteraction<Project, ProjectWorkPeriod>>();

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }



        #region public properties


        public ObservableCollection<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

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

        public ObservableCollection<Address> Addresses
        {
            get { return _addresses; }
            set
            {
                SetProperty(ref _addresses, value);

            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public DelegateCommand WorkingHoursCommand { get; }

        public DelegateCommand AddWorkPeriodCommand { get; }

        public DelegateCommand<ProjectWorkPeriod> DeleteWorkPeriodCommand { get; }

        public InteractionRequest<NavigationChildInteraction<Project, ProjectWorkPeriod>> EditWorkPeriodInteraction { get; }

        public ConstructionSite ConstructionSite { get; set; }

        public Project Project
        {
            get { return _Project; }
            set { SetProperty(ref _Project, value); }
        }
        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        #endregion


        #region ViewModelBase overrides

        public override bool KeepAlive => false;

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            _interaction = (navigationContext.Parameters["navigation"] as NavigationInteraction<Project>)?.EditInteraction;
            _navigationContext = navigationContext;
            _loaded = false;

            ConstructionSite = navigationContext.Parameters["sitedata"] as ConstructionSite;

            EditMode = _interaction.EditMode;
            LoadProjectData(_interaction.EditMode == EditMode.New ? new Project() {UuId = Guid.NewGuid().ToString(),  ProjectNumber = DateTime.Now.ToProjectNumber(), Start = new Day(DateTime.Now), End = new Day(DateTime.Now.LastDayOfMonth()) } : _interaction.InteractionObject);

            NavigationParameters  par = new NavigationParameters();
            par.Add("navigation", new NavigationInteraction<Document>()
            {
                Header = "Dokumenti",
                EditInteraction = new EditDocumentInteraction()
                {
                    Title = "Dokumenti",
                    SaveAction = OnSaveDocumentCallback,
                    DataProvider = callback => { RefreshDocuments(callback); },
                    EditMode = _interaction.EditMode
                }
            });
            _regionManager.RequestNavigate(RegionNames.PDocumentsRegion, "DocmentsExt", OnRequestNavigateCallBack, par);

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
            _regionManager.RequestNavigate(RegionNames.PContactsRegion, "Contacts", OnRequestNavigateCallBack, par);

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
            _regionManager.RequestNavigate(RegionNames.PAddressRegion, "Addresses", OnRequestNavigateCallBack, par);

            RaiseCanExecuteChanged();
            _loaded = true;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }
        #endregion

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

        private void LoadProjectData(Project project)
        {
            try
            {
                Project = project;
                Project.Errors.ValidateProperties();
                Project.ErrorsChanged += (sender, args) =>
                {
                    SaveCommand.RaiseCanExecuteChanged();
                };

                Project.PropertyDeletegate = OnPropertyChange;

                Project.IsDirty = false;
                DownloadProjectImage(Project);
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
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveProjectCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = true, PayLoad = Project });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmSaveProjectCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                Project project = args.PayLoad as Project;
                _interaction.SaveAction?.Invoke(project, _interaction.EditMode);
                GoBack();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }

        }

        private void OnCancelCommand()
        {
            try
            {
                GoBack();

                //var par = new NavigationParameters();
                //par.Add("navigation", new NavigationInteraction<BaseModel>()
                //{
                //    Header = "Seznam gradbišč",
                //});

                //_regionManager.RequestNavigate(Infrastructure.RegionNames.CSiteRegion, "ConstructionSitesList", par);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private bool CanExecuteSaveCommand()
        {
            return Project != null && Project.IsDirty && !Project.HasErrors;
        }

        private void DownloadProjectImage(Project obj)
        {
            try
            {
                return;
                if (obj?.UuId == null) return;

                //ProjectImagePath = "/Ism.Infrastructure;component/Images/no-image.png";

                var commons = _serviceLocator.GetInstance<ICommonService>();

                using (var rep = _serviceLocator.GetInstance<IRestRepository<Project, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"projects/{obj.UuId}/document/{commons.GetDocumentTypes().FirstOrDefault(t => t.Name == "SLIKA")?.Name}").ToString(), _securityService.GetCurrentUser().AccessToken, Project =>
                        {
                            //var doc = Project?.Documents?.FirstOrDefault();

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
                            //        ProjectImagePath = fileName;
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
                                AddDocument<Project> addDocument = new AddDocument<Project>(Project, document);
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Project, AddDocument<Project>>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "project/addDocument").ToString(), addDocument, (e) =>
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
                                    var url = new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/document/delete");
                                    repositroy.PostRequestAsync(url.ToString(), document, _securityService.GetCurrentUser().AccessToken, (e) =>
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
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(Project, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "project/addContact").ToString(), addContact, (e) =>
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
                                    AddContact<BaseModel> addContact = new AddContact<BaseModel>(Project, contact) { UuId = contact.UuId };

                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/contact/update").ToString(),
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
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/contact/delete").ToString(), contact,
                                   _securityService.GetCurrentUser().AccessToken,
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
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(Project, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "project/addAddress").ToString(), updateAddress, (e) =>
                                    {
                                        RefreshAddresses(_lastRefreshAddressCallback);
                                    });
                                }
                                break;
                            case EditMode.Edit:
                                if (null == address) return;
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                                {
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(Project, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(
                                        new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/address/update").ToString(),
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
                                    var url = new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/address/delete");

                                    repositroy.PostRequestAsync(url.ToString(), address,
                                        _securityService.GetCurrentUser().AccessToken,
                                        (e) =>
                                        {
                                            RefreshAddresses(_lastRefreshAddressCallback);
                                        });
                                }
                                break;
                            case EditMode.Bind:
                                if (null == address) return;
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<BaseModel, AddAddress<BaseModel>>>())
                                {
                                    AddAddress<BaseModel> updateAddress = new AddAddress<BaseModel>(Project, address) { UuId = address.UuId };
                                    rep.PostRequestAsync(
                                        new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/address/bind").ToString(),
                                        updateAddress, (e) =>
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


        private void RefreshDocuments(Action<List<Document>> callback)
        {
            try
            {

                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Project, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/document").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (
                            e) =>
                        {
                            callback.Invoke(e == null ? null : e.Documents.ToList());
                            _lastDocumentRefreshCallback = callback;
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

                using (var rep = _serviceLocator.GetInstance<IRestRepository<Project, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/contact").ToString(),
                        _securityService.GetCurrentUser().AccessToken, (
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
        private void RefreshAddresses(Action<List<Address>> callback)
        {
            try
            {
                Addresses = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Project, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/address").ToString(),
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
        private void GoBack()
        {
            try
            {

                try
                {
                    _eventAggregator.GetEvent<EditEvent<ConstructionSite>>().Publish(new EditEventArgs<ConstructionSite>() { EditObject = ConstructionSite, EditMode = EditMode.Edit });
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                }


                //if (_navigationContext.NavigationService.Journal.CanGoBack)
                //    _navigationContext.NavigationService.Journal.GoBack();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private bool CanExecuteWorkingHoursCommand()
        {
            return EditMode == EditMode.Edit;

        }

        //private void OnWorkingHoursCommand()
        //{
        //    try
        //    {
        //        _eventAggregator.GetEvent<EditEvent<ForemanConstructionSite>>().Publish(new EditEventArgs<ForemanConstructionSite>() { EditMode = EditMode.Edit, EditObject = new ForemanConstructionSite() { ConstructionSite = ConstructionSite, Project = Project} });
        //    }
        //    catch (Exception exc)
        //    {
        //        _exceptionService.RaiseException(exc);
        //    }
        //}

        private void RaiseCanExecuteChanged()
        {
            try
            {
                SaveCommand.RaiseCanExecuteChanged();
                //WorkingHoursCommand.RaiseCanExecuteChanged();

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private bool CanExecuteAddWorkPeriodCommand()
        {
            return true;
        }

        private void OnAddWorkPeriodCommand()
        {
            try
            {
                EditWorkPeriodInteraction.Raise(new NavigationChildInteraction<Project, ProjectWorkPeriod>() { Title = "Urejanje obdobja", EditChildInteraction = new EditChidlInteraction<Project, ProjectWorkPeriod>() {InteractionObject = Project, ChildInteractionObject = null, EditMode = EditMode.New, SaveAction = OnEditWorkPeriodInteractionCallback} });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEditWorkPeriodInteractionCallback(Project project, ProjectWorkPeriod workperiod, EditMode editmode)
        {
            try
            {
                var plans = workperiod.WorkPlans;
                workperiod.WorkPlans = null;
                switch (editmode)
                {
                    case EditMode.New:
                        using (var rep = _serviceLocator.GetInstance<IRestRepository<ProjectWorkPeriod, AddChild<Project, ProjectWorkPeriod>>>())
                        {
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/wp/add").ToString(), new AddChild<Project, ProjectWorkPeriod>(project, workperiod), (period) =>
                            {
                
                                Project.WorkPeriods.Add(period);

                                period.Plan = plans?.Sum(p => p.Plan);

                                using (var repp = _serviceLocator.GetInstance<IRestRepository<ProjectWorkPeriod, AddChild<ProjectWorkPeriod, WorkPlan>>>())
                                {

                                    foreach (var item in plans)
                                    {

                                        repp.PostRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/wp/{period.UuId}/add").ToString(), new AddChild<ProjectWorkPeriod, WorkPlan>(period, item), (l) =>
                                        {
                                            ;
                                        });
                                    }

                                }
                            });
                        }

                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnDeleteWorkPeriodCommand(ProjectWorkPeriod period)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<ProjectWorkPeriod, object>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/wp/{period.UuId}/delete").ToString(), null, (d) =>
                    {
                        Project.WorkPeriods.Remove(period);
                    });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteDeleteWorkPeriodCommand(ProjectWorkPeriod period)
        {
            return true;
        }
    }
}

