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

namespace Ism.Sys.ViewModels
{
    class ReportViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly IExceptionService _exceptionService;
        private ReportInteraction<Report> _notification;
        

        public ReportViewModel(IExceptionService exceptionService )
        {
            _exceptionService = exceptionService;
            try
            {
                CancelCommand = new DelegateCommand(OnCancelCommand);
                ReportCommand = new DelegateCommand(OnReportCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }



        #region Commands

        public DelegateCommand ReportCommand { get; }
        public DelegateCommand CancelCommand { get; }
        #endregion

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set { _notification = value as ReportInteraction<Report>; }
        }

        public Action FinishInteraction { get; set; }


        #endregion
        #region Public properties

        #endregion

        #region Command Handlers

        private void OnReportCommand()
        {
            try
            {
                var security = _serviceLocator.GetInstance<ISecurityService>();
                var settings = _serviceLocator.GetInstance<ISettingsService>();
                var documents = _serviceLocator.GetInstance<IDocumentService>();

                var param = new Dictionary<string, string>();
                param.Add("company", security.GetCurrentCompany().UuId);
                param.Add("auth", security.GetCurrentUser().AccessToken.AccessToken);


                var ps = settings.GetPrintServer();

                if (null == ps) return;
               
                Report report = new Report() {PrintServer = ps, ReportPath = "reports/ISM/el1.pdf" };
                
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                {
                  repositroy.GetReportAsync(report, security.GetCurrentUser(), param, stream =>
                  {
                      string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
                      using (stream)
                      {
                          using (var outputStream = System.IO.File.OpenWrite(fileName))
                          {
                              stream.CopyTo(outputStream);
                          }
                      }

                      _notification.InteractionObject.ReportFilePath = fileName;

                      //documents.UploadUserReport(_notification.InteractionObject);

                      Process p = new Process();
                      p.StartInfo = new ProcessStartInfo()
                      {
                          CreateNoWindow = true,
                          Verb = "open",
                          FileName = _notification.InteractionObject.ReportFilePath //put the correct path here
                      };
                      p.Start();

                      FinishInteraction?.Invoke();

                  }, "Čakam na izpis...", true );
                }
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
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #endregion


        #region IRegionMemberLifetime

        public bool KeepAlive => true;

        

        #endregion


    }
}
