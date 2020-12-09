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
    public class ValidToBooleanConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

 

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var date = (DateTime)values[0];
                var from = values[1];
                var to = values[2];
                double days = 0;

                if (null == from && null == to)
                {
                    return false;
                }
                days = (date - DateTime.Now.Date).TotalDays;


                if (null == from)
                {
                    return days <= (int)to;
                }

                if (null == to)
                {
                    return days >= (int)from;
                }

                return days >= (int)from && days <= (int)to;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
