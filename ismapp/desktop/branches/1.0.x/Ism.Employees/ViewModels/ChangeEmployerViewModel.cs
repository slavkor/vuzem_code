using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

namespace Ism.Employees.ViewModels
{
    public class ChangeEmployerViewModel : ViewModelBase, IInteractionRequestAware
    {
        private EditInteraction<EmployeeChangeEmployer> _notification;

        private EmployeeChangeEmployer _employee;
        private EditMode _editMode;
        private bool _isFired;
        private readonly IExceptionService _exceptionService;

        public ChangeEmployerViewModel(IExceptionService exceptionService)
        {
            try
            {
                _exceptionService = exceptionService;
                SaveCommand = new DelegateCommand(OnSaveCommand, CanExecuteSaveCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        #region public properties

        public bool IsFired
        {
            get { return _isFired; }
            set
            {
                SetProperty(ref _isFired,value);
                if (_isFired)
                {
                    ;//if (null != Employee && Employee.FireDate == null) Employee.FireDate = DateTime.Now;
                }
                else
                {
                    Employee.FireDate = null;
                }
            }
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public EmployeeChangeEmployer Employee
        {
            get { return _employee; }
            set
            {
                SetProperty(ref _employee, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
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
                try
                {
                    if (!(value is EditInteraction<EmployeeChangeEmployer>)) return;
                    _notification = (EditInteraction<EmployeeChangeEmployer>)value;
                    Employee = _notification.InteractionObject;

                    if (null == Employee) return;

                    var equalEmployer = Employee.CompanyHire.ShortName == Employee.CompanyFire.ShortName;

                    if (Employee.HireDate == null)
                        Employee.HireDate = DateTime.Now;
                       
                    IsFired = Employee.FireDate != null;
                    Employee.PropertyDeletegate = OnPropertyChange;
                }
                catch (Exception exception)
                {
                    _exceptionService.RaiseException(exception);
                }

            }
        }
        public Action FinishInteraction { get; set; }

        #endregion

        private void OnPropertyChange(BaseModel model)
        {
            try
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
 
        private void OnFinishInteraction( bool confirm = false)
        {
            try
            {
                SaveCommand.RaiseCanExecuteChanged();
                _notification.Confirmed = confirm;
                FinishInteraction?.Invoke();

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
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmSaveCallback, Title = "ALO", Content = "Želiš shraniti spremembe?", PayLoad = Employee });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmSaveCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> payload)
        {
            try
            {
                OnFinishInteraction(confirmed);
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
            return Employee != null && Employee.IsDirty && !Employee.HasErrors;
        }
        
    }
}
