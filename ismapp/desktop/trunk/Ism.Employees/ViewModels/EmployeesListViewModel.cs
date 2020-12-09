using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;
using System.ComponentModel;
using System.Windows.Data;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

using ViewModelBase = Ism.Infrastructure.Mvvm.ViewModelBase;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using Telerik.Windows.Documents.Fixed.Model;
using Microsoft.Win32;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model.Editing;

using Telerik.Windows.Documents.Spreadsheet.FormatProviders;

namespace Ism.Employees.ViewModels
{
    public class EmployeesListViewModel :  ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<Employee> _employees;
        private ObservableCollection<EmployeeDepature> _employeesAway;
        private ObservableCollection<EmployeeDepature> _employeesHome;
        private Employee _selectedEmployee;
        private IList<Document> _documents;
        private ListInteraction<Employee> _notification;
        private int _tabindex = 0;
        public EmployeesListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {

            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));


            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));
  
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                _eventAggregator.GetEvent<EmployeeAdded>().Subscribe(OnEmployeeAdded);
                _eventAggregator.GetEvent<EmployeeEdited>().Subscribe(OnEmployeeEdited);
                _eventAggregator.GetEvent<CompanySelectedEvent>().Subscribe(OnCompanySelectedEvent);

                DoubleClickCommand = new DelegateCommand<Employee>(OnDoubleClickCommand);
                TabSelectionChangedCommand = new DelegateCommand<object>(OnTabSelectionChangedCommand);
                PrintCommand = new DelegateCommand<object>(OnPrintCommand);
                ElementExportingToDocument = new DelegateCommand<GridViewElementExportingToDocumentEventArgs>(OnElementExportingToDocument);

                //_eventAggregator.GetEvent<ListEvent<Employee>>().Subscribe(OnListEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        public DelegateCommand<GridViewElementExportingToDocumentEventArgs> ElementExportingToDocument { get; }

        private void OnPrintCommand(object obj)
        {
            try
            {
                //var grid = obj as RadGridView;

                //RadSpreadsheet sheet = new RadSpreadsheet();
                //sheet.Workbook = grid.ExportToWorkbook();
                //PrintWhatSettings printWhatSettings = new PrintWhatSettings(ExportWhat.ActiveSheet, false);


                ////Instantiate the RadFixedDocument object 
                //RadFixedDocument fixedDoc = grid.ExportToRadFixedDocument(new GridViewDocumentExportOptions() { });
                ////RadFixedDocumentEditor editor = new RadFixedDocumentEditor(fixedDoc);
                ////editor.SectionProperties.PageRotation = Telerik.Windows.Documents.Fixed.Model.Data.Rotation.Rotate90;
                ////editor.SectionProperties.PageSize = new Size(100, 100);


                ////Modify the RadFixedDocument object 
                //foreach (var page in fixedDoc.Pages)
                //{
                //    var actualPage = page as RadFixedPage;
                //    actualPage.Size = new Size(840, 480); 
                //    //actualPage.Rotation = Telerik.Windows.Documents.Fixed.Model.Data.Rotation.Rotate90;
                //}


                ////Export the RadFixedDocument to a PDF file 
                //SaveFileDialog dialog = new SaveFileDialog();
                //dialog.DefaultExt = "*.pdf";

                //if (dialog.ShowDialog() == true)
                //{

                //    var provider = new PdfFormatProvider();
                //    using (var output = dialog.OpenFile())
                //    {
                //        provider.Export(fixedDoc, output);
                //    }
                //}

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnElementExportingToDocument(GridViewElementExportingToDocumentEventArgs e)
        {
            var a = e.VisualParameters;
            throw new NotImplementedException();
        }

        private void OnTabSelectionChangedCommand(object obj)
        {
            try
            {
                var tabitem = obj as Telerik.Windows.Controls.RadTabItem;

                if (null == tabitem) return;

                int.TryParse(tabitem.Tag.ToString(), out _tabindex);

                RefreshEmployees();
                
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<object> TabSelectionChangedCommand { get; }
        private void OnListEvent(ListEventArgs<Employee> obj)
        {
            throw new NotImplementedException();
        }

        private void OnDoubleClickCommand(Employee employee)
        {
            try
            {
                if(null == employee) return;

                _eventAggregator.GetEvent<EditEvent<Employee>>().Publish(new EditEventArgs<Employee>() { EditMode = EditMode.Edit, EditObject = employee } );
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand<Employee> DoubleClickCommand { get; }

        public DelegateCommand<object> PrintCommand { get; }

        private void OnCompanySelectedEvent(Company obj)
        {
            try
            {
                RefreshEmployees();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public ObservableCollection<Employee> Employees { get { return _employees; } set { SetProperty(ref _employees, value); } }
        public ObservableCollection<EmployeeDepature> EmployeesAway { get { return _employeesAway; } set { SetProperty(ref _employeesAway, value); } }
        public ObservableCollection<EmployeeDepature> EmployeesHome { get { return _employeesHome; } set { SetProperty(ref _employeesHome, value); } }

        public Employee SelectedEmployee {
            get { return _selectedEmployee; }
            set {
                try
                {
                    SetProperty(ref _selectedEmployee, value);
                    _eventAggregator.GetEvent<SelectedEvent<Employee>>().Publish(new SelectedEventArgs<Employee>(_selectedEmployee));
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                }
            }
        }

        public IList<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);

            }
        }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                if (!(value is ListInteraction<Employee>)) return;
                _notification = (ListInteraction<Employee>)value;
            }
        }
        public Action FinishInteraction { get; set; }
        #endregion

        


        private void OnEmployeeAdded(Employee employee)
        {
            try
            {
                _eventAggregator.GetEvent<SelectedEvent<Employee>>().Publish(null);
                RefreshEmployees();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEmployeeEdited(Employee employee)
        {
            try
            {
                _eventAggregator.GetEvent<SelectedEvent<Employee>>().Publish(null);
                RefreshEmployees();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RefreshEmployees(bool global = false)
        {
            try
            {


                Employees = null;

                switch (_tabindex)
                {
                    case 0:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Employee>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listemployed").ToString(), _securityService.GetCurrentToken(),
                             (list) =>
                             {
                                 Employees = new ObservableCollection<Employee>(list.OrderBy(emp => emp.LastName));
                             });
                        }
                        break;
                    case 1:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Employee>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listfired").ToString(), _securityService.GetCurrentToken(),
                             (list) =>
                             {
                                 Employees = new ObservableCollection<Employee>(list.Where(emp => emp.Loaner == null).OrderBy(emp => emp.LastName));
                             });
                        }
                        break;
                    case 2:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Employee>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listloaned").ToString(), _securityService.GetCurrentToken(),
                             (list) =>
                             {
                                 Employees = new ObservableCollection<Employee>(list.Where(emp => emp.Loaner != null).OrderBy(emp => emp.LastName));
                             });
                        }
                        break;

                    case 3:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listaway").ToString(), _securityService.GetCurrentToken(),
                             (list) =>
                             {
                                 EmployeesAway = new ObservableCollection<EmployeeDepature>(list.OrderBy(emp => emp.LastName));
                             });
                        }
                        break;
                    case 4:
                        using (var repository = _serviceLocator.GetInstance<IRestRepository<List<EmployeeDepature>, string>>())
                        {

                            repository.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), "employees/listhome").ToString(), _securityService.GetCurrentToken(),
                             (list) =>
                             {
                                 EmployeesHome= new ObservableCollection<EmployeeDepature>(list.OrderBy(emp => emp.LastName));
                             });
                        }
                        break;
                }


            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedEmployee = null;
            base.OnNavigatedTo(navigationContext);
            RefreshEmployees();
        }

    }
}
