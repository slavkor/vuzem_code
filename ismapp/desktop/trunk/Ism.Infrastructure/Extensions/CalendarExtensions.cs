using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace Ism.Infrastructure.Extensions
{

    public class CalendarExtensions
    {
        public static bool GetTrackCalendarSelectedDates(DependencyObject obj)
        {
            return (bool)obj.GetValue(TrackCalendarSelectedDatesProperty);
        }

        public static void SetTrackCalendarSelectedDates(DependencyObject obj, bool value)
        {
            obj.SetValue(TrackCalendarSelectedDatesProperty, value);
        }

        public static readonly DependencyProperty TrackCalendarSelectedDatesProperty =
            DependencyProperty.RegisterAttached("TrackCalendarSelectedDates", typeof(bool), typeof(CalendarExtensions), new PropertyMetadata(false, OnTrackCalendarSelectedDatesPropertyChanged));

        public static IEnumerable<DateTime> GetSelectedDates(DependencyObject obj)
        {
            return (IEnumerable<DateTime>)obj.GetValue(SelectedDatesProperty);
        }

        public static void SetSelectedDates(DependencyObject obj, IEnumerable<DateTime> value)
        {
            obj.SetValue(SelectedDatesProperty, value);
        }

        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.RegisterAttached("SelectedDates", typeof(IEnumerable<DateTime>), typeof(CalendarExtensions), new PropertyMetadata(Enumerable.Empty<DateTime>(), OnSelectedDatesPropertyChanged));

        private static CalendarExtensions GetExtensions(DependencyObject obj)
        {
            return (CalendarExtensions)obj.GetValue(ExtensionsProperty);
        }

        private static void SetExtensions(DependencyObject obj, CalendarExtensions value)
        {
            obj.SetValue(ExtensionsProperty, value);
        }

        private static readonly DependencyProperty ExtensionsProperty =
            DependencyProperty.RegisterAttached("Extensions", typeof(CalendarExtensions), typeof(CalendarExtensions), null);

        private static void OnSelectedDatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var extensions = GetExtensions(d);
            if (extensions != null)
            {
                extensions.UpdateCalendarSelectedDates(GetSelectedDates(d));
            }
        }

        private static void OnTrackCalendarSelectedDatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendar = d as RadCalendar;
            if (calendar != null)
            {
                if ((bool)e.NewValue)
                {
                    var extensions = new CalendarExtensions(calendar);
                    SetExtensions(calendar, extensions);
                    extensions.UpdateSelectedDates();
                }
                else
                {
                    GetExtensions(calendar).Detach();
                    calendar.ClearValue(ExtensionsProperty);
                }
            }
        }

        private RadCalendar calendar;
        private bool updatingSelectedDates;
        private CalendarExtensions(RadCalendar calendar)
        {
            this.calendar = calendar;
            this.calendar.SelectionChanged += Calendar_SelectionChanged;
            //this.calendar.SelectionChanged += this.OnCalendarSelectionChanged;
        }

        private void Calendar_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.UpdateSelectedDates();
        }

        private void Detach()
        {
            this.calendar.SelectionChanged -= this.Calendar_SelectionChanged;
            //this.calendar.SelectionChanged -= this.OnCalendarSelectionChanged;
        }

        //private void OnCalendarSelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    this.UpdateSelectedDates();
        //}

        private void UpdateSelectedDates()
        {
            if (!this.updatingSelectedDates)
            {
                this.updatingSelectedDates = true;
                SetSelectedDates(this.calendar, this.calendar.SelectedDates);
                this.updatingSelectedDates = false;
            }
        }

        private void UpdateCalendarSelectedDates(IEnumerable<DateTime> dates)
        {
            if (!this.updatingSelectedDates)
            {
                this.updatingSelectedDates = true;
                this.calendar.SelectedDates.Clear();
                if (dates != null)
                {
                    foreach (var date in dates)
                    {
                        this.calendar.SelectedDates.Add(date);
                    }
                }
                this.updatingSelectedDates = false;
            }
        }
    }
}
