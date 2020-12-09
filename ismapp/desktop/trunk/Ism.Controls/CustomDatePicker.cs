using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ism.Controls
{
    public class CustomDatePicker : DatePicker
    {
        protected DatePickerTextBox _datePickerTextBox;

        public static readonly new DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(CustomDatePicker),
                new FrameworkPropertyMetadata(null, SelectedDateChanged) { BindsTwoWayByDefault = true });

        private static new void SelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomDatePicker)d).SelectedDate = (DateTime?)e.NewValue;
        }

        public new DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set
            {
                if (base.SelectedDate != value) base.SelectedDate = value;
                if (this.SelectedDate != value) SetValue(SelectedDateProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _datePickerTextBox = this.Template.FindName("PART_TextBox", this) as DatePickerTextBox;
            if (_datePickerTextBox != null)
                _datePickerTextBox.TextChanged += dptb_TextChanged;
        }

        private void dptb_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = _datePickerTextBox.SelectionStart;
            string text = _datePickerTextBox.Text;

            DateTime dt;
            if (DateTime.TryParse(_datePickerTextBox.Text, Thread.CurrentThread.CurrentCulture,
                System.Globalization.DateTimeStyles.None, out dt))
                this.SelectedDate = dt;
            else
                this.SelectedDate = null;

            _datePickerTextBox.Text = text;
            _datePickerTextBox.SelectionStart = index;
        }
    }
}
