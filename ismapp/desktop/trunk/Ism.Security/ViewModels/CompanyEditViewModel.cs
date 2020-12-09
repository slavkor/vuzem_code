using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

namespace Ism.Security.ViewModels
{
    public class CompanyEditViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private EditInteraction<Company> _notification;

        private readonly Uri _baseUri;
        private Company _company;
        private string _logoImagePath;

        public CompanyEditViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {

            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                _baseUri = settingsService.GetApiServer(true);
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);

                LogoImagePath = "/Ism.Infrastructure;component/Images/no-image.png";
                AddCompanyLogoCommand = new   DelegateCommand(OnAddCompanyLogoCommand);
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
                OnFinishInteraction(false);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand CancelCommand { get; }

        public DelegateCommand AddCompanyLogoCommand { get; }

        public Company Company
        {
            get { return _company; }
            set
            {
                SetProperty(ref _company, value);
                
            }
        }

        public string LogoImagePath
        {
            get { return _logoImagePath; }
            set { SetProperty(ref _logoImagePath, value); }
        }

        private void OnSaveCommand()
        {
            try
            {
                SaveCompany(Company, EditMode.Undefined);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private bool CanExecuteSaveCommand()
        {
            return Company != null && Company.IsDirty;
        }

        public DelegateCommand SaveCommand { get;  }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as EditInteraction<Company>;
                if (notification != null)
                {
                    _notification = notification;
                    Company = notification.InteractionObject ?? new Company();
                    Company.IsDirty = false;
                    Company.PropertyDeletegate = OnPropertyChange;
                    _notification.SaveAction = SaveCompany;
                    DownloadCompanyLogo(Company);
                }
            }
        }

        public Action FinishInteraction { get; set; }

        

        #endregion


        private void OnAddCompanyLogoCommand()
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

        private void OnDocumentCallbackAction(Document obj, EditMode editMode)
        {
            try
            {
                if (Company == null) return;
                AddDocument<Company> addDocument = new AddDocument<Company>(Company, obj);
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Company, AddDocument<Company>>>())
                {
                    rep.PostRequestAsync(new Uri(_baseUri, $"company/{Company.UuId}/addlogo").ToString(), addDocument, DownloadCompanyLogo);
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void SaveCompany(Company obj, EditMode editMode)
        {
            try
            {
                if (!Company.IsDirty)
                {
                    OnFinishInteraction(false);
                    return;
                }

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveEventCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = obj});

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnPropertyChange(BaseModel model)
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
        private void OnConfirmSaveEventCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                Company company = args.PayLoad as Company;
                if(null == company) return;

                if (!confirmed || !Company.IsDirty)
                {
                    OnFinishInteraction();
                    return;
                }

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Company, Company>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_baseUri, "company/add")
                        : new Uri(_baseUri, "company/update");

                    repositroy.PostRequestAsync(url.ToString(), Company,
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
                OnFinishInteraction();
            }
        }
        private void OnFinishInteraction(bool confirmed = false)
        {
            try
            {
                Company = null;
                _notification.Confirmed = confirmed;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void DownloadCompanyLogo(Company obj)
        {
            try
            {
                using(var rep = _serviceLocator.GetInstance<IRestRepository<List<Document>,string>>())
                {
                    rep.GetRequestAsync(new Uri(_baseUri, $"company/{Company.UuId}/documents/LOGO").ToString(), _securityService.GetCurrentToken(), documents =>
                     {
                         if (documents == null) return;
                         if (documents.Count ==0) return;

                         var logoDoc = documents.FirstOrDefault();
                         if (logoDoc == null) return;

                         using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                         {
                             var file = logoDoc?.Files.FirstOrDefault();
                             if (null == file) return;

                             string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_{file.UniqueName}_{file.Name}");
                             var url = new Uri(_settingsService.GetApiServer(false), $"documents/{logoDoc.UuId}/files/{file.UuId}");
                             repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentToken(), null, inputStream =>
                             {
                                 using (inputStream)
                                 {
                                     using (var outputStream = System.IO.File.OpenWrite(fileName))
                                     {
                                         inputStream.CopyTo(outputStream);
                                     }
                                 }
                                 LogoImagePath = fileName;
                             }, "Pridobivam logo datoteko...", false);
                         }
                     }
                    );
                }
                if (obj?.Logo?.Files == null) return;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}
