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
using Ism.Employees.Commands;
using Prism;
using Ism.Infrastructure.Extensions;
using TCtrl = Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Ism.Employees.ViewModels
{
    public class VacSickLeaveViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;
        
        private EditInteraction<Employee> _notification;

        private Employee _employee;
        private EditMode _editMode;

        public VacSickLeaveViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService, ICommonService commonService)
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
                DeleteCommand = new DelegateCommand<object>(OnDeleteCommand);
                EditCommand = new DelegateCommand<object>(OnEditCommand);
                ValidateCommand = new DelegateCommand<object>(OnValidateCommand);
                AddingNewDataItemCommand = new DelegateCommand<object>(OnAddingNewDataItemCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }





        #region public properties

        public Employee Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        public EditMode EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }


        private ObservableCollection<Absence> absences;

        public ObservableCollection<Absence> Absences
        {
            get { return absences; }
            set { SetProperty(ref absences ,value); }
        }
        private ObservableCollection<AbsenceType> absenceTypes;

        public ObservableCollection<AbsenceType> AbsenceTypes
        {
            get
            {
                return absenceTypes;
            }
            set
            {
                SetProperty(ref absenceTypes, value);
            }
        }


        public DelegateCommand<object> DeleteCommand { get; private set; }
        public DelegateCommand<object> EditCommand { get; private set; }
        public DelegateCommand<object> ValidateCommand { get; private set; }
        public DelegateCommand<object> AddingNewDataItemCommand { get; private set; }
        

        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is EditInteraction<Employee>)) return;

                _notification = (EditInteraction<Employee>)value;
            }
        }
        public Action FinishInteraction { get; set; }


        #endregion


        #region VieModelBase overrides

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                var navigation = navigationContext.Parameters["navigation"] as EditInteraction<Employee>;
                Employee = navigation.Content as Employee;

                AbsenceTypes = new ObservableCollection<AbsenceType>();
                AbsenceTypes.Add(new AbsenceType() { Type = 0, Description = "Dopust" });
                AbsenceTypes.Add(new AbsenceType() { Type = 1, Description = "Bolniška" });
                AbsenceTypes.Add(new AbsenceType() { Type = 2, Description = "Odsotnost" });
                AbsenceTypes.Add(new AbsenceType() { Type = 3, Description = "Čakanje na delo" });
                AbsenceTypes.Add(new AbsenceType() { Type = 4, Description = "Višja sila-varstvo otrok" });
                AbsenceTypes.Add(new AbsenceType() { Type = 5, Description = "Višja sila-ukinitev javnega prevoza" });
                AbsenceTypes.Add(new AbsenceType() { Type = 6, Description = "Višja sila-zaprtje meja" });
                AbsenceTypes.Add(new AbsenceType() { Type = 7, Description = "Karantena" });
                AbsenceTypes.Add(new AbsenceType() { Type = 8, Description = "Samoizolacija" });

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Absence>, Absence>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/absence/list").ToString(), null, _securityService.GetCurrentToken(),
                   (e) =>
                   {
                       Absences = new ObservableCollection<Absence>(e);
                   });
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        public override bool KeepAlive => false;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion

        private string emp;

        public string Emp
        {
            get { return emp; }
            set { SetProperty(ref emp ,value); }
        }


        #region private methods 

        private void OnValidateCommand(object obj)
        {
            try
            {
                var args = obj as TCtrl.GridViewRowValidatingEventArgs;
                var item = args.Row.Item as Absence;

                switch (args.EditOperationType)
                {
                    case TCtrl.GridView.GridViewEditOperationType.Insert:
                        _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>()
                        {
                            CallBackAction = (confirme, arg) => {
                                if (!confirme)
                                {
                                    ((TCtrl.RadGridView)args.OriginalSource).CancelEdit();
                                    args.Handled = true;
                                }
                            },
                            Title = "POZOR",
                            Content = $"Želiš dodati odsotnost tipa {item.Type} od {item.From.ToShortDateString()} do {item.To.ToShortDateString()}?",
                            FinishUp = true,
                            PayLoad = null
                        });
                        break;
                    case TCtrl.GridView.GridViewEditOperationType.Edit:
                        _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>()
                        {
                            CallBackAction = (confirme, arg) => {
                                if (!confirme)
                                {
                                    ((TCtrl.RadGridView)args.OriginalSource).CancelEdit();
                                    args.Handled = true;
                                }
                            },
                            Title = "POZOR",
                            Content = $"Želiš spremeniti odsotnost tipa {item.Type} od {item.From.ToShortDateString()} do {item.To.ToShortDateString()}?",
                            FinishUp = true,
                            PayLoad = null
                        });
                        break;
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnEditCommand(object obj)
        {
            try
            {
                var args = obj as TCtrl.GridViewRowEditEndedEventArgs;
                var absence = args.NewData as Absence;

                if (args.EditAction == TCtrl.GridView.GridViewEditAction.Cancel) return;

                switch (args.EditOperationType)
                {
                    case TCtrl.GridView.GridViewEditOperationType.Insert:
                        AddAbsence(absence);
                        break;
                    case TCtrl.GridView.GridViewEditOperationType.Edit:
                        EditAbsence(absence);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void OnDeleteCommand(object obj)
        {
            try
            {
                var args = obj as TCtrl.GridViewDeletingEventArgs;
                var absence = args.Items.FirstOrDefault() as Absence;

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>()
                {
                    CallBackAction = (confirme, argg) => {
                        try
                        {

                            if (!confirme)
                            {
                                args.Cancel = true;
                                args.Handled = true;
                                return;
                            }

                            DeleteAbsence(absence);
                        }
                        catch (Exception exception)
                        {
                            _exceptionService.RaiseException(exception);
                        }
                    },
                    Title = "POZOR",
                    Content = $"Želiš izbrisati odsotnost tipa {absence.Type} za obdobje od {absence.From.ToShortDateString()} do {absence.To.ToShortDateString()}",
                    FinishUp = true,
                    PayLoad = absence
                });

   
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }

        }

        private void OnAddingNewDataItemCommand(object obj)
        {
            try
            {
                var agrs = obj as GridViewAddingNewEventArgs;
                agrs.NewObject = new Absence() { From = DateTime.Now, To = DateTime.Now, Type = "Dopust" };
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void AddAbsence(Absence absence)
        {
            try
            {
                if (absence == null) return;
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Absence, Absence>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/absence/add").ToString(), absence, _securityService.GetCurrentToken(),
                   (e) =>
                   {
                       int a = 0;
                   });
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }


            //_eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = (confirme, args) => {
            //    try
            //    {
            //        using (var rep = _serviceLocator.GetInstance<IRestRepository<Absence, Absence>>())
            //        {
            //            rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/absence/add").ToString(), absence, _securityService.GetCurrentUser().AccessToken,
            //           (e) =>
            //           {
            //               int a = 0;
            //           });
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        _exceptionService.RaiseException(exception);
            //    }
            //}, Title = "POZOR", Content = $"Želiš dodati odsotnost tipa {absence.Type} za obdobje od {absence.From.ToShortDateString()} do {absence.To.ToShortDateString()}", FinishUp = true, PayLoad = absence
            //});


            
        }

        private void EditAbsence(Absence absence)
        {
            if (absence == null) return;
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Absence, Absence>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/absence/update").ToString(), absence, _securityService.GetCurrentToken(),
                   (e) =>
                   {
                       int a = 0;
                   });
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
            //_eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>()
            //{
            //    CallBackAction = (confirme, args) => {

            //    },
            //    Title = "POZOR",
            //    Content = $"Želiš spremeniti odsotnost tipa {absence.Type} za obdobje od {absence.From.ToShortDateString()} do {absence.To.ToShortDateString()}",
            //    FinishUp = true,
            //    PayLoad = absence
            //});
        }

        private void DeleteAbsence(Absence absence)
        {

            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Absence, Absence>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), $"employees/{Employee.UuId}/absence/delete").ToString(), absence, _securityService.GetCurrentToken(),
                   (e) =>
                   {
                       int a = 0;
                   });
                }
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }



        }

        #endregion
    }
}
