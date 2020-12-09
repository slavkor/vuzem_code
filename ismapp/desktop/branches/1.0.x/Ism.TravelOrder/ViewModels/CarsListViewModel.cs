using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;

namespace Ism.TravelOrder.ViewModels
{
    class CarsListViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<CarList> _cars;
        private CarList _car;

        public CarsListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {

            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));


            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);
                DoubleClickCommand = new DelegateCommand<CarList>(OnDoubleClickCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        public DelegateCommand<CarList> DoubleClickCommand { get; }
        public ObservableCollection<CarList> Cars { get { return _cars; } set { SetProperty(ref _cars, value); } }
        public CarList SelectedCar
        {
            get { return _car; }
            set
            {
                SetProperty(ref _car, value);
                _eventAggregator.GetEvent<SelectedEvent<Car>>().Publish(new SelectedEventArgs<Car>(_car?.Car));
            }
        }

        #region ViewModelBase overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            SelectedCar = null;
            RefreshCars();

        }

        #endregion

        private void RefreshCars(bool global = false)
        {
            try
            {
                Cars = null;

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<CarList>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "cars/list").ToString(),
                        _securityService.GetCurrentUser().AccessToken,
                        (e) =>
                        {
                            Cars = new ObservableCollection<CarList>(e.OrderBy(c => c.Car.Registration));
                        });
                }
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
                RefreshCars();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnDoubleClickCommand(CarList car)
        {
            try
            {
                if (null == car?.Car) return;
                _eventAggregator.GetEvent<EditEvent<Car>>().Publish(new EditEventArgs<Car>() { EditMode = EditMode.Edit, EditObject = car.Car, RefreshAction = OnEditCarEventRefresCallBack });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnEditCarEventRefresCallBack(Car obj)
        {
            try
            {
                RefreshCars();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            };
        }
    }
}
