using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript
{
    public class DBSettingScripting
    {
        public static async Task<bool> CheckConnectCreateBase(string server, string dbase)
        {

            Main.Instance.OverlayAndConsoleSetMessage("Начинаю проверку подключения к стандартной базе данных " + String.Format(" Server:{0} Base {1}", server, dbase));
            try
            {
                SqlConnection connection = new SqlConnection();
                string connectionString = @"Data Source=" + server + ";Initial Catalog=" + dbase + ";Integrated Security=True;MultipleActiveResultSets=True";
                connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                Logger.Error($"DBSettingScripting.CheckConnectCreateBase вызвало ошибку: {ex.Message}");

                return false;
            }

        }


        public static async Task<bool> CreateBase(string server, string defaultbase, string newBase)
        {


            Main.Instance.OverlayAndConsoleSetMessage(String.Format("Начинаю создание новой базы данных"));
            await Task.Delay(200);

            try
            {
                Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю новое подключение"));
                await Task.Delay(200);
                SqlConnection connection = new SqlConnection();
                string connectionString = @"Data Source=" + server + ";Initial Catalog=" + defaultbase + ";Integrated Security=True;MultipleActiveResultSets=True";
                Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю строку подключения.."));
                connection = new SqlConnection(connectionString);
                Main.Instance.OverlayAndConsoleSetMessage(String.Format("Подключаюсь..."));
                await Task.Delay(200);
                await connection.OpenAsync();

                await Task.Delay(400);
                SqlTransaction transaction;
                SqlCommand command;

                try
                {


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю базу данных '{0}'", newBase));
                    await Task.Delay(200);
                    command = new SqlCommand(CreateDataBase.GetSql_NewDataBase(newBase), connection);
                    await command.ExecuteNonQueryAsync();



                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Переподключаюсь к базе данных"));
                    await Task.Delay(200);
                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Отключаюсь от '{0}'", defaultbase));
                    await Task.Delay(200);
                    connection.Close();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Подключаюсь к '{0}'", newBase));
                    await Task.Delay(200);
                    connectionString = @"Data Source=" + server + ";Initial Catalog=" + newBase + ";Integrated Security=True;MultipleActiveResultSets=True";
                    connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю новую транзакцию"));
                    await Task.Delay(200);
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Продолжаю создание базы данных '{0}'", newBase));

                    await Task.Delay(200);


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Journal'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Journal();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Login'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Login();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Position'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Position();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Roly'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Roly();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'User'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_UserTable();
                    await command.ExecuteNonQueryAsync();



                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Klass'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Klass();
                    await command.ExecuteNonQueryAsync();



                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Klass_User'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Klass_User();
                    await command.ExecuteNonQueryAsync();



                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Predmet'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Predmet();
                    await command.ExecuteNonQueryAsync();


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Predmet_User'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Predmet_User();
                    await command.ExecuteNonQueryAsync();


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Answer'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Answer();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Questions'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Questions();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Testing'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Testing();
                    await command.ExecuteNonQueryAsync();


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю таблицу 'Resultation'"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Table_Resultation();
                    await command.ExecuteNonQueryAsync();


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Применяю настройки таблиц"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_AlterTable();
                    await command.ExecuteNonQueryAsync();


                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создаю системного администратора"));
                    await Task.Delay(200);
                    command.CommandText = CreateDataBase.GetSql_Insert_Admin();
                    await command.ExecuteNonQueryAsync();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Завершаю транзакцию"));
                    await Task.Delay(200);
                    transaction.Commit();

                    Main.Instance.OverlayAndConsoleSetMessage(String.Format("Создание новой базы данных успешно завершено"));
                    await Task.Delay(450);
                    connection.Close();
                    return true;

                }
                catch (SqlException ex) when (ex.Number == 1801)
                {
                    MessageBox.Show(String.Format("Ошибка #{0}: {1}", ex.Number, ex.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                

                    Logger.Error($"DBSettingScripting.CreateBase вызвало ошибку в строке #{ex.LineNumber} №{ex.Number}: {ex.Message}");
                    return false;
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(String.Format("Ошибка #{0}: {1}", ex.Number, ex.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.Error($"DBSettingScripting.CreateBase вызвало ошибку в строке #{ex.LineNumber} №{ex.Number}: {ex.Message}");
                    Logger.Log(String.Format("Удаляю базу данных '{0}'", newBase));
                    command = new SqlCommand("DROP DATABASE " + newBase, connection);
                    await command.ExecuteNonQueryAsync();


                    return false;
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error($"DBSettingScripting.CreateBase вызвало ошибку: {ex.Message}");

                return false;

            }
        }

    }
}

