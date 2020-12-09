using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Prism.Events;

namespace Ism.Security.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IEventAggregator _eventAggregator;
        private static User _currentUser;
        private static Company _cuurentCompany;
        private static List<Company> _allCompanies;
        private readonly IExceptionService _exceptionService;

        private static Employee _currentEmployee;
        private static ConstructionSite _currentSite;
        public SecurityService(IEventAggregator eventAggregator, IExceptionService exceptionService)
        {
            _eventAggregator = eventAggregator;
            _exceptionService = exceptionService;
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public void SetCurrentUser(User user)
        {
            try
            {
                SetCurrentUser(user, null);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        public void SetCurrentUser(User user, Company company)
        {
            try
            {
                _currentUser = user;
                SetCurrentCompany(company);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        public Company GetCurrentCompany()
        {
            return _cuurentCompany;
        }

        public void SetCurrentCompany(Company company)
        {
            try
            {
                _cuurentCompany = company;
                _eventAggregator.GetEvent<CompanySelectedEvent>().Publish(_cuurentCompany);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

        }

        public bool HasPermission(string key)
        {
            if (_currentUser?.Scopes == null) return false;
            return _currentUser.Scopes.Any(scope => scope.Identifier == "admin") || _currentUser.Scopes.Any(scope => scope.Identifier == key);
        }

        public List<Company> GetAllCompanies()
        {
            return _allCompanies;
        }

        public void SetAllCompanies(List<Company> companies)
        {
            _allCompanies = companies;
        }

        public void SetCurrentEmployee(Employee employee)
        {
            try
            {
                _currentEmployee = employee;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public Employee GetCurrentEmployee()
        {
            return _currentEmployee;
        }

        public bool HasPermissionExcplicit(string key)
        {
            if (_currentUser?.Scopes == null) return false;
            return _currentUser.Scopes.Any(scope => scope.Identifier == key);

        }

        public void SetCurrentSite(ConstructionSite site)
        {
            try
            {
                _currentSite = site;
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public ConstructionSite GetCurrentSite()
        {
            return _currentSite;
        }
    }
}
