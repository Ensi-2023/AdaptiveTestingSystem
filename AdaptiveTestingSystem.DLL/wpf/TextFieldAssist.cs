#nullable disable

using System.Windows;
using System.Windows.Controls;

namespace AdaptiveTestingSystem.DLL.wpf
{
    public partial class TextFieldAssist
    {

        public static bool GetIsEditable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEditableProperty);
        }

        public static void SetIsEditable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEditableProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.RegisterAttached("IsEditable", typeof(bool), typeof(TextFieldAssist), new PropertyMetadata(true));




        public static bool GetWoteMarkView(DependencyObject obj)
        {
            return (bool)obj.GetValue(WoteMarkViewProperty);
        }

        public static void SetWoteMarkView(DependencyObject obj, bool value)
        {
            obj.SetValue(WoteMarkViewProperty, value);
        }

        // Using a DependencyProperty as the backing store for WoteMarkView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WoteMarkViewProperty =
            DependencyProperty.RegisterAttached("WoteMarkView", typeof(bool), typeof(TextFieldAssist), new PropertyMetadata(true));


        public static string GetWotemark(DependencyObject obj)
        {
            return (string)obj.GetValue(WotemarkProperty);
        }

        public static void SetWotemark(DependencyObject obj, string value)
        {
            obj.SetValue(WotemarkProperty, value);
        }

        // Using a DependencyProperty as the backing store for Wotemark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WotemarkProperty =
            DependencyProperty.RegisterAttached("Wotemark", typeof(string), typeof(TextFieldAssist), new PropertyMetadata(""));



        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(TextFieldAssist), new PropertyMetadata(new CornerRadius(0)));





        public static double GetWotemarkFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(WotemarkFontSizeProperty);
        }

        public static void SetWotemarkFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(WotemarkFontSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for WotemarkFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WotemarkFontSizeProperty =
            DependencyProperty.RegisterAttached("WotemarkFontSize", typeof(double), typeof(TextFieldAssist), new PropertyMetadata(1.1));













        // Using a DependencyProperty as the backing store for PasswordText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordTextProperty =
            DependencyProperty.RegisterAttached("PasswordText", typeof(bool), typeof(TextFieldAssist), new PropertyMetadata(false));


    

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
            DependencyProperty.RegisterAttached("Icon", typeof(PackIconMaterialKind), typeof(TextFieldAssist));

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
            DependencyProperty.RegisterAttached("IconView", typeof(bool), typeof(TextFieldAssist), new PropertyMetadata(false, SetIconViewChange));

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
            DependencyProperty.RegisterAttached("IconVisibility", typeof(Visibility), typeof(TextFieldAssist), new PropertyMetadata(Visibility.Collapsed));







        public static bool GetViewButtonCheckPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(ViewButtonCheckPasswordProperty);
        }

        public static void SetViewButtonCheckPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(ViewButtonCheckPasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewButtonCheckPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewButtonCheckPasswordProperty =
            DependencyProperty.RegisterAttached("ViewButtonCheckPassword", typeof(bool), typeof(TextFieldAssist), new PropertyMetadata(true));







        public static double GetCustomBorderThickness(DependencyObject obj)
        {
            return (double)obj.GetValue(CustomBorderThicknessProperty);
        }

        public static void SetCustomBorderThickness(DependencyObject obj, double value)
        {
            obj.SetValue(CustomBorderThicknessProperty, value);
        }

        // Using a DependencyProperty as the backing store for CustomBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBorderThicknessProperty =
            DependencyProperty.RegisterAttached("CustomBorderThickness", typeof(double), typeof(TextFieldAssist), new PropertyMetadata(1.0));


    }
}
