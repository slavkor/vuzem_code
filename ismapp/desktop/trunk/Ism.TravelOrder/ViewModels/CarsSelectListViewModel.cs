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
    public class CarsSelectListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<CarList> _cars;
        private IList<Document> _documents;
        private ListInteractionEx<CarList> _notification;

        public CarsSelectListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
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
                CancelCommand = new DelegateCommand(OnCancelCommand);
                SelectCommand = new DelegateCommand(OnSelectCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }



        public ObservableCollection<CarList> Cars { get { return _cars; } set { SetProperty(ref _cars, value); } }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SelectCommand { get; }

        public IList<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);
            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is ListInteractionEx<CarList>)) return;
                _notification = (ListInteractionEx<CarList>)value;


                RefreshCars(true);
            }
        }
        public Action FinishInteraction { get; set; }
        #endregion


        private void RefreshCars(bool global = false)
        {
            try
            {

                Cars = null;

                if (null != _notification?.DataProvider)
                {
                    _notification?.ListEventArgs.DataProvider.Invoke(OnDataProviderCallback);
                    return;
                }

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<CarList>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "cars/list").ToString(),
                        _securityService.GetCurrentToken(),
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


        private void OnDataProviderCallback(List<CarList> list)
        {
            try
            {
                Cars = new ObservableCollection<CarList>(list.OrderBy(car => car.Car.Registration));
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }
        private void OnSelectCommand()
        {
            try
            {
                OnFinishInteraction(true);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
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
        private void OnFinishInteraction(bool confirmed = false)
        {
            try
            {
                if (confirmed)
                {
                    _notification.Confirmed = confirmed;
                    _notification.SelectManyAction.Invoke(Cars.Where(e => e.Car.IsSelected == true).ToList());
                }
                FinishInteraction?.Invoke();

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

    }
}
