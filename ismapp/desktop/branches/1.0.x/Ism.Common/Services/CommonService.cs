using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using System.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Ism.Common.Services
{
    public class CommonService: ICommonService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private static List<DocumentType> _documentTypes;
        private static List<Language> _languages;
        private static List<WorkPlace> _workPlaces;
        private static List<MartialStatus> _martialStatuses;
        private static List<AddressType> _addressTypes;
        private static List<Country> _countries;
        private static List<Company> _companies;

        public CommonService(IEventAggregator eventAggregator, IServiceLocator serviceLocator, ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _eventAggregator = eventAggregator;
            _serviceLocator = serviceLocator;
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
        }

        #region ICommonService

        public List<DocumentType> GetDocumentTypes()
        {
            return _documentTypes;
        }

        public List<Language> GetLanguages()
        {
            //try
            //{
            //    //if (_languages == null)
            //        _languages = GetLanguagesFromServer();
            //}
            //catch (Exception e)
            //{
            //    _exceptionService.RaiseException(e);
            //}

            return _languages;
        }

        public List<MartialStatus> GetMartilaStatuses()
        {
            List<MartialStatus> statuses = null;
            try
            {
                statuses = GetMartialStatusesFromServer();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return statuses;
        }

        public List<AddressType> GetAddressTypes()
        {
            List<AddressType> types = null;
            try
            {
                types = GetAddressTypesFromServer();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return types;
        }

        #endregion

        private List<DocumentType> GetDocumentTypesFromServer()
        {
            try
            {
                if (null != _documentTypes) return _documentTypes;

                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<DocumentType>, string>>())
                {
                    _documentTypes = rep.GetRequest(new Uri(_settingsService.GetApiServer(true), "documents/types/list").ToString(),
                        _securityService.GetCurrentUser().AccessToken).OrderBy(t=>t.Name).ToList();
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return _documentTypes;
        }

        private List<MartialStatus> GetMartialStatusesFromServer()
        {
            try
            {
                if (null != _martialStatuses) return _martialStatuses;

                _martialStatuses = new List<MartialStatus>();
                _martialStatuses.AddRange(new []
                {
                    new MartialStatus() {Name = ""},
                    new MartialStatus() {Name = "Poročen"},
                    new MartialStatus() {Name = "Živim z izvenzakonskim partnerjem(ko)"},
                    new MartialStatus() {Name = "Imam trajnega partnerja, s katerim pa ne živim"},
                    new MartialStatus() {Name = "Samski, nikoli porocen"},
                    new MartialStatus() {Name = "Samski, locen"},
                    new MartialStatus() {Name = "Samski, ovdovel" }
                });

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return _martialStatuses;
        }

        private List<AddressType> GetAddressTypesFromServer()
        {
            try
            {
                if (null != _addressTypes) return _addressTypes;
                _addressTypes = new List<AddressType>();
                _addressTypes.AddRange(new[]
                {
                    new AddressType() {Name = "STALNI", Description= "Naslov stalnega prebivališča"},
                    new AddressType() {Name = "ZAČASNI", Description= "Naslov začasnega prebivališča"},
                    new AddressType() {Name = "POŠTA", Description= "Naslov za prejemanje pošte"},
                });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }

            return _addressTypes;
        }

        public void FetchShared<T>(string url, Action<T> callback, bool global = true)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<T, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(global), url).ToString(), callback);
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public void SetCountries(List<Country> countries)
        {
            _countries = countries;
        }

        public List<Country> GetCountries()
        {
            return _countries;
        }

        public void SetLanguages(List<Language> languages)
        {
            _languages = languages;
        }

        public void SetDocumentTypes(List<DocumentType> types)
        {
            _documentTypes = types;
        }

        public void SetCompanies(List<Company> companies)
        {
            _companies = companies;
            _companies.ForEach(CompnyDowloadLogo);
        }

        public List<Company> GetCompanies()
        {
            return _companies;
        }


        private void CompnyDowloadLogo(Company company)
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<Document>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetApiServer(true), $"company/{company.UuId}/documents/LOGO").ToString(),
                        _securityService.GetCurrentUser().AccessToken, documents =>
                        {
                            try
                            {
                                if (null == documents) return;
                                if (documents.Count == 0) return;
                                var logoDoc = documents.FirstOrDefault();

                                if (null == logoDoc) return;

                                using (var repositroy = _serviceLocator.GetInstance<IRestRepository<Stream, object>>())
                                {
                                    var file = logoDoc?.Files.FirstOrDefault();
                                    if (null == file) return;

                                    string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_{file.UniqueName}_{file.Name}");
                                    var url = new Uri(_settingsService.GetApiServer(false), $"documents/{logoDoc.UuId}/files/{file.UuId}");
                                    repositroy.GetFileAsync(url.ToString(), _securityService.GetCurrentUser(), null,
                                        inputStream =>
                                        {
                                            try
                                            {
                                                using (inputStream)
                                                {
                                                    using (var outputStream = System.IO.File.OpenWrite(fileName))
                                                    {
                                                        inputStream.CopyTo(outputStream);
                                                    }
                                                }
                                                company.Logo = logoDoc;
                                                company.LogoPath = fileName;
                                                company.LogoImage = LoadImage(fileName, UriKind.Absolute);
                                            }
                                            catch (Exception exception)
                                            {
                                                _exceptionService.RaiseException(exception);
                                            }

                                        }, "Pridobivam logo datoteko...", false);
                                }

                            }
                            catch (Exception exception)
                            {
                                _exceptionService.RaiseException(exception);
                            }
                        }
                    );
                }
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private BitmapImage LoadImage(string uri, UriKind uriKind)
        {
            return new BitmapImage(new Uri(uri, uriKind));
        }

        public void SetWokrPlaces(List<WorkPlace> workplaces)
        {
            _workPlaces = workplaces;
        }

        public List<WorkPlace> GetWorkPlaces()
        {
            return _workPlaces;
        }

        public List<FileExtension> GetExtensions()
        {
            return new List<FileExtension>(new FileExtension[] { new FileExtension() { FileExtesion = ".pdf", Extension = "Pdf" }, new FileExtension() { FileExtesion = ".xlsx", Extension = "Excel" }, new FileExtension() { FileExtesion = ".docx", Extension = "Word" } });
        }
    }
}
