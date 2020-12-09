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

namespace Ism.Common.ViewModels
{
    public class WorkPlacesListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;
        private ListInteraction<WorkPlace> _notification;
        private List<WorkPlace> _workPlaces;
        private WorkPlace _selectedWorkPlace;
        private bool _isSelect;

        public WorkPlacesListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService, ICommonService commonService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            _commonService = commonService;
            try
            {
                SelectCommand = new DelegateCommand<WorkPlace>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        public List<WorkPlace> WorkPlaces
        {
            get { return _workPlaces; }
            set
            {
                SetProperty(ref _workPlaces, value);

            }
        }
        public DelegateCommand<WorkPlace> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public WorkPlace SelectedWorkPlace
        {
            get { return _selectedWorkPlace; }
            set
            {
                SetProperty(ref _selectedWorkPlace, value);
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
                var notification = value as ListInteraction<WorkPlace>;
                if (notification != null)
                {
                    _notification = notification;
                    RefreshWorkPlaces();
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

                var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<WorkPlace>;
                Header = navigation.Header;

                _notification = navigation.EditInteraction as ListInteraction<WorkPlace>;
                RefreshWorkPlaces();
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

        private void RefreshWorkPlaces()
        {
            try
            {
                WorkPlaces = _commonService.GetWorkPlaces();
                SelectedWorkPlace = null;

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
        private void OnSelectCommand(WorkPlace obj)
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
                WorkPlaces = null;
                SelectedWorkPlace = null;

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
