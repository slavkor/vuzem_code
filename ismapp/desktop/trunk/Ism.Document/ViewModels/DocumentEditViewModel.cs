using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
using File = Ism.Infrastructure.Model.File;
using Omu.ValueInjecter;
using Ism.Infrastructure.Mvvm;

namespace Ism.Document.ViewModels
{
    public class DocumentEditViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;
        private EditDocumentInteraction _notification;
        private Infrastructure.Model.Document _document;
        private readonly Uri _baseUri;
        private ObservableCollection<File> _files;
        private List<DocumentType> _documentTypes;
        private DocumentType _selectedDocumentType;
        private EditMode _editMode;
        private Action<Infrastructure.Model.Document, EditMode> SaveAction;
        private File _selectedFile;
        private InteractionRequest<FileAddInteraction> _fileAddInteractionRequest;
        private bool _documentTypeEnabled;
        private bool _filesChanged;
        private bool _hasValidTo;
        private bool _hasExpireDate;
        private bool _notReadOnly;
        public DocumentEditViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                _baseUri = _settingsService.GetApiServer();
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                SelectFilesCommand = new DelegateCommand(OnSelectFilesCommand);
                DeleteFileCommand = new DelegateCommand(OnDeleteFileCommand, CanExecuteDeleteFileCommand);
                OpenFileCommand= new DelegateCommand(OnOpenFileCommand, CanExecuteOpenFileCommand);
                ExtendDocumentCommand = new DelegateCommand(OnExtendDocumentCommand, CanExecuteExtendDocumentCommand);
                CancelDocumentCommand = new DelegateCommand(OnCancelDocumentCommand, CanExecuteCancelDocumentCommand);
                NotReadOnly = true;
                //FileAddInteractionRequest = new InteractionRequest<FileAddInteraction>();
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        public bool FilesChanged
        {
            get
            {
                return _filesChanged;
            }
            set
            {
                SetProperty(ref _filesChanged, value);
            }
        }

        public bool DocumentTypeEnabled
        {
            get { return _documentTypeEnabled; }
            set
            {
                SetProperty(ref _documentTypeEnabled, value);
            }
        }

        public bool HasValidTo
        {
            get { return _hasValidTo; }
            set
            {
                SetProperty(ref _hasValidTo, value);
                if (!_hasValidTo && null != Document?.ValidTo)
                    Document.ValidTo = null;

                if (_hasValidTo && null == Document?.ValidTo)
                    Document.ValidTo = new Infrastructure.Model.Day(DateTime.Now);

            }
        }

        public bool HasExpireDate
        {
            get { return _hasExpireDate; }
            set
            {
                SetProperty(ref _hasExpireDate, value);
                if (!_hasExpireDate && null != Document?.ExpireDate)
                    Document.ExpireDate = null;

                if (_hasExpireDate && null == Document?.ExpireDate)
                    Document.ExpireDate = DateTime.Now;

            }
        }

