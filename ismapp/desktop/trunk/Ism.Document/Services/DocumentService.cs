using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using File = Ism.Infrastructure.Model.File;

namespace Ism.Document.Services
{
    public class DocumentService: IDocumentService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISecurityService _security;
        private readonly ISettingsService _settings;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;

        public DocumentService(IEventAggregator eventAggregator, IServiceLocator serviceLocator,  ISecurityService security, ISettingsService settings, IExceptionService exceptionService, ICommonService commonService)
        {
            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _security = security;
            _settings = settings;
            _commonService = commonService;
        }

        public void UploadUserReport(Report report)
        {
            try
            {
                report.OpenReportAction?.Invoke(report, OnOpenReportActionCallback);


            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        private void OnOpenReportActionCallback(Report report)
        {
            try
            {

                var token = _security.GetCurrentToken();
                var api = _settings.GetApiServer();
                DocumentType type = _commonService.GetDocumentTypes().Where(t => t.Name == "REPORT").FirstOrDefault();

                if (null == type) return;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<Infrastructure.Model.Document, AddDocument<Infrastructure.Model.Document>>>())
                {
                    //if (null != SelectedDocumentType) Document.Type = SelectedDocumentType;
                    var document = new Infrastructure.Model.Document() { Type = type, Name=$"{report.FriendlyName} {token.GetClaim("sub")} {DateTime.Now.ToString("yyyyMMddhhmmss")}", DocDate = new Day(DateTime.Now), ValidFrom = new Day(DateTime.Now), Active = 1, Deleted = 0 };
                    AddDocument<Infrastructure.Model.Document> addDocument = new AddDocument<Infrastructure.Model.Document>(null, document);
                    rep.PostRequestAsync(
                        new Uri(api, "documents/initUpload").ToString(),
                        addDocument,
                        token,
                        doc =>
                        {
                            using (var repository = _serviceLocator.GetInstance<IRestRepository<File, string>>())
                            {
                                var query = new Dictionary<string, string>
                                {
                                    {"uniquename", "newfile"},
                                    {"uuid", document.UuId},
                                    { "language", report.Language.UuId}
                                };
                                repository.PostFileAsync(
                                    new Uri(api, "documents/uploadFile").ToString(),
                                    report.ReportFileInfo.FullName,
                                    token,
                                    query,
                                    file =>
                                    {
                                        using (var repos = _serviceLocator
                                            .GetInstance<IRestRepository<Infrastructure.Model.Document,
                                                Infrastructure.Model.Document>>())
                                        {
                                            repos.PostRequestAsync(new Uri(api, "documents/finishUpload").ToString(),
                                                document,
                                                token,
                                                (fd) =>
                                                {
                                                    report?.ReportDocumentAction?.Invoke(report, doc);
                                                }, "Nalagam dokument...", false);
                                        }

                                    }, "Nalagam datoteko ...", false);
                            }

                        }, "Ustvarajm dokument na strežniku...", false);
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
