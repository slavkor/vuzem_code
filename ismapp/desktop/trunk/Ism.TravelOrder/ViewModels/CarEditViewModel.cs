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

namespace Ism.TravelOrder.ViewModels
{
    class CarEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        
        private EditInteraction<Car> _notification;

        private Car _car;
        private EditMode _editMode;
        private bool _loaded;

        private ObservableCollection<Document> _documents;
        private Action<List<Document>> _lastRefreshDocumentCallback;

        public CarEditViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
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
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; }

        public Car Car
        {
            get { return _car; }
            set { SetProperty(ref _car, value); }
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

        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                _loaded = false;

                if (!(value is EditInteraction<Car>)) return;
                _notification = (EditInteraction<Car>)value;
                _notification.SaveAction = SaveAction;
                EditMode = _notification.EditMode;
                LoadCarData(_notification.EditMode == EditMode.New ? new Car() : _notification.InteractionObject);
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

            var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Car>;

            if (!(navigation.EditInteraction is EditInteraction<Car>)) return;
            _notification = navigation.EditInteraction;
            _notification.SaveAction = SaveAction;
            EditMode = _notification.EditMode;
            LoadCarData(_notification.EditMode == EditMode.New ? new Car() { UuId = Guid.NewGuid().ToString() } : _notification.InteractionObject);


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
            
            _loaded = true;

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
                                AddDocument<Car> addDocument = new AddDocument<Car>(Car, document);
                                using (var rep = _serviceLocator.GetInstance<IRestRepository<Car, AddDocument<Car>>>())
                                {
                                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "car/addDocument").ToString(), addDocument, (e) =>
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
                            case EditMode.Edit:
                                RefreshDocuments(_lastRefreshDocumentCallback);
                                break;
                            case EditMode.Delete:
                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Document, Document>>())
                                {
                                    var url = new Uri(_settings.GetApiServer(), $"car/{Car.UuId}/document/delete");
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
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Car, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"cars/{Car.UuId}/document").ToString(),
                        _securityService.GetCurrentToken(), (e) =>
                        {
                            //callback.Invoke(e == null ? null : e.Documents == null ? null : e.Documents.ToList());
                            _lastRefreshDocumentCallback = callback;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void SaveAction(Car obj, EditMode editMode)
        {
            try
            {
                SaveCar(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveCar(Car obj, bool finish = true)
        {
            try
            {
                if (!Car.IsDirty || !SaveCommand.CanExecute())
                {
                    if (finish) OnFinishInteraction();
                    return;
                }
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveCarCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish, PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnConfirmSaveCarCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {

                if (!confirmed || !Car.IsDirty)
                {
                    if (args.FinishUp) OnFinishInteraction();
                    return;
                }

                Car car = args.PayLoad as Car;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Car, Car>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_settings.GetApiServer(), "cars/add")
                        : new Uri(_settings.GetApiServer(), "cars/update");

                    repositroy.PostRequestAsync(url.ToString(), car,
                        _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            if (_notification.EditMode == EditMode.New)
                            {
                                Car = e;
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
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<Car, AddDocument<Car>>>())
                    {
                        foreach (var document in Documents)
                        {

                            AddDocument<Car> addDocument = new AddDocument<Car>(Car, document);
                            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "car/addDocument").ToString(), addDocument, (e) =>
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
        private void LoadCarData(Car car)
        {
            try
            {
                Car = car;
                Car.Errors.ValidateProperties();
                Car.ErrorsChanged += (sender, args) =>
                {

                    CanSave = true;
                    SaveCommand.RaiseCanExecuteChanged();
                };

                Car.PropertyDeletegate = OnPropertyChange;

                Car.IsDirty = false;
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
                SaveCar(Car);
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
            return Car != null && Car.IsDirty && !Car.HasErrors;
        }

        private void Clear()
        {
            try
            {
                Car = null;
                Documents = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
