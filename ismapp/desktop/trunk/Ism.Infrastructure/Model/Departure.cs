using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Departure : DepartureBase
    {
        private readonly ISecurityService security;
        public Departure()
        {
            security = ServiceLocator.Current.TryResolve<ISecurityService>();
        }

        private IDepartureArrival _origin;
        private IDepartureArrival _destination;
        private IList<Employee> _employeesRemove;
        private IList<Employee> _employeesAdd;
        private IList<Car> _carsRemove;
        private IList<Car> _carsAdd;


        [JsonProperty(PropertyName = "origin", NullValueHandling = NullValueHandling.Ignore)]
        public IDepartureArrival Origin
        {
            get { return _origin; }
            set
            {
                SetProperty(ref _origin, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "destination", NullValueHandling = NullValueHandling.Ignore)]
        public IDepartureArrival Destination
        {
            get { return _destination; }
            set
            {
                SetProperty(ref _destination, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "employeesremove", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Employee> EmployeesRemove
        {
            get { return _employeesRemove; }
            set
            {
                SetProperty(ref _employeesRemove, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty(PropertyName = "employeesadd", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Employee> EmployeesAdd
        {
            get { return _employeesAdd; }
            set
            {
                SetProperty(ref _employeesAdd, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "carsremove", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Car> CarsRemove
        {
            get { return _carsRemove; }
            set
            {
                SetProperty(ref _carsRemove, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty(PropertyName = "carsadd", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Car> CarsAdd
        {
            get { return _carsAdd; }
            set
            {
                SetProperty(ref _carsAdd, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonIgnore]
        public IList<Employee> Employees { get; set; }

        [JsonIgnore]
        public IList<Car> Cars { get; set; }


        [JsonIgnore]
        public DateTime DepartDate
        {
            get
            {
                return DepartTime.Date.AddHours(12);
            }
        }

        [JsonIgnore]
        public bool Inbound
        {
            get
            {
                try
                {
                    if (security == null) return false;


                    if (security.HasPermissionExcplicit("foreman"))
                    {
                        if (Destination.DepartureArrivalType == "PROJECT") return true;
                    }
                    else
                    {
                        if (Destination.DepartureArrivalType == "COMPANY") return true;
                    }

                }
                catch (Exception)
                {
                    return false;
                }
                return false;
            }
        }
    }
}
