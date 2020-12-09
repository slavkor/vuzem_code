using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ism.Infrastructure.Ui
{
    public static class FocusAdvancement
    {
        public static bool GetAdvancesByKey(DependencyObject obj)
        {
            return (bool)obj.GetValue(AdvancesByKeyProperty);
        }

        public static void SetAdvancesByKey(DependencyObject obj, bool value)
        {
            obj.SetValue(AdvancesByKeyProperty, value);
        }

        public static readonly DependencyProperty AdvancesByKeyProperty =
            DependencyProperty.RegisterAttached("AdvancesByKey", typeof(bool), typeof(FocusAdvancement),
            new UIPropertyMetadata(OnAdvancesByKeyPropertyChanged));

        static void OnAdvancesByKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null) return;

            if ((bool)e.NewValue)
            {
                element.KeyDown += Keydown;
                element.PreviewKeyDown += PreviewKeyDown;
            }
            else
            {
                element.KeyDown -= Keydown;
                element.PreviewKeyDown -= PreviewKeyDown;
            }
        }

        private static void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    ((UIElement)e.OriginalSource).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                case Key.Up:
                    ((UIElement)e.OriginalSource).MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    break;
                default:
                    break;
            }
        }

        static void Keydown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    ((UIElement)e.OriginalSource).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                case Key.Down:
                    ((UIElement)e.OriginalSource).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                case Key.Up:
                    ((UIElement)e.OriginalSource).MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    break;
                default:
                    break;
            }
        }
    }
}
