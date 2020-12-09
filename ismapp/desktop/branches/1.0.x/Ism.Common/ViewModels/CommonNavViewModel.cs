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
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Common.ViewModels
{
    public class CommonNavViewModel : ViewModelBase
    {
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ListViewNotification<BaseModel> _notification;
        private List<BaseModel> _data;
        private BaseModel _selectedData;

        public CommonNavViewModel(ISecurityService securityService, IExceptionService exceptionService)
        {
            _securityService = securityService;

            _exceptionService = exceptionService;
            CommonSifrantCommand = new DelegateCommand(OnCommonSifrantCommand, CanExecuteCommonSifrantCommand);


            ListViewInteractionRequest = new InteractionRequest<ListInteraction<BaseModel>>();
            _eventAggregator.GetEvent<ListEvent<BaseModel>>().Subscribe(OnListViewEvent);

            AddressInteractionRequest = new InteractionRequest<EditInteraction<Address>>();
            _eventAggregator.GetEvent<EditChildEvent<BaseModel, Address>>().Subscribe(OnEditAddress);

            ContactInteractionRequest = new InteractionRequest<EditInteraction<Contact>>();
            _eventAggregator.GetEvent<EditChildEvent<BaseModel, Contact>>().Subscribe(OnEditContact);

            WorkPlaceListInteractionRequest = new InteractionRequest<ListInteraction<WorkPlace>>();
            _eventAggregator.GetEvent<ListEvent<WorkPlace>>().Subscribe(OnListWorkPlaceEvent);

        }


        public InteractionRequest<ListInteraction<WorkPlace>> WorkPlaceListInteractionRequest { get; }


        public DelegateCommand CommonSifrantCommand { get;  }
        public InteractionRequest<EditInteraction<Address>> AddressInteractionRequest { get; }
        public InteractionRequest<EditInteraction<Contact>> ContactInteractionRequest{ get; }
        public InteractionRequest<ListInteraction<BaseModel>> ListViewInteractionRequest { get; }

        

        private void OnListViewEvent(ListEventArgs<BaseModel> obj)
        {
            try
            {
                ListViewInteractionRequest.Raise(new ListInteraction<BaseModel>() {Title = "TT", SelectAction = obj.SelectAction}, OnListViewInteractionRequestCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnListViewInteractionRequestCallback(ListInteraction<BaseModel> obj)
        {
            try
            {
                if (obj.Confirmed)
                    obj.SelectAction?.Invoke(obj.InteractionObject);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
     

        private void OnCommonSifrantCommand()
        {
            try
            {
                _regionManager.RequestNavigate(RegionNames.MainContentRegion, "CommonSifrant", NavigaionCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
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
        private void OnListWorkPlaceEvent(ListEventArgs<WorkPlace> args)
        {
            try
            {
                WorkPlaceListInteractionRequest.Raise(new ListInteraction<WorkPlace>() { Title = "Delovna mesta", SelectAction = args.SelectAction, SelectManyAction = args.SelectManyAction, ListEventArgs = args });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteCommonSifrantCommand()
        {

            try
            {
                return _securityService.HasPermission("common");
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            return false;
        }

        private void OnEditContact(EditChildEventArgs<BaseModel, Contact> obj)
        {
            try
            {
                ContactInteractionRequest.Raise(new EditInteraction<Contact>() { Title = obj.EditChildMode == EditMode.Edit ? "Urejanje kontakta " : "Nov kontakt ", InteractionObject = obj.EditChildObject, SaveAction = obj.SaveChildAction, EditMode = obj.EditChildMode });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnEditAddress(EditChildEventArgs<BaseModel, Address> obj)
        {
            try
            {
                AddressInteractionRequest.Raise(new EditInteraction<Address>() { Title = obj.EditChildMode == EditMode.Edit ? "Urejanje naslova " : "Nov naslov ", InteractionObject = obj.EditChildObject, SaveAction = obj.SaveChildAction, EditMode = obj.EditChildMode });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

    }
}


