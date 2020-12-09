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

using Prism;
using Ism.Infrastructure.Extensions;

namespace Ism.Construction.ViewModels
{
    class EwrEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;

        private EditInteraction<Ewr> _notification;

        private Ewr _ewr;
        private EditMode _editMode;

        private bool _loaded;
        private ObservableCollection<Document> _documents;
        private Action<List<Document>> _lastRefreshDocumentCallback;
        private Action<List<Contact>> _lastRefreshContactCallback;
        private ObservableCollection<EwrWorker> _foremans;
        private ObservableCollection<EwrWorker> _welders;
        private ObservableCollection<EwrWorker> _installers;
        private ObservableCollection<EwrWorker> _pipers;
        private bool _canSave;
        private List<EwrWorker> _addEmployess;
        private List<EwrWorker> _delEmployess;
        private EwrWorker _selectedforeman;
        private EwrWorker _selectedwelder;
        private EwrWorker _selectedinstaller;
        private EwrWorker _selectedpiper;

        public EwrEditViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService, ICommonService commonService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));


            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
            _commonService = commonService;

            try
            {
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                EmployeeSelectCommand = new DelegateCommand<string>(OnEmployeeSelectCommand);
                ForemanRemoveCommand = new DelegateCommand(OnForemanRemoveCommand, () => SelectedForeman != null);
                WelderRemoveCommand = new DelegateCommand(OnWelderRemoveCommand, () => SelectedWelder != null);
                InstallerRemoveCommand = new DelegateCommand(OnInstallerRemoveCommand, () => SelectedInstaller != null);
                PiperRemoveCommand = new DelegateCommand(OnPiperRemoveCommand, () => SelectedPiper != null);

                CanSave = false;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }

        private void OnForemanRemoveCommand()
        {
            try
            {
                OnEmployeeRemoveCommand("foreman");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnWelderRemoveCommand()
        {
            try
            {
                OnEmployeeRemoveCommand("welder");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnInstallerRemoveCommand()
        {
            try
            {
                OnEmployeeRemoveCommand("installer");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnPiperRemoveCommand()
        {
            try
            {
                OnEmployeeRemoveCommand("piper");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #region public properties

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

        public DelegateCommand<string> EmployeeSelectCommand { get; }

        public DelegateCommand ForemanRemoveCommand { get; }
        public DelegateCommand WelderRemoveCommand { get; }
        public DelegateCommand InstallerRemoveCommand { get; }
        public DelegateCommand PiperRemoveCommand { get; }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; }

        public Ewr Ewr
        {
            get { return _ewr; }
            set { SetProperty(ref _ewr, value); }
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
        
        public Project Project { get; set; }

        public string WorkPlace { get; set; }

        public EwrWorker SelectedForeman { get { return _selectedforeman; } set { SetProperty(ref _selectedforeman, value); RaiseCanExecuteChanged(); } }
        public EwrWorker SelectedWelder { get { return _selectedwelder; } set { SetProperty(ref _selectedwelder, value); RaiseCanExecuteChanged(); } }
        public EwrWorker SelectedInstaller { get { return _selectedinstaller; } set { SetProperty(ref _selectedinstaller, value); RaiseCanExecuteChanged(); } }
        public EwrWorker SelectedPiper { get { return _selectedpiper; } set { SetProperty(ref _selectedpiper, value); RaiseCanExecuteChanged(); } }
        public ObservableCollection<EwrWorker> ForeMans
        {
            get
            {
                return _foremans;
            }
            set
            {
                SetProperty(ref _foremans, value);
            }
        }
        public ObservableCollection<EwrWorker> Welders
        {
            get
            {
                return _welders;
            }
            set
            {
                SetProperty(ref _welders, value);
            }
        }
        public ObservableCollection<EwrWorker> Installers
        {
            get
            {
                return _installers;
            }
            set
            {
                SetProperty(ref _installers, value);
            }
        }
        public ObservableCollection<EwrWorker> Pipers
        {
            get
            {
                return _pipers;
            }
            set
            {
                SetProperty(ref _pipers, value);
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

                if (!(value is EditInteraction<Ewr>)) return;
                _notification = (EditInteraction<Ewr>)value;
                _notification.SaveAction = SaveAction;
                EditMode = _notification.EditMode;
                LoadEwrData(_notification.EditMode == EditMode.New ? new Ewr() { SiteManager = _securityService.GetCurrentEmployee()} : _notification.InteractionObject);
                _loaded = true;
            }
        }
        public Action FinishInteraction { get; set; }


        #endregion


        #region VieModelBase overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {

            try
            {
                _loaded = false;
                Clear();
                base.OnNavigatedTo(navigationContext);

                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Ewr>;
                Project = navigationContext.Parameters["project"] as Project;
                if (null == navigation) return;

                _notification = navigation.EditInteraction;
                _notification.SaveAction = SaveAction;
                EditMode = _notification.EditMode;
                LoadEwrData(_notification.EditMode == EditMode.New ? new Ewr() { UuId = Guid.NewGuid().ToString(), Number = DateTime.Now.ToProjectNumber(), Project = Project, SiteManager = _securityService.GetCurrentEmployee(), Date = DateTime.Now } : _notification.InteractionObject);

                var par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<Document>()
                {
                    Header = "Dokumenti",
                    EditInteraction = new EditDocumentInteraction()
                    {
                        Title = "Dokumenti",
                        SaveAction = OnSaveDocumentCallbackAction,
                        DataProvider = callback => { RefreshDocuments(callback); },
                        EditMode = _editMode, 
                        DocumentTypesProvider = DocumentTypesProvider
                    }
                });
                _regionManager.RequestNavigate(RegionNames.EwrDocumentsRegion, "DocmentsExt", OnRequestNavigateCallBack, par);



                par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<Contact>()
                {
                    Header = "Site manager",
                    EditInteraction = new EditInteraction<Contact>()
                    {
                        Title = "Site manager",
                        SaveAction = OnSaveExSiteManagerCallbackAction,
                        DataProvider = callback => { RefreshExSiteManagers(callback); },
                        InteractionObject = Ewr.ExSiteManager,
                        EditMode = _editMode
                    }
                });
                _regionManager.RequestNavigate(RegionNames.EwrExSiteManagerRegion, "OneContact", OnRequestNavigateCallBack, par);

                _loaded = true;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        private void DocumentTypesProvider(Action<List<DocumentType>> callback)
        {
            try
            {
                if (_securityService.HasPermissionExcplicit("foreman"))
                    callback?.Invoke(_commonService.GetDocumentTypes().Where(t => t.EwrBound).ToList());
                else
                    callback?.Invoke(_commonService.GetDocumentTypes());

            }
            catch (Exception exc) 
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public override bool KeepAlive => false;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
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

                            case EditMode.Extend:
                                AddDocument<Ewr> addDocument = new AddDocument<Ewr>(Ewr, document);
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Ewr, AddDocument<Ewr>>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "ewr/addDocument").ToString(), addDocument, (e) =>
                                    {
                                        try
                                        {
                                            RefreshDocuments(_lastRefreshDocumentCallback);
                                        }
                                        catch (Exception exception)
                                        {
                                            _exceptionService.RaiseException(exception);
                                        }

                                    });
                                }
                                break;
                            case EditMode.Cancel:
                                break;
                            case EditMode.Edit:
                                RefreshDocuments(_lastRefreshDocumentCallback);
                                break;
                            case EditMode.Delete:
                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Document, Document>>())
                                {
                                    var url = new Uri(_settings.GetApiServer(), $"ewr/{Ewr.UuId}/document/delete");
                                    repositroy.PostRequestAsync(url.ToString(), document, _securityService.GetCurrentToken(), (e) => {

                                        try
                                        {
                                            RefreshDocuments(_lastRefreshDocumentCallback);
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


        private void RefreshDocuments(Action<List<Document>> callback)
        {
            try
            {
                Documents = null;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Ewr, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"ewr/{Ewr.UuId}/document").ToString(),
                        _securityService.GetCurrentToken(), (e) =>
                        {
                            if(_securityService.HasPermissionExcplicit("foreman"))
                                callback.Invoke(e == null ? null : e.Documents == null ? null : e.Documents.Where(d => d.Type.EwrBound).ToList());
                            else
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

        private void RefreshExSiteManagers(Action<List<Contact>> callback)
        {
            try
            {


                using (var rep = _serviceLocator.GetInstance<IRestRepository<Project, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/contact").ToString(),
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

        private void OnSaveExSiteManagerCallbackAction(Contact contact, EditMode editMode)
        {
            try
            {
                switch (_editMode)
                {
                    case EditMode.New:
                        Ewr.ExSiteManager = contact;
                        break;
                    default:
                        Ewr.ExSiteManager = contact;
                        break;
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void SaveAction(Ewr obj, EditMode editMode)
        {
            try
            {
                SaveEwr(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveEwr(Ewr obj, bool finish = true)
        {
            try
            {
                if (!Ewr.IsDirty || !SaveCommand.CanExecute())
                {
                    if (finish) OnFinishInteraction();
                    return;
                }
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveEwrCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish, PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnConfirmSaveEwrCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {

                if (!confirmed || !Ewr.IsDirty)
                {
                    if (args.FinishUp) OnFinishInteraction();
                    return;
                }

                Ewr employee = args.PayLoad as Ewr;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Ewr, Ewr>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/ewr/add")
                        : new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/ewr/update");

                    repositroy.PostRequestAsync(url.ToString(), employee,
                        _securityService.GetCurrentToken(),
                        (e) =>
                        {

                            Ewr = e;
                            SaveRelatedData();
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
                if (Ewr == null) return;

                if (EditMode == EditMode.Edit) return;
                AddEmployee(_addEmployess);
                DeleteEmployee(_delEmployess);
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
        private void LoadEwrData(Ewr ewr)
        {
            try
            {
                Ewr = ewr;

                ForeMans = new ObservableCollection<EwrWorker>();
                Welders = new ObservableCollection<EwrWorker>();
                Installers = new ObservableCollection<EwrWorker>();
                Pipers = new ObservableCollection<EwrWorker>();

                if(null != Ewr?.Workers) ForeMans.AddRange(Ewr.Workers.Where(w => w.WorkPlace == "foreman"));
                if (null != Ewr?.Workers) Welders.AddRange(Ewr.Workers.Where(w => w.WorkPlace == "welder"));
                if (null != Ewr?.Workers) Installers.AddRange(Ewr.Workers.Where(w => w.WorkPlace == "installer"));
                if (null != Ewr?.Workers) Pipers.AddRange(Ewr.Workers.Where(w => w.WorkPlace == "piper"));

                ForeMans.CollectionChanged += ForeMans_CollectionChanged;
                Welders.CollectionChanged += Welders_CollectionChanged;
                Installers.CollectionChanged += Installers_CollectionChanged;
                Pipers.CollectionChanged += Pipers_CollectionChanged;

                Ewr.Errors.ValidateProperties();
                Ewr.ErrorsChanged += (sender, args) =>
                {

                    CanSave = true;
                    SaveCommand.RaiseCanExecuteChanged();
                };

                Ewr.PropertyDeletegate = OnPropertyChange;
                Ewr.IsDirty = false;
                CanSave = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void ForeMans_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (EditMode)
                {
                    case EditMode.New:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                if (null == _addEmployess) _addEmployess = new List<EwrWorker>();
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                {
                                    l.Add(item as EwrWorker);
                                }
                                _addEmployess.AddRange(l.Except(_addEmployess, new PropertyComparer<EwrWorker>("WorkerId")));
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                if (null == _delEmployess) _delEmployess = new List<EwrWorker>();
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                {
                                    ld.Add(item as EwrWorker);
                                }
                                _delEmployess.AddRange(ld.Except(_delEmployess, new PropertyComparer<EwrWorker>("WorkerId")));
                                break;
                        }
                        break;
                    case EditMode.Edit:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                    l.Add(item as EwrWorker);
                                AddEmployee(l);
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                    ld.Add(item as EwrWorker);
                                DeleteEmployee(ld);
                                break;
                        }
                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void Welders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (EditMode)
                {
                    case EditMode.New:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                if (null == _addEmployess) _addEmployess = new List<EwrWorker>();
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                {
                                    l.Add(item as EwrWorker);
                                }
                                _addEmployess.AddRange(l.Except(_addEmployess, new PropertyComparer<EwrWorker>("WorkerId")));
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                if (null == _delEmployess) _delEmployess = new List<EwrWorker>();
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                {
                                    ld.Add(item as EwrWorker);
                                }
                                _delEmployess.AddRange(ld.Except(_delEmployess, new PropertyComparer<EwrWorker>("WorkerId")));
                                break;
                        }
                        break;
                    case EditMode.Edit:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                    l.Add(item as EwrWorker);
                                AddEmployee(l);
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                    ld.Add(item as EwrWorker);
                                DeleteEmployee(ld);
                                break;
                        }
                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void Installers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (EditMode)
                {
                    case EditMode.New:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                if (null == _addEmployess) _addEmployess = new List<EwrWorker>();
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                {
                                    l.Add(item as EwrWorker);
                                }
                                _addEmployess.AddRange(l.Except(_addEmployess, new PropertyComparer<EwrWorker>("Worker")));
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                if (null == _delEmployess) _delEmployess = new List<EwrWorker>();
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                {
                                    ld.Add(item as EwrWorker);
                                }
                                _delEmployess.AddRange(ld.Except(_delEmployess, new PropertyComparer<EwrWorker>("Worker")));
                                break;
                        }
                        break;
                    case EditMode.Edit:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                    l.Add(item as EwrWorker);
                                AddEmployee(l);
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                    ld.Add(item as EwrWorker);
                                DeleteEmployee(ld);
                                break;
                        }
                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void Pipers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (EditMode)
                {
                    case EditMode.New:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                if (null == _addEmployess) _addEmployess = new List<EwrWorker>();
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                {
                                    l.Add(item as EwrWorker);
                                }
                                _addEmployess.AddRange(l.Except(_addEmployess, new PropertyComparer<EwrWorker>("WorkerId")));
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                if (null == _delEmployess) _delEmployess = new List<EwrWorker>();
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                {
                                    ld.Add(item as EwrWorker);
                                }
                                _delEmployess.AddRange(ld.Except(_delEmployess, new PropertyComparer<EwrWorker>("WorkerId")));
                                break;
                        }
                        break;
                    case EditMode.Edit:
                        switch (e.Action)
                        {
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                var l = new List<EwrWorker>();
                                foreach (var item in e.NewItems)
                                    l.Add(item as EwrWorker);
                                AddEmployee(l);
                                break;
                            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                var ld = new List<EwrWorker>();
                                foreach (var item in e.OldItems)
                                    ld.Add(item as EwrWorker);
                                DeleteEmployee(ld);
                                break;
                        }
                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void AddEmployee(List<EwrWorker> employees)
        {
            try
            {
                if (null == employees) return;
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<object, object>>())
                {
                    foreach (var employee in employees)
                    {
                        try
                        {
                            var url = new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/ewr/{Ewr.UuId}/worker/{employee.Worker.UuId}/add/{employee.WorkPlace}");

                            repositroy.PostRequestAsync(url.ToString(), null, _securityService.GetCurrentToken(), (e) => {
                                int a = 0;
                            });

                        }
                        catch (Exception exc)
                        {
                            _exceptionService.RaiseException(exc);
                        }

                    }
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void DeleteEmployee(List<EwrWorker> employees)
        {
            try
            {
                if (null == employees) return;
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<object, object>>())
                {
                    foreach (var employee in employees)
                    {
                        try
                        {
                            var url = new Uri(_settings.GetApiServer(), $"project/{Project.UuId}/ewr/{Ewr.UuId}/worker/{employee.Worker.UuId}/delete");

                            repositroy.PostRequestAsync(url.ToString(), null, _securityService.GetCurrentToken(), (e) => { });

                        }
                        catch (Exception exc)
                        {
                            _exceptionService.RaiseException(exc);
                        }

                    }
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnSaveCommand()
        {
            try
            {
                SaveEwr(Ewr);
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
            return Ewr != null && Ewr.IsDirty && !Ewr.HasErrors;
        }
        private void Clear()
        {
            try
            {
                Ewr = null;
                Documents = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnEmployeeSelectCommand(string workplace)
        {
            try
            {
                WorkPlace = workplace;
                _eventAggregator.GetEvent<ListEvent<EmployeeList>>().Publish(new ListEventArgs<EmployeeList>() { SelectManyAction = OnSelectEmployees, DataProvider = RefreshEmployees });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshEmployees(Action<List<EmployeeList>> callback)
        {
            try
            {
                if (Project == null) return;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeList>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settings.GetApiServer(false), $"project/{Project.UuId}/employee/list").ToString(),
                        _securityService.GetCurrentToken(),
                        (list) =>
                        {
                            callback?.Invoke(list);
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }
        private void OnSelectEmployees(List<EmployeeList> list)
        {
            try
            {
                List<EwrWorker> addlist = null;
                var listAll = ForeMans.Concat(Welders).Concat(Installers).Concat(Pipers).ToList();
                
                switch (WorkPlace)
                {
                    case "foreman":
                        addlist = list.Select(l => new EwrWorker() { WorkPlace = "foreman", Worker = l.Employee }).ToList();
                        ForeMans.AddRange(addlist.Except(listAll, new PropertyComparer<EwrWorker>("WorkerId")));
                        break;
                    case "welder":
                        addlist = list.Select(l => new EwrWorker() { WorkPlace = "welder", Worker = l.Employee }).ToList();
                        Welders.AddRange(addlist.Except(listAll, new PropertyComparer<EwrWorker>("WorkerId")));
                        break;
                    case "installer":
                        addlist = list.Select(l => new EwrWorker() { WorkPlace = "installer", Worker = l.Employee }).ToList();
                        Installers.AddRange(addlist.Except(listAll, new PropertyComparer<EwrWorker>("WorkerId")));
                        break;
                    case "piper":
                        addlist = list.Select(l => new EwrWorker() { WorkPlace = "piper", Worker = l.Employee }).ToList();
                        Pipers.AddRange(addlist.Except(listAll, new PropertyComparer<EwrWorker>("WorkerId")));
                        break;
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEmployeeRemoveCommand(string workplace)
        {
            try
            {
                
                switch (workplace)
                {
                    
                    case "foreman":
                        ForeMans.Remove(SelectedForeman);
                        break;
                    case "welder":
                        Welders.Remove(SelectedWelder);
                        break;
                    case "installer":
                        Installers.Remove(SelectedInstaller);
                        break;
                    case "piper":
                        Pipers.Remove(SelectedPiper);
                        break;
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RaiseCanExecuteChanged()
        {
            try
            {
                ForemanRemoveCommand.RaiseCanExecuteChanged();
                WelderRemoveCommand.RaiseCanExecuteChanged();
                InstallerRemoveCommand.RaiseCanExecuteChanged();
                PiperRemoveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);

            }
        }

    }
}
