using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;
using System.Collections.ObjectModel;

namespace Ism.Departure.ViewModels
{
    class EmployeeSelectListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private ListInteraction<EmployeeDepature> _notification;
        private ObservableCollection<EmployeeDepature> _list;
        private EmployeeDepature _selected;
        private bool _isSelect;

        public EmployeeSelectListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                SelectCommand = new DelegateCommand<EmployeeDepature>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public ObservableCollection<EmployeeDepature> List
        {
            get { return _list; }
            set
            {
                SetProperty(ref _list, value);

            }
        }
        public DelegateCommand<EmployeeDepature> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public EmployeeDepature Selected
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
            }
        }

        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                SetProperty(ref _isSelect, value);
            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as ListInteraction<EmployeeDepature>;
                if (notification != null)
                {
                    _notification = notification;
                    RefreshList();
                    IsSelect = true;
                }
            }
        }

        public Action FinishInteraction { get; set; }

        #endregion

        private void RefreshList()
        {
            try
            {
                List = null;

                if (null != _notification?.ListEventArgs?.DataProvider)
                {
                    _notification?.ListEventArgs?.DataProvider?.Invoke(OnDataProvideCallBack);
                    return;
                }

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(true), $"employees/listh").ToString(), _securityService.GetCurrentToken(),
                    list =>
                    {
                        List = new ObservableCollection<EmployeeDepature>(list);
                        Selected = null;
                    });
                    return;
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnDataProvideCallBack(List<EmployeeDepature> obj)
        {
            try
            {
                List = new ObservableCollection<EmployeeDepature>(obj);
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
                _notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnSelectCommand(EmployeeDepature obj)
        {
            try
            {
                _notification.Confirmed = true;
                _notification.SelectManyAction?.Invoke(List.Where(item =>item.IsSelected).ToList());
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void Clear()
        {
            try
            {
                List = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
