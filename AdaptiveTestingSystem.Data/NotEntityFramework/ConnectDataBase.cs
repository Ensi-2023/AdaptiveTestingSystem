#nullable disable

namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public class ConnectDataBase
    {
        private static SqlConnection _dataTableSQLConnection { get; set; }
        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="connectionString">Строка соединения с базой данных</param>
        /// <returns>Возвращает состояние подключения</returns>
        public static async Task<bool> Connect(string connectionString)
        {
            try
            {
                _dataTableSQLConnection = new SqlConnection(connectionString);
                await _dataTableSQLConnection.OpenAsync();
                //Потом сделаю проверку бд
                return true;
            }
            catch (Exception ex)
            {
             
                Logger.Error($"Ошибка подключения к базе данных. {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Подключение к базе данных. Строка подключения будет браться из файла confing.ini
        /// </summary>
        /// <returns>Возвращает состояние подключения</returns>

        public static async Task<bool> Connect()
        {
            try
            {
                _dataTableSQLConnection = new SqlConnection(DBSettings.ConnectionString);
                await _dataTableSQLConnection.OpenAsync();
                //Потом сделаю проверку бд
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка подключения к базе данных. {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Получить объект подключения к базе данных
        /// </summary>
        /// <returns>Объект подключения</returns>
        public static SqlConnection Get()
        {
            return _dataTableSQLConnection;
        }

        /// <summary>
        /// Завершить подклчюение
        /// </summary>
        /// <returns></returns>
        public static async Task Disconnect()
        {
            await Task.Delay(250);
            await _dataTableSQLConnection.DisposeAsync();
        }
    }
}
