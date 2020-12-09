using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using log4net;
using log4net.Config;

namespace Mailer
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            try
            {
                string user = ConfigurationManager.AppSettings["user"];
                string pass = ConfigurationManager.AppSettings["pass"];
                string urla = ConfigurationManager.AppSettings["urla"];
                string url = ConfigurationManager.AppSettings["url"];

                var userCredentials = new UserCredentials() { GrantType = "password", ClinetId = "", ClinetSecret = "abc123", UserName = user, PassWord = pass };
                using (Rest<Token, UserCredentials> rest = new Rest<Token, UserCredentials>())
                {
                    Token token = rest.PostRequest(urla, userCredentials, null);

                    using (Rest<string, PayLoad> service = new Rest<string, PayLoad>())
                    {
                        string response = service.PostRequest(url, new PayLoad() { Date = DateTime.Now}, token.AccessToken);
                        log.Info(response);
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error("Napaka: ", exc);
            }

        }
    }
}
