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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI
{
    /// <summary>
    /// Логика взаимодействия для GUI_Loading.xaml
    /// </summary>
    public partial class GUI_Loading : UserControl
    {


        public GUI_Loading()
        {
            InitializeComponent();

            if (_Main.Instance.Settings != null)
            {
                _Main.Instance.Settings.SettingLoaded -= Settings_SettingLoaded;
                _Main.Instance.Settings.ErrorLoadSetting -= Settings_ErrorLoadSetting;
            }


            _Main.Instance.Settings = new AppSettings();
            _Main.Instance.Settings.SettingLoaded += Settings_SettingLoaded;
            _Main.Instance.Settings.ErrorLoadSetting += Settings_ErrorLoadSetting;
        }

        private void Settings_ErrorLoadSetting()
        {
            _Main.Instance.Theme.Set(_Main.Instance.Settings.ThemeName);
            _Main.Instance.ShowWindow();
            _Main.Instance._Notification.Add("Загрузка", $"Некорректная связка IP и Port", TypeNotification.Error);
            _Main.Instance.SetSingeltonChilden(new GUI.GUI_StartupEditSetting());

       
        }

        private void Settings_SettingLoaded()
        {
            _Main.Instance.Theme.Set(_Main.Instance.Settings.ThemeName);
            _Main.Instance.ShowWindow();

            int reconnect = 0;

            if (_Main.Instance.Client != null) reconnect = _Main.Instance.Client.ReconnectCount;

            _Main.Instance.Client = new Client.InternetClient(_Main.Instance.Settings.IP, _Main.Instance.Settings.Port, reconnect);
            _Main.Instance.Client.Start();

            Logger.Log("Настройки загружены");


        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(250);
            _Main.Instance.Settings.Load();
        }
    }
}
