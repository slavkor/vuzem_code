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
    public class DocumentOneAddViewModel : ViewModelBase
    {
        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;
        private readonly Uri _baseUri;
        private Infrastructure.Model.Document _selectedDocument;
        private ObservableCollection<Infrastructure.Model.File> _files;
        private EditInteraction<Infrastructure.Model.Document> _interaction;


        public DocumentOneAddViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                DocumentCommand = new DelegateCommand(OnDocumentCommand);
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


        public DelegateCommand DocumentsPrintCommand { get; }

        public DelegateCommand DocumentCommand { get; }
        public DelegateCommand<Infrastructure.Model.Document> DocumentCommandEdit { get; }
        public DelegateCommand<Infrastructure.Model.Document> DocumentCommandDelete { get; }


        public ObservableCollection<Infrastructure.Model.File> Files
        {
            get { return _files; }
            set
            {
                SetProperty(ref _files, value);

            }
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Infrastructure.Model.Document>;

                _interaction = navigation.EditInteraction as EditInteraction<Infrastructure.Model.Document>;
                _interaction.DataProvider.Invoke(DataProviderCallback);

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
                Files = null;
                if (null == documents) return;
                List<Infrastructure.Model.File> files = new List<Infrastructure.Model.File>();
                files.Add(new Infrastructure.Model.File() { AddDummy = true });
                files.AddRange(documents.Where(d => d.Type.Name == "EWR_SLIKA").SelectMany(d => d.Files).ToList());

                Files = new ObservableCollection<Infrastructure.Model.File>(files);
            }
            catch (Exception exception)
            {
                RaiseException(exception);
            }
        }

        #region Helper methods for Document edit

        private void OnDocumentCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditDocumentEvent>().Publish(new EditDocumentEventArgs() { SaveAction = OnDocumentCallbackAction, EditMode = EditMode.New, EditObject = null });
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
                _eventAggregator.GetEvent<EditDocumentEvent>().Publish(new EditDocumentEventArgs()
                {
                    SaveAction = OnDocumentCallbackAction,
                    EditMode = EditMode.Edit,
                    EditObject = obj,
                    RefreshAction = d => {
                        //_interaction.DataProvider.Invoke(DataProviderCallback);
                    }
                });
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
