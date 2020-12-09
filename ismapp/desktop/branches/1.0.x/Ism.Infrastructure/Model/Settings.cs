using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class Settings
    {
        public IDictionary<string, string> GlobalAppSettings { get; set; }
        public IDictionary<string, string> CompanySettings { get; set; }
        public IDictionary<string, string> UserSettings { get; set; }
        public IList<Scope> UserScopes { get; set; }

    }
}
