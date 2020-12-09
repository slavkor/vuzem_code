using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Ism.Infrastructure.Services;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace Ism.Infrastructure.Converters
{
    public class SecurityScopeToVisibilityConverter : IValueConverter
    {
        private readonly ISecurityService _security;
        public SecurityScopeToVisibilityConverter()
        {
            _security = ServiceLocator.Current.GetInstance<ISecurityService>();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterString = parameter as string;
            if (parameterString == null)
                return Visibility.Collapsed;
            if (_security == null) return Visibility.Collapsed;

            string[] parameters = parameterString.Split(new char[] { '|' });
            if(parameters.Length == 1)
                return _security.HasPermission(parameterString) ? Visibility.Visible : Visibility.Collapsed;

            bool expl = false;
            bool.TryParse(parameters[1], out expl);

            if(expl)
                return _security.HasPermissionExcplicit(parameters[0]) ? Visibility.Visible : Visibility.Collapsed;
            else
                return _security.HasPermission(parameters[0]) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
