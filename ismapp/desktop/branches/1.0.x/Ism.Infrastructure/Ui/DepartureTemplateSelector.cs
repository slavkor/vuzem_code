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
    public class DepartureTemplateSelector : DataTemplateSelector
    {
        private readonly ISecurityService security;
        public DepartureTemplateSelector()
        {
            security = ServiceLocator.Current.TryResolve<ISecurityService>();
        }
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate DepartureInboudConfirmedTemplate { get; set; }
        public DataTemplate DepartureOutboudConfirmedTemplate { get; set; }
        public DataTemplate DepartureInboudInProgressTemplate { get; set; }
        public DataTemplate DepartureOutboudInProgressTemplate { get; set; }
        public DataTemplate DepartureInternalTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            var dep = (item as TimelineDataItem)?.DataItem as DepartureList;

            if (null == dep) return DefaultTemplate;
            if(null == security) return DefaultTemplate;

            bool inProgress = dep.Departure.Status == 0;
            bool inbound = false;


            if (dep.Departure.Internal) return DepartureInternalTemplate;

            //if (security.HasPermissionExcplicit("foreman"))
            //{
            //    if (dep.Departure.Destination.DepartureArrivalType == "PROJECT")
            //        inbound = true;
            //}
            //else
            //{
            //    if (dep.Departure.Destination.DepartureArrivalType == "COMPANY")
            //        inbound = true;

            //}
            inbound = dep.Departure.Inbound;

            if (inProgress)
                return inbound ? DepartureInboudInProgressTemplate : DepartureOutboudInProgressTemplate;
            else
                return inbound ? DepartureInboudConfirmedTemplate : DepartureOutboudConfirmedTemplate;

        }
    }
}
