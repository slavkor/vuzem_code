using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Ism.Infrastructure.Extensions;
namespace Ism.Infrastructure.Converters
{
    public class RangeSliderConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int yf = 0, yt = 0, mf = 0, mt = 0;
            int.TryParse(values[0].ToString(), out yf);
            int.TryParse(values[1].ToString(), out yt);
            int.TryParse(values[2].ToString(), out mf);
            int.TryParse(values[3].ToString(), out mt);

            //yf = (int)values[0];
            //yt = (int)values[1];
            //mf = (int)values[2];
            //mt = (int)values[3];

            return new Range(new DateTime(yf, mf, 1), new DateTime(yt, mt, new DateTime(yt, mt, 1).LastDayOfMonth().Day));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
