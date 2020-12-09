using Ism.Infrastructure.Events;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
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

namespace Ism.Infrastructure.Ui
{
    public class ShiftCalendarTemplateSelector : DataTemplateSelector
    {
        private readonly IEventAggregator events;
        public ShiftCalendarTemplateSelector()
        {

            this.events = ServiceLocator.Current.TryResolve<IEventAggregator>();
            events.GetEvent<SelectedEvent<ObservableCollection<DateTime>>>().Subscribe(OnSelectedEvent);

        }

        private void OnSelectedEvent(SelectedEventArgs<ObservableCollection<DateTime>> obj)
        {
            this.ShiftDays = obj.SelectedData;
        }

        private ObservableCollection<DateTime> _shiftDays;

        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate ShiftDayTemplate { get; set; }

        public RadCalendar Calendar { get; set; }


        public ObservableCollection<DateTime> ShiftDays
        {
            get
            {
                return this._shiftDays;
            }
            set
            {
                if (this._shiftDays != null)
                {
                    this._shiftDays.CollectionChanged -= OnDatesCollectionChanged;
                }

                if (this._shiftDays != value)
                {
                    this._shiftDays = value;
                    this._shiftDays.CollectionChanged += OnDatesCollectionChanged;
                    ResetCalendarTemplate();
                }
            }

        }


        public List<DateTime> BookedDays { get; set; }
        public List<DateTime> SpecialHolidays { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            this.Calendar = (container as FrameworkElement).ParentOfType<RadCalendar>();
            var calendarButton = (CalendarButtonContent)item;
            if (null == this.ShiftDays) return this.DefaultTemplate;

            if (this.ShiftDays.Contains(calendarButton.Date))
            {
                return this.ShiftDayTemplate;
            }

            return this.DefaultTemplate;
        }



        private void OnDatesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ResetCalendarTemplate();
        }
        private void ResetCalendarTemplate()
        {
            var template = this.Calendar.Template;
            this.Calendar.Template = null;
            this.Calendar.Template = template;
        }
    }
}
