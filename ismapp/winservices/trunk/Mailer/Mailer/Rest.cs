using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Mailer
{
    public class Rest<T, TP> : IDisposable
    {
        public T PostRequest(string uri, TP payLoad, string token)
        {
            T response = default(T);
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.Method = "POST";
                if (null != token)
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, token);
                    
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
                throw ex;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return response;
        }

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
    }
}
