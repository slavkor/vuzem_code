using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ism.Common.Views;
using Ism.Infrastructure.Events;
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
using Ism.Infrastructure.Mvvm;

namespace Ism.Common.ViewModels
{
    class LanguagesViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;


        private AddContact<BaseModel> _contact;
        private ContactViewInteraction _notification;
        private Uri _baseUri;
        private List<Language> _languages;

        public LanguagesViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                AddLanguageCommand = new DelegateCommand(OnAddLanguageCommand);
                LanguageInteractionRequest = new InteractionRequest<EditInteraction<Language>>();

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand AddLanguageCommand { get; }

        public InteractionRequest<EditInteraction<Language>> LanguageInteractionRequest { get; }

        public List<Language> Languages
        {
            get { return _languages; }
            set
            {
                SetProperty(ref _languages, value);

            }
        }


        private void OnAddLanguageCommand()
        {
            try
            {
                LanguageInteractionRequest.Raise(new EditInteraction<Language>() { Title = "Dodajanje jezika" }, OnLanguageInteractionRequestCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnLanguageInteractionRequestCallback(EditInteraction<Language> obj)
        {
            try
            {
                if (!obj.Confirmed) return;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<Language, Language>>())
                {
                    rep.PostRequestAsync(new Uri(_baseUri, "shrd/lang/add").ToString(), obj.InteractionObject, _securityService.GetCurrentToken(),
                        list =>
                        {
                            RefreshLanguages();
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void RefreshLanguages()
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Language>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_baseUri, "shrd/lang/list").ToString(), _securityService.GetCurrentToken(),
                        list =>
                        {
                            Languages = list;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                RefreshLanguages();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        #endregion
    }
}
