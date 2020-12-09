using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure;
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
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Mvvm;
namespace Ism.Arrival.ViewModels
{
    public class ArrivalsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        public ArrivalsViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
        }

        #region ViewModelBase overrides
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        public override bool KeepAlive => false;

        #endregion



    }

}

