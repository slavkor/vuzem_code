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
using System.ComponentModel;
using System.Windows.Data;

namespace Ism.Reports.ViewModels
{
    class ReportsUserBindViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<Report> _reports;
        private ObservableCollection<User> _users;

        private User _selectedUser;
        public ReportsUserBindViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
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

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public ObservableCollection<Report> Reports { get { return _reports; } set { SetProperty(ref _reports, value); } }
        public ObservableCollection<User> Users { get { return _users; } set { SetProperty(ref _users, value); } }
        public DelegateCommand CancelCommand { get; }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                try
                {
                    SetProperty(ref _selectedUser, value);
                    RefreshReports();
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                }
            }
        }


        private void RefreshReports(bool global = false)
        {
            try
            {

                Reports = null;

                if (null == SelectedUser) return;



                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Report>, string>>())
                {
                    repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "reports/list/all").ToString(),
                        _securityService.GetCurrentToken(),
                        (list) =>
                        {
                            try
                            {
                                repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), $"reports/list/user/{SelectedUser.UserName}").ToString(),
                                        _securityService.GetCurrentToken(),
                                        (userlist) =>
                                        {

                                            try
                                            {
                                                var comparer = new PropertyComparer<Report>("UuId");
                                                list.ForEach((r) => r.IsSelected = true);
                                                list.Except(userlist, comparer).ToList().ForEach(r => r.IsSelected = false);

                                                list.ForEach(ReportChangeHandler);
                                                Reports = new ObservableCollection<Report>(list.OrderBy(item => item.ReportId));
                                            }
                                            catch (Exception exc)
                                            {
                                                _exceptionService.RaiseException(exc);
                                            }
                                        });


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

        private void ReportChangeHandler(Report report)
        {
            try
            {
                report.PropertyDeletegate = (model) =>
                {
                    if (SelectedUser == null) return;
                    try
                    {
                        using (var rep = _serviceLocator.GetInstance<IRestRepository<object, object>>())
                        {
                            var rp = model as Report;
                            var ur = _settingsService.GetApiServer(false);
                            var url = rp.IsSelected ? ur.ToString() + $"reports/bind/{rp.UuId}/{SelectedUser.UserName}" : ur.ToString() + $"reports/unbind/{rp.UuId}/{SelectedUser.UserName}";

                            rep.PostRequestAsync(url, null, _securityService.GetCurrentToken(), (u) =>
                            {
                            });
                        }

                    }
                    catch (Exception exc)
                    {
                        _exceptionService.RaiseException(exc);
                    }
                };
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }


        private void RefreshUsers(bool global = false)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<User>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetAuthServer(), "/users/list").ToString(), _securityService.GetCurrentToken(), (u) =>
                    {
                        Users = new ObservableCollection<User>(u);
                    });
                }

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            RefreshUsers();
        }

        private void OnFinishInteraction()
        {
            try
            {
                Clear();
                NavigateBack();
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
                Users = null;
                SelectedUser = null;
                Reports = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
    }
}
