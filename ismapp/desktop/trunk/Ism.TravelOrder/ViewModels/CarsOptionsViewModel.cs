using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;


namespace Ism.TravelOrder.ViewModels
{
    class CarsOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _security;
        private readonly IExceptionService _exceptionService;
        private Car _currentCar;
        private string _lastNavigated = "";

        public CarsOptionsViewModel(ISettingsService settings, ISecurityService security, IExceptionService exceptionService)
        {
            try
            {


                CarsList = new DelegateCommand(OnCarsList);
                CarEdit = new DelegateCommand<Car>(OnCarEdit, (e) => CurrentCar != null  /*&& CurrentCar.WorkPeriod.Active == 1*/ );
                CarDelete = new DelegateCommand<Car>(OnCarDelete, (e) => CurrentCar != null /*&& CurrentCar.WorkPeriod.Active == 1*/);
                CarAdd = new DelegateCommand(OnCarAdd);
                _eventAggregator.GetEvent<SelectedEvent<Car>>().Subscribe(OnCarSelected);
                _eventAggregator.GetEvent<EditEvent<Car>>().Subscribe(OnCarEditEvent);

                _settings = settings;
                _security = security;
                _exceptionService = exceptionService;
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void OnCompanySelectedEvent(Company obj)
        {
            try
            {
                OnCarsList();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnCarEditEvent(EditEventArgs<Car> args)
        {
            try
            {
                if (CarEdit.CanExecute(args.EditObject)) CarEdit.Execute(args.EditObject);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand CarsList { get; }
        public DelegateCommand CarAdd { get; }
        public DelegateCommand<Car> CarEdit { get; }

        public DelegateCommand<Car> CarDelete { get; }

        public Car CurrentCar
        {
            get { return _currentCar; }
            set
            {
                SetProperty(ref _currentCar, value);
                RaiseCanExecuteChanged();
            }
        }


        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                CurrentCar = null;
                _lastNavigated = navigationResult.Context.Uri.OriginalString;
                var b = !navigationResult.Result;
                if (b != null && (bool)b)
                {
                    _exceptionService.RaiseException(navigationResult.Error);
                }

                RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCarsList()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Pregled avtomobilov" });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.CarsRegion, "CarsList", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCarAdd()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Car>() { Header = "Dodajanje avtomobila ", EditInteraction = new EditInteraction<Car>() { Title = "Dodajanje novega avtomobila ", InteractionObject = null, EditMode = EditMode.New } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.CarsRegion, "CarEdit", NavigaionCallback, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCarEdit(Car car)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Car>() { Header = "Urejanje avtomobila", EditInteraction = new EditInteraction<Car>() { Title = "Urejanje avtomobila", TitleExtendet = $"{car.Model} {car.Model}", InteractionObject = car, EditMode = EditMode.Edit } });
                _regionManager.RequestNavigate(Infrastructure.RegionNames.CarsRegion, "CarEdit", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCarSelected(SelectedEventArgs<Car> args)
        {
            try
            {
                CurrentCar = null;
                if (args?.SelectedData == null) return;
                CurrentCar = args.SelectedData;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnCarDelete(Car obj)
        {
            try
            {
                //_eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmCarDelete, Title = "ALO", Content = "Želiš izbrisati zaposlenega?", PayLoad = obj });
                _eventAggregator.GetEvent<ReportEvent>().Publish(new ReportEventArgs());
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmCarDelete(bool confirmed, BaseModel payLoad)
        {
            try
            {
                if (!confirmed)
                    return;

                Car car = payLoad as Car;
                if (null == car) return;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Car, Car>>())
                {
                    var url = new Uri(_settings.GetApiServer(), "cars/delete");

                    repositroy.PostRequestAsync(url.ToString(), car,
                        _security.GetCurrentToken(),
                        (e) =>
                        {
                            OnCarsList();
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        private void RaiseCanExecuteChanged()
        {
            try
            {
                CarsList.RaiseCanExecuteChanged();
                CarEdit.RaiseCanExecuteChanged();
                CarDelete.RaiseCanExecuteChanged();
                CarAdd.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

    }
}
