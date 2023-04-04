using System.Windows;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public partial class BarAssist
    {

        public static bool GetVisibleSortButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(VisibleSortButtonProperty);
        }

        public static void SetVisibleSortButton(DependencyObject obj, bool value)
        {
            obj.SetValue(VisibleSortButtonProperty, value);
        }

        public static readonly DependencyProperty VisibleSortButtonProperty =
            DependencyProperty.RegisterAttached("VisibleSortButton", typeof(bool), typeof(BarAssist), new PropertyMetadata(true));




        public static Style GetCustomStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(CustomStyleProperty);
        }

        public static void SetCustomStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(CustomStyleProperty, value);
        }

        public static readonly DependencyProperty CustomStyleProperty =
            DependencyProperty.RegisterAttached("CustomStyle", typeof(Style), typeof(BarAssist), new PropertyMetadata(null));




        public static CornerRadius GetBarCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(BarCornerRadiusProperty);
        }

        public static void SetBarCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(BarCornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for BarCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarCornerRadiusProperty =
            DependencyProperty.RegisterAttached("BarCornerRadius", typeof(CornerRadius), typeof(BarAssist), new PropertyMetadata(new CornerRadius(0)));
    }
}
