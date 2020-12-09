using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Services
{
    public interface ISecurityService
    {
        Token GetCurrentToken();
        void SetCurrentToken(Token token);
        void SetCurrentToken(Token user, Company  company);


        Company GetCurrentCompany();
        void SetCurrentCompany(Company company);

        void SetCurrentEmployee(Employee employee);
        Employee GetCurrentEmployee();

        void SetCurrentSite(ConstructionSite site);
        ConstructionSite GetCurrentSite();

        //void SetCurrentScope(List<Scope> scopes);
        bool HasPermission(string key);
        bool HasPermissionExcplicit(string key);

    }
}
