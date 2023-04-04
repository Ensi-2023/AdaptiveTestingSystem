#nullable disable

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public class PopupAssist
    {

        public static UIElement GetPopupContent(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(PopupContentProperty);
        }

        public static void SetPopupContent(DependencyObject obj, UIElement value)
        {
            obj.SetValue(PopupContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for PopupContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.RegisterAttached("PopupContent", typeof(UIElement), typeof(PopupAssist), new PropertyMetadata(null, ItemChildenCallBack));


        private static void ItemChildenCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            var item = (UIElement)e.NewValue;
            var comp = (Control)d;
            if (comp.FindName("PART_popupContent") is not Popup search) return;
            search.Child = item;
        }

        public static bool GetIsView(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsViewProperty);
        }

        public static void SetIsView(DependencyObject obj, bool value)
        {
            obj.SetValue(IsViewProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsViewProperty =
            DependencyProperty.RegisterAttached("IsView", typeof(bool), typeof(PopupAssist), new PropertyMetadata(false));


        public static bool GetIsOpenPopup(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsOpenPopupProperty);
        }

        public static void SetIsOpenPopup(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOpenPopupProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsOpenPopup.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenPopupProperty =
            DependencyProperty.RegisterAttached("IsOpenPopup", typeof(bool), typeof(PopupAssist), new PropertyMetadata(false, IsOpenPopupCallback));

        private static void IsOpenPopupCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _isopen = (bool)e.NewValue;
        }

        private static bool _isopen;

        private bool _ismouseenterpopup;
        private Control _control = new();
        public void MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (sender is not Control el) return;
            if (GetIsView(el) == false) return;


            if (_isopen) { SetIsOpenPopup(el, false); return; }

            SetIsOpenPopup(el, true);
        }

        public async void Main_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (sender is not Control el) return;

            await StartDelay(el);
        }

        public void Main_MouseEnter(object sender, RoutedEventArgs e)
        {

            this._control = (Control)sender;
        }

        public async void Main_PpupMouseLeave(object sender, RoutedEventArgs e)
        {

            if (_control == null) return;
            _ismouseenterpopup = false;
            await StartDelay(_control);
        }

        public void Main_PpupMouseEnter(object sender, RoutedEventArgs e)
        {
            this._ismouseenterpopup = true;
        }

        private async Task StartDelay(Control el)
        {
            await Task.Delay(55);
            if (_ismouseenterpopup) return;
            SetIsOpenPopup(el, false);
            _control = new Control();
        }






        public static double GetVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalOffsetProperty, value);
        }


        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(PopupAssist), new PropertyMetadata(0.0));



        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }


        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(PopupAssist), new PropertyMetadata(0.0));


    }
}
