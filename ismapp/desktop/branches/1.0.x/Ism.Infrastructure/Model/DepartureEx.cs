using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ism.Infrastructure.Model
{
    public class DepartureEx : DepartureBase
    {
        private Company _origincopmany;
        private Company _destinationcompany;

        private Project _originproject;
        private Project _destinationproject;


        [JsonProperty(PropertyName = "origincopmany", NullValueHandling = NullValueHandling.Ignore)]
        public Company OriginCompany {
            get
            {
                return _origincopmany;
            }
            set
            {
                SetProperty(ref _origincopmany, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "originproject", NullValueHandling = NullValueHandling.Ignore)]
        public Project OriginProject
        {
            get
            {
                return _originproject;
            }
            set
            {
                SetProperty(ref _originproject, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty(PropertyName = "destinationcompany", NullValueHandling = NullValueHandling.Ignore)]
        public Company DestinationCompany
        {
            get
            {
                return _destinationcompany;
            }
            set
            {
                SetProperty(ref _destinationcompany, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty(PropertyName = "destinationproject", NullValueHandling = NullValueHandling.Ignore)]
        public Project DestinationProject
        {
            get
            {
                return _destinationproject;
            }
            set
            {
                SetProperty(ref _destinationproject, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public string DestinationName => DestinationCompany == null ? DestinationProject.PointName : DestinationCompany.PointName;
    }
}
