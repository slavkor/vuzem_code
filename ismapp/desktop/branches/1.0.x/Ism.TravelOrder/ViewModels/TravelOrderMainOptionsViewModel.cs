using Ism.Infrastructure;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Prism.Interactivity.InteractionRequest;

namespace Ism.TravelOrder.ViewModels
{
    class TravelOrderMainOptionsViewModel : ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        public TravelOrderMainOptionsViewModel(IExceptionService exceptionService)
        {
            try
            {
                _exceptionService = exceptionService;
                NavigateCars = new DelegateCommand(OnNavigateCars);
                CarSelectListRequest = new InteractionRequest<ListInteraction<CarList>>();
                _eventAggregator.GetEvent<ListEvent<CarList>>().Subscribe(OnCarListEvent);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand NavigateCars { get; }


        public InteractionRequest<ListInteraction<CarList>> CarSelectListRequest { get; }


        private void OnCarListEvent(ListEventArgs<CarList> args)
        {
            try
            {
                CarSelectListRequest.Raise(new ListInteractionEx<CarList>() { Title = "Izbira Avtomobila", ListEventArgs = args, SelectManyAction = args.SelectManyAction, DataProvider = args.DataProvider }, OnCarSelectListRequestCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }



        private void OnCarSelectListRequestCallback(ListInteraction<CarList> obj)
        {
            //throw new NotImplementedException();
        }


        private void OnNavigateCars()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "Cars", NavigaionCallback);
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
    }
}
