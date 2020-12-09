using Ism.Infrastructure;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using Ism.Infrastructure.Interaction;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Mvvm;
using Prism;


namespace Ism.Reports.ViewModels
{
    public class ReportEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private EditInteraction<Report> _notification;

        private Report _reportMetaData;
        private EditMode _editMode;

        private bool _loaded;

        public ReportEditViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));


            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;

            try
            {

                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);

                CanSave = false;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            _loaded = false;
        }

        #region public properties

        private bool _canSave;


        public bool CanSave
        {
            get
            {
                return _canSave;
            }
            set
            {
                SetProperty(ref _canSave, value);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; }
        public Report Report
        {
            get { return _reportMetaData; }
            set { SetProperty(ref _reportMetaData, value); }
        }
        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }
        
        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                _loaded = false;

                if (!(value is EditInteraction<Report>)) return;
                _notification = (EditInteraction<Report>)value;
                _notification.SaveAction = SaveAction;
                EditMode = _notification.EditMode;
                LoadReportMetaDataData(_notification.EditMode == EditMode.New ? new Report() { UuId = Guid.NewGuid().ToString(), Active = 1, Deleted = 0} : _notification.InteractionObject);
                _loaded = true;
            }
        }
        public Action FinishInteraction { get; set; }

        #endregion


        #region VieModelBase overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {

            _loaded = false;
            Clear();
            base.OnNavigatedTo(navigationContext);

            var navigation = navigationContext.Parameters["navigation"] as NavigationInteraction<Report>;

            if (!(navigation.EditInteraction is EditInteraction<Report>)) return;
            _notification = navigation.EditInteraction;
            _notification.SaveAction = SaveAction;
            EditMode = _notification.EditMode;
            LoadReportMetaDataData(_notification.EditMode == EditMode.New ? new Report() { UuId = Guid.NewGuid().ToString() } : _notification.InteractionObject);

            _loaded = true;

        }

        public override bool KeepAlive => false;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion

        private void OnPropertyChange(BaseModel model)
        {
            try
            {
                if (!_loaded)
                    return;

                CanSave = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void SaveAction(Report obj, EditMode editMode)
        {
            try
            {
                SaveReportMetaData(obj);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void SaveReportMetaData(Report obj, bool finish = true)
        {
            try
            {
                if (!Report.IsDirty || !SaveCommand.CanExecute())
                {
                    if (finish) OnFinishInteraction();
                    return;
                }
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveReportMetaDataCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", FinishUp = finish, PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnConfirmSaveReportMetaDataCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed || !Report.IsDirty)
                {
                    if (args.FinishUp) OnFinishInteraction();
                    return;
                }

                Report employee = args.PayLoad as Report;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Report, Report>>())
                {
                    var url = _notification.EditMode == EditMode.New
                        ? new Uri(_settings.GetApiServer(), "reports/add")
                        : new Uri(_settings.GetApiServer(), "reports/update");

                    repositroy.PostRequestAsync(url.ToString(), employee,
                        _securityService.GetCurrentUser().AccessToken,
                        (data) =>
                        {
                            if (_notification.EditMode == EditMode.New)
                            {
                                Report = data;
                            }
                            else
                                if (args.FinishUp) OnFinishInteraction();
                        });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                OnFinishInteraction();
            }
        }
        private void OnFinishInteraction()
        {
            try
            {
                Clear();
                FinishInteraction?.Invoke();
                NavigateBack();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void LoadReportMetaDataData(Report report)
        {
            try
            {
                Report = report;
                Report.Errors.ValidateProperties();
                Report.ErrorsChanged += (sender, args) =>
                {

                    CanSave = true;
                    SaveCommand.RaiseCanExecuteChanged();
                };

                Report.PropertyDeletegate = OnPropertyChange;

                Report.IsDirty = false;
                CanSave = true;
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
                SaveReportMetaData(Report);
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
                OnFinishInteraction();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private bool CanExecuteSaveCommand()
        {
            return Report != null && Report.IsDirty && !Report.HasErrors;
        }
        private void Clear()
        {
            try
            {
                Report = null;
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
    }
}
