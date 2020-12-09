using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;

namespace Ism.Infrastructure.Ui
{
    public class CompabyBackgroundColorSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var style =  new Style(typeof(GridViewRow));

            var row = (GridViewRow)container;
            var obj = (ConstructionSiteList)item;
            
            style.Setters.Add(new Setter(GridViewRow.BackgroundProperty, new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(obj.Site.Company.Color))));

            return style;
        }
    }
}
