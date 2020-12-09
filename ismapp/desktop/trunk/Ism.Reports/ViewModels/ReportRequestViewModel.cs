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
using System.Xml.Linq;
using System.Windows;

namespace Ism.Reports.ViewModels
{
    class ReportRequestViewModel : ViewModelBase, IInteractionRequestAware
    {

        private readonly ISettingsService _settings;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private ReportInteraction<Report> _notification;

        private bool _dateVisible;
        private bool _dateToVisible;
        private DateTime _date;
        private DateTime _dateTo;
        private string _dateSelectionMode;
        private List<Language> _languages;
        private Language _reportLanguage;
        private List<FileExtension> _extensions;
        private FileExtension _extension;
        public ReportRequestViewModel(ISettingsService settings, ISecurityService securityService, IExceptionService exceptionService, ICommonService commonService, IDocumentService documentService)
        {
            if (null == settings)
                throw new ArgumentNullException(nameof(settings));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _settings = settings;
            _securityService = securityService;
            _exceptionService = exceptionService;
            _commonService = commonService;
            _documentService = documentService;
            
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



        #region Public properties


        #region dates

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                SetProperty(ref _date, value);
            }
        }

        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                SetProperty(ref _dateTo, value);
            }
        }


        public string DateSelectionMode
        {
            get { return _dateSelectionMode; }
            set
            {
                SetProperty(ref _dateSelectionMode, value);
            }
        }
        public bool DateVisible
        {
            get
            {
                return _dateVisible;
            }
            set
            {
                SetProperty(ref _dateVisible, value);
            }
        }

        public bool DateToVisible
        {
            get
            {
                return _dateToVisible;
            }
            set
            {
                SetProperty(ref _dateToVisible, value);
            }
        }


        #endregion

        public List<Language> Languages { get { return _languages; } set { SetProperty(ref _languages, value); } }
        public List<FileExtension> Extensions { get { return _extensions; } set { SetProperty(ref _extensions, value); } }
        public FileExtension Extension {
            get { return _extension; }
            set { SetProperty(ref _extension, value); }
        }
        public Language ReportLanguage { get { return _reportLanguage; } set { SetProperty(ref _reportLanguage, value); } }

        public Dictionary<string, string> ReportParams { get; private set; }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                _notification = value as ReportInteraction<Report>;
                ReportParams = null;
                if (null != _notification?.InteractionObject?.MetaDataProvider)
                    _notification.InteractionObject.MetaDataProvider?.Invoke(_notification.InteractionObject.MetaData, OnMetaDataProviderCallback);
                else
                    OnMetaDataProviderCallback(_notification.InteractionObject.MetaData);
            }
        }

        public Action FinishInteraction { get; set; }


        #endregion


        #endregion

        #region Command Handlers

        private void OnReportCommand()
        {
            try
            {
                var ps = _settings.GetPrintServer();
                if (null == ps) return;

                _notification.InteractionObject.ReportPath = _notification.InteractionObject.ReportPath.Replace(Path.GetExtension(_notification.InteractionObject.ReportPath), Extension.FileExtesion);

                _notification.InteractionObject.PrintServer = ps;

                Language lang = _commonService.GetLanguages().FirstOrDefault();
                _notification.InteractionObject.Language = ReportLanguage;

                

                ParseRunTimeParams();
                _notification.InteractionObject.ReportParameters = ReportParams;
                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                {
                    repositroy.GetReportAsync(_notification.InteractionObject, _securityService.GetCurrentToken(), stream =>
                    {
                        try
                        {
                            using (stream)
                            {
                                using (var outputStream = System.IO.File.OpenWrite(_notification.InteractionObject.GetFilePath()))
                                {
                                    stream.CopyTo(outputStream);
                                }
                            }

                            if (_notification.InteractionObject.OpenReportAction == null)
                                _notification.InteractionObject.OpenReportAction = OpenReport;

                            _documentService.UploadUserReport(_notification.InteractionObject);
                            SetClipboard();
                            OnFinishInteraction();
                        }
                        catch (Exception exc)
                        {
                            _exceptionService.RaiseException(exc);
                            OnFinishInteraction();
                        }


                    }, "Čakam na izpis...", true, (exc) =>
                    {
                        OnFinishInteraction();
                    });
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
                OnFinishInteraction();
            }
        }

        private void OpenReport(Report report, Action<Report> callback)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "open",
                    FileName = report.ReportFileInfo.FullName
                };
                p.Start();

                string val;
                report.ReportParameters.TryGetValue("savetoserver", out val);

                bool savetoserver;
                bool.TryParse(val, out savetoserver);

                if (savetoserver)
                    callback?.Invoke(report);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnCancelCommand()
        {
            try
            {
                OnFinishInteraction();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        #endregion


        #region IRegionMemberLifetime

        public override bool KeepAlive => true;

        #endregion

        private void OnMetaDataProviderCallback(string metadata)
        {
            try
            {
                _notification.InteractionObject.MetaData = metadata;
                ParseMetaData(metadata);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ParseMetaData(string metadata)
        {
            try
            {
                var xdoc = XDocument.Parse(metadata);

                var root = xdoc.Element("root");
                if (root == null) return;

                var datesNode = root.Element("dates");

                DateVisible = false;
                DateToVisible = false;

                if(null != datesNode)
                {
                    DateSelectionMode = "Day";
                    if (null != datesNode.Attribute("DateSelectionMode")) DateSelectionMode = datesNode.Attribute("DateSelectionMode").Value;

                    DateVisible = datesNode.Element("date") != null;
                    DateToVisible = datesNode.Element("dateto") != null;
                }

                if (null != xdoc.Element("root").Element("contextparams"))
                {
                    ReportParams = new Dictionary<string, string>();

                    foreach (var item in xdoc.Element("root").Element("contextparams").Elements())
                    {
                        if (!ReportParams.ContainsKey(item.Name.LocalName))
                            ReportParams.Add(item.Name.LocalName, item.Value);
                    }
                }

                var savetoserver = root.Element("savetoserver");
                if (null != savetoserver)
                    ReportParams.Add(savetoserver.Name.LocalName, savetoserver.Value);

                Languages = _commonService.GetLanguages();
                ReportLanguage = Languages.FirstOrDefault();
                Extensions = _commonService.GetExtensions();
                Extension = Extensions.Where(e => e.FileExtesion.Contains("pdf")).FirstOrDefault();

                if (root.Attribute("DirectRun") == null) return;
                if(Convert.ToBoolean(root.Attribute("DirectRun").Value) && ReportCommand.CanExecute())
                    ReportCommand.Execute();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ParseRunTimeParams()
        {
            try
            {
                if(ReportParams == null) ReportParams = new Dictionary<string, string>();

                ReportParams.Add("url", _settings.GetApiServer().Host);
                ReportParams.Add("company", _securityService.GetCurrentCompany().UuId);
                ReportParams.Add("token", _securityService.GetCurrentToken().GetTokenId());
                ReportParams.Add("user", _securityService.GetCurrentToken().GetClaim("sub"));

                var logo = _securityService.GetCurrentCompany().Logo;
                if(null != logo && logo?.Files.Count> 0)
                    ReportParams.Add("hdr", logo.Files.FirstOrDefault().UuId);

                if (DateVisible)
                {
                    if (ReportParams.ContainsKey("date"))
                        ReportParams["date"] = Date.ToString("yyyyMMdd");
                    else
                        ReportParams.Add("date", Date.ToString("yyyyMMdd"));

                    if (ReportParams.ContainsKey("year"))
                        ReportParams["year"] = Date.Year.ToString();
                    else
                        ReportParams.Add("year", Date.Year.ToString());

                    if (ReportParams.ContainsKey("month"))
                        ReportParams["month"] = Date.Month.ToString();
                    else
                        ReportParams.Add("month", Date.Month.ToString());
                    
                    if (ReportParams.ContainsKey("day"))
                        ReportParams["day"] = Date.Day.ToString();
                    else
                        ReportParams.Add("day", Date.Day.ToString());
                }

                if (DateToVisible)
                {
                    if (ReportParams.ContainsKey("dateto"))
                        ReportParams["dateto"] = DateTo.ToString("yyyyMMdd");
                    else
                        ReportParams.Add("dateto", DateTo.ToString("yyyyMMdd"));

                    if (ReportParams.ContainsKey("yearto"))
                        ReportParams["yearto"] = DateTo.Year.ToString();
                    else
                        ReportParams.Add("yearto", DateTo.Year.ToString());

                    if (ReportParams.ContainsKey("monthto"))
                        ReportParams["monthto"] = DateTo.Month.ToString();
                    else
                        ReportParams.Add("monthto", DateTo.Month.ToString());

                    if (ReportParams.ContainsKey("dayto"))
                        ReportParams["dayto"] = DateTo.Day.ToString();
                    else
                        ReportParams.Add("dayto", DateTo.Day.ToString());
                }

                ReportParams.Add("reportid", _notification.InteractionObject.UuId);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnFinishInteraction()
        {
            try
            {
                ReportParams = null;
                FinishInteraction?.Invoke();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void SetClipboard()
        {
            try
            {
                if (null == _notification.InteractionObject) return;
                if (_securityService.HasPermission("admin"))
                {


                }
            }
            catch (Exception)
            {
            }
        }
    }
}
