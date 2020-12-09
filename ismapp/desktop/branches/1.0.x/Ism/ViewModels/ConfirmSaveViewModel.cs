using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Ism.Infrastructure.Services;

namespace Ism.ViewModels
{
    public class ConfirmSaveViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IExceptionService _exceptionService;
        private ConfirmSaveNotification<BaseModel> _notification;

        public ConfirmSaveViewModel(IEventAggregator eventAggregator, IExceptionService exceptionService)
        {
            _eventAggregator = eventAggregator;
            _exceptionService = exceptionService;
            ConfirmSaveCommand = new DelegateCommand<bool?>(OnConfirmSaveCommand);
        }

        private void OnConfirmSaveCommand(bool? obj)
        {
            try
            {
                if (obj != null) _notification.Confirmed = obj.Value;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand<bool?> ConfirmSaveCommand { get; set; }

        #region IInteractionRequestAware

        public Action FinishInteraction
        {
            get;
            set;
        }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is ConfirmSaveNotification<BaseModel>)) return;
                SetProperty(ref _notification, (ConfirmSaveNotification<BaseModel>)value);
            }
        }
        #endregion
    }
}
