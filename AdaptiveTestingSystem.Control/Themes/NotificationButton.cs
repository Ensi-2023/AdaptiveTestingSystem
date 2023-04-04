using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdaptiveTestingSystem.Control.Themes
{
    public class NotificationButton:Button
    {
        public int Count
        {
            get { return (int )GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }


        public bool VisibleCountNotification
        {
            get { return (bool)GetValue(VisibleCountNotificationProperty); }
            set { SetValue(VisibleCountNotificationProperty, value); }
        }




        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

     
        public static readonly DependencyProperty IconSizeProperty ;




        public static readonly DependencyProperty VisibleCountNotificationProperty ;



        public static readonly DependencyProperty CountProperty;


        static NotificationButton()
        {
            IconSizeProperty =  DependencyProperty.Register("IconSize", typeof(double), typeof(NotificationButton), new PropertyMetadata(15.0));
            VisibleCountNotificationProperty = DependencyProperty.Register("VisibleCountNotification", typeof(bool), typeof(NotificationButton), new PropertyMetadata(false));
            CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(NotificationButton), new PropertyMetadata(0));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationButton), new FrameworkPropertyMetadata(typeof(NotificationButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}

