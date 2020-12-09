using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Validation;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class EmployeeList : BaseModel
    {
        private Employee _employee;
        private WorkPeriod _workPeriod, _lastWorkPeriod;
        private Address _address;
        private Contact _contact;
        private Company _company, _lastCompany;
        private IList<Document> _documents;

        [JsonProperty(@"employee")]
        public Employee Employee
        {
            get { return _employee; }
            set
            {

                SetProperty(ref _employee, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(@"workperiod", NullValueHandling = NullValueHandling.Ignore)]
        public WorkPeriod WorkPeriod
        {
            get { return _workPeriod; }
            set
            {
                SetProperty(ref _workPeriod, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(@"address", NullValueHandling = NullValueHandling.Ignore)]
        public Address Address
        {
            get { return _address; }
            set
            {

                SetProperty(ref _address, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(@"contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact
        {
            get { return _contact; }
            set
            {

                SetProperty(ref _contact, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(@"company", NullValueHandling = NullValueHandling.Ignore)]
        public Company Company
        {
            get { return _company; }
            set
            {

                SetProperty(ref _company, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(@"lastcompany", NullValueHandling = NullValueHandling.Ignore)]
        public Company LastCompany
        {
            get { return _lastCompany; }
            set
            {

                SetProperty(ref _lastCompany, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(@"lastworkperiod", NullValueHandling = NullValueHandling.Ignore)]
        public WorkPeriod LastWorkPeriod
        {
            get { return _lastWorkPeriod; }
            set
            {
                SetProperty(ref _lastWorkPeriod, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(@"project", NullValueHandling = NullValueHandling.Ignore)]
        public Project Project { get; set; }

        [JsonProperty(@"site", NullValueHandling = NullValueHandling.Ignore)]
        public ConstructionSite ConstructionSite { get; set; }

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

        [JsonIgnore]
        public string WedlerCerts
        {
            get
            {
                if (null == _documents) return null;

                string certs = string.Empty;

                foreach (var item in _documents.Where(i => i.Type.Name.StartsWith("CERT_VAR_")))
                {
                    certs += $"{item?.Type?.Name}({item?.ValidTo?.Date.ToShortDateString()}) ";
                }
                return certs;
            }
        }
    }
}
