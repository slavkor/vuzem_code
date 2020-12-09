using Ism.Infrastructure.Model;
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
    public class DayModelToDateTimeConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Day day = value as Day;
            return null == day ? DateTime.MinValue : day.Date;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            return null == value ? new Day(DateTime.Now) : new Day((DateTime)value);
        }
    }
}
