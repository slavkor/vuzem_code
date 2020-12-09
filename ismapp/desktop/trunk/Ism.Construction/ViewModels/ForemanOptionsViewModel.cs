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
using System.Xml.Linq;
using Ism.Construction.Events;

namespace Ism.Construction.ViewModels
{
    public class ForemanOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _security;
        private readonly IExceptionService _exceptionService;
        private ForemanConstructionSite _siteData;
        private Project _project;
 
        public ForemanOptionsViewModel(ISettingsService settings, ISecurityService security, IExceptionService exceptionService)
        {
            _settings = settings;
            _security = security;
            _exceptionService = exceptionService;

            try
            {
                HoursCommand = new DelegateCommand(OnHoursCommand, CanExeuteHoursCommand);
                EwrCommand = new DelegateCommand(OnEwrCommand, CanExecuteEwrCommand);
                _eventAggregator.GetEvent<SelectedEvent<ForemanConstructionSite>>().Subscribe(OnProjectSelectedEvent);
                _eventAggregator.GetEvent<DateSelectedEvent>().Subscribe(OnDateSelectedEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDateSelectedEvent(DateTime obj)
        {
            try
            {
                Date = obj;
            }
            catch (Exception exc)
            {

                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand HoursCommand { get; }
        public DelegateCommand EwrCommand { get; }

        public ForemanConstructionSite SiteData
        {
            get
            {
                return _siteData;
            }
            set
            {
                SetProperty(ref _siteData, value);
            }
        }
        public Project Project {
            get
            {
                return _project;
            }
            set
            {
                SetProperty(ref _project, value);
                RaiseCanExecuteChanged();
                //if (_regionManager.Regions[Infrastructure.RegionNames.ForemanCSiteRegion] != null && _regionManager.Regions[Infrastructure.RegionNames.ForemanCSiteRegion].ActiveViews.Count() > 0)
                  //  _regionManager.Regions[Infrastructure.RegionNames.ForemanCSiteRegion].RemoveAll();
      
            }
        }

        public DateTime Date { get; set; }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                SiteData = navigationContext.Parameters["sitedata"] as ForemanConstructionSite;
                Project = null;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                var b = !navigationResult.Result;
                if (b != null && (bool)b)
                {
                    _exceptionService.RaiseException(navigationResult.Error);
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
 
        private void ReportMetaDataProvider(string meta, Action<string> callback)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(meta);
                XElement element = xdoc.Element("root").Element("contextparams").Element("projectId");
                if (element != null) element.Value = Project.UuId;
                element = xdoc.Element("root").Element("contextparams").Element("year");
                if (element != null) element.Value = Date.Year.ToString();
                element = xdoc.Element("root").Element("contextparams").Element("month");
                if (element != null) element.Value = Date.Month.ToString();

                callback?.Invoke(xdoc.ToString());
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        private void OnEwrCommand()
        {
            try
            {
                if (_regionManager.Regions.Any(r => r.Name == RegionNames.EwrReportsRegion))
                    _regionManager.Regions.Remove(RegionNames.EwrReportsRegion);

                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Dodatna dela" });
                parameters.Add("project", Project);
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.ForemanCSiteRegion, "Ewr", NavigaionCallback, parameters);

                _regionManager.Regions[RegionNames.ForemanReportsRegion].RemoveAll();

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnHoursCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Vnos ur" });
                parameters.Add("sitedata", SiteData);
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.ForemanCSiteRegion, "WorkingHours", parameters);

                parameters = new NavigationParameters();
                parameters.Add("context", "Foreman.Project");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                _regionManager.Regions[RegionNames.ForemanReportsRegion].RemoveAll();
                _regionManager.RequestNavigate(RegionNames.ForemanReportsRegion, "ReportsContext", NavigaionCallback, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void OnProjectSelectedEvent(SelectedEventArgs<ForemanConstructionSite> obj)
        {
            try
            {
                Project = obj.SelectedData.Projects.Where(p=>p.IsSelected).FirstOrDefault();
                HoursCommand.Execute();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteEwrCommand()
        {
            return Project != null;
        }

        private bool CanExeuteHoursCommand()
        {
            return Project != null;
        }

        private void RaiseCanExecuteChanged()
        {
            try
            {
                HoursCommand.RaiseCanExecuteChanged();
                EwrCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

    }
}
