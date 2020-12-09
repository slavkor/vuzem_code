using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ism.Infrastructure;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Events;
using Prism.Modularity;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using Ism.Infrastructure.Services;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System.Net.Mail;
using System.Configuration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Ism.Infrastructure.Repository;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Ism.ViewModels
{

    public class ShellViewModel : WindowAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly IRegionManager _regionManager;
        private readonly IExceptionService _exceptionService;
        private BusyIndicatorNotification _busyNotification;
        private ExceptionNotification _exceptionNotification;
        private string _windowTitle;
        //private double _borderOpacity;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("EmailLogger");
        public ShellViewModel(IEventAggregator eventAggregator, IServiceLocator serviceLocator, IRegionManager regionManager, IExceptionService exceptionService)
        {
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));

            if (null == serviceLocator)
                throw new ArgumentNullException(nameof(serviceLocator));

            if (null == regionManager)
                throw new ArgumentNullException(nameof(regionManager));

            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _regionManager = regionManager;
            _exceptionService = exceptionService;
            AppBusyRequest = new InteractionRequest<BusyIndicatorNotification>();
            ExceptionRequest = new InteractionRequest<ExceptionNotification>();
            ConfirmSaveRequest = new InteractionRequest<ConfirmSaveNotification<BaseModel>>();

            _eventAggregator.GetEvent<BusyEvent>().Subscribe(OnBusyEvent, ThreadOption.UIThread);
            _eventAggregator.GetEvent<LoggedInEvent>().Subscribe(OnLoginEvent);
            _eventAggregator.GetEvent<LogoutEvent>().Subscribe(OnLogoutEvent);
            _eventAggregator.GetEvent<RaiseException>().Subscribe(OnRaiseException);
            _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Subscribe(OnConfirmSaveEvent);

            _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);



        }
        private bool _adornerVisible;

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set {  SetProperty( ref _isBusy ,value); }
        }


        public bool AdornerVisible
        {
            get { return _adornerVisible; }
            set { SetProperty(ref _adornerVisible, value); }
        }


        //public double BorderOpacity
        //{
        //    get { return _borderOpacity; }
        //    set { SetProperty(ref _borderOpacity, value); }
        //}

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
        }

        private void OnConfirmSaveEvent(ConfirmSaveEventArgs<BaseModel> obj)
        {
            try
            {
                //BorderOpacity = 0.2;
                ConfirmSaveRequest.Raise(new ConfirmSaveNotification<BaseModel>() {EventArgs = obj, Title = obj.Title, Content = obj.Content}, (request) =>
                {
                    //BorderOpacity = 0;
                    request.EventArgs.CallBackAction?.Invoke(request.Confirmed, request.EventArgs);
                });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                //BorderOpacity = 0;
            }

        }

        public InteractionRequest<BusyIndicatorNotification> AppBusyRequest { get; }
        public InteractionRequest<ExceptionNotification> ExceptionRequest { get; }
        public InteractionRequest<ConfirmSaveNotification<BaseModel>> ConfirmSaveRequest { get; }

        private void OnBusyEvent(BusyEventArgs args)
        {
            //return;
            //if (args.Busy)
            //{
            //    // ensure that we're not showing indicator already
            //    if (_busyNotification != null)
            //        return;

            //    _busyNotification = new BusyIndicatorNotification() {Title = args.Notification};
            //    //BorderOpacity = 0.2;
            //    AppBusyRequest.Raise(_busyNotification, notification =>
            //    {
            //        //BorderOpacity = 0;
            //    });
            //}
            //else
            //{
            //    if (_busyNotification == null)
            //        return;
            //    _busyNotification.RequestClose();
            //    _busyNotification = null;
            //    //BorderOpacity = 0;

            //}

            IsBusy = args.Busy;
        }

        private void OnLogoutEvent(User obj)
        {
            Application.Current.Shutdown(0);
        }

        private void OnLoginEvent(User user)
        {
            try
            {
                // set current user
                var security = _serviceLocator.TryResolve<ISecurityService>();

                if (null == security)
                    throw new Exception("Error unable to resolve an instance of ISecurityService");

                security.SetCurrentUser(user);

                // load modules
                var moduleManager = _serviceLocator.GetInstance<IModuleManager>();
                var moduleCatalog = _serviceLocator.GetInstance<IModuleCatalog>();

                foreach (var item in moduleCatalog.Modules.Where(m => m.InitializationMode == InitializationMode.OnDemand))
                    moduleManager.LoadModule(item.ModuleName);

                _eventAggregator.GetEvent<ApplySettingsEvent>().Publish();

                // dowload common data
                var common = _serviceLocator.TryResolve<ICommonService>();
                if (null != common)
                {
                    common.FetchShared<List<Country>>(@"/shrd/country/list", list =>
                    {
                        try
                        {
                            common.SetCountries(list);
                        }
                        catch (Exception exception)
                        {
                            _exceptionService.RaiseException(exception);
                        }
                    });
                    common.FetchShared<List<Language>>(@"/shrd/lang/list", list =>
                    {
                        try
                        {
                            common.SetLanguages(list);
                        }
                        catch (Exception exception)
                        {
                            _exceptionService.RaiseException(exception);
                        }
                    });
                    common.FetchShared<List<WorkPlace>>(@"/shrd/workplace/list", list =>
                    {
                        try
                        {
                            common.SetWokrPlaces(list);
                        }
                        catch (Exception exception)
                        {
                            _exceptionService.RaiseException(exception);
                        }
                    });
                    common.FetchShared<List<Company>>(@"/company/list", list =>
                    {
                        try
                        {
                            common.SetCompanies(list);

                            var settings = _serviceLocator.TryResolve<ISettingsService>();


                            using (var rep = _serviceLocator.GetInstance<IRestRepository<Employee, Employee>>())
                            {
                                rep.GetRequestAsync(new Uri(settings.GetApiServer(), $"/employees/{user.UserName}").ToString(), security.GetCurrentUser().AccessToken, (employee) =>
                                {
                                    try
                                    {
                                        if (null == employee)
                                        {
                                            _eventAggregator.GetEvent<ListEvent<Company>>().Publish(new ListEventArgs<Company>() { SelectAction = c => { security.SetCurrentCompany(c); }, RememberSelection = true });
                                            return;
                                        }

                                        security.SetCurrentEmployee(employee);

                                        using (var repo = _serviceLocator.GetInstance<IRestRepository<ForemanConstructionSite, object>>())
                                        {
                                            repo.GetRequestAsync(new Uri(settings.GetApiServer(), $"csite/byemployee/{security.GetCurrentEmployee().UuId}").ToString(), security.GetCurrentUser().AccessToken, (site) =>
                                            {
                                                try
                                                {
                                                    if (null == site?.ConstructionSite?.Company) return;

                                                    if (security.HasPermissionExcplicit("foreman"))
                                                        _eventAggregator.GetEvent<SelectedEvent<ForemanConstructionSite>>().Publish(new SelectedEventArgs<ForemanConstructionSite>() { SelectedData = site });

                                                    security.SetCurrentSite(site?.ConstructionSite);
                                                    security.SetCurrentCompany(list.Where(c => c.ShortName == site.ConstructionSite.Company.ShortName).FirstOrDefault());
                                                }
                                                catch (Exception exc)
                                                {
                                                    _exceptionService.RaiseException(exc);
                                                }
                                            });
                                        }

                                        //using (var repo = _serviceLocator.GetInstance<IRestRepository<CurrentWorkPeriod, object>>())
                                        //{
                                        //    var url = new Uri(settings.GetApiServer(true), $"employees/{employee.UuId}/workperiods/current");
                                        //    repo.GetRequestAsync(url.ToString(), security.GetCurrentUser().AccessToken,
                                        //        currentWork =>
                                        //        {
                                        //            try
                                        //            {
                                        //                if (null == curresentWork) return;
                                        //                security.SetCurrentCompany(list.Where(c => c.ShortName == currentWork.Current.Company.ShortName).FirstOrDefault());
                                        //            }
                                        //            catch (Exception exc)
                                        //            {
                                        //                _exceptionService.RaiseException(exc);
                                        //            }
                                        //        });
                                        //}
                                    }
                                    catch (Exception exc)
                                    {
                                        _exceptionService.RaiseException(exc);
                                    }
                                });
                            }
                        }
                        catch (Exception exception)
                        {
                            _exceptionService.RaiseException(exception);
                        }
                    });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnRaiseException(Exception exception)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _exceptionNotification?.RequestClose();
                _exceptionNotification = new ExceptionNotification() { Title = "Napaka", Exception = exception };
                ExceptionRequest.Raise(_exceptionNotification, notification =>
                {
                    LogException(exception);
                });
            });
        }

        public override bool CanExecuteCloseCommandByEscape(object window)
        {
            return false;
        }

        private void LogException(Exception exception)
        {
            try
            {
                var security = _serviceLocator.TryResolve<ISecurityService>();
                var settings = _serviceLocator.TryResolve<ISettingsService>();
                if(null == security || null == security.GetCurrentUser() || null == security.GetCurrentCompany())
                {
                    log.Error(exception);
                    return;
                }

                log.Error(new Exception($"\n Application error \n user:{security.GetCurrentUser().UserName} \n company:{security.GetCurrentCompany().ShortName} \n api:{settings.GetApiServer().AbsoluteUri} \n auth:{ settings.GetAuthServer().AbsoluteUri } \n\n", exception));
            }
            catch (Exception exc)
            {
                int a = 0;
            }
        }

        private void OnCompanySelectedEvent(Company company)
        {
            try
            {
                WindowTitle = null != company ? company.ShortName : "";

                // dowload company related data data
                var common = _serviceLocator.TryResolve<ICommonService>();
                if (null != common)
                {
                    common.FetchShared<List<DocumentType>>(@"/documents/types/list", list => { common.SetDocumentTypes(list.OrderBy(t => t.Name).ToList()); }, false);
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

    }
}
