using Ism.Infrastructure.Model;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Calendar;
using Telerik.Windows.Controls.Timeline;

namespace Ism.Infrastructure.Ui
{
    public class DepartureEmployeeListItemTemplateSelector : DataTemplateSelector
    {
        //private readonly ISecurityService security;
        //public DepartureEmployeeListItemTemplateSelector()
        //{
        //    security = ServiceLocator.Current.TryResolve<ISecurityService>();
        //}
        public DataTemplate AddTemplate { get; set; }
        public DataTemplate EmployeeTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var emp = item as EmployeeListItem;

            if (emp == null) return EmployeeTemplate;

            if (emp.IsAddItem)
                return AddTemplate;

            return EmployeeTemplate;
        }
    }
}
