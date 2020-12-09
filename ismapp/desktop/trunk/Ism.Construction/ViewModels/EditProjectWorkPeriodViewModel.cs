using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Extensions;
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
    class EditProjectWorkPeriodViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;
        private EditMode _editMode;
        
        private bool _loaded;
 
        private NavigationChildInteraction<Project, ProjectWorkPeriod> _notification;


        public EditProjectWorkPeriodViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService, ICommonService commonService)
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
                //WorkingHoursCommand = new DelegateCommand(OnWorkingHoursCommand, CanExecuteWorkingHoursCommand);
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
              

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }

        #region public properties



        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private ObservableCollection<WorkPlace> _workPlaces;
        private ObservableCollection<WorkPlan> _workPlans;

        public ObservableCollection<WorkPlace> WorkPlaces
        {
            get
            {
                return _workPlaces;
            }
            set
            {
                SetProperty(ref _workPlaces, value);
            }
        }

        
        public ObservableCollection<WorkPlan> WorkPlans
        {
            get
            {
                return _workPlans;
            }
            set
            {
                SetProperty(ref _workPlans, value);
            }
        }
        private ProjectWorkPeriod _workPeriod;
        public ProjectWorkPeriod WorkPeriod
        {

            get
            {
                return _workPeriod;
            }
            set
            {
                SetProperty(ref _workPeriod, value);
            }
        }

        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        #endregion


        #region ViewModelBase overrides

        public override bool KeepAlive => false;

        public INotification Notification
        {
            get
            {
                return _notification;
            }

            set
            {
                if (value is NavigationChildInteraction<Project, ProjectWorkPeriod>)
                {
                    _notification = (NavigationChildInteraction<Project, ProjectWorkPeriod>)value;

                    WorkPeriod = new ProjectWorkPeriod() { UuId = Guid.NewGuid().ToString(), Start = DateTime.Now, End = DateTime.Now.AddDays(7)};
                    WorkPlans = new ObservableCollection<WorkPlan>();
                    WorkPlaces = new ObservableCollection<WorkPlace>(_commonService.GetWorkPlaces());

                    RaisePropertyChanged(nameof(Notification));
                }
            }
        }

        public Action FinishInteraction
        {
            get;
            set;
        }


        #endregion

        private void OnPropertyChange(BaseModel model)
        {
            try
            {
                if (!_loaded)
                    return;

                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        private void OnSaveCommand()
        {
            try
            {

                _notification.Confirmed = true;
                WorkPeriod.WorkPlans = WorkPlans;
                _notification?.EditChildInteraction?.SaveAction?.Invoke(_notification.EditChildInteraction.InteractionObject, WorkPeriod, EditMode = EditMode);
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmSaveProjectCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                //Project project = args.PayLoad as Project;
                //_interaction.SaveAction?.Invoke(project, _interaction.EditMode);
                GoBack();
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
                GoBack();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private bool CanExecuteSaveCommand()
        {
            return true;
        }

        private void GoBack()
        {
            try
            {

                try
                {
                    FinishInteraction?.Invoke();
                    //eventAggregator.GetEvent<EditEvent<ConstructionSite>>().Publish(new EditEventArgs<ConstructionSite>() { EditObject = ConstructionSite, EditMode = EditMode.Edit });
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                }


                //if (_navigationContext.NavigationService.Journal.CanGoBack)
                //    _navigationContext.NavigationService.Journal.GoBack();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void RaiseCanExecuteChanged()
        {
            try
            {
                FinishInteraction?.Invoke();

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

    }
}

