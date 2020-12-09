using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Document.ViewModels
{
    public class DocumentTypeViewModel : ViewModelBase,  IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;
        private DocumentTypeInteraction _notification;
        private readonly Uri _baseUri;
        private List<DocumentType> _documentTypes;
        private bool _isSelect;
        private bool _isEdit;

        public DocumentTypeViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                _baseUri = _settingsService.GetApiServer(true);
                SelectCommand = new DelegateCommand<DocumentType>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
                NewDocumentTypeCommand = new DelegateCommand(OnNewDocumentTypeCommand, CanExecuteNewDocumentTypeCommand);
                EditDocumentTypeCommand = new DelegateCommand<DocumentType>(OnEditDocumentTypeCommand, CanExecuteEditDocumentTypeCommand);
                DocumentTypeAddInteractionRequest = new InteractionRequest<EditInteraction<DocumentType>>();

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private bool CanExecuteNewDocumentTypeCommand()
        {
            try
            {
                return _securityService.HasPermission("documents");
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }

        private bool CanExecuteEditDocumentTypeCommand(DocumentType arg)
        {
            try
            {
                return _securityService.HasPermission("documents");
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }


        public InteractionRequest<EditInteraction<DocumentType>> DocumentTypeAddInteractionRequest { get;  }
        public List<DocumentType> DocumentTypes
        {
            get { return _documentTypes; }
            set { SetProperty(ref _documentTypes, value); }
        }
        public DelegateCommand<DocumentType> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand NewDocumentTypeCommand { get; }
        public DelegateCommand<DocumentType> EditDocumentTypeCommand { get; }
        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                SetProperty(ref _isSelect, value);
                IsEdit = !value;
            }
        }

        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                SetProperty(ref _isEdit, value);
                
            }
        }


        #region IInteractionRequestAware

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    var notificaton = value as DocumentTypeInteraction;
                    SetProperty(ref _notification, notificaton);
                    IsSelect = true;
                    RefreshDocumentTypes();
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                    FinishInteraction?.Invoke();
                }
            }
        }

        #endregion
        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                
                IsSelect = false;
                RefreshDocumentTypes();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }



        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            try
            {
                DocumentTypes = null;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion
       

        #region Private helper methods

        private void OnCancelCommand()
        {
            try
            {
                _notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnSelectCommand(DocumentType obj)
        {
            try
            {
                _notification.Confirmed = true;
                _notification.DocumentType = obj;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void RefreshDocumentTypes()
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<DocumentType>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_baseUri, "documents/types/list").ToString(),_securityService.GetCurrentUser().AccessToken,
                        list =>
                        {
                            DocumentTypes = list;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnNewDocumentTypeCommand()
        {
            try
            {
                DocumentTypeAddInteractionRequest.Raise(new EditInteraction<DocumentType>() {Title = "Dodajanje tipa dokumenta", InteractionObject = null, EditMode = EditMode.New}, OnDocumentTypeAddInteractionRequestCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnEditDocumentTypeCommand(DocumentType documentType)
        {
            try
            {
                DocumentTypeAddInteractionRequest.Raise(new EditInteraction<DocumentType>() { Title = "Urejanje tipa dokumenta", TitleExtendet = documentType.Name, InteractionObject = documentType, EditMode = EditMode.Edit }, OnDocumentTypeAddInteractionRequestCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnDocumentTypeAddInteractionRequestCallback(EditInteraction<DocumentType> obj)
        {
            try
            {
                RefreshDocumentTypes();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion
    }
}
