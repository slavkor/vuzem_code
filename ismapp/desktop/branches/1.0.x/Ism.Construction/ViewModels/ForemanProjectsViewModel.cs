using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using System.Collections.ObjectModel;

namespace Ism.Construction.ViewModels
{
    public class ForemanProjectsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAppCommands _appCommands;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ConstructionSite _currentConstructionSite;
        private bool _canSave;
        private InteractionRequest<EditInteraction<ConstructionSite>> _constructionSiteEditRequest;
        private ForemanConstructionSite _siteData;
        private Project _selectedProject;
        ObservableCollection<Project> _projects;
        public ForemanProjectsViewModel(ISettingsService settingsService, IAppCommands appCommands, ISecurityService securityService, IExceptionService exceptionService)
        {

            _appCommands = appCommands;
            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
        }

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                SetProperty(ref _selectedProject, value);
                if (null == _selectedProject) return;
                _eventAggregator.GetEvent<SelectedEvent<ForemanConstructionSite>>().Publish(new SelectedEventArgs<ForemanConstructionSite>() { SelectedData = SiteData });
            }
        }

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

        public ObservableCollection<Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                SetProperty(ref _projects, value);
            }
        }


        #region private methods


        #endregion


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                SiteData = navigationContext.Parameters["sitedata"] as ForemanConstructionSite;

                if (null != SiteData?.Projects)
                {
                    Projects = new ObservableCollection<Project>(SiteData?.Projects);
                }
                else
                {
                    using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Project>, string>>())
                    {
                        rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), $"csite/{SiteData.ConstructionSite.UuId}/project/list").ToString(),
                            _securityService.GetCurrentUser().AccessToken, (list) =>
                            {
                                try
                                {
                                    Projects = new ObservableCollection<Project>(list);
                                    SiteData.Projects = list;
                                }
                                catch (Exception exception)
                                {
                                    _exceptionService.RaiseException(exception);
                                }
                            });
                    }
                }


            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
