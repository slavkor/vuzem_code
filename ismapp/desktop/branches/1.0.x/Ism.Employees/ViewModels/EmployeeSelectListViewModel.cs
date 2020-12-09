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
namespace Ism.Employees.ViewModels
{
    public class EmployeeSelectListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<EmployeeList> _employees;
        private IList<Document> _documents;
        private ListInteractionEx<EmployeeList> _notification;

        public EmployeeSelectListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
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
                DoubleClickCommand = new DelegateCommand<EmployeeList>(OnDoubleClickCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<EmployeeList> DoubleClickCommand { get; }


        public ObservableCollection<EmployeeList> Employees { get { return _employees; } set { SetProperty(ref _employees, value); } }

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
                if (!(value is ListInteractionEx<EmployeeList>)) return;
                _notification = (ListInteractionEx<EmployeeList>)value;
                
                RefreshEmployees(true);
            }
        }
        public Action FinishInteraction { get; set; }
        #endregion

        
        private void RefreshEmployees(bool global = false)
        {
            try
            {
                Employees = null;

                if (null != _notification?.DataProvider)
                {
                    _notification?.ListEventArgs.DataProvider.Invoke(OnDataProviderCallback);
                    return;
                }

                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeList>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listh").ToString(),
                        _securityService.GetCurrentUser().AccessToken,
                        (e) =>
                        {
                            Employees = new ObservableCollection<EmployeeList>(e.OrderBy(emp => emp.Employee.LastName));
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDataProviderCallback(List<EmployeeList> list)
        {
            try
            {
                Employees = new ObservableCollection<EmployeeList>(list.OrderBy(emp => emp.Employee.LastName));
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
                    _notification.SelectManyAction.Invoke(Employees.Where(e=>e.Employee.IsSelected).ToList());
                }
                FinishInteraction?.Invoke();

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnDoubleClickCommand(EmployeeList employee)
        {
            try
            {

                if (null == employee?.Employee) return;

                employee.Employee.IsSelected = true;
                OnFinishInteraction(true);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}
