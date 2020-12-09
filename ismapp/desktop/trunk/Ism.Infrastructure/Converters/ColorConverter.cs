using Ism.Infrastructure.Extensions;
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

namespace Ism.Infrastructure.Converters
{
    public class ColorConverter : MarkupExtension, IValueConverter
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
            return string.IsNullOrEmpty(col) ? (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFD3D3D3") : factor != 1 ? ((Color)System.Windows.Media.ColorConverter.ConvertFromString((string)value)).ChangeColorBrightness(factor) : (Color)System.Windows.Media.ColorConverter.ConvertFromString((string)value); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }
    }
}
