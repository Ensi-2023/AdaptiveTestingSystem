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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Setting._pages
{
    /// <summary>
    /// Логика взаимодействия для _settings_page_themeSetting.xaml
    /// </summary>
    public partial class _settings_page_themeSetting : UserControl
    {
        public _settings_page_themeSetting()
        {
            InitializeComponent();
        }


        private string GetCheckNameRadioButton()
        {
            string name = "";
            foreach (var child in body.Children)
            {
                var item = child as Border;
                if (item != null)
                {
                    var grid = item.Child as Grid;
                    if (grid != null)
                    {
                        foreach (var radio in grid.Children)
                        {
                            var checkRadio = radio as RadioButton;
                            if (checkRadio != null)
                            {
                                if (checkRadio.IsChecked == true) return checkRadio.Name; else break;
                            }
                        }
                    }
                }
            }

            return name;
        }
        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            var str = GetCheckNameRadioButton();
            if (str.Trim().Length > 0)
            {
                _Main.Instance.Theme.Set(str);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_Main.Instance.Theme.Theme.Trim().ToLower())
            {
                case "blacktheme": blackTheme.IsChecked = true; break;
                case "bluetheme": blueTheme.IsChecked = true; break;
                case "bluepurpletheme": bluePurpleTheme.IsChecked = true; break;
            }
        }
    }
}