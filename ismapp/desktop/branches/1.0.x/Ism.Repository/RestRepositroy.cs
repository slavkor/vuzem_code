using Newtonsoft.Json;
using System.Net;
using System.IO;

using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Prism.Events;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Validation;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json.Converters;

namespace Ism.Repository
{
    public class RestRepositroy<T, TP> : IRestRepository<T, TP>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecurityService _securityService;
        private readonly X509Certificate2 _apiCertificate;
        private readonly X509Certificate2 _authCertificate;
        private readonly IExceptionService _exceptionService;
        public RestRepositroy(IEventAggregator eventAggregator, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == eventAggregator)
                throw new ArgumentNullException(nameof(eventAggregator));
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));

            _eventAggregator = eventAggregator;
            _securityService = securityService;
            _exceptionService = exceptionService;

            var r = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("Ism.Repository.api.crt"))
            {
                if (stream.CanRead)
                {
                    var bytes = new byte[stream.Length];
                    for (int Index = 0; Index < stream.Length; Index++)
                    {
                        bytes[Index] = (byte) stream.ReadByte();
                    }
                    _apiCertificate = new X509Certificate2();
                    _apiCertificate.Import(bytes);
                }
            }

            using (Stream stream = assembly.GetManifestResourceStream("Ism.Repository.auth.crt"))
            {
                if (stream.CanRead)
                {
                    var bytes = new byte[stream.Length];
                    for (int Index = 0; Index < stream.Length; Index++)
                    {
                        bytes[Index] = (byte) stream.ReadByte();
                    }
                    _authCertificate = new X509Certificate2();
                    _authCertificate.Import(bytes);
                }
            }
        }

        #region IRestRepository<T, P>

        
        public T PostRequest(string uri, TP payLoad)
        {
            return PostRequest(uri, payLoad, _securityService?.GetCurrentUser()?.AccessToken);
        }
        public T PostRequest(string uri, TP payLoad, Token token)
        {
            T response = default(T);
            try
            {
                var webRequest = (HttpWebRequest) WebRequest.Create(uri);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "POST";
                if (null != token)
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, token.AccessToken);
                    //webRequest.Headers.Add("voodoo", $"{token.GetClaim("sub")}/{token.GetTokenId()}/{(payLoad as IBaseModel)?.ToString()}");
                    webRequest.Headers.Add("voodoo", $"{token.GetClaim("sub")}/{token.GetTokenId()}");
                }


                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(payLoad);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    response = JsonConvert.DeserializeObject<T>(result);
                }
            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                }
                try
                {
                    ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                    _exceptionService.RaiseException(serverException);
                }
                catch (Exception)
                {
                    _exceptionService.RaiseException(new Exception(resp));
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
            return response;
        }
        public T GetRequest(string url)
        {
            return GetRequest(url, _securityService?.GetCurrentUser()?.AccessToken);
        }
        public T GetRequest(string url, Token token)
        {
            T response = default(T);
            try
            {
                var webRequest = (HttpWebRequest) WebRequest.Create(url);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "GET";
                if (null != token)
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, token.AccessToken);
                    webRequest.Headers.Add("voodoo", $"{token.GetClaim("sub")}/{token.GetTokenId()}");
                }

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                webRequest.ClientCertificates.Add(_apiCertificate);
                webRequest.ClientCertificates.Add(_authCertificate);


                var httpResponse = (HttpWebResponse) webRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    response = JsonConvert.DeserializeObject<T>(result);
                }
            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _exceptionService.RaiseException(e);
                }
                try
                {
                    ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                    _exceptionService.RaiseException(serverException);
                }
                catch (Exception)
                {
                    _exceptionService.RaiseException(new Exception(resp));
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

            return response;
        }

        public void GetRequestAsync(string url, Action<T> callback)
        {
            GetRequestAsync(url, _securityService?.GetCurrentUser()?.AccessToken, callback, "Počakaj...", true);
        }
        public void GetRequestAsync(string url, Token token, Action<T> callback)
        {
                GetRequestAsync(url, token, callback, "Počakaj...", true);
        }
        public void GetRequestAsync(string url, Token token, Action<T> callback, string waitNotification, bool busyNotification)
        {
            BusyEventArgs busyEventArgs = null;
            try
            {
                if (busyNotification)
                {
                    busyEventArgs = new BusyEventArgs() { Busy = true, Notification = waitNotification };
                    _eventAggregator.GetEvent<BusyEvent>().Publish(busyEventArgs);
                }
            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(e);
            }

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "GET";
                if (null != token)
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, token.AccessToken);
                    webRequest.Headers.Add("voodoo", $"{token.GetClaim("sub")}/{token.GetTokenId()}");
                }

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                webRequest.ClientCertificates.Add(_apiCertificate);
                webRequest.ClientCertificates.Add(_authCertificate);

                RequestState requestState = new RequestState() { Request = webRequest, CallbackAction = callback, BusyEventArgs = busyEventArgs};

                //webRequest.Proxy = null;

                webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), requestState);

            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                }
                try
                {
                    ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                    _exceptionService.RaiseException(serverException);
                }
                catch (Exception)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(new Exception(resp));
                }
            }
            catch (Exception exc)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(exc);
            }
        }

        public void PostRequestAsync(string uri, TP payLoad, Action<T> callback)
        {
            PostRequestAsync(uri, payLoad, _securityService?.GetCurrentUser()?.AccessToken, callback, "Počakaj...");
        }
        public void PostRequestAsync(string uri, TP payLoad, Token token, Action<T> callback)
        {
            PostRequestAsync(uri, payLoad, token, callback, "Počakaj...");
        }
        public void PostRequestAsync(string uri, TP payLoad, Token token, Action<T> callback, string waitNotification)
        {
            PostRequestAsync(uri, payLoad, token, callback, waitNotification, true);
        }
        public void PostRequestAsync(string uri, TP payLoad, Token token, Action<T> callback, string waitNotification, bool busyNotification)
        {
            BusyEventArgs busyEventArgs = null;
            try
            {
                if (busyNotification)
                {
                    busyEventArgs = new BusyEventArgs() { Busy = true, Notification = waitNotification };
                    _eventAggregator.GetEvent<BusyEvent>().Publish(busyEventArgs);
                }


            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(e);
            }
            HttpWebRequest webRequest = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "POST";
                if (null != token)
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, token.AccessToken);
                    //webRequest.Headers.Add("voodoo", $"{token.GetClaim("sub")}/{token.GetTokenId()}/{(payLoad as IBaseModel)?.ToString()}");
                    webRequest.Headers.Add("voodoo", $"{token.GetClaim("sub")}/{token.GetTokenId()}");
                }
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                webRequest.ClientCertificates.Add(_apiCertificate);
                webRequest.ClientCertificates.Add(_authCertificate);

                RequestState requestState =
                    new RequestState() { Request = webRequest, CallbackAction = callback, PayLoad = payLoad, BusyEventArgs = busyEventArgs };

                webRequest.BeginGetRequestStream(BeginGetRequestStreamCallback, requestState);

            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                }
                try
                {
                    ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                    _exceptionService.RaiseException(serverException);
                }
                catch (Exception)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(new Exception(resp));
                }

            }
            catch (Exception exc)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(exc);
            }
        }

        public async void PostFileAsync(string url, string fileName, User user, IDictionary<string, string> querystring, Action<T> callback, string waitNotification, bool busyNotification)
        {
            BusyEventArgs busyEventArgs = null;
            try
            {
                if (busyNotification)
                {
                    busyEventArgs = new BusyEventArgs() { Busy = true, Notification = waitNotification };
                    _eventAggregator.GetEvent<BusyEvent>().Publish(busyEventArgs);
                }
            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(e);
            }

            var fileBytes = await GetBytesAsync(fileName);

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

            var postdata = "?";
            if (querystring != null)
            {
                foreach (var data in querystring)
                {
                    postdata += data.Key + "=" + data.Value + "&";
                }
            }
            Uri uri = new Uri(url + postdata);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            if (!string.IsNullOrEmpty(user.AccessToken.AccessToken))
                webRequest.Headers.Add(HttpRequestHeader.Authorization, user.AccessToken.AccessToken);
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            webRequest.ClientCertificates.Add(_apiCertificate);
            webRequest.ClientCertificates.Add(_authCertificate);

            RequestState requestState = new RequestState() { Request = webRequest, CallbackAction = callback, PayLoad = fileBytes, BusyEventArgs = busyEventArgs };
            //webRequest.Proxy = null;
            webRequest.BeginGetRequestStream((result) =>
            {
                try
                {
                    RequestState state = result.AsyncState as RequestState;
                    HttpWebRequest request = state.Request;
                    using (Stream requestStream = request.EndGetRequestStream(result))
                    {
                        WriteMultipartForm(requestStream, boundary, querystring["uniquename"], fileName, "application/octet-stream", (byte[])state.PayLoad);
                    }

                    request.BeginGetResponse(FinishWebRequest, state);
                }
                catch (WebException ex)
                {
                    string resp = string.Empty;
                    try
                    {
                        resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    catch (Exception e)
                    {
                        _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                        _exceptionService.RaiseException(e);
                    }
                    try
                    {
                        ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                        _exceptionService.RaiseException(serverException);
                    }
                    catch (Exception)
                    {
                        _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                        _exceptionService.RaiseException(new Exception(resp));
                    }
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                }
            }, requestState);
        }

        public void GetFileAsync(string url, User user, IDictionary<string, string> querystring, Action<T> callback, string waitNotification, bool busyNotification)
        {
            BusyEventArgs busyEventArgs = null;
            try
            {
                if (busyNotification)
                {
                    busyEventArgs = new BusyEventArgs() { Busy = true, Notification = waitNotification };
                    _eventAggregator.GetEvent<BusyEvent>().Publish(busyEventArgs);
                }
            }
            catch (Exception e)
            {
                busyEventArgs = new BusyEventArgs() { Busy = false, Notification = waitNotification };
                _eventAggregator.GetEvent<BusyEvent>().Publish(busyEventArgs);
                _exceptionService.RaiseException(e);
            }

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "GET";
                if (!string.IsNullOrEmpty(user.AccessToken.AccessToken))
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, user.AccessToken.AccessToken);

                

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                webRequest.ClientCertificates.Add(_apiCertificate);
                webRequest.ClientCertificates.Add(_authCertificate);

                RequestState requestState = new RequestState() { Request = webRequest, CallbackAction = callback, BusyEventArgs = busyEventArgs };
                //webRequest.Proxy = null;
                webRequest.BeginGetResponse(new AsyncCallback(FinishGetFileAsync), requestState);

            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                }
                try
                {
                    ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                    _exceptionService.RaiseException(serverException);
                }
                catch (Exception)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(new Exception(resp));
                }
            }
            catch (Exception exc)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(exc);
            }
        }

        public void GetReportAsync(Report report, User user, Action<T> callback, string waitNotification, bool busyNotification, Action<Exception> errcallback)
        {
            BusyEventArgs busyEventArgs = null;
            try
            {
                if (busyNotification)
                {
                    busyEventArgs = new BusyEventArgs() { Busy = true, Notification = waitNotification };
                    _eventAggregator.GetEvent<BusyEvent>().Publish(busyEventArgs);
                }
            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(e);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    errcallback?.Invoke(e);
                });
            }

            try
            {
                var locale = $"userLocale={"en"}";
                if (null != report.Language?.Alpha2)
                    locale = $"userLocale={report.Language.Alpha2.Replace("-", "_")}";

                var postdata = $"?{locale}&j_username={report.PrintServer.User}&j_password={report.PrintServer.Password}&"; 

                if (report.ReportParameters != null)
                {
                    postdata = report.ReportParameters.Aggregate(postdata, (current, data) => current + (data.Key + "=" + data.Value + "&"));
                }

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(report.ReportUri + postdata);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "GET";
                //webRequest.Credentials = new NetworkCredential(report.PrintServer.User, report.PrintServer.Password);
                RequestState requestState = new RequestState() { Request = webRequest, CallbackAction = callback, BusyEventArgs = busyEventArgs, ErrorCallbackAction = errcallback };

                SetClipboard(webRequest.Address.AbsoluteUri);
                webRequest.BeginGetResponse(new AsyncCallback(FinishGetFileAsync), requestState);

            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        errcallback?.Invoke(e);
                    });

                }
                try
                {
                    ApiServerException serverException = JsonConvert.DeserializeObject<ApiServerException>(resp);
                    _exceptionService.RaiseException(serverException);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        errcallback?.Invoke(serverException);
                    });
                }
                catch (Exception)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    Exception exc = new Exception(resp);
                    _exceptionService.RaiseException(exc);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        errcallback?.Invoke(exc);
                    });
                }
            }
            catch (Exception exc)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(exc);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    errcallback?.Invoke(exc);
                }); ;
            }
        }

        private void BeginGetRequestStreamCallback(IAsyncResult ar)
        {
            RequestState requestState = null;
            try
            {
                requestState = ar.AsyncState as RequestState;
                HttpWebRequest request = requestState.Request;

                using (Stream postStream = request.EndGetRequestStream(ar))
                {
                    if (requestState.PayLoad is IBaseModel)
                        if(((IBaseModel)requestState.PayLoad).UuId == Guid.Empty.ToString() || ((IBaseModel)requestState.PayLoad).UuId == null)
                            ((IBaseModel)requestState.PayLoad).UuId= Guid.NewGuid().ToString();

                    string postData = JsonConvert.SerializeObject(requestState.PayLoad);
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();
                }
                request.BeginGetResponse(FinishWebRequest, requestState);
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                });


                if (requestState?.BusyEventArgs != null)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() {Busy = false});
                    });
            }
        }
        #endregion

        #region Private helper methods

        private void FinishWebRequest(IAsyncResult result)
        {
            RequestState requestState = null;
            string strResponose;
            try
            {
                requestState = (RequestState)result.AsyncState;
                var httpResponse = (HttpWebResponse)requestState.Request.EndGetResponse(result);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strResponose = streamReader.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<T>(strResponose, new DateTimeConverter());
                    if (data is IDirty)
                        ((IDirty)data).IsDirty = false;
                    var callbackAction = (Action<T>)requestState.CallbackAction;
                    //if (null == callbackAction)
                        Application.Current.Dispatcher.Invoke(callbackAction, DispatcherPriority.Normal, data);
                }
            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                }
                try
                {
                    _exceptionService.RaiseException(new Exception(resp));
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                    _exceptionService.RaiseException(e);
                }
            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });
                _exceptionService.RaiseException(e);
            }
            finally
            {
                if (null != requestState && null != requestState.BusyEventArgs)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs());
                    });
            }
        }

        private void FinishGetFileAsync(IAsyncResult result)
        {
            RequestState requestState = null;
            try
            {
                requestState = (RequestState)result.AsyncState;
                var httpResponse = (HttpWebResponse)requestState.Request.EndGetResponse(result);
                var callbackAction = (Action<T>)requestState.CallbackAction;
                //if(null != callbackAction)
                    Application.Current.Dispatcher.Invoke(callbackAction, DispatcherPriority.Normal, httpResponse.GetResponseStream());
                
            }
            catch (WebException ex)
            {
                string resp = string.Empty;
                try
                {
                    resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });

                    _exceptionService.RaiseException(e);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        requestState?.ErrorCallbackAction?.Invoke(e);
                    });

                }
                try
                {

                    Exception exc = new Exception(resp);
                    _exceptionService.RaiseException(exc);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        requestState?.ErrorCallbackAction?.Invoke(exc);
                    });

                }
                catch (Exception e)
                {
                    _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });

                    _exceptionService.RaiseException(e);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        requestState?.ErrorCallbackAction?.Invoke(e);
                    });

                }
            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs() { Busy = false });

                _exceptionService.RaiseException(e);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    requestState?.ErrorCallbackAction?.Invoke(e);
                });


            }
            finally
            {
                if (null != requestState && null != requestState.BusyEventArgs)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _eventAggregator.GetEvent<BusyEvent>().Publish(new BusyEventArgs());
                    });
            }
        }

        /// <summary>
        /// Writes multi part HTTP POST request. Author : Farhan Ghumra
        /// </summary>
        private void WriteMultipartForm(Stream s, string boundary, string formName, string fileName, string fileContentType, byte[] fileData)
        {
            /// The first boundary
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            /// the last boundary.
            //byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            /// the form data, properly formatted
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            /// the form-data file upload, properly formatted
            string fileheaderTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\";\r\nContent-Type: {2}\r\n\r\n";

            WriteToStream(s, boundaryBytes);
            WriteToStream(s, string.Format(fileheaderTemplate, formName, fileName, fileContentType));
            // Write the file data to the stream.
            WriteToStream(s, fileData);
            WriteToStream(s, boundaryBytes);
        }

        /// <summary>
        /// Writes string to stream. Author : Farhan Ghumra
        /// </summary>
        private void WriteToStream(Stream s, string txt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(txt);
            s.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes byte array to stream. Author : Farhan Ghumra
        /// </summary>
        private void WriteToStream(Stream s, byte[] bytes)
        {
            s.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Returns byte array from StorageFile. Author : Farhan Ghumra
        /// </summary>
        private async Task<byte[]> GetBytesAsync(string file)
        {
            byte[] result;
            using (FileStream SourceStream = new FileStream(file, FileMode.Open, FileAccess.Read)) //File.Open(file, FileMode.Open))
            {
                result = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
            }
            return result;
        }

        private void SetClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  

            }
            ////////// free native resources if there are any.  
            ////////if (nativeResource != IntPtr.Zero)
            ////////{
            ////////    Marshal.FreeHGlobal(nativeResource);
            ////////    nativeResource = IntPtr.Zero;
            ////////}
        }

        #endregion


    }

}
