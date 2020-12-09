using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ism.Infrastructure.Ui
{
    public class DepartureCarListItemSelector : DataTemplateSelector
    {
        //private readonly ISecurityService security;
        //public DepartureEmployeeListItemTemplateSelector()
        //{
        //    security = ServiceLocator.Current.TryResolve<ISecurityService>();
        //}
        public DataTemplate AddTemplate { get; set; }
        public DataTemplate CarTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var car = item as CarListItem;

            if (car == null) return CarTemplate;

            if (car.IsAddItem)
                return AddTemplate;

            return CarTemplate;
        }
    }
}