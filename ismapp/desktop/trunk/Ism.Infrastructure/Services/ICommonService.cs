using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;

namespace Ism.Infrastructure.Services
{
    public interface ICommonService
    {
        void FetchShared<T>(string url, Action<T> callback, bool global = true);

        void SetDocumentTypes(List<DocumentType> types);
        List<DocumentType> GetDocumentTypes();

        void SetLanguages(List<Language> languages);
        List<Language> GetLanguages();

        void SetWokrPlaces(List<WorkPlace> workplaces);
        List<WorkPlace> GetWorkPlaces();


        List<MartialStatus> GetMartilaStatuses();
        List<AddressType> GetAddressTypes();
        void SetCountries(List<Country> countries);
        List<Country> GetCountries();


        void SetCompanies(List<Company> companies);
        List<Company> GetCompanies();

        List<FileExtension> GetExtensions();
        
    }
}
