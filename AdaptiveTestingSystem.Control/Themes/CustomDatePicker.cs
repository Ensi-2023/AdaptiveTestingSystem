
using System.Windows;
using System.Windows.Controls;


namespace AdaptiveTestingSystem.Control.Themes
{

    public class CustomDatePicker : DatePicker
    {
        public bool WotemarkView
        {
            get { return (bool)GetValue(WotemarkViewProperty); }
            set { SetValue(WotemarkViewProperty, value); }
        }


        public string Wotemark
        {
            get { return (string)GetValue(WotemarkProperty); }
            set { SetValue(WotemarkProperty, value); }
        }



        public double WotemarkFontSize
        {
            get { return (double)GetValue(WotemarkFontSizeProperty); }
            set { SetValue(WotemarkFontSizeProperty, value); }
        }


        public static readonly DependencyProperty WotemarkFontSizeProperty;
        public static readonly DependencyProperty WotemarkProperty;
        public static readonly DependencyProperty WotemarkViewProperty;

        static CustomDatePicker()
        {
            WotemarkFontSizeProperty = DependencyProperty.Register("WotemarkFontSize", typeof(double), typeof(CustomDatePicker), new PropertyMetadata(0.0));
            WotemarkViewProperty = DependencyProperty.Register("WotemarkView", typeof(bool), typeof(CustomDatePicker), new PropertyMetadata(true));
            WotemarkProperty = DependencyProperty.Register("Wotemark", typeof(string), typeof(CustomDatePicker), new PropertyMetadata(string.Empty));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomDatePicker), new FrameworkPropertyMetadata(typeof(CustomDatePicker)));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
