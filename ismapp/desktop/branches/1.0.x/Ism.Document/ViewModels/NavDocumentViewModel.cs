using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
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
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace Ism.Document.ViewModels
{
    public class NavDocumentViewModel : WindowAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly IRegionManager _regionManager;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private InteractionRequest<EditInteraction<Infrastructure.Model.Document>> _documentEditInteractionRequest;
        private InteractionRequest<EditInteraction<Infrastructure.Model.Document>> _documentChilkEditInteractionRequest;

        public NavDocumentViewModel(IEventAggregator eventAggregator, IServiceLocator serviceLocator, IRegionManager regionManager, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));
            if (null == serviceLocator)
                throw new ArgumentNullException(nameof(serviceLocator));

            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _regionManager = regionManager;
            _securityService = securityService;
            _exceptionService = exceptionService;

            _eventAggregator.GetEvent<EditDocumentEvent>().Subscribe(OnDocumentEditEvent);
            _eventAggregator.GetEvent<EditChildEvent<Infrastructure.Model.Document, Infrastructure.Model.Document>>().Subscribe(OnDocumentEditChildEvent);
            DocumentEditInteractionRequest = new InteractionRequest<EditInteraction<Infrastructure.Model.Document>>();
            DocumentChildEditInteractionRequest = new InteractionRequest<EditInteraction<Infrastructure.Model.Document>>();
            DocumentsCommand = new DelegateCommand(OnDocumentsCommand, CanExecuteDocumentsCommand);
        }



        private bool CanExecuteDocumentsCommand()
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

        private void OnDocumentsCommand()
        {
            try
            {
                _regionManager.RequestNavigate(RegionNames.MainContentRegion, "Documents", NavigaionCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand DocumentsCommand { get; }


        public InteractionRequest<EditInteraction<Infrastructure.Model.Document>> DocumentEditInteractionRequest
        {
            get { return _documentEditInteractionRequest; }
            set
            {
                SetProperty(ref _documentEditInteractionRequest, value);
            }
        }

        public InteractionRequest<EditInteraction<Infrastructure.Model.Document>> DocumentChildEditInteractionRequest
        {
            get { return _documentChilkEditInteractionRequest; }
            set
            {
                SetProperty(ref _documentChilkEditInteractionRequest, value);
            }
        }

        private void OnDocumentEditEvent(EditDocumentEventArgs args)
        {
            try
            {

                var interaction = new EditDocumentInteraction()
                {
                    Title = args.EditMode == EditMode.New ? "Dodajanje dokumenta za" : "Urejanje dokumenta za",
                    TitleExtendet = args.EditObject?.ToString(),
                    InteractionObject = args.EditObject,
                    SaveAction = args.SaveAction,
                    RefreshAction = args.RefreshAction,
                    EditMode = args.EditMode,
                    EditEventArgs = args, 
                    DocumentTypesProvider = args.DocumentTypesProvider
                };

                DocumentEditInteractionRequest.Raise(interaction, DocumentEditInteractionRequestCallBack);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        private void DocumentEditInteractionRequestCallBack(EditInteraction<Infrastructure.Model.Document> interaction)
        {
            try
            {
                if(interaction.Confirmed && interaction.EditMode == EditMode.Edit)
                {
                    interaction.EditEventArgs.RefreshAction?.Invoke(interaction.InteractionObject);
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                var b = !navigationResult.Result;
                if (b != null && (bool)b)
                {
                    _exceptionService.RaiseException(navigationResult.Error);
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDocumentEditChildEvent(EditChildEventArgs<Infrastructure.Model.Document, Infrastructure.Model.Document> args)
        {
            try
            {
                var interaction = new EditDocumentInteraction()
                {
                    Title =  "Podaljševanje dokumenta",
                    TitleExtendet = args.EditObject?.ToString(),
                    InteractionObject = args.EditObject,
                    SaveAction = args.SaveChildAction,
                    EditMode = args.EditChildMode,
                    EditEventArgs = args
                };

                DocumentChildEditInteractionRequest.Raise(interaction, DocumentChildEditInteractionRequestCallBack);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void DocumentChildEditInteractionRequestCallBack(EditInteraction<Infrastructure.Model.Document> interaction)
        {
            try
            {
                if (!interaction.Confirmed) return;

                interaction.EditEventArgs.SaveAction?.Invoke(interaction.InteractionObject, EditMode.Undefined);
                //throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}
