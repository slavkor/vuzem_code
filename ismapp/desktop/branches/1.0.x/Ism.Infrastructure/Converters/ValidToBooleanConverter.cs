using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Ism.Infrastructure.Converters
{
    public class ValidToBooleanConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if (value.ToString().Length == 0)
                return false;
            try
            {
                var i = System.Convert.ToInt32(parameter);
                var d = System.Convert.ToDateTime(value);
                var dd = ( d - DateTime.Now.Date).TotalDays;
                return dd >= 0 && dd <= i;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
