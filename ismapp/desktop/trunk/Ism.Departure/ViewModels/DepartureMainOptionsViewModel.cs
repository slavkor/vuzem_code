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

using System.Collections;

namespace Ism.Departure.ViewModels
{
    class DepartureMainOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        public DepartureMainOptionsViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;

            OriginDestinationSelectRequest = new InteractionRequest<ListInteraction<IDepartureArrival>>();
            NavigateDeparturesCommand = new DelegateCommand(OnNavigateDeparturesCommand, CanExecuteNavigateDeparturesCommand);
            NavigateArrivalsCommand = new DelegateCommand(OnNavigateArrivalsCommand, CanExecuteNavigateArrivalsCommand);
            _eventAggregator.GetEvent<SelectedEvent<ForemanConstructionSite>>().Subscribe(OnForemanSiteEvent);
            _eventAggregator.GetEvent<ListEvent<IDepartureArrival>>().Subscribe(OnListDepartureArrivalEvent);
        }

        public InteractionRequest<ListInteraction<IDepartureArrival>> OriginDestinationSelectRequest { get; }

        private void OnForemanSiteEvent(SelectedEventArgs<ForemanConstructionSite> obj)
        {
            try
            {
                ForemanSite = obj.SelectedData;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public ForemanConstructionSite ForemanSite { get; set; }

        #region commands

        public DelegateCommand NavigateDeparturesCommand { get; }
        public DelegateCommand NavigateArrivalsCommand { get; }
        
        #endregion

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
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }
        #endregion

        #region private methods

        #region commands

        private bool CanExecuteNavigateDeparturesCommand()
        {
            return true;
        }

        private void OnNavigateDeparturesCommand()
        {
            try
            {
                var param = new NavigationParameters();
                if (_securityService.HasPermissionExcplicit("foreman"))
                {
                    param.Add("origin", ForemanSite.ConstructionSite);
                    param.Add("destination", ForemanSite.ConstructionSite);
                }

                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "Departures", param);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteNavigateArrivalsCommand()
        {
            return true;
        }

        private void OnNavigateArrivalsCommand()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "Arrivals");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #endregion


        private void OnListDepartureArrivalEvent(ListEventArgs<IDepartureArrival> obj)
        {
            try
            {
                var interaction = new ListInteraction<IDepartureArrival>() { Title = "Izbira projekta", SelectAction = obj.SelectAction, InteractionObject = null, ListEventArgs = obj };
                OriginDestinationSelectRequest.Raise(interaction);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #endregion

    }

}

