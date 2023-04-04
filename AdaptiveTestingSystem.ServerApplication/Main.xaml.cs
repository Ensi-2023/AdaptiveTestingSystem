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
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.ServerApplication
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {

        public bool RichStatusScroll { get; private set; } = true;


        public static Main Instance = null;

        public APServer Server;
        public AppSettings Settings;
        public Notification Notification;

        public bool IsCheckActiveServer { get; private set; } = false;


        public Main()
        {
            ConsoleContol.HideConsole();
            InitializeComponent();
            Instance = this;
     
            SmallPageManager.Set(ChildenContent);
            Logging.SetupLogBox(this.Log);
            DoIt();
            Logger.Log("Запуск программы");
            Loaded += Main_Loaded;
            Closed += Main_Closed;
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            if(Notification!=null) Notification.Close();
        }

        void DoIt()
        {
            var writer = new StringWriterExt(true); // true = AutoFlush
            writer.Flushed += new StringWriterExt.FlushedEventHandler(writer_Flushed);
            Console.SetOut(writer);
            Console.SetError(writer);
        }

        private void writer_Flushed(object sender, EventArgs args)
        {
            Logging.Send(sender.ToString());
        }


        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            Settings = new AppSettings();
            Settings.LoadSetting += Settings_LoadSetting;
            Settings.ErrorLoadSetting += Settings_ErrorLoadSetting;
            Settings.Load();

            Notification = new Notification();
   


            LoadingPopupTextBox();
        }

        private void LoadingPopupTextBox()
        {
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/help", Description = " Справка" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/start", Description = " [IP Адрес] [Port] - запуск основного сервера" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/stop", Description = " Остановка сервера" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/restart", Description = " Перезапуск сервера" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/socket", Description = " Справка" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/socket view", Description = " Активные сокеты" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/socket port", Description = " Установить порт для сокета" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/socket delete", Description = " Установить порт для сокета" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/ban", Description = " [login] - Забанить аккаунт" });
            ConsoleBox.Items.Add(new PopupItemControl() { Caption = "/unban", Description = " [login] - Разбанить аккаунт" });
        }

        public void OverlayShowing(bool show)
        {
            OverlayMessage.Text = "Подождите...";
            if (show) Overlay.Visibility = Visibility.Visible; else { Overlay.Visibility = Visibility.Collapsed; }
            IsEnabled = !show;
        }

        public void OverlayAndConsoleSetMessage(string message)
        {
            OverlayMessage.Text = message;;
            Logger.Log(message);
        }


        private void Settings_ErrorLoadSetting()
        {
            Logger.Log("Настройки не были загружены. Проверьте файл config.ini");
        }

        private void Settings_LoadSetting(IPAddress ip, int port, bool onAutoConnect, bool onAutoSaveLog)
        {
            Logger.Log("Настройки загружены");
            Server = new APServer(Settings);
            Server.StartingToConnect += Server_StartingToConnect;
            Server.ServerRunning += Server_ServerRunning;

            if (onAutoConnect)
            {
                ConsoleScript.ConsoleCommandParser("/start", Server, false);
            }
        }

        private void Server_ServerRunning()
        {
            ConsoleBox.IsEnabled = true;
            ButtonCommand.IsEnabled = true;
            OverlayShowing(false);

            ServerOnButton.IsEnabled = false;
            ServerOffButton.IsEnabled = true;
            ServerRebootingButton.IsEnabled = true;
            ServerClientViewButton.IsEnabled = true;
            MainMeny.IsEnabled = true;


            Title = String.Format($"Сервер запущен: {Server.Settings.IP}:{Server.Settings.Port}");
        }

        private void Server_StartingToConnect()
        {
            Title = String.Format($"Запуск сервера");

            ConsoleBox.IsEnabled = false;
            ButtonCommand.IsEnabled = false;
            OverlayShowing(true);
            MainMeny.IsEnabled = false;
        }

        private void ServerOnButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleScript.ConsoleCommandParser("/start", Server, false);
        }

        private void ServerOffButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Остановить сервер?", "Остановка сервера", MessageShow.Type.Question)==true)
            {
                ConsoleScript.ConsoleCommandParser("/stop", Server, false);
            }
   
        }


        public void OFFServer()
        {
            ConsoleBox.IsEnabled = true;
            ButtonCommand.IsEnabled = true;

            OverlayShowing(false);
            ServerOnButton.IsEnabled = true;
            ServerOffButton.IsEnabled = false;

            ServerRebootingButton.IsEnabled = false;
            ServerClientViewButton.IsEnabled = false;

            MainMeny.IsEnabled = true;
            
            Title = String.Format($"Сервер остановлен");
        }

        private  void ServerRebootingButton_Click(object sender, RoutedEventArgs e)
        {
            //OverlayShowing(true);
            //await Task.Delay(1000);
            //if (Server.IsRunning) ConsoleScript.ConsoleCommandParser("/restart", Server, false);
            //else ConsoleScript.ConsoleCommandParser("/start", Server, false);

            if (MessageShow.Show("Перезагрузить сервер?", "Перезагрузка", MessageShow.Type.Question) == true)
            {
                ConsoleScript.ConsoleCommandParser("/restart", Server, false);
            }

        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SmallPageManager.Next(new Assets.GUI.GUI_Setting.GUI_Main());
        }

        private void FormMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void FormMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal) State(WindowState.Maximized, true); else State(WindowState.Normal, false);
        }

        private void State(WindowState state, bool maxmin)
        {
            WindowState = state;
            if (maxmin) root.Margin = new Thickness(0, 0, 0, 0); else root.Margin = new Thickness(10);
        }

        private void FormClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OverlayMeny_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MainMenyViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MainMeny_Click(object sender, RoutedEventArgs e)
        {
            MainMenyViewer.Visibility = Visibility.Visible;
            Animation.AnimatedOpacity(OverlayMeny, 0, .4, TimeSpan.FromMilliseconds(260));
            Animation.AnimatedWidth(MainMenyViewerContent, 0, 260, TimeSpan.FromMilliseconds(260));
        }

        private async void MenyClosed_Click(object sender, RoutedEventArgs e)
        {
            await StartCloseMeny();
        }

        private async Task StartCloseMeny()
        {
            double duration = 260;
            Animation.AnimatedOpacity(OverlayMeny, .4, 0, TimeSpan.FromMilliseconds(duration));
            Animation.AnimatedWidth(MainMenyViewerContent, 260, 0, TimeSpan.FromMilliseconds(duration));
            await Task.Delay((int)duration);
            MainMenyViewer.Visibility = Visibility.Collapsed;
        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!RichStatusScroll) return;
            var obj = (RichTextBox)sender;
            obj.ScrollToEnd();
            if (obj.Document.Blocks.Count > 500)
            {
                TextRange textRange = new TextRange(obj.Document.ContentStart, obj.Document.ContentEnd);
                Logging.SendLogNotAutoSave(textRange.Text);
                obj.Document.Blocks.Clear();
            }
        }
        private void Log_MouseEnter(object sender, MouseEventArgs e)
        {
            RichStatusScroll = false;
        }
        private void Log_MouseLeave(object sender, MouseEventArgs e)
        {

            RichStatusScroll = true;
        }

        private void ConsoleBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ComboTextBox obj = sender as ComboTextBox;
                if (obj == null || obj.Text.Trim().Length < 2) return;
                if (ConsoleScript.ConsoleCommandParser(obj.Text, Server, obj.IsOpen)) obj.Clear();
            }
        }

       
    }
}
