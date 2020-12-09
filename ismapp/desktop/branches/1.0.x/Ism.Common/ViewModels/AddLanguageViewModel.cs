using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
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
    public class AddLanguageViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private EditInteraction<Language> _notification;

        private Language _language;


        public AddLanguageViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
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
                OkCommand = new DelegateCommand(OnOkCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public Language Language
        {
            get { return _language; }
            set
            {
                SetProperty(ref _language, value);
            }
        }

        public DelegateCommand OkCommand { get; }

        private void OnOkCommand()
        {
            try
            {
                _notification.Confirmed = true;
                _notification.InteractionObject = Language;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as EditInteraction<Language>;
                if (null == notification) return;

                SetProperty(ref _notification, notification);
                Language = notification.InteractionObject ?? new Language() {UuId = Guid.NewGuid().ToString()};
            }
        }

        public Action FinishInteraction { get; set; }

        

        #endregion
    }
}
