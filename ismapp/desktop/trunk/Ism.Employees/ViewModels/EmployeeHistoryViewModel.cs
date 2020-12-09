using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ism.Infrastructure.Interaction;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;


namespace Ism.Employees.ViewModels
{
    class EmployeeHistoryViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;

        private EditInteraction<Employee> _notification;

        private Employee _employee;
        private EditMode _editMode;

        public EmployeeHistoryViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService, ICommonService commonService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
            _commonService = commonService;

            try
            {
   
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #region public properties

        public Employee Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }


        private ObservableCollection<HireHistory> history;

        public ObservableCollection<HireHistory> History
        {
            get { return history; }
            set { SetProperty(ref history, value); }
        }

        private ObservableCollection<WorkHistory> workHistory;

        public ObservableCollection<WorkHistory> WorkHistory
        {
            get { return workHistory; }
            set { SetProperty(ref workHistory, value); }
        }

        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is EditInteraction<Employee>)) return;

                _notification = (EditInteraction<Employee>)value;
            }
        }
        public Action FinishInteraction { get; set; }


        #endregion


        #region VieModelBase overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                var navigation = navigationContext.Parameters["navigation"] as EditInteraction<Employee>;
                Employee = navigation.Content as Employee;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<HireHistory>, HireHistory>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/hirehistory").ToString(), _securityService.GetCurrentToken(),
                    (e) =>
                    {
                        History = new ObservableCollection<HireHistory>(e);
                    });
                }
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<WorkHistory>, WorkHistory>>())
                {
                    rep.GetRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/workhistory").ToString(), _securityService.GetCurrentToken(),
                    (e) =>
                    {
                        WorkHistory = new ObservableCollection<WorkHistory>(e);
                    });
                }
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

        private string emp;

        public string Emp
        {
            get { return emp; }
            set { SetProperty(ref emp, value); }
        }
    }
}
