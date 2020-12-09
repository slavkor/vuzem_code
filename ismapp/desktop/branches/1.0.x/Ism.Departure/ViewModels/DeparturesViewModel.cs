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
    public class DeparturesViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private bool _invalidateMatches;
        public DeparturesViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;

            DepartureEditRequest = new InteractionRequest<EditInteraction<Infrastructure.Model.Departure>>();
            NewDepartureCommand = new DelegateCommand(OnNewDepartureCommand);
            EditDepartureCommand = new DelegateCommand<DepartureList>(OnEditDepartureCommand);
            DeleteDepartureCommand = new DelegateCommand<DepartureList>(OnDeleteDepartureCommand, CanExecuteDeleteDepartureCommand);
            ConfirmDepartureCommand = new DelegateCommand<DepartureList>(OnConfirmDepartureCommand, CanExecuteConfirmDepartureCommand);
            ConfirmEmployeeCommand = new DelegateCommand(OnConfirmEmployeeCommand);
        }

        private bool CanExecuteDeleteDepartureCommand(DepartureList arg)
        {
            return true; // arg?.Departure?.Status <= 0;
        }

        private bool CanExecuteConfirmDepartureCommand(DepartureList arg)
        {
            return true; // arg?.Departure?.Status <= 0;
        }


        #region public properties

        public InteractionRequest<EditInteraction<Infrastructure.Model.Departure>> DepartureEditRequest { get; }

        public bool InvalideMathes
        {
            get { return _invalidateMatches; }
            set
            {
                SetProperty(ref _invalidateMatches, value);
            }
        }

        #endregion


        #region commands
        public DelegateCommand NewDepartureCommand { get; }
        public DelegateCommand<DepartureList> EditDepartureCommand { get; }
        public DelegateCommand<DepartureList> DeleteDepartureCommand { get; }
        public DelegateCommand<DepartureList> ConfirmDepartureCommand { get; }

        public DelegateCommand ConfirmEmployeeCommand { get; }

        #endregion

        #region ViewModelBase overrides
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Možnosti" });

                if (null != navigationContext.Parameters?["origin"])
                    parameters.Add("origin", navigationContext.Parameters?["origin"]);
                if (null != navigationContext.Parameters?["destination"])
                    parameters.Add("destination", navigationContext.Parameters?["destination"]);

                _regionManager.RequestNavigate(Infrastructure.RegionNames.DepartureOptRegion, "DepartureOptions", parameters);


            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
        public override bool KeepAlive => true;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion
     

        #region private methods

        #region commands
        private void OnNewDepartureCommand()
        {
            try
            {
                DepartureEditRequest.Raise(new EditInteraction<Infrastructure.Model.Departure>() {Title ="Novi odhod", EditMode = EditMode.New }, OnDepartureEditRequestCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDepartureCommand(DepartureList obj)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDepartureCallback, Title = "ALO", Content = "Želiš potrditi odhod? Kasnejše spremembe več ne bodo možne!!", FinishUp = false, PayLoad = obj });

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDepartureCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                DepartureList data = args.PayLoad as DepartureList;

                if (null == data) return;

                ConfirmDeparture(data);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ConfirmDeparture(DepartureList data)
        {
            try
            {
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Departure, Infrastructure.Model.Departure>>())
                {
                    var url = new Uri(_settings.GetApiServer(), $"departures/{data.Departure.UuId}/confirm");
                    repositroy.PostRequestAsync(url.ToString(), data.Departure,
                        _securityService.GetCurrentUser().AccessToken,
                        (d) =>
                        {
                            try
                            {
                                //if (args.FinishUp) OnFinishInteraction();
                                
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnDeleteDepartureCommand(DepartureList obj)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDeleteDepartureCallback, Title = "ALO", Content = "Želiš izbrisati odhod? Kasnejše spremembe več ne bodo možne!!", FinishUp = false, PayLoad = obj });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDeleteDepartureCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                DepartureList data = args.PayLoad as DepartureList;

                if (null == data) return;

                DeleteDeparture(data);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void DeleteDeparture(DepartureList data)
        {
            try
            {
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Departure, Infrastructure.Model.Departure>>())
                {
                    var url = new Uri(_settings.GetApiServer(), $"departures/{data.Departure.UuId}/cancel");
                    repositroy.PostRequestAsync(url.ToString(), data.Departure,
                        _securityService.GetCurrentUser().AccessToken,
                        (d) =>
                        {
                            try
                            {
                                //if (args.FinishUp) OnFinishInteraction();

                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEditDepartureCommand(DepartureList obj)
        {
            try
            {
                var dep = obj.Departure;
                dep.Employees = obj.Employees;
                dep.Cars = obj.Cars;
                DepartureEditRequest.Raise(new EditInteraction<Infrastructure.Model.Departure>() { Title = "Ureajnje odhoda", EditMode = EditMode.Edit, InteractionObject = dep }, OnDepartureEditRequestCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDepartureEditRequestCallback(EditInteraction<Infrastructure.Model.Departure> obj)
        {
            try
            {
                InvalideMathes = !InvalideMathes;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmEmployeeCommand()
        {
            try
            {

                throw new Exception("OnConfirmEmployeeCommand");
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        #endregion

        #endregion
    }

}

