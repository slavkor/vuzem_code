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
    public class OriginDepartureTemplateSelector : DataTemplateSelector
    {
        private readonly ISecurityService security;
        public OriginDepartureTemplateSelector()
        {
            security = ServiceLocator.Current.TryResolve<ISecurityService>();
        }
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate OriginTemplate { get; set; }
        public DataTemplate DestinationTemplace { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is IOriginDestination)
            {
                var od= item as IOriginDestination;

                if (od == null) return DefaultTemplate;
                if (od.IsOrigin) return OriginTemplate;
                return DestinationTemplace;

            }
            return DefaultTemplate;
        }
    }
}
