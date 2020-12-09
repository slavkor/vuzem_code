﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure.Mvvm;

using System.Collections;
using System.Xml.Linq;
using Ism.Departure.Events;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

namespace Ism.Departure.ViewModels
{
    class DepartureOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private Range _dateRange;
        public DepartureOptionsViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;

            DepartureEditRequest = new InteractionRequest<EditInteraction<Infrastructure.Model.Departure>>();
            

            ListDepartureCommand = new DelegateCommand(OnListDepartureCommand);
            NewDepartureCommand = new DelegateCommand(OnNewDepartureCommand);
            EditDepartureCommand = new DelegateCommand(OnEditDepartureCommand, CanExecuteEditDepartureCommand);
            DeleteDepartureCommand = new DelegateCommand(OnDeleteDepartureCommand, CanExecuteDeleteDepartureCommand);
            ConfirmDepartureCommand = new DelegateCommand(OnConfirmDepartureCommand, CanExecuteConfirmDepartureCommand);
            PrintDocumentsCommand = new DelegateCommand(OnPrintDocumentsCommand, CanPrintDocumentsCommand);
            _eventAggregator.GetEvent<SelectedEvent<DepartureList>>().Subscribe(OnDepartureSelectedEvent);
            _eventAggregator.GetEvent<EditEvent<DepartureList>>().Subscribe(OnDepartureEditEvent);
            _eventAggregator.GetEvent<DateRange>().Subscribe(OnDateRangeEvent);
        }



        private void OnDateRangeEvent(Range obj)
        {
            _dateRange = obj;
        }


        #region public properties

        public InteractionRequest<EditInteraction<Infrastructure.Model.Departure>> DepartureEditRequest { get; }
        
        
        public DepartureList CurrentDeparture { get; set; }

        #endregion


        #region commands

        
        public DelegateCommand ListDepartureCommand { get; }
        public DelegateCommand NewDepartureCommand { get; }
        public DelegateCommand EditDepartureCommand { get; }
        public DelegateCommand DeleteDepartureCommand { get; }
        public DelegateCommand ConfirmDepartureCommand { get; }
        public DelegateCommand PrintDocumentsCommand { get; }
        
        #endregion

        #region ViewModelBase overrides
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                CurrentDeparture = null;
                RaiseCanExecuteChanged();
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }
        public override bool KeepAlive => true;
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            _regionManager.Regions[RegionNames.ReportsRegion].RemoveAll();
            CurrentDeparture = null;
            RaiseCanExecuteChanged();

        }
        #endregion

        #region private methods

        #region commands
        private void OnNewDepartureCommand()
        {
            try
            {
                DepartureEditRequest.Raise(new EditInteraction<Infrastructure.Model.Departure>() { Title = "Novi odhod", EditMode = EditMode.New }, OnDepartureEditRequestCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDepartureCommand()
        {
            try
            {
                if (CurrentDeparture == null) return;
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDepartureCallback, Title = "ALO", Content = "Želiš potrditi odhod? Kasnejše spremembe več ne bodo možne!!", FinishUp = false, PayLoad = CurrentDeparture.Departure });

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDepartureCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                Infrastructure.Model.Departure data = args.PayLoad as Infrastructure.Model.Departure;

                if (null == data) return;

                ConfirmDeparture(data);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ConfirmDeparture(Infrastructure.Model.Departure data)
        {
            try
            {
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Departure, Infrastructure.Model.Departure>>())
                {
                    var url = new Uri(_settings.GetApiServer(), $"departures/{data.UuId}/confirm");
                    repositroy.PostRequestAsync(url.ToString(), data,
                        _securityService.GetCurrentToken(),
                        (d) =>
                        {
                            try
                            {
                                OnDepartureEditRequestCallback(new EditInteraction<Infrastructure.Model.Departure>() { EditMode = EditMode.List, InteractionObject = data, Confirmed = true });
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnDeleteDepartureCommand()
        {
            try
            {
                if (CurrentDeparture == null) return;

                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmDeleteDepartureCallback, Title = "ALO", Content = "Želiš izbrisati odhod? Kasnejše spremembe več ne bodo možne!!", FinishUp = false, PayLoad = CurrentDeparture.Departure });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnConfirmDeleteDepartureCallback(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed) return;

                Infrastructure.Model.Departure data = args.PayLoad as Infrastructure.Model.Departure;

                if (null == data) return;

                DeleteDeparture(data);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void DeleteDeparture(Infrastructure.Model.Departure data)
        {
            try
            {
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Departure, Infrastructure.Model.Departure>>())
                {
                    var url = new Uri(_settings.GetApiServer(), $"departures/{data.UuId}/cancel");
                    repositroy.PostRequestAsync(url.ToString(), data,
                        _securityService.GetCurrentToken(),
                        (d) =>
                        {
                            try
                            {
                                OnDepartureEditRequestCallback(new EditInteraction<Infrastructure.Model.Departure>() { EditMode = EditMode.Delete, InteractionObject = data, Confirmed = true });

                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEditDepartureCommand()
        {
            try
            {
                if (CurrentDeparture == null) return;

                var dep = CurrentDeparture.Departure;
                dep.Employees = CurrentDeparture.Employees;
                dep.Cars = CurrentDeparture.Cars;

                DepartureEditRequest.Raise(new EditInteraction<Infrastructure.Model.Departure>() { Title = "Ureajnje odhoda", EditMode = EditMode.Edit, InteractionObject = dep }, OnDepartureEditRequestCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDepartureEditRequestCallback(EditInteraction<Infrastructure.Model.Departure> obj)
        {
            try
            {
                if (!obj.Confirmed) return;

                if (ListDepartureCommand.CanExecute())
                    ListDepartureCommand.Execute();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private bool CanExecuteDeleteDepartureCommand()
        {
            return CurrentDeparture != null && CurrentDeparture.Departure.Status == 0;

        }

        private bool CanExecuteConfirmDepartureCommand()
        {
            return CurrentDeparture != null && CurrentDeparture.Departure.Status == 0;
        }

        private bool CanExecuteEditDepartureCommand()
        {
            return CurrentDeparture != null && CurrentDeparture.Departure.Status == 0;
        }

        private void OnListDepartureCommand()
        {
            try
            {
                var parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Časovnica odhodov" });


                if (null != _navigationContext.Parameters?["origin"])
                    parameters.Add("origin", _navigationContext.Parameters?["origin"]);
                if (null != _navigationContext.Parameters?["destination"])
                    parameters.Add("destination", _navigationContext.Parameters?["destination"]);

                _regionManager.RequestNavigate(Infrastructure.RegionNames.DepartureRegion, "DepartureList", parameters);


                parameters = new NavigationParameters();
                parameters.Add("context", "Departures.List");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                _regionManager.RequestNavigate(RegionNames.ReportsRegion, "ReportsContext",  parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void ReportMetaDataProvider(string meta, Action<string> callback)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(meta);

                XElement element = xdoc.Element("root").Element("contextparams").Element("year");
                if (element != null) element.Value = _dateRange.From.Date.Year.ToString();
                element = xdoc.Element("root").Element("contextparams").Element("month");
                if (element != null) element.Value = _dateRange.From.Date.Month.ToString();

                if (CurrentDeparture != null)
                {
                    element = xdoc.Element("root").Element("contextparams").Element("projectId");
                    if (element != null) element.Value = CurrentDeparture.Departure.Destination.UuId;
                    element = xdoc.Element("root").Element("contextparams").Element("departureId");
                    if (element != null) element.Value = CurrentDeparture.Departure.UuId;
                }

                if(null != _dateRange)
                {
                    element = xdoc.Element("root").Element("contextparams").Element("date");
                    if (element != null) element.Value = _dateRange.From.Date.ToString("yyyyMMdd");
                    element = xdoc.Element("root").Element("contextparams").Element("dateto");
                    if (element != null) element.Value = _dateRange.To.Date.ToString("yyyyMMdd"); ;
                }

                callback?.Invoke(xdoc.ToString());
            }
            catch (Exception exc)
            {
                callback?.Invoke(meta);
            }

        }
        #endregion

        #region events
        private void OnDepartureSelectedEvent(SelectedEventArgs<DepartureList> args)
        {
            try
            {
                CurrentDeparture = args.SelectedData;
                RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnDepartureEditEvent(EditEventArgs<DepartureList> args)
        {
            try
            {
                CurrentDeparture = args.EditObject;
                RaiseCanExecuteChanged();
                if(EditDepartureCommand.CanExecute())
                    EditDepartureCommand.Execute();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        #endregion


        private void RaiseCanExecuteChanged()
        {
            try
            {
                EditDepartureCommand.RaiseCanExecuteChanged();
                DeleteDepartureCommand.RaiseCanExecuteChanged();
                ConfirmDepartureCommand.RaiseCanExecuteChanged();
                PrintDocumentsCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanPrintDocumentsCommand()
        {
            return CurrentDeparture != null; //&& CurrentDeparture.Departure.Status == 0;
        }

        private void OnPrintDocumentsCommand()
        {
            try
            {

                if (CurrentDeparture == null) return;

                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<List<DocumentPrint>, Infrastructure.Model.Departure>>())
                {
                    var url = new Uri(_settings.GetApiServer(), $"departures/{CurrentDeparture.Departure.UuId}/printabledocuments");
                    repositroy.GetRequestAsync(url.ToString(), _securityService.GetCurrentToken(),
                        (list) =>
                        {
                            try
                            {

                                string group = "";
                                Token token = _securityService.GetCurrentToken();

                                List<string> files = new List<string>();
                                if (list != null)
                                {
                                    foreach (var item in list)
                                    {
                                        if (group != item.DocumentGroup)
                                        {
                                            var ff = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}__{item.Document}.pdf");

                                            // Create a new PDF document
                                            PdfDocument document = new PdfDocument();
                                            // Create an empty page
                                            PdfPage page = document.AddPage();
                                            // Get an XGraphics object for drawing
                                            XGraphics gfx = XGraphics.FromPdfPage(page);
                                            // Create a font
                                            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
                                            // Draw the text
                                            gfx.DrawString($"{item.DocumentGroup}", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

                                            document.Save(ff);
                                            files.Add(ff);
                                            group = item.DocumentGroup;
                                        }

                                        using (var rep = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                                        {

                                            var urlf = new Uri(_settings.GetApiServer(true), $"files/{item.File}");

                                            var query = new Dictionary<string, string>();
                                            query.Add("token", token.GetTokenId());

                                            using (var stream = rep.GetFile(urlf.ToString(), token, query))
                                            {
                                                string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}__{item.Document}.pdf");
                                                using (var outputStream = System.IO.File.OpenWrite(fileName))
                                                {
                                                    stream.CopyTo(outputStream);
                                                }

                                                files.Add(fileName);

                                            }

                                        }
                                    }

                                }

                                // Open the output document
                                PdfDocument outputDocument = new PdfDocument();

                                // Iterate files
                                foreach (string file in files)
                                {
                                    // Open the document to import pages from it.
                                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                                    // Iterate pages
                                    int count = inputDocument.PageCount;
                                    for (int idx = 0; idx < count; idx++)
                                    {
                                        // Get the page from the external document...
                                        PdfPage page = inputDocument.Pages[idx];
                                        // ...and add it to the output document.
                                        outputDocument.AddPage(page);
                                    }
                                }

                                // Save the document...
                                string filename = Path.Combine(Path.GetTempPath(), $"{CurrentDeparture.Destination}({CurrentDeparture.Departure.DepartTime.ToString("dd MM yyyy")}).pdf"); ;
                                outputDocument.Save(filename);

                                var process = new Process()
                                {
                                    StartInfo = new ProcessStartInfo()
                                    {
                                        CreateNoWindow = true,
                                        Verb = "open",
                                        FileName = filename,

                                    }
                                };

                                process.Start();

                                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                            }
                            catch (Exception exc)
                            {
                                _exceptionService.RaiseException(exc);
                            }
                        });
                }

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        #endregion

    }

}
