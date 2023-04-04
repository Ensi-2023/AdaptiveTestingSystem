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

namespace AdaptiveTestingSystem.ServerApplication.Assets.GUI.GUI_Setting._Page
{
    /// <summary>
    /// Логика взаимодействия для GUI_Page_ApplicationSetting.xaml
    /// </summary>
    public partial class GUI_Page_ApplicationSetting : UserControl
    {
        public GUI_Page_ApplicationSetting()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Main.Instance.Settings.OnAutoConnectToServer)
            {
                radio_3.IsChecked = true;
            }
            else radio_4.IsChecked = true;


            if (Main.Instance.Settings.OnAutoSaveLogToServer)
            {
                radio_1.IsChecked = true;
            }
            else radio_2.IsChecked = true;
        }

        private void radio_1_Checked(object sender, RoutedEventArgs e)
        {
            Main.Instance.Settings.SetAutoSaveLog(true);
        }

        private void radio_2_Checked(object sender, RoutedEventArgs e)
        {
            Main.Instance.Settings.SetAutoSaveLog(false);
        }

        private void radio_3_Checked(object sender, RoutedEventArgs e)
        {
            Main.Instance.Settings.SetAutoConnect(true);
        }

        private void radio_4_Checked(object sender, RoutedEventArgs e)
        {
            Main.Instance.Settings.SetAutoConnect(false);
        }
    }
}
