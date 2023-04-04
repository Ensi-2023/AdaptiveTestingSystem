
using System.Windows;
using System.Windows.Controls;


namespace AdaptiveTestingSystem.Control.Themes
{

    public class CustomCheckBox : CheckBox
    {
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }
        public static readonly DependencyProperty IDProperty;

        static CustomCheckBox()
        {
            IDProperty = DependencyProperty.Register("ID", typeof(int), typeof(CustomCheckBox), new PropertyMetadata(0));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomCheckBox), new FrameworkPropertyMetadata(typeof(CustomCheckBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
