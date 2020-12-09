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
using Ism.Infrastructure;

namespace Ism.Departure.ViewModels
{
    class OriginDestinationSelectListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private ListInteraction<IDepartureArrival> _notification;
        private ObservableCollection<IDepartureArrival> _list;
        private IDepartureArrival _selected;
        private bool _isSelect;

        public OriginDestinationSelectListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                SelectCommand = new DelegateCommand<IDepartureArrival>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public ObservableCollection<IDepartureArrival> List
        {
            get { return _list; }
            set
            {
                SetProperty(ref _list, value);

            }
        }
        public DelegateCommand<IDepartureArrival> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public IDepartureArrival Selected
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
                var notification = value as ListInteraction<IDepartureArrival>;
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

                if(null != _notification?.ListEventArgs?.DataProvider)
                {
                    _notification?.ListEventArgs?.DataProvider?.Invoke(OnDataProvideCallBack);
                    return;
                }

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Project>, string>>())
                {

                    if (_securityService.HasPermissionExcplicit("foreman"))
                    {
                        rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"csite/{_securityService.GetCurrentSite().UuId}/project/list").ToString(), _securityService.GetCurrentUser().AccessToken,
                        list =>
                        {
                            List = new ObservableCollection<IDepartureArrival>();
                            List.Add(_securityService.GetCurrentCompany());
                            List.AddRange(list);
                            List.Remove(List.Where(item => item.UuId == _notification.InteractionObject?.UuId).FirstOrDefault());
                            Selected = null;
                        });
                        return;
                    }

                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/list/{(int)ProjectState.InProgress}").ToString(), _securityService.GetCurrentUser().AccessToken,
                        list =>
                        {
                            List = new ObservableCollection<IDepartureArrival>();
                            List.Add(_securityService.GetCurrentCompany());
                            List.AddRange(list);
                            List.Remove(List.Where(item => item.UuId == _notification.InteractionObject?.UuId).FirstOrDefault());
                            Selected = null;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnDataProvideCallBack(List<IDepartureArrival> obj)
        {
            try
            {
                List = new ObservableCollection<IDepartureArrival>(obj);
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
        private void OnSelectCommand(IDepartureArrival obj)
        {
            try
            {
                _notification.Confirmed = true;
                _notification.SelectAction?.Invoke(obj);
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
