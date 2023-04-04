using System.Windows;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public partial class WindowsAssist
    {

        public static string GetUCTitle(DependencyObject obj)
        {
            return (string)obj.GetValue(UCTitleProperty);
        }

        public static void SetUCTitle(DependencyObject obj, string value)
        {
            obj.SetValue(UCTitleProperty, value);
        }

        public static readonly DependencyProperty UCTitleProperty =
            DependencyProperty.RegisterAttached("UCTitle", typeof(string), typeof(WindowsAssist), new PropertyMetadata(string.Empty));


    }
}
