#nullable disable
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
    /// Логика взаимодействия для GUI_Page_ServerSetting.xaml
    /// </summary>
    public partial class GUI_Page_ServerSetting : UserControl
    {
        public GUI_Page_ServerSetting()
        {
            InitializeComponent();
        }

        private void ip_1_KeyDown(object sender, KeyEventArgs e)
        {
            var obj = (TextBox)sender;

            if (!((e.Key.GetHashCode() >= 34) && (e.Key.GetHashCode() <= 43)) && !((e.Key.GetHashCode() >= 74) && (e.Key.GetHashCode() <= 83)) && !(e.Key.GetHashCode() == 3))
            {

                e.Handled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Main.Instance.Settings.IP != null)
            {
                string[] str = Main.Instance.Settings.IP.ToString().Split('.');

                ip_1.Text = str[0];
                ip_2.Text = str[1];
                ip_3.Text = str[2];
                ip_4.Text = str[3];
            }
            else
            {
                ip_1.Text = "127";
                ip_2.Text = "0";
                ip_3.Text = "0";
                ip_4.Text = "1";
            }

            Port.Text = Main.Instance.Settings.Port.ToString();
        }

        private void SaveSettingBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ip1 = int.Parse(ip_1.Text);
                int ip2 = int.Parse(ip_2.Text);
                int ip3 = int.Parse(ip_3.Text);
                int ip4 = int.Parse(ip_4.Text);
                int port = int.Parse(Port.Text);
                Main.Instance.Settings.Set(String.Format($"{ip1}.{ip2}.{ip3}.{ip4}"), port);

                Main.Instance.Notification.Add("Параметры", "Настройки сохранены", TypeNotification.Message);


            }
            catch
            {
                // MessageBox.Show("IP или Port указаны не верно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                Main.Instance.Notification.Add("Ошибка", "IP или Port указаны не верно", TypeNotification.Error);


            }
        }
    }
}
