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
    public class AbsenceTypeToIntConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var typ = (int)value;

            switch (typ)
            {
                case 0:
                    return "Dopust";
                case 1:
                    return "Bolniška";
                case 2:
                    return "Odsotnost";
                default:
                    throw new NotImplementedException();

            }

            ///return typ == 0 ? "Dopust" : "Bolniška";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Dopust":
                    return 0;
                case "Bolniška":
                    return 1;
                case "Odsotnost":
                    return 2;
                default:
                    throw new NotImplementedException() ;
                    
            }
        }
    }
}
