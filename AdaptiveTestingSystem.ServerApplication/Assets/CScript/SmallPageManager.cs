using System.Windows;
using System.Windows.Controls;

namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript
{
    public class SmallPageManager
    {
        private static Grid? Main { get; set; }

        public static void Set(Grid? main)
        {
            Main = main;
        }

        public static void Next(UIElement ui)
        {
            if (Main == null) return;
            Main.Visibility = Visibility.Visible;
            Main.Children.Clear();
            Main.Children.Add(ui);
        }

        public static void Close()
        {
            if (Main == null) return;
            Main.Visibility = Visibility.Visible;
            Main.Children.Clear();
        }
    }
}
