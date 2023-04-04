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
    /// Логика взаимодействия для _settings_Page_applicationSetting.xaml
    /// </summary>
    public partial class _settings_Page_serverSetting : UserControl
    {
        public _settings_Page_serverSetting()
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

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            if (_Main.Instance.Settings.IP != null)
            {
                string[] str = _Main.Instance.Settings.IP.ToString().Split('.');

                ip1.Text = str[0];
                ip2.Text = str[1];
                ip3.Text = str[2];
                ip4.Text = str[3];
            }
            else
            {
                ip1.Text = "127";
                ip2.Text = "0";
                ip3.Text = "0";
                ip4.Text = "1";
            }

            Port.Text = _Main.Instance.Settings.Port.ToString();
        }

        private void SaveSettingBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _ip1 = int.Parse(ip1.Text);
                int _ip2 = int.Parse(ip2.Text);
                int _ip3 = int.Parse(ip3.Text);
                int _ip4 = int.Parse(ip4.Text);
                int port = int.Parse(Port.Text);
              
                if (_Main.Instance == null) return;

                _Main.Instance.Settings.Set(String.Format($"{_ip1}.{_ip2}.{_ip3}.{_ip4}"), port);
                _Main.Instance._Notification.Add("Параметры", "Настройки сохранены", TypeNotification.Message);


                _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Настройки программы сохранены", "Параметры", type: TypeNotification.Warning);

                if (MessageShow.Show("Нужен перезапуск приложения для вступления в силу новых настроек. Переазпустить?", "Настройки", MessageShow.Type.Question) == true)
                {
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }
            catch
            {
                // MessageBox.Show("IP или Port указаны не верно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                if (_Main.Instance == null) return;
                _Main.Instance._Notification.Add("Ошибка", "IP или Port указаны не верно", TypeNotification.Error);

                _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"IP или Port указаны не верно", "Параметры", type: TypeNotification.Error);

            }
        }
    }
}
