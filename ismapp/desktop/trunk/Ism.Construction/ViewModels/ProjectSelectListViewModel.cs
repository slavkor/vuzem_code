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
namespace Ism.Construction.ViewModels
{
    public class ProjectSelectListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private ListInteraction<Project> _notification;
        private List<Project> _projects;
        private Project _selectedProject;
        private bool _isSelect;

        public ProjectSelectListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                SelectCommand = new DelegateCommand<Project>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public List<Project> Projects
        {
            get { return _projects; }
            set
            {
                SetProperty(ref _projects, value);

            }
        }
        public DelegateCommand<Project> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                SetProperty(ref _selectedProject, value);
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
                var notification = value as ListInteraction<Project>;
                if (notification != null)
                {
                    _notification = notification;
                    RefreshProjects();
                    IsSelect = true;
                }
            }
        }

        public Action FinishInteraction { get; set; }


        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Project>;
                Header = navigation.Header;

                _notification = navigation.EditInteraction as ListInteraction<Project>;
                RefreshProjects();
                IsSelect = false;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            try
            {
                Clear();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion

        private void RefreshProjects()
        {
            try
            {
                Projects = null;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Project>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(), "project/list").ToString(), _securityService.GetCurrentToken(),
                        list =>
                        {
                            Projects = list;
                            SelectedProject = null;
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
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
        private void OnSelectCommand(Project obj)
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
                Projects = null;

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
