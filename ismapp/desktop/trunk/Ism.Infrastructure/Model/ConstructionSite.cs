using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ism.Infrastructure.Extensions;

namespace Ism.Infrastructure.Model
{
    public class ConstructionSite : BaseModel
    {
        private string _name;
        private string _description;
        private IList<Project> _projects;
        private DateTime _start;
        private DateTime _end;
        private string _color;

        private IList<Document> _documents;
        private IList<Contact> _contacts;
        private BusinessPartner _customer;

        private Company _company;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color
        {
            get { return _color; }
            set
            {
                SetProperty(ref _color, value);
                PropertyDeletegate?.Invoke(this);
            }
        }



        [JsonProperty("projects")]
        public IList<Project> Projects
        {
            get { return _projects; }
            set
            {
                SetProperty(ref _projects, value);
                PropertyDeletegate?.Invoke(this);

                Start = null == Projects || Projects.Count == 0 ? DateTime.Now : (from p in Projects select p.Start == null ? DateTime.Now.FirstDayOfMonth() : p.Start.Date).Min();
                End = null == Projects || Projects.Count == 0 ? DateTime.Now : (from p in Projects select p.End == null ? DateTime.Now.LastDayOfMonth() : p.End.Date).Max();

            }
        }


        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Document> Documents
        {
            get { return _documents; }
            set
            {
                SetProperty(ref _documents, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("customer")]
        public BusinessPartner Customer
        {
            get { return _customer; }
            set
            {
                SetProperty(ref _customer, value);
                PropertyDeletegate?.Invoke(this);

            }
        }

        [JsonProperty("contacts", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                SetProperty(ref _contacts, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public Company Company
        {
            get { return _company; }
            set
            {
                SetProperty(ref _company, value);
                PropertyDeletegate?.Invoke(this);
            }
        }



        [JsonIgnore]
        public DateTime Start
        {
            get
            {
                return _start;

            }
            set
            {
                SetProperty(ref _start, value);
                PropertyDeletegate?.Invoke(this);

            }
        }
        [JsonIgnore]
        public DateTime End
        {
            get
            {
                return _end;

            }
            set
            {
                SetProperty(ref _end, value);
                PropertyDeletegate?.Invoke(this);
            }

        }

        [JsonIgnore]
        public TimeSpan Duration
        {
            get
            {
                return Start - End;
            }
        }
    }
}
