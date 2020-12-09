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
    public class PasswordToCryptConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // BCrypt.Net.BCrypt.HashPassword((string)value);
        }
    }
}
