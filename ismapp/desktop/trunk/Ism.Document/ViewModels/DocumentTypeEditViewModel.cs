using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Ism.Infrastructure.Mvvm;

namespace Ism.Document.ViewModels
{
    public class DocumentTypeEditViewModel: ViewModelBase, IInteractionRequestAware
    {
        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;
        private EditInteraction<DocumentType> _notification;
        private DocumentType _documentType;

        public DocumentTypeEditViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _securityService = securityService;
            _exceptionService = exceptionService;
            _settingsService = settingsService;
            try
            {
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveComand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #region public properties

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public DocumentType DocumentType
        {
            get { return _documentType; }
            set
            {
                SetProperty(ref _documentType, value);
                
            }
        }
        

        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as EditInteraction<DocumentType>;
                if(null == notification) return;
                notification.SaveAction = SaveDocumentType;
                DocumentType = notification.InteractionObject ?? new DocumentType() {UuId = Guid.NewGuid().ToString()};
                DocumentType.PropertyDeletegate = OnPopertyChange;
                SetProperty(ref _notification, notification);
                
            }
        }

        private void OnPopertyChange(BaseModel model)
        {
            try
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public Action FinishInteraction { get; set; }

        

        #endregion

        #region private helper methods


        private bool CanExecuteSaveComand()
        {
            try
            {
                return DocumentType != null && DocumentType.IsDirty;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return false;
        }

        private void OnSaveCommand()
        {
            try
            {
                SaveDocumentType(DocumentType, EditMode.Undefined);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
                OnFinishInteraction(false);
            }
        }
        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction(false);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void SaveDocumentType(DocumentType documentType, EditMode editMode)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveEventCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = documentType });

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmSaveEventCallback(bool obj, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                DocumentType documentType = args.PayLoad as DocumentType;

                if (documentType != null && (!obj || !documentType.IsDirty))
                {
                    OnFinishInteraction(false);
                    return;
                }
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<DocumentType, DocumentType>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_settingsService.GetApiServer(), "documents/types/add")
                        : new Uri(_settingsService.GetApiServer(), "documents/types/update");

                    repositroy.PostRequestAsync(url.ToString(), documentType,
                        _securityService.GetCurrentToken(),
                        (e) =>
                        {
                            OnFinishInteraction(true);
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                OnFinishInteraction(false);
            }
        }

        private void OnFinishInteraction(bool confirmed)
        {
            try
            {
                _notification.Confirmed = confirmed;
                if (confirmed) _notification.InteractionObject = DocumentType;
                DocumentType = null;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #endregion
    }
}
