using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Report: BaseModel, ISelectionAware
    {
        private string _reportId;
        private string _module;
        private string _friendlyName;
        private string _reportPath;
        private string _metaData;
        private bool _isSelected;
        private string uniquePath;
        public Report()
        {
           
        }
    

        [JsonProperty("reportid")]
        public string ReportId
        {
            get { return _reportId; }
            set
            {
                SetProperty(ref _reportId, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("module")]
        public string Module
        {
            get { return _module; }
            set
            {
                SetProperty(ref _module, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("friendlyname")]
        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                SetProperty(ref _friendlyName, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("metadata")]
        public string MetaData
        {
            get { return _metaData; }
            set
            {
                SetProperty(ref _metaData, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("reportpath")]
        public string ReportPath
        {
            get { return _reportPath; }
            set
            {
                SetProperty(ref _reportPath, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public Uri ReportUri
        {
            get
            {
                var uriBuilder = new UriBuilder(PrintServer.ServerUri);
                uriBuilder.Path += ReportPath;
                return uriBuilder.Uri;
            }
        }
        [JsonIgnore]
        public PrintServer PrintServer { get; set; }
        [JsonIgnore]
        public Language Language { get; set; }
        [JsonIgnore]
        public string ReportFilePath { get { return $"{uniquePath}{Path.GetExtension(ReportPath)}";} }
        [JsonIgnore]
        public FileInfo ReportFileInfo => new FileInfo(ReportFilePath);
        [JsonIgnore]
        public Action<Report, Action<Report>> OpenReportAction { get; set; }
        public Action<Report, Document> ReportDocumentAction { get; set; }

        [JsonIgnore]
        public Action<string, Action<string>> MetaDataProvider { get; set; }

        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                SetProperty(ref _isSelected, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public DelegateCommand<Report> Command { get; set; }

        [JsonIgnore]
        public IDictionary<string, string> ReportParameters { get; set; }

        public string GetFilePath()
        {
            uniquePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}");
            return ReportFilePath;
        }
    }
}
