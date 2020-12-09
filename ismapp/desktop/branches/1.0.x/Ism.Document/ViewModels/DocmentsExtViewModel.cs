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
    class DocmentsExtViewModel : ViewModelBase
    {
        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;
        private readonly Uri _baseUri;
        private Infrastructure.Model.Document _selectedDocument;
        private ObservableCollection<Infrastructure.Model.Document> _documents;
        private EditDocumentInteraction _interaction;


        public DocmentsExtViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                //FileAddInteractionRequest = new InteractionRequest<FileAddInteraction>();
                DocumentCommand = new DelegateCommand(OnDocumentCommand);
                DocumentCommandEdit = new DelegateCommand<Infrastructure.Model.Document>(OnDocumentCommandEdit, CanExecuteDocumentCommandEdit);
                DocumentCommandDelete = new DelegateCommand<Infrastructure.Model.Document>(OnDocumentCommandDelete, CanExecuteDocumentCommandEdit);
                DocumentsPrintCommand = new DelegateCommand(OnDocumentsPrintCommand, CanExecuteDocumentsPrintCommand);

            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

        private bool CanExecuteDocumentsPrintCommand()
        {
            return true;
        }

        private void OnDocumentsPrintCommand()
        {
            try
            {
                foreach (var document in Documents.Where(d=>d.IsSelected))
                {
                    foreach (var file in document.Files)
                    {
                        using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                        {

                            string fileName = Path.Combine(Path.GetTempPath(),
                                $"{file.UniqueName}_{file.Name}");
                            var url = new Uri(_settingsService.GetApiServer(),
                                $"documents/{document.UuId}/files/{file.UuId}");
                            repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentUser(), null, inputStream =>
                            {
                                using (inputStream)
                                {
                                    using (var outputStream = System.IO.File.OpenWrite(fileName))
                                    {
                                        inputStream.CopyTo(outputStream);
                                    }
                                }
                                //Process.Start(fileName);

                                Process p = new Process();
                                p.StartInfo = new ProcessStartInfo()
                                {
                                    CreateNoWindow = true,
                                    Verb = "print",
                                    FileName = fileName //put the correct path here
                                };
                                p.Start();

                            }, "Tiskam ...", true);

                        }
                    }
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand DocumentsPrintCommand { get; }

        public DelegateCommand DocumentCommand { get; }
        public DelegateCommand<Infrastructure.Model.Document> DocumentCommandEdit { get; }
        public DelegateCommand<Infrastructure.Model.Document> DocumentCommandDelete { get; }


        public ObservableCollection<Infrastructure.Model.Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

            }
        }

        public Infrastructure.Model.Document SelectedDocument
        {
            get { return _selectedDocument; }
            set
            {
                _selectedDocument = value;
                DocumentCommandEdit.RaiseCanExecuteChanged();
                DocumentCommandDelete.RaiseCanExecuteChanged();
            }
        }

        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Infrastructure.Model.Document>;

                _interaction = navigation.EditInteraction as EditDocumentInteraction;
                _interaction.DataProvider.Invoke(DataProviderCallback);

                var types = navigationContext.Parameters["types"] as Action<Action<List<DocumentType>>>;


            }
            catch (Exception exception)
            {
                RaiseException(exception);
            }
        }



        #region private methods

        private void DataProviderCallback(List<Infrastructure.Model.Document> documents)
        {
            try
            {
                Documents = null;
                if (null == documents) return;
                Documents = new ObservableCollection<Infrastructure.Model.Document>(documents);
            }
            catch (Exception exception)
            {
                RaiseException(exception);
            }
        }

        #region Helper methods for Document edit

        private bool CanExecuteDocumentCommandEdit(Infrastructure.Model.Document arg)
        {
            return SelectedDocument != null && _interaction.EditMode != EditMode.New;
        }

        private void OnDocumentCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditDocumentEvent>().Publish(new EditDocumentEventArgs() { SaveAction = OnDocumentCallbackAction, DocumentTypesProvider = _interaction.DocumentTypesProvider, EditMode = EditMode.New, EditObject = null });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnDocumentCallbackAction(Infrastructure.Model.Document document, EditMode editMode)
        {
            try
            {
                _interaction.SaveAction.Invoke(document, editMode);
                //_interaction.DataProvider.Invoke(DataProviderCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnDocumentCommandDelete(Infrastructure.Model.Document obj)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDocumentDelete, Title = "ALO", Content = "Želiš izbrisati dokument zaposlenega?", PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }
        private void OnConfirmDocumentDelete(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed)
                    return;
                Infrastructure.Model.Document document = args.PayLoad as Infrastructure.Model.Document;
                if (null == document) return;

                _interaction.SaveAction.Invoke(document, EditMode.Delete);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnDocumentCommandEdit(Infrastructure.Model.Document obj)
        {
            try
            {
                _eventAggregator.GetEvent<EditDocumentEvent>().Publish(new EditDocumentEventArgs() { SaveAction = OnDocumentCallbackAction, DocumentTypesProvider = _interaction.DocumentTypesProvider, EditMode = EditMode.Edit, EditObject = obj, RefreshAction = d => {
                    //_interaction.DataProvider.Invoke(DataProviderCallback);
                } });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #endregion

        private void RaiseException(Exception exception)
        {
            _exceptionService.RaiseException(exception);
        }

        #endregion


    }
}
