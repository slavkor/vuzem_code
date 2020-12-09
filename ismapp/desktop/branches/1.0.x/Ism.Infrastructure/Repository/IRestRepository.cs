using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Validation;

namespace Ism.Infrastructure.Repository
{
    public interface IRestRepository<out T, in TP> : IDisposable
    {

        T PostRequest(string uri, TP payLoad);
        T PostRequest(string uri, TP payLoad, Token token);

        T GetRequest(string url);
        T GetRequest(string url, Token token);


        void GetRequestAsync(string url, Action<T> callback);
        void GetRequestAsync(string url, Token token, Action<T> callback);
        void GetRequestAsync(string url, Token token, Action<T> callback, string waitNotification, bool busyNotification);

        void PostRequestAsync(string uri, TP payLoad, Action<T> callback);
        void PostRequestAsync(string uri, TP payLoad, Token token, Action<T> callback);
        void PostRequestAsync(string uri, TP payLoad, Token token, Action<T> callback, string waitNotification, bool busyNotification);

        void PostFileAsync(string url, string fileName, User user, IDictionary<string, string> querystring, Action<T> callback, string waitNotification, bool busyNotification);

        void GetFileAsync(string url, User user, IDictionary<string, string> querystring, Action<T> callback, string waitNotification, bool busyNotification);
        void GetReportAsync(Report report, User user, Action<T> callback, string waitNotification, bool busyNotification, Action<Exception> errcallback);

    }
}
