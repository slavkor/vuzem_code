using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ism.Infrastructure.Converters
{
    public   class TimelinePeriodConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is DateTime)) throw new ArgumentException();
            if (!(values[1] is DateTime)) throw new ArgumentException();

            Tuple<DateTime, DateTime> tuple = new Tuple<DateTime, DateTime>((DateTime)values[0], (DateTime)values[1]);
            return (object)tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
