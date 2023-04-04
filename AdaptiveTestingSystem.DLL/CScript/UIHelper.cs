#nullable disable

using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdaptiveTestingSystem.DLL.CScript
{
    public class UIHelper
    {






        public static void SaveUIToPNG(FrameworkElement uielement)
        {
            SaveFileDialog svDlg = new SaveFileDialog();
            svDlg.Filter = "PNG файлы|*.png|Все файлы|*.*";
            svDlg.Title = "Сохранить диаграмму в PNG";
            if (svDlg.ShowDialog().Value == true)
            {
                RenderTargetBitmap render = new RenderTargetBitmap((int)uielement.ActualWidth, (int)uielement.ActualHeight+150, 96, 96, PixelFormats.Pbgra32);
                render.Render(uielement);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(render));
                using (FileStream fs = new FileStream(svDlg.FileName, FileMode.Create))
                    encoder.Save(fs);
            }

        }

        public static Window FindParent(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            var test = parentObject as Window;
            if (test != null)
            {
                return test;
            }
            else
                return FindParent(parentObject);
        }


        public static PasswordBox FindPasswordParent(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            var test = parentObject as PasswordBox;
            if (test != null)
            {
                return test;
            }
            else
                return FindPasswordParent(parentObject);
        }

        public static List<object> FindParentBorder(DependencyObject child)
        {
            List<object> list = new List<object>();

            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return list;
            var test = parentObject as Border;
            if (test != null)
            {
                list.Add(test);
            }

            FindParentBorder(parentObject);
            return list;
        }

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == type)
            {
                return element;
            }
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                {
                    break;
                }
            }
            return foundElement;
        }


        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

    }

    public sealed class IgnoreMouseWheelBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
            base.OnDetaching();
        }
        void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!(sender is DependencyObject))
            {
                return;
            }

            DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject)sender);
            if (!(parent is UIElement))
            {
                return;
            }

        ((UIElement)parent).RaiseEvent(
            new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) { RoutedEvent = UIElement.MouseWheelEvent });
            e.Handled = true;
        }
    }

}
