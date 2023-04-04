#nullable disable
using System.Windows;
using System.Windows.Controls;

namespace AdaptiveTestingSystem.Control.ControlAssist
{
    partial class TextFieldClassAssist
    {


        public static bool GetVertyPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(VertyPasswordProperty);
        }

        public static void SetVertyPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(VertyPasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for VertyPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VertyPasswordProperty =
            DependencyProperty.RegisterAttached("VertyPassword", typeof(bool), typeof(TextFieldClassAssist), new PropertyMetadata(true));



        public static string GetPasswords(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordsProperty);
        }

        public static void SetPasswords(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Passwords.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordsProperty =
            DependencyProperty.RegisterAttached("Passwords", typeof(string), typeof(TextFieldClassAssist), new PropertyMetadata(""));





        public void OnPasswordText(object sender, RoutedEventArgs e)
        {
            var el = sender as PasswordBox;
            if (el == null) return;

            if (el.Password.Length > 0)
            {
                SetVertyPassword(el, false);
            }
            else SetVertyPassword(el, true);

        }

        public static bool GetViewPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(ViewPasswordProperty);
        }

        public static void SetViewPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(ViewPasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewPasswordProperty =
            DependencyProperty.RegisterAttached("ViewPassword", typeof(bool), typeof(TextFieldClassAssist), new PropertyMetadata(false));


        private DependencyObject GetDependencyObject(object obj)
        {
            var x = obj as Button;
            if (x == null) return null;
            var pass = UIHelper.FindPasswordParent(x);
            return pass;
        }

        public void OnMouse_Down(object sender, RoutedEventArgs e)
        {
            var obj = GetDependencyObject(sender);
            if (obj == null) return;
            SetPasswords(obj, (obj as PasswordBox).Password);
            SetViewPassword(obj, true);
        }
        public void OnMouse_Up(object sender, RoutedEventArgs e)
        {
            var obj = GetDependencyObject(sender);
            if (obj == null) return;
            SetPasswords(obj, "");
            SetViewPassword(obj, false);
            (obj as PasswordBox).Focus();
        }


    }
}
