using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class EmployeeChangeEmployer : BaseModel
    {
        private Company _companyFire;
        private Company _companyHire;
        private Employee _employee;
        private DateTime? _fireDate;
        private DateTime? _hireDate;

        private CurrentWorkPeriod _curentActiveWorkPeriod;

        public Company CompanyFire
        {
            get { return _companyFire; }
            set
            {
                SetProperty(ref _companyFire, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public Company CompanyHire
        {
            get { return _companyHire; }
            set
            {
                SetProperty(ref _companyHire, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public Employee Employee
        {
            get { return _employee; }
            set
            {
                SetProperty(ref _employee, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public DateTime? FireDate
        {
            get { return _fireDate; }
            set
            {
                SetProperty(ref _fireDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public DateTime? HireDate
        {
            get { return _hireDate; }
            set
            {
                SetProperty(ref _hireDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public CurrentWorkPeriod CurentWorkPeriod
        {
            get { return _curentActiveWorkPeriod; }
            set
            {
                SetProperty(ref _curentActiveWorkPeriod, value);
                PropertyDeletegate?.Invoke(this);



            }
        }


    }
}
