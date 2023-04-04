#nullable disable
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для GUI_Page_DataBaseSetting.xaml
    /// </summary>
    public partial class GUI_Page_DataBaseSetting : UserControl
    {
        public GUI_Page_DataBaseSetting()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            DServer.Text = DBSettings.Get()[0];
            DBase.Text = DBSettings.Get()[1];
            DServerCreate.Text =DBSettings.Get()[0];

            if (Main.Instance.Server != null && Main.Instance.Server.IsRunning)
            {
                SettingServerBase.IsEnabled = false;
                StatusServer.Visibility = Visibility.Visible;
                RecoveryAndCopyBase.Visibility = Visibility.Visible;
                CreateBasePanel.IsEnabled = true;


                DSAdminLogin.IsEnabled = true;
                DBBassword.IsEnabled = true;

            }
            else
            {
                EditSysdmin.IsEnabled = false;
                CreateSysdmin.IsEnabled = false;
                DeleteSysdmin.IsEnabled = false;
                StatusServer_2.Visibility = Visibility.Visible;

                DSAdminLogin.IsEnabled = false;
                DBBassword.IsEnabled = false;

            }

            if (DBSettings.Get()[0].Trim().Length == 0)
            {
                ReservCopir.Visibility = Visibility.Collapsed;
                RecoveryBase.Visibility = Visibility.Collapsed;
                Border_1.Visibility = Visibility.Collapsed;
                Border_2.Visibility = Visibility.Collapsed;
                Border_3.Visibility = Visibility.Collapsed;
                Border_4.Visibility = Visibility.Collapsed;
            }
        }

        private async void ConnectView_Click(object sender, RoutedEventArgs e)
        {
            string dbase = DBaseDefault.Text;
            string server = DServerCreate.Text;
            string error = "";


            if (server.Trim().Length == 0)
            {
                error += "Сервер не указан";
            }

            if (dbase.Trim().Length == 0)
            {
                error += "\nБаза данных не указана";
            }

            if (error.Trim().Length > 0)
            {
                // MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                Main.Instance.Notification.Add("Ошибка", error, TypeNotification.Error);

                return;
            }


            CreateSettingBase.IsEnabled = false;
            Main.Instance.OverlayShowing(true);


            if (await DBSettingScripting.CheckConnectCreateBase(server, dbase))
            {


                CreateSettingBase.IsEnabled = true;
                DBaseDefault.IsEnabled = false;
                DServerCreate.IsEnabled = false;
                ConnectView.IsEnabled = false;
                //MessageBox.Show("Подключение успешно", "Проверка подключения", MessageBoxButton.OK, MessageBoxImage.Information);

                Main.Instance.Notification.Add("Проверка подключения", "Подключение к серверу успешно", TypeNotification.Message);


                DBaseCreate.Focus();
            }
            else
            {

                CreateSettingBase.IsEnabled = true;
            }

            Main.Instance.OverlayShowing(false);

        }

        private void SaveSettingBase_Click(object sender, RoutedEventArgs e)
        {
            string dserv = DServer.Text.Trim();
            string dbase = DBase.Text.Trim();
            string error = "";

            if (dserv.Length == 0)
            {
                error += "Поле сервер базы данных не заполнено!\n";
            }

            if (dbase.Length == 0)
            {
                error += "Поле база данных не заполнено!\n";
            }

            if (error.Trim().Length > 0)
            {
                // MessageBox.Show("Ошибка\n" + error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                Main.Instance.Notification.Add("Ошибка", error, TypeNotification.Error);


                return;
            }


            DBSettings.Set(dbase, dserv);
            Main.Instance.Notification.Add("Сохранение параметров", "Параметры успешно сохранены", TypeNotification.Message);


            //MessageBox.Show("Успешно!", "Сохранение параметров", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void CreateSettingBase_Click(object sender, RoutedEventArgs e)
        {
            string newBase = DBaseCreate.Text;

            if (newBase.Trim().Length == 0)
            {
                // MessageBox.Show("База данных не может быть пустой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                Main.Instance.Notification.Add("Ошибка", "База данных не может быть пустой!", TypeNotification.Error);


                DBaseCreate.Focus();
                return;
            }


            if (MessageBox.Show("Создать новую базу данных?", "Создание новой базы", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Main.Instance.OverlayShowing(true);

                CreateSettingBase.IsEnabled = false;

                if (await DBSettingScripting.CreateBase(DServerCreate.Text, DBaseDefault.Text, newBase))
                {

                    Main.Instance.OverlayShowing(false);
                    CreateSettingBase.IsEnabled = false;
                    ConnectView.IsEnabled = true;
                    Main.Instance.Notification.Add("БД", $"База данных {newBase} создана ",TypeNotification.Message);


                    if (MessageBox.Show("Создание базы данных успешно завершено. Подключится к новой бд?", "Создание базы данных", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        DServer.Text = DServerCreate.Text;
                        DBase.Text = newBase;

                        SaveSettingBase_Click(null, null);
                        SmallPageManager.Close();
                        DBSettings.Set(DBase.Text, DServer.Text);
                        Main.Instance.Server.Restart();
                        return;

                    }

                }


                CreateSettingBase.IsEnabled = true;
                Main.Instance.OverlayShowing(false);
            }

        }

        private void Visible(Button obj, StackPanel stack, int indexCommand)
        {

            SaveSysdmin.Visibility = Visibility.Collapsed;
            AddSysdmin.Visibility = Visibility.Collapsed;
            DelSysdmin.Visibility = Visibility.Collapsed;
            EditPanel.Visibility = Visibility.Collapsed;
            obj.Visibility = Visibility.Visible;
            CancelEdit.Visibility = Visibility.Visible;


            PasswordView.Visibility = Visibility.Visible;
            LoginViewer.Visibility = Visibility.Visible;

            if (stack != null)
            {
                stack.Visibility = Visibility.Visible;

                if (indexCommand == 1) { PasswordTitle.Content = "Старый пароль"; PasswordNewTitle.Content = "Новый пароль"; }
                if (indexCommand == 2) { PasswordTitle.Content = "Пароль"; PasswordNewTitle.Content = "Повторите пароль"; }

            }
        }

        private void EditSysdmin_Click(object sender, RoutedEventArgs e)
        {
            Visible(SaveSysdmin, EditPanel, 1);
        }

        private void CreateSysdmin_Click(object sender, RoutedEventArgs e)
        {
            Visible(AddSysdmin, EditPanel, 2);
        }

        private void DeleteSysdmin_Click(object sender, RoutedEventArgs e)
        {
            Visible(DelSysdmin, null, 3);
        }

        private void CancelEditingAdmin_Click(object sender, RoutedEventArgs e)
        {

            PasswordTitle.Content = "Пароль";
            SaveSysdmin.Visibility = Visibility.Collapsed;
            AddSysdmin.Visibility = Visibility.Collapsed;
            DelSysdmin.Visibility = Visibility.Collapsed;
            EditPanel.Visibility = Visibility.Collapsed;
            CancelEdit.Visibility = Visibility.Collapsed;

        }

        private async void DelSysdmin_Click(object sender, RoutedEventArgs e)
        {
            string login = DSAdminLogin.Text.Trim();
            string password = DBBassword.Password.Trim();

            string error = "";


            if (login.Length == 0)
            {
                error += "Поле логин обязательно к заполненю";
            }

            if (password.Length == 0)
            {
                error += "\nПоле пароль обязательно к заполненю";
            }


            if (await DBSearchMethods.VertySysAdminPassword(DBBassword.Password, DSAdminLogin.Text) == false)
            {
                error += "Старый пароль введён не верно!\n";
            }

            if (error.Trim().Length > 0)
            {
                Main.Instance.Notification.Add("БД", $"Ошибка: {error}", TypeNotification.Error);
                return;
            }

            if (MessageBox.Show("Удалить системного администратора?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {

                if (await DBSearchMethods.DeleteSysAdmin(DSAdminLogin.Text, DBBassword.Password))
                {

                    Main.Instance.Notification.Add("БД", $"Удаление системного адмитистратора прошло успешно!", TypeNotification.Message);
                    DSAdminLogin.Clear();
                    DBBassword.Clear();
                    NewDBBassword.Clear();
                    CancelEditingAdmin_Click(null, null);
                }
                else
                {
                    Main.Instance.Notification.Add("БД", $"По какой то причине удаление не удалось, смотрите лог", TypeNotification.Error);
                    DBBassword.Clear();
                    NewDBBassword.Clear();
                    DSAdminLogin.Clear();
                }
            }
        }

        private async void SaveSysdmin_Click(object sender, RoutedEventArgs e)
        {
            string login = DSAdminLogin.Text.Trim();
            string password = DBBassword.Password.Trim();
            string vertyPassword = NewDBBassword.Password.Trim();
            string error = "";


            if (login.Length == 0)
            {
                error += "Поле логин обязательно к заполненю\n";
            }

            if (password.Length == 0)
            {
                error += "Поле пароль обязательно к заполненю\n";
            }


            if (vertyPassword.Length == 0)
            {
                error += "Поле проверка пароля обязательно к заполненю\n";
            }


            if (await DBSearchMethods.VertySysAdminPassword(DBBassword.Password, DSAdminLogin.Text) == false)
            {
                error += "Старый пароль введён не верно!\n";
            }

            if (error.Trim().Length > 0)
            {
                Main.Instance.Notification.Add("БД", $"Ошибка: {error}", TypeNotification.Error);
                return;
            }


            if (MessageBox.Show("Изменить пароль системного администратора?", "Изменение пароля", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {

                if (await DBDataMethod.EditPasswodSysAdmin(DSAdminLogin.Text, vertyPassword))
                {
                    Main.Instance.Notification.Add("БД", $"Изменение пароля системного адмитистратора прошло успешно!",TypeNotification.Message);
                    DSAdminLogin.Clear();
                    DBBassword.Clear();
                    NewDBBassword.Clear();
                    CancelEditingAdmin_Click(null, null);
                }
                else
                {
                    Main.Instance.Notification.Add("БД", $"По какой то причине изменение не удалось, смотрите лог", TypeNotification.Error);
                    DBBassword.Clear();
                    NewDBBassword.Clear();
                }
            }

        }

        private async void AddSysdmin_Click(object sender, RoutedEventArgs e)
        {
            string login = DSAdminLogin.Text.Trim();
            string password = DBBassword.Password.Trim();
            string vertyPassword = NewDBBassword.Password.Trim();
            string error = "";


            if (login.Length == 0)
            {
                error += "Поле логин обязательно к заполненю\n";
            }

            if (password.Length == 0)
            {
                error += "Поле пароль обязательно к заполненю\n";
            }


            if (vertyPassword.Length == 0)
            {
                error += "Поле проверка пароля обязательно к заполненю\n";
            }


            if (vertyPassword != password)
            {
                error += "Пароли не совпадают!\n";
            }

            if (error.Trim().Length > 0)
            {
                Main.Instance.Notification.Add("БД", $"Ошибка: {error}", TypeNotification.Error);
                return;
            }


            if (MessageBox.Show("Добавить системного администратора?", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                try
                {

                    if (await DBAddingMethod.ADDSysAdmin(DSAdminLogin.Text, DBBassword.Password))
                    {

                        Main.Instance.Notification.Add("БД", $"Добавление системного администратора прошло успешно!", TypeNotification.Message);

                        DSAdminLogin.Clear();
                        DBBassword.Clear();
                        NewDBBassword.Clear();
                        CancelEditingAdmin_Click(null, null);
                    }
                    else
                    {
                        Main.Instance.Notification.Add("БД", $"По какой то причине добавление не удалось, смотрите лог", TypeNotification.Error);
                        DBBassword.Clear();
                        NewDBBassword.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Main.Instance.Notification.Add("БД", $"{ex.Message}", TypeNotification.Error);

                }
            }
        }

        private void FolderOpen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                FileFolder.Text = dialog.SelectedPath;
            }

        }

        private void FolderOpen_recovery_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Выбор резервной копии";
            openFileDialog.Filter = "Файлы резервной копии|*.bak";

            if (openFileDialog.ShowDialog() == true)
            {

                FileFolder_recovery.Text = openFileDialog.FileName;

            }
        }

        private async void CopyStart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Начать резервное копирование?\n Основной сервер будет отключен", "База данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await StartCopyDataBase();
            }

        }

        public string File { get; private set; }
       
        private async Task StartCopyDataBase()
        {
            SqlConnection Connection = null;
            try
            {

                Logger.Log($"Начинаю резервное копирование базы данных: {DBSettings.DBase}");
                Logger.Log($"Проверяю запущен ли сервер");
   
                if (Main.Instance.Server != null && Main.Instance.Server.IsRunning)
                {
                   
                    Logger.Log($"Останавливаю сервер");

                    Main.Instance.Server.Stop();
                    ConsoleScript.ConsoleCommandParser("/stop", Main.Instance.Server, false);
                }
                else Logger.Log("Сервер не запущен, продолжаю резервное копирование");

                while (Main.Instance.Server.IsRunning)
                {
                    await Task.Delay(150);
                }


                Main.Instance.OverlayShowing(true);
                Logger.Log("Создаю файл резервной копии");
                File = String.Format("{2}\\backup_{0}_{1}.bak", DateTime.Now.ToShortDateString().Replace('.', '_'), DateTime.Now.ToLongTimeString().Replace(':', '_'), FileFolder.Text);
                Logger.Log("Файл создан: " + File);





                Logger.Log("Запускаю новое подключение к базе данных 'master'");

                string connectionString = @"Data Source=" + DBSettings.DBServer + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True";
                Connection = new SqlConnection(connectionString);
                await Connection.OpenAsync();
                Logger.Log("Подключение активно");

                Logger.Log("Запускаю резерное копирование базы данных: " + DBSettings.DBase);
                string sql = String.Format("BACKUP DATABASE {0} TO disk='{1}'", DBSettings.DBase, File);
                SqlCommand command = new SqlCommand(sql, Connection);
                await command.ExecuteNonQueryAsync();
                Logger.Message("Успешно");


                if (MessageBox.Show(String.Format("Резервное копирование успешно выполнено!\nПерезапустить сервер?"), "Резервное копирование", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Logger.Log("Запускаю сервер");

                    RestartServer();


                }


            }
            catch (SqlException sqlEx)
            {
                Logger.Error("Ошибка резервного копирования: " + String.Format("Ошибка #{0}: {1}", sqlEx.Number, sqlEx.Message));
                Main.Instance.Notification.Add("БД", $"{sqlEx.Message}", TypeNotification.Error);
            }
            finally
            {
                Connection.Close();
                Main.Instance.OverlayShowing(false);
            }
        }

        private void RestartServer()
        {

            if (Main.Instance.Server != null && (Main.Instance.Server.IsRunning))
            {
                ConsoleScript.ConsoleCommandParser("/restart", Main.Instance.Server, false);
            }
            else
            {
                ConsoleScript.ConsoleCommandParser("/start", Main.Instance.Server, false);
            }


        }

        private async void Recoverytart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Начать восстановление базы данных?\n Основной сервер будет отключен", "Восстановление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await StartRecoveryDataBase();
            }
        }

        private async Task StartRecoveryDataBase()
        {
            SqlConnection Connection = null;
            try
            {
                Logger.Log("Начинаю резервное восстановление базы данных: " + DBSettings.DBase);
                Logger.Log("Проверяю запущен ли сервер");
                if (Main.Instance.Server != null && Main.Instance.Server.IsRunning)
                {
                    Logger.Log("Останавливаю сервер");
                    ConsoleScript.ConsoleCommandParser("/stop", Main.Instance.Server, false);
                }
                else Logger.Log("Сервер не запущен, продолжаю резервное копирование");

                while (Main.Instance.Server.IsRunning)
                {
                    await Task.Delay(150);
                }

                Main.Instance.OverlayShowing(true);
                Logger.Log("Запускаю новое подключение к базе данных 'master'");

                string connectionString = @"Data Source=" + DBSettings.DBServer + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True";
                Connection = new SqlConnection(connectionString);
                await Connection.OpenAsync();
                Logger.Log("Подключение активно");

                Logger.Log("Отключаю всех от базы данных: " + DBSettings.DBase);
                string sql = String.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", DBSettings.DBase);
                SqlCommand command2 = new SqlCommand(sql, Connection);
                await command2.ExecuteNonQueryAsync();



                Logger.Log("Запускаю восстановление базы данных: " + DBSettings.DBase);
                Logger.Log("Файл резервной копии: " + FileFolder_recovery.Text);
                sql = String.Format("RESTORE DATABASE {0} FROM DISK ='{1}' WITH REPLACE", DBSettings.DBase, FileFolder_recovery.Text);
                SqlCommand command = new SqlCommand(sql, Connection);
                await command.ExecuteNonQueryAsync();
                Logger.Message("Успешно");



                if (MessageBox.Show(String.Format("Резервное восстановление успешно выполнено!\nПерезапустить сервер?"), "Резервное восстановление", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Logger.Log("Запускаю сервер");
                    RestartServer();

                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Error("Ошибка резервного восстановления: " + String.Format("Ошибка #{0}: {1}", sqlEx.Number, sqlEx.Message));
                Main.Instance.Notification.Add("БД", $"{sqlEx.Message}", TypeNotification.Error);
            }
            finally
            {
                Connection.Close();
                Main.Instance.OverlayShowing(false);
            }

        }

        public string TSQL { get; private set; }
     
        private void SQLREQUES_Click(object sender, RoutedEventArgs e)
        {
            if (Main.Instance.Server != null && Main.Instance.Server.IsRunning)
            {

                TextRange textRange = new TextRange(SQL.Document.ContentStart, SQL.Document.ContentEnd);

                TSQL = textRange.Text.Trim();
                SQLCreate().GetAwaiter();
            }
            else
            {
                Main.Instance.Notification.Add("БД", $"Сервер отключен", TypeNotification.Error);

            }
        }

        private async Task SQLCreate()
        {


            try
            {
                Main.Instance.OverlayShowing(true);
                DataGrid.Visibility = Visibility.Hidden;


                DataGrid.Visibility = Visibility.Visible;
                // Создаем объект DataAdapter
                Logger.Log("Выполняю SQL запрос: " + TSQL.Trim());
                SqlDataAdapter adapter = new SqlDataAdapter(TSQL.Trim(), ConnectDataBase.Get());
                await Task.Delay(5);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные

                DataGrid.ItemsSource = ds.Tables[0].DefaultView;

                DataGrid.Visibility = Visibility.Visible;
            }
            catch (SqlException sqlEx)
            {

                DataGrid.Visibility = Visibility.Collapsed;
                Main.Instance.Notification.Add("БД", $"{sqlEx.Message}", TypeNotification.Error);
                Logger.Error("Ошибка sql запроса: " + String.Format("Ошибка #{0}: {1}", sqlEx.Number, sqlEx.Message));

            }

            finally
            {
                Main.Instance.OverlayShowing(false);
            }

        }

        private void SQLCLEAR_Click(object sender, RoutedEventArgs e)
        {
            SQL.Document.Blocks.Clear();
        }

    }
}
