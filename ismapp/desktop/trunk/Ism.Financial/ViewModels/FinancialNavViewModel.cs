﻿using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Financial.ViewModels
{
    public class FinancialNavViewModel : ViewModelBase
    {
        private readonly IExceptionService _exceptionService;
        public FinancialNavViewModel(IExceptionService exceptionService)
        {
            try
            {
                _exceptionService = exceptionService;
                NavigateEmployees = new DelegateCommand(OnNavigateEmployees);

                EmployeeSelectListRequest = new InteractionRequest<ListInteractionEx<EmployeeList>>();
                _eventAggregator.GetEvent<ListEvent<EmployeeList>>().Subscribe(OnEmployeeListEvent);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand NavigateEmployees { get; }


        public InteractionRequest<ListInteractionEx<EmployeeList>> EmployeeSelectListRequest { get; }

        private void OnEmployeeListEvent(ListEventArgs<EmployeeList> args)
        {
            try
            {
                EmployeeSelectListRequest.Raise(new ListInteractionEx<EmployeeList>() { Title = "Izbira zaposlenih", ListEventArgs = args, SelectManyAction = args.SelectManyAction, DataProvider = args.DataProvider }, OnEmployeeSelectListRequestCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEmployeeSelectListRequestCallback(ListInteraction<EmployeeList> obj)
        {
            //throw new NotImplementedException();
        }


        private void OnNavigateEmployees()
        {
            try
            {
                _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, "Employees", NavigaionCallback);
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