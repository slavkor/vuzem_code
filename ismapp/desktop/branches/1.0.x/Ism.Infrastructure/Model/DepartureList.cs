﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class DepartureList : BaseModel
    {
        private Departure _departures;
        private IList<Employee> _employees;
        private Company _fromcompany;
        private Project _fromproject;
        private Company _tocompany;
        private Project _toproject;
        private IList<Car> _cars;


        [JsonProperty(PropertyName = "departure", NullValueHandling = NullValueHandling.Ignore)]
        public Departure Departure
        {
            get { return _departures; }
            set
            {
                SetProperty(ref _departures, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "cars", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Car> Cars
        {
            get { return _cars; }
            set
            {
                SetProperty(ref _cars, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty(PropertyName = "employees", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Employee> Employees
        {
            get { return _employees; }
            set
            {
                SetProperty(ref _employees, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty(PropertyName = "fromcompany", NullValueHandling = NullValueHandling.Ignore)]
        public Company FromCompany
        {
            get { return _fromcompany; }
            set
            {
                SetProperty(ref _fromcompany, value);
                PropertyDeletegate?.Invoke(this);
                if (null != value) Departure.Origin = value;
            }
        }

        [JsonProperty(PropertyName = "fromproject", NullValueHandling = NullValueHandling.Ignore)]
        public Project FromProject
        {
            get { return _fromproject; }
            set
            {
                SetProperty(ref _fromproject, value);
                PropertyDeletegate?.Invoke(this);
                if (null != value) Departure.Origin = value;
            }
        }

        [JsonProperty(PropertyName = "tocompany", NullValueHandling = NullValueHandling.Ignore)]
        public Company ToCompany
        {
            get { return _tocompany; }
            set
            {
                SetProperty(ref _tocompany, value);
                PropertyDeletegate?.Invoke(this);
                if (null != value) Departure.Destination = value;
            }
        }

        [JsonProperty(PropertyName = "toproject", NullValueHandling = NullValueHandling.Ignore)]
        public Project ToProject
        {
            get { return _toproject; }
            set
            {
                SetProperty(ref _toproject, value);
                PropertyDeletegate?.Invoke(this);
                if (null != value) Departure.Destination = value;
            }
        }




    }
}
