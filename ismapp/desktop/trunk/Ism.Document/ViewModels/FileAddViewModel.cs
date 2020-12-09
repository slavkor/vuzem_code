using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;

namespace Ism.Document.ViewModels
{
    public class FileAddViewModel : ViewModelBase, IInteractionRequestAware
    {
        private FileAddInteraction _notification;

        private readonly IExceptionService _exceptionService;

        private Infrastructure.Model.Document _document;
        private string[] _selectedFiles;
        private List<DocumentType> _documentTypes;
        private DocumentType _documentType;
        private List<Language> _languages;
        private Language _language;


        public FileAddViewModel(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;

            try
            {
                OkCommand = new DelegateCommand(OnOkCommand, CanExecuteOkCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                SelectFilesCommand = new DelegateCommand(OnSelectFilesCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }



        public DelegateCommand OkCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public DelegateCommand SelectFilesCommand { get; }

        public List<Language> Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }

        public Language Language
        {
            get { return _language; }
            set
            {
                SetProperty(ref _language, value);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as FileAddInteraction;
                if (null == notification)
                    return;

                SetProperty(ref _notification, notification);
                try
                {
                    var common = _serviceLocator.GetInstance<ICommonService>();
                    Languages = common.GetLanguages();
                    Language = Languages?.FirstOrDefault(l => l.Alpha2 == "sl");
                    SelectedFiles = null;

                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                }
            }
        }

        public Action FinishInteraction { get; set; }

        #endregion

        private void OnSelectFilesCommand()
        {
            try
            {
                SelectedFiles = null;
                var fileDialog = new OpenFileDialog
                {
                    Filter = "All files (*.*)|*.*",
                    Multiselect = true
                };
                var result = fileDialog.ShowDialog();

                if (result == false)
                    return;
                SelectedFiles = fileDialog.FileNames;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public string[] SelectedFiles
        {
            get { return _selectedFiles; }
            set { SetProperty(ref _selectedFiles, value); }
        }

        
        private void OnCancelCommand()
        {
            try
            {
                _notification.Confirmed = false;
                Languages = null;
                Language = null;
                SelectedFiles = null;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnOkCommand()
        {
            try
            {
                _notification.Files = SelectedFiles.Select(f => new File() {FullName = f, Language = Language})
                    .ToList();
                _notification.Confirmed = true;
                FinishInteraction?.Invoke();
                Languages = null;
                Language = null;
                SelectedFiles = null;

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private bool CanExecuteOkCommand()
        {
            return Language != null;
        }
    }
}
