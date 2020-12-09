using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Prism.Regions;
using Ism.Infrastructure.Model;
using System.Threading;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed;
using Ism.Infrastructure.Repository;
using System.IO;
using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using Microsoft.Win32;
using System.Drawing.Printing;

namespace Ism.Common.ViewModels
{
    public class PrintPdfViewModel : Infrastructure.Mvvm.ViewModelBase, IInteractionRequestAware
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        //private AddAddress<BaseModel> _address;
        //private AddressViewInteraction _notification;
        private ListInteraction<PrintEventAgrs> _notification;

        
        public PrintPdfViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
            try
            {
   


            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        
        public INotification Notification
        {
            get { return _notification; }
            set
            {
                try
                {
                    string group = "";
                    _notification = value as ListInteraction<PrintEventAgrs>;
                    Token token = _securityService.GetCurrentToken();

                    List<string> files = new List<string>();
                    if (_notification.InteractionObject.DownloadFiles != null)
                    {
                        foreach (var item in _notification.InteractionObject.DownloadFiles)
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

                            using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                            {

                                var url = new Uri(_settingsService.GetApiServer(true), $"files/{item.File}");

                                var query = new Dictionary<string, string>();
                                query.Add("token", token.GetTokenId());

                                using (var stream = repositroy.GetFile(url.ToString(), token, query))
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
                    string filename = Path.Combine(Path.GetTempPath(), $"{_notification.InteractionObject.PrintKey}.pdf"); ;
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

                    FinishInteraction?.Invoke();
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                    OnFinishInteraction();
                }
            }
        }

        public Action FinishInteraction { get; set; }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

        }

        private void OnFinishInteraction()
        {
            try
            {
                FinishInteraction?.Invoke();
                NavigateBack();

                _notification.CloseCommand.Execute(null);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public Task DoWork(IList<DocumentPrint> files)
        {
           


            return null;
            //return Task.WhenAll(ids.Select(i => DoSomething(1, i, blogClient)));
        }

        public bool Print(string file, string printer)
        {
            try
            {
                Process.Start(Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion" +
                        @"\App Paths\AcroRd32.exe").GetValue("").ToString(),
                        string.Format("/h /t \"{0}\" \"{1}\"", file, printer));
                return true;
            }
            catch
            { return false; }
        }
    }
}
