
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdaptiveTestingSystem.Control.Themes
{

    public class CustomRadioButton : RadioButton
    {
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }
        public static readonly DependencyProperty IDProperty;

        static CustomRadioButton()

        {
            IDProperty = DependencyProperty.Register("ID", typeof(int), typeof(CustomRadioButton), new PropertyMetadata(0));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomRadioButton), new FrameworkPropertyMetadata(typeof(CustomRadioButton)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (this.IsChecked == true) this.IsChecked = false;
        }
    }
}
