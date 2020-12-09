using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Services
{
    public interface ISettingsService: IDisposable
    {
        Uri GetApiServer();
        Uri GetApiServer(bool global);
        Uri GetApiServer(string company);

        void SetApiServer(string apiServer);
        Uri GetAuthServer();
        void SetAuthServer(string authServer);

        string GetLastUsedFirm(string user);
        void SetLastUsedFirm(string user, string firmId);

        void SetPrintServer(string printServer, string user, string pwd);
        PrintServer GetPrintServer();

        void SetCompanySettings(IDictionary<string, string> settings);
        IDictionary<string, string> GetCompanySettings();

        void SetUserSettings(IDictionary<string, string> settings);
        IDictionary<string, string> GetUserSettings();

        void SetUserScopes(IList<Scope> scopes);
        IList<Scope> GetUserScopes();
    }
}
