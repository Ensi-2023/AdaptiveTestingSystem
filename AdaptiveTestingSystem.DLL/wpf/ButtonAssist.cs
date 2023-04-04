#nullable disable

using System.Windows;
using System.Windows.Media;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public enum IconPosition
    {
        left, Right
    }

    public class ButtonAssist
    {




        public static Thickness GetIconMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(IconMarginProperty);
        }

        public static void SetIconMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(IconMarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.RegisterAttached("IconMargin", typeof(Thickness), typeof(ButtonAssist), new PropertyMetadata(new Thickness(0.0)));




        public static double GetIconSize(DependencyObject obj)
        {
            return (double)obj.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(IconSizeProperty, value);
        }


        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.RegisterAttached("IconSize", typeof(double), typeof(ButtonAssist), new PropertyMetadata(15.0));




        public static IconPosition GetIconPosition(DependencyObject obj)
        {
            return (IconPosition)obj.GetValue(IconPositionProperty);
        }

        public static void SetIconPosition(DependencyObject obj, IconPosition value)
        {
            obj.SetValue(IconPositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconPositionProperty =
            DependencyProperty.RegisterAttached("IconPosition", typeof(IconPosition), typeof(ButtonAssist), new PropertyMetadata(IconPosition.left));



        public static int GetIndexDB(DependencyObject obj)
        {
            return (int)obj.GetValue(IndexDBProperty);
        }

        public static void SetIndexDB(DependencyObject obj, int value)
        {
            obj.SetValue(IndexDBProperty, value);
        }


        public static readonly DependencyProperty IndexDBProperty =
            DependencyProperty.RegisterAttached("IndexDB", typeof(int), typeof(ButtonAssist), new PropertyMetadata(0));





        public static Brush GetForegroundMouseEnter(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundMouseEnterProperty);
        }

        public static void SetForegroundMouseEnter(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundMouseEnterProperty, value);
        }

        // Using a DependencyProperty as the backing store for ForegroundMouseEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundMouseEnterProperty =
            DependencyProperty.RegisterAttached("ForegroundMouseEnter", typeof(Brush), typeof(ButtonAssist), new PropertyMetadata(Brushes.Transparent));




        public static readonly DependencyProperty ColorMouseEnterProperty = DependencyProperty.RegisterAttached(
          "ColorMouseEnter", typeof(Brush), typeof(ButtonAssist), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetColorMouseEnter(DependencyObject element, Brush value)
        {
            element.SetValue(ColorMouseEnterProperty, value);
        }

        public static Brush GetColorMouseEnter(DependencyObject element)
        {
            return (Brush)element.GetValue(ColorMouseEnterProperty);
        }

        public static Brush GetColorMouseClick(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ColorMouseClickProperty);
        }

        public static void SetColorMouseClick(DependencyObject obj, Brush value)
        {
            obj.SetValue(ColorMouseClickProperty, value);
        }

        // Using a DependencyProperty as the backing store for ColorMouseClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorMouseClickProperty =
            DependencyProperty.RegisterAttached("ColorMouseClick", typeof(Brush), typeof(ButtonAssist), new PropertyMetadata(Brushes.Transparent));



        public static CornerRadius GetBorderRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(BorderRadiusProperty);
        }

        public static void SetBorderRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(BorderRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for BorderRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.RegisterAttached("BorderRadius", typeof(CornerRadius), typeof(ButtonAssist));





        public static PackIconMaterialKind GetIcon(DependencyObject obj)
        {
            return (PackIconMaterialKind)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, PackIconMaterialKind value)
        {
            obj.SetValue(IconProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(PackIconMaterialKind), typeof(ButtonAssist));

        public static bool GetIconView(DependencyObject obj)
        {
            return (bool)obj.GetValue(IconViewProperty);
        }

        public static void SetIconView(DependencyObject obj, bool value)
        {
            obj.SetValue(IconViewProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconViewProperty =
            DependencyProperty.RegisterAttached("IconView", typeof(bool), typeof(ButtonAssist), new PropertyMetadata(false, SetIconViewChange));

        private static void SetIconViewChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as UIElement;
            if (el == null) return;
            if ((bool)e.NewValue)
            {
                SetIconVisibility(el, Visibility.Visible);
            }
            else SetIconVisibility(el, Visibility.Collapsed);
        }

        public static Visibility GetIconVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(IconVisibilityProperty);
        }

        private static void SetIconVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(IconVisibilityProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.RegisterAttached("IconVisibility", typeof(Visibility), typeof(ButtonAssist), new PropertyMetadata(Visibility.Collapsed));





        public static Visibility GetVisibilityButton(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(VisibilityButtonProperty);
        }

        public static void SetVisibilityButton(DependencyObject obj, Visibility value)
        {
            obj.SetValue(VisibilityButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for VisibilityButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibilityButtonProperty =
            DependencyProperty.RegisterAttached("VisibilityButton", typeof(Visibility), typeof(ButtonAssist), new PropertyMetadata(Visibility.Collapsed));

    }
}
