#nullable disable
namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public class DBSettings
    {
        public static string DBServer { get; private set; }
        public static string DBase { get; private set; }
        public static string ConnectionString => $"Data Source = {DBServer}; Initial Catalog = {DBase}; {connectionParametrs}";
        private static string connectionParametrs = "";
        private static bool isError = false;
        private readonly static IniFile settingFile = new ($"config\\configdb.ini");

        public DBSettings()
        {
            Directory.CreateDirectory("config");
            Load();
        }
        public static void Set(string dbname, string dbserver ,string commandParametrs="Integrated Security = True; MultipleActiveResultSets=True",bool logging=false )
        {
            DBServer = dbserver;
            DBase = dbname;
            connectionParametrs = commandParametrs;
            settingFile.Write("Server", "ServerDB", dbserver);
            settingFile.Write("Server", "DBase", dbname);
            settingFile.Write("Server", "Parametrs", commandParametrs);
            if (logging)
            {
                Logger.Message($"Настроки сохрарены ServerDB:{dbserver}");
                Logger.Message($"Настроки сохрарены DBase:{dbname}");
                Logger.Message($"Настроки сохрарены Parametrs:{commandParametrs}");
            }
        }

        public static void Load()
        {
            isError = false;
            Logger.Log("Загружаю настройки базы данных");


            
            DBServer = settingFile.ReadINI("Server", "ServerDB");
            if (!CheckLoad(DBServer))
            {
                Logger.Error("DBSettings.Load.DBServer: Ошибка загрузки. Проверьте настройки");
                isError = true; 
            }
            else
                Logger.Message($"DBServer: loaded ({DBServer})");

            DBase = settingFile.ReadINI("Server", "DBase");

            if (!CheckLoad(DBase))
            {
                Logger.Error("DBSettings.Load.DBase: Ошибка загрузки. Проверьте настройки");
                isError = true;
            }
            else
                Logger.Message($"DBase: loaded ({DBase})");

            connectionParametrs = settingFile.ReadINI("Server", "Parametrs");


            if (!isError)
            {
                if (CheckLoad(connectionParametrs))
                    Set(DBase, DBServer, connectionParametrs);
                else
                    Set(DBase, DBServer);
            }
            else
                throw new Exception("Ошибка загрузки настроек базы данных");
    
        }

        /// <summary>
        /// Возвращает Server и DataBase
        /// </summary>
        /// <returns>mas[0] - Server mas[1] - DataBase</returns>
        public static string[] Get()
        {
            return new string[2] { DBServer, DBase };
        }

        private static bool CheckLoad(string value)
        {
            return value.Trim().Length > 0;
        }
    }
}
