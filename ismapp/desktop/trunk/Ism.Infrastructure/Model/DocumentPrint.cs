using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{


    public class ServerDocumentPrint : BaseModel
    {
        private string _group;

        [JsonProperty("group")]
        public string DocumentGroup
        {
            get { return _group; }
            set { _group = value; }
        }

        private string _document;
        [JsonProperty("document")]
        public string Document
        {
            get { return _document; }
            set { _document = value; }
        }

        private string _typeId;
        [JsonProperty("typeId")]
        public string TypeId
        {
            get { return _typeId; }
            set { _typeId = value; }
        }
        private string _type;
        [JsonProperty("type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _file;

        [JsonProperty("file")]
        public string File
        {
            get { return _file; }
            set { _file = value; }
        }
    }



    public class LocalDocument : BaseModel
    {
        private string _group;

        [JsonProperty("group")]
        public string DocumentGroup
        {
            get { return _group; }
            set { _group = value; }
        }

        private string _document;
        [JsonProperty("document")]
        public string Document
        {
            get { return _document; }
            set { _document = value; }
        }

        private string _typeId;
        [JsonProperty("typeId")]
        public string TypeId
        {
            get { return _typeId; }
            set { _typeId = value; }
        }
        private string _type;
        [JsonProperty("type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _file;

        [JsonProperty("file")]
        public string File
        {
            get { return _file; }
            set { _file = value; }
        }
    }

    public class DocumentPrint : BaseModel
    {
        private string _group;

        [JsonProperty("group")]
        public string DocumentGroup
        {
            get { return _group; }
            set { _group = value; }
        }

        private string _document;
        [JsonProperty("document")]
        public string Document
        {
            get { return _document; }
            set { _document = value; }
        }

        private string _typeId;
        [JsonProperty("typeId")]
        public string TypeId
        {
            get { return _typeId; }
            set { _typeId = value; }
        }
        private string _type;
        [JsonProperty("type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _file;

        [JsonProperty("file")]
        public string File
        {
            get { return _file; }
            set { _file = value; }
        }
    }
}
