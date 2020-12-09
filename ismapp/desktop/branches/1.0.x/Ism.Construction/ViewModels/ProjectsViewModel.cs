using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using System.Collections.ObjectModel;
using Prism.Commands;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Repository;
using Telerik.Windows.Controls;

namespace Ism.Construction.ViewModels
{
    public class ProjectsViewModel : Infrastructure.Mvvm.ViewModelBase
    {
        private ISecurityService _securityService;
        private ISettingsService _settingsService;
        private IExceptionService _exceptionService;
        private ObservableCollection<Project> _projects;
        private DateTime _minProjectDate, _maxProjectDate;
        private EditInteraction<Project> _interaction;

        public ProjectsViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {
            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;

            AddProjectCommand = new Prism.Commands.DelegateCommand(OnAddProjectCommand, CanExecuteAddProjectCommand);
            EditProjectCommand = new DelegateCommand<Project>(OnEditProjectCommand, CanExecuteEditProjectCommand);
            DeleteProjectCommand = new DelegateCommand<Project>(OnDeleteProjectCommand, CanExecuteDeleteProjectCommand);
            ConfirmProjectCommand = new DelegateCommand<Project>(OnConfirmProjectCommand, CanExecuteConfirmProjectCommand);
            CloseProjectCommand = new DelegateCommand<Project>(OnCloseProjectCommand, CanExecuteCloseProjectCommand);
        }



        #region public properties

        public Prism.Commands.DelegateCommand AddProjectCommand { get; set; }
        public DelegateCommand<Project> DeleteProjectCommand { get; set; }
        public DelegateCommand<Project> ConfirmProjectCommand { get; set; }
        public DelegateCommand<Project> CloseProjectCommand { get; set; }
        public DelegateCommand<Project> EditProjectCommand { get; set; }
        public ConstructionSite ConstructionSite { get; set; }

        

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set { SetProperty(ref _projects, value); }
        }

        public DateTime MinProjectDate
        {
            get { return _minProjectDate; }
            set { SetProperty(ref _minProjectDate, value); }
        }
        public DateTime MaxProjectDate
        {
            get { return _maxProjectDate; }
            set { SetProperty(ref _maxProjectDate, value); }
        }
        #endregion

        #region ViewModelBase overrides

        
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                MinProjectDate = DateTime.Now.FirstDayOfMonth();
                MaxProjectDate = DateTime.Now.LastDayOfMonth();

                _interaction = (navigationContext.Parameters["navigation"] as NavigationInteraction<Project>)?.EditInteraction;
                _interaction?.DataProvider?.Invoke(DataProviderCallback);

                ConstructionSite = navigationContext.Parameters["sitedata"] as ConstructionSite;

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        public override bool KeepAlive => false;
        #endregion



        #region private methods

        private bool CanExecuteEditProjectCommand(Project project)
        {
            try
            {
                return _securityService.HasPermission("csite.project.edit");

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }

        private bool CanExecuteAddProjectCommand()
        {
            try
            {
                return _securityService.HasPermission("csite.project.add");

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }

        private void OnAddProjectCommand()
        {
            try
            {
                _eventAggregator.GetEvent<EditChildEvent<ConstructionSite, Project>>().Publish(new EditChildEventArgs<ConstructionSite, Project>() { SaveChildAction = _interaction.SaveAction, EditChildMode = EditMode.New, EditObject = ConstructionSite, EditChildObject = null, EditMode = EditMode.Edit});

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnEditProjectCommand(Project project)
        {
            try
            {
                if (null == project) return;
                _eventAggregator.GetEvent<EditChildEvent<ConstructionSite, Project>>().Publish(new EditChildEventArgs<ConstructionSite, Project>() { SaveChildAction = _interaction.SaveAction, EditChildMode = EditMode.Edit, EditChildObject = project, EditMode = EditMode.Edit, EditObject = ConstructionSite});
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnUpdateProject(Project project, EditMode editMode)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnUpdateProjectConfirm, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = project });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnUpdateProjectConfirm(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;
                Project project = args.PayLoad as Project;
                _interaction.SaveAction.Invoke(project, EditMode.Edit);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void DataProviderCallback(List<Project> projects)
        {
            try
            {
                Projects  = null;
                if (null == projects) return;
                Projects = new ObservableCollection<Project>(projects);

                if(Projects.Count > 0)
                {

                    MinProjectDate = (from p in Projects select p.Start == null ? DateTime.Now.FirstDayOfMonth() : p.Start.Date).Min().AddDays(-1);
                    MaxProjectDate = (from p in Projects select p.End == null ? DateTime.Now.LastDayOfMonth() : p.End.Date).Max().AddDays(1);
                }
                else
                {
                    MinProjectDate = DateTime.Now.FirstDayOfMonth();
                    MaxProjectDate = DateTime.Now.LastDayOfMonth();
                }

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private bool CanExecuteDeleteProjectCommand(Project project)
        {
            return _securityService.HasPermission("csite.project.delete");
        }

        private void OnDeleteProjectCommand(Project project)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDeleteProjectCallback, Title = "ALO", Content = "Želiš pobrisati projekt?", FinishUp = true, PayLoad = project });

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDeleteProjectCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<object, object>>())
                {
                    rep.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/{args.PayLoad.UuId}/delete").ToString(), null, (e) =>
                    {
                        _interaction?.DataProvider?.Invoke(DataProviderCallback);

                    });
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private bool CanExecuteConfirmProjectCommand(Project arg)
        {
            return _securityService.HasPermission("csite.project.confirm");
        }

        private void OnConfirmProjectCommand(Project project)
        {
            try
            {
                project.ProjectState = ProjectState.InProgress;
                _interaction?.SaveAction?.Invoke(project, EditMode.Edit);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteCloseProjectCommand(Project arg)
        {
            return _securityService.HasPermission("csite.project.close");
        }

        private void OnCloseProjectCommand(Project project)
        {
            try
            {
                project.ProjectState = ProjectState.Closed;
                _interaction?.SaveAction?.Invoke(project, EditMode.Edit);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #endregion
    }
}
