using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Setting
{
    /// <summary>
    /// Логика взаимодействия для GUI_Setting.xaml
    /// </summary>
    public partial class GUI_Setting : UserControl
    {
        public GUI_Setting()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();



        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ListBox;
            switch (obj.SelectedIndex)
            {
                case 0: SetChild(new _pages._settings_Page_serverSetting()); break;
                case 1: SetChild(new _pages._settings_page_themeSetting()); break;
            }
        }

        private void SetChild(UIElement ui)
        {
            MyContent.Children.Clear();
            MyContent.Children.Add(ui);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MenyItems.SelectedIndex = 0;
        }
    }
}
