using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Ism.Infrastructure.Validation
{
    public class Services
    {
        private static IServiceLocator _serviceLocator;

        public Services(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public static IServiceLocator ServiceLocator => _serviceLocator;
    }
}