        public List<DocumentType> DocumentTypes
        {
            get
            {
                return _documentTypes;
                
            }
            set
            {
                SetProperty(ref _documentTypes, value);
                
            }
        }
        public DocumentType SelectedDocumentType
        {
            get { return _selectedDocumentType; }
            set
            {
                SetProperty(ref _selectedDocumentType, value);
            }
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SelectFilesCommand { get; }
        public DelegateCommand DeleteFileCommand { get; }
        public DelegateCommand OpenFileCommand { get; }

        public DelegateCommand ExtendDocumentCommand { get; }

        public DelegateCommand CancelDocumentCommand { get; }

        public File SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                SetProperty(ref _selectedFile, value);
            }
        }

        public bool NotReadOnly
        {
            get
            {
                return _notReadOnly;
            }
            set
            {
                SetProperty(ref _notReadOnly, value);
                
            }
        }
        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    var notificaton = value as EditDocumentInteraction;
                    SetProperty(ref _notification, notificaton);
                    _editMode = _notification.EditMode;
                    DocumentTypeEnabled = _editMode == EditMode.New || _editMode == EditMode.Cancel;

                    switch (_editMode)
                    {
                        case EditMode.New:
                        case EditMode.Cancel:
                            Document = new Infrastructure.Model.Document() { Type = new DocumentType() { Name = "UNDEFINED" }, DocDate = Infrastructure.Model.Day.Now, ValidFrom = new Infrastructure.Model.Day(DateTime.Today.FirstDayOfMonth()), ValidTo =  new Infrastructure.Model.Day(DateTime.Today.LastDayOfMonth()) };
                            break;
                        case EditMode.Edit:
                            Document = _notification.InteractionObject;
                            break;
                        case EditMode.Extend:
                            Document = new Infrastructure.Model.Document();
                            Document.InjectFrom(_notification.InteractionObject);
                            Document.ValidFrom = _notification.InteractionObject.ValidTo;
                            Document.ValidTo = new Infrastructure.Model.Day(DateTime.Today.LastDayOfMonth());
                            Document.Files = null;
                            Document.UuId = Guid.NewGuid().ToString();
                            break;
                        case EditMode.ReadOnly:
                            Document = _notification.InteractionObject;
                            break;

                    }

                    NotReadOnly = _editMode != EditMode.ReadOnly;

                    SaveAction = _notification.SaveAction;
                    _notification.SaveAction = SaveDocument;

                    LoadDocumentData();
                }
                catch (Exception e)
                {
                    RaiseException(e, true);
                    OnFinishInteraction();
                }
            }
        }

        public Infrastructure.Model.Document Document
        {
            get { return _document; }
            set
            {
                SetProperty(ref _document, value);
                
            }
        }

        public InteractionRequest<FileAddInteraction> FileAddInteractionRequest
        {
            get { return _fileAddInteractionRequest; }
            set
            {
                SetProperty(ref _fileAddInteractionRequest ,value);
                
            }
        }

        public ObservableCollection<File> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }
        }

        public Action FinishInteraction { get; set; }

        

        private void SaveDocument(Infrastructure.Model.Document document, EditMode editMode)
        {
            try
            {
                if (!SaveCommand.CanExecute())
                {
                    OnFinishInteraction();
                    return;
                }

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = Document });

            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void OnConfirmSaveCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed)
                {
                    OnFinishInteraction();
                    return;
                }
                var document = args.PayLoad as Infrastructure.Model.Document;

                if (_editMode == EditMode.Edit)
                    UpdateDocument(document);
                else
                    InitUpload(document);
            }
            catch (Exception e)
            {
                RaiseException(e, true);
                OnFinishInteraction();
            }
        }

        private void UpdateDocument(Infrastructure.Model.Document document)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Document, Infrastructure.Model.Document>>())
                {
                    if (null != SelectedDocumentType) Document.Type = SelectedDocumentType;
                    rep.PostRequestAsync(
                        new Uri(_settingsService.GetApiServer(), "documents/update").ToString(),
                        document,
                        _securityService?.GetCurrentToken(),
                        (d) =>
                        {
                            SaveAction?.Invoke(document, EditMode.Edit);
                            OnFinishInteraction(true);
                        }, "Posodabljam dokument na strežniku...", false);
                }
                
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void OnSaveCommand()
        {
            try
            {
                SaveDocument(Document, _notification.EditMode);
            }
            catch (Exception e)
            {
                RaiseException(e, true);
                OnFinishInteraction();
            }
        }

        private void InitUpload(Infrastructure.Model.Document document)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Document, AddDocument<Infrastructure.Model.Document>>>())
                {
                    if (null != SelectedDocumentType) Document.Type = SelectedDocumentType;

                    AddDocument<Infrastructure.Model.Document> addDocument = new AddDocument<Infrastructure.Model.Document>(_notification.InteractionObject, document);

                    rep.PostRequestAsync(
                        new Uri(_settingsService.GetApiServer(), "documents/initUpload").ToString(),
                        addDocument,
                        _securityService?.GetCurrentToken(),
                        (d) =>
                        {
                            if (Files == null || Files.Count == 0)
                            {
                                FinishUpload(d);
                            }       
                            else
                            {
                                for (int i = 0; i < Files.Count; i++)
                                {
                                    UploadFile(d, Files[i].FullName, Files[i].Language.UuId, i, Files.Count - 1, true);
                                }
                            }
                        }, "Ustvarajm dokument na strežniku...", false);
                }
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void UploadFile(Infrastructure.Model.Document document, string file, string language, int currentFile, int allFiles, bool finishUpoad)
        {
            try
            {
                using (var repository = _serviceLocator.GetInstance<IRestRepository<File, string>>())
                {
                    FileInfo f = new FileInfo(file);
                    var query = new Dictionary<string, string> { { "uniquename", "newfile" }, { "uuid", document.UuId }, {"language", language} };

                    repository.PostFileAsync(
                        new Uri(_settingsService.GetApiServer(), "documents/uploadFile").ToString(),
                        f.FullName,
                        _securityService.GetCurrentToken(),
                        query,
                        (fd) =>
                        {
                            FilesChanged = true;
                            RaiseCanExecuteChanged();
                            if (currentFile != allFiles) return;
                            if(finishUpoad) FinishUpload(document);
                            else LoadDocumentFiles();

                        },
                        $"Nalagam datoteko {currentFile} od {allFiles}...", true);
                }
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void FinishUpload(Infrastructure.Model.Document document)
        {
            try
            {
                using (var repository = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Document, Infrastructure.Model.Document>>())
                {
                    repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), "documents/finishUpload").ToString(),
                        document,
                        _securityService?.GetCurrentToken(),
                        (fd) =>
                        {
                            SaveAction?.Invoke(fd, _notification.EditMode);
                            OnFinishInteraction(true);
                        },
                        "Nalagam dokument...", true);
                }
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void ClearState()
        {
            try
            {
                Files = null;
                Document = null;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void RaiseException(Exception e, bool clearState = false)
        {
            try
            {
                if(clearState) ClearState();
                _exceptionService.RaiseException(e);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void RefreshDocumentTypes()
        {
            try
            {
        
                if(_notification?.DocumentTypesProvider != null)
                {
                    _notification?.DocumentTypesProvider?.Invoke(OnDocumentTypesProviderCallback);
                    return;
                }

                var ser = _serviceLocator.GetInstance<ICommonService>();
                DocumentTypes =  ser.GetDocumentTypes();

            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void OnDocumentTypesProviderCallback(List<DocumentType> list)
        {
            try
            {
                DocumentTypes = list;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnSelectFilesCommand()
        {
            try
            {
                FileAddInteractionRequest = new InteractionRequest<FileAddInteraction>();
                FileAddInteractionRequest.Raise(new FileAddInteraction() {Title = "Dodajanje datotek", Document = Document}, OnFileAddInteractionRequestCallback);
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void OnFileAddInteractionRequestCallback(FileAddInteraction obj)
        {
            try
            {
                if(!obj.Confirmed)
                    return;
                if (null == Files)
                    Files = new ObservableCollection<File>();

                int allFiles = obj.Files.Count;
                int current = 0;

                if (_editMode == EditMode.Edit)
                {
                    foreach (var file in obj.Files)
                    {
                        current++;
                        UploadFile(obj.Document, file.FullName, file.Language.UuId, current, allFiles, false);
                    }
                }
                else
                {
                    Files.AddRange(obj.Files);
                }
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private bool CanExecuteSaveCommand()
        {
            try
            {
                return Document != null && Document.IsDirty && !Document.HasErrors || FilesChanged;
            }
            catch (Exception e)
            {
                RaiseException(e);
            }

            return false;
        }

        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction();
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private void OnFinishInteraction(bool confirm = false)
        {
            try
            {
                _notification.Confirmed = confirm;
                _notification.InteractionObject = Document;
                FinishInteraction?.Invoke();
                ClearState();
            }
            catch (Exception e)
            {
                RaiseException(e, true);
            }
        }


        private bool CanExecuteOpenFileCommand()
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }

        private void OnOpenFileCommand()
        {
            try
            {
                if (SelectedFile == null) return;

                if(_editMode == EditMode.New || _editMode == EditMode.Cancel)
                {
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        Verb = "open",
                        FileName = SelectedFile.FullName //put the correct path here
                    };
                    p.Start();
                    return;
                }


                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                {

                    string fileName = Path.Combine(Path.GetTempPath(),
                        $"{SelectedFile.UniqueName}_{SelectedFile.Name}");
                    var url = new Uri(_settingsService.GetApiServer(),
                        $"documents/{Document.UuId}/files/{SelectedFile.UuId}");
                    repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentToken(), null, inputStream =>
                    {
                        using (inputStream)
                        {
                            using (var outputStream = System.IO.File.OpenWrite(fileName))
                            {
                                inputStream.CopyTo(outputStream);
                            }
                        }

                        Process p = new Process();
                        p.StartInfo = new ProcessStartInfo()
                        {
                            CreateNoWindow = true,
                            Verb = "open",
                            FileName = fileName //put the correct path here
                        };
                        p.Start();

                    }, "Odpiram datoteko...", true);

                }

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        private bool CanExecuteDeleteFileCommand()
        {
            try
            {
                return _editMode == EditMode.Edit;

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }

        private void OnDeleteFileCommand()
        {
            try
            {
                switch (_editMode)
                {
                    case EditMode.Edit:
                        _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDeleteCallback, Title = "ALO", Content = "Želiš izbrisati datoteko spremembe?", PayLoad = Document });
                        break;
                    default:
                        Files.Remove(SelectedFile);
                        break;
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        private void OnConfirmDeleteCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Document, string>>())
                {
                    var url = new Uri(_settingsService.GetApiServer(), $"documents/{Document.UuId}/files/{SelectedFile.UuId}/delete");
                    repositroy.PostRequestAsync(url.ToString(), "", _securityService?.GetCurrentToken(), document =>
                    {
                        FilesChanged = true;
                        RaiseCanExecuteChanged();
                        LoadDocumentFiles();
                    }, "brišem datoteko na strežniku...", true);
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        private void LoadDocumentData()
        {
            try
            {

                RefreshDocumentTypes();

                Document.PropertyDeletegate = (model) =>
                {
                    RaiseCanExecuteChanged();
                };

                if (null != Document.ValidFrom)
                    Document.ValidFrom.PropertyDeletegate = (model) =>
                    {
                        Document.IsDirty = true;
                        RaiseCanExecuteChanged();
                    };

                if (null != Document.ValidTo)
                    Document.ValidTo.PropertyDeletegate = (model) =>
                    {
                        Document.IsDirty = true;
                        RaiseCanExecuteChanged();
                    };

                SelectedDocumentType = DocumentTypes.FirstOrDefault(t => t.UuId == Document.Type.UuId);

                LoadDocumentFiles();
                Document.IsDirty = false;
                FilesChanged = false;

                HasValidTo = Document.ValidTo != null;
                HasExpireDate = Document.ExpireDate != null;

                RaiseCanExecuteChanged();

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void LoadDocumentFiles()
        {
            try
            {
                if(null == Document) return;
                Files = null;
                switch (_editMode)
                {
                    case EditMode.Edit:
                    case EditMode.ReadOnly:
                        using (var rep = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Document, string>>())
                        {
                            rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"documents/{Document.UuId}").ToString(),
                                _securityService.GetCurrentToken(), (doc) =>
                                {
                                    if (null == doc) return;
                                    Files = new ObservableCollection<File>();
                                    if (null == doc.Files) return;

                                    Files.AddRange(doc.Files);
                                });
                        }
                        break;
                    default:
                        return;
                        break;
                }


            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private bool CanExecuteExtendDocumentCommand()
        {
            return _editMode == EditMode.Edit && (bool)SelectedDocumentType?.Expirable;
        }

        private void OnExtendDocumentCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<Infrastructure.Model.Document, Infrastructure.Model.Document>>().Publish(new EditChildEventArgs<Infrastructure.Model.Document, Infrastructure.Model.Document>() { SaveAction = OnExpiredDocumentSave, EditMode = EditMode.Extend, EditObject = Document, EditChildMode = EditMode.Extend, EditChildObject = null, SaveChildAction = SaveAction });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private bool CanExecuteCancelDocumentCommand()
        {
            return _editMode == EditMode.Edit && (bool)SelectedDocumentType?.Expirable;
        }

        private void OnCancelDocumentCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<Infrastructure.Model.Document, Infrastructure.Model.Document>>().Publish(new EditChildEventArgs<Infrastructure.Model.Document, Infrastructure.Model.Document>() { SaveAction = OnCancelDocumentSave, EditMode = EditMode.Cancel, EditObject = Document, EditChildMode = EditMode.Cancel, EditChildObject = null, SaveChildAction = SaveAction });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnCancelDocumentSave(Infrastructure.Model.Document document, EditMode editMode)
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

        private void OnExpiredDocumentSave(Infrastructure.Model.Document document, EditMode editMode)
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

        private void RaiseCanExecuteChanged()
        {
            try
            {
                SaveCommand.RaiseCanExecuteChanged();
                DeleteFileCommand.RaiseCanExecuteChanged();
                OpenFileCommand.RaiseCanExecuteChanged();
                ExtendDocumentCommand.RaiseCanExecuteChanged();
                CancelDocumentCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
