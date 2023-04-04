using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public class ProgressbarAssist
    {
     

        public static double GetLineHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(LineHaithProperty);
        }

        public static void SetLineHeight(DependencyObject obj, double value)
        {
            obj.SetValue(LineHaithProperty, value);
        }

        // Using a DependencyProperty as the backing store for LineHaith.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineHaithProperty =
            DependencyProperty.RegisterAttached("LineHeight", typeof(double), typeof(ProgressbarAssist), new PropertyMetadata(2.0));


    }
}
