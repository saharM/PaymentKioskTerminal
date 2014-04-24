using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PS.Kiosk.UI
{
    public class UIStateManager : DependencyObject
    {
        public static string GetVisualStateProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(VisualStatePropertyProperty);
        }
        public static void SetVisualStateProperty(DependencyObject obj, string value)
        {
            obj.SetValue(VisualStatePropertyProperty, value);
        }
        public static readonly DependencyProperty VisualStatePropertyProperty =
            DependencyProperty.RegisterAttached(
            "VisualStateProperty",
            typeof(string),
            typeof(UIStateManager),
            new PropertyMetadata((dependencyObject, args) =>
            {
                var frameworkElement = dependencyObject as FrameworkElement;
                if (frameworkElement == null)
                    return;
                VisualStateManager.GoToState(frameworkElement, (string)args.NewValue, true);
            }));
    }
}
