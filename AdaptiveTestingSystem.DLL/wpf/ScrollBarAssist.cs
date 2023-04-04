using System.Windows;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public class ScrollBarAssist
    {
        public static bool GetIsMouseOnThumb(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseOnThumbProperty);
        }

        public static void SetIsMouseOnThumb(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMouseOnThumbProperty, value);
        }


        public static readonly DependencyProperty IsMouseOnThumbProperty =
            DependencyProperty.RegisterAttached("IsMouseOnThumb", typeof(bool), typeof(ScrollBarAssist), new PropertyMetadata(false));


    }
}
