using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Ism.Infrastructure.Extensions;
namespace Ism.Infrastructure.Converters
{
    public class SolidColorBrushConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float factor = 1;
            float.TryParse(parameter as string, out factor);
            if (factor == 0) factor = 1;

            string col = (string)value;
            Color color =  string.IsNullOrEmpty(col) ? (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFD3D3D3") : factor != 1 ? ((Color)System.Windows.Media.ColorConverter.ConvertFromString((string)value)).ChangeColorBrightness(factor) :  (Color)System.Windows.Media.ColorConverter.ConvertFromString((string)value);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

