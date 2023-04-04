#nullable disable

namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public static class CreateDataBase
    {
        private static string _dataBase { get; set; }
        /// <summary>
        /// Создание новой базы данных
        /// </summary>
        /// <param name="dbase">Название новой базы данных</param>
        /// <returns>Возвращает строку ввиде TSQL запроса на создание базы данных </returns>
        public static string GetSql_NewDataBase(string dbase)
        {
            _dataBase = dbase;
            return String.Format("CREATE DATABASE {0}", _dataBase);
        }
        /// <summary>
        /// Создание таблицы журнал
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Journal()
        {
            return
                " CREATE TABLE [dbo].[Journal] " +
                " ( " +
                " [ID_Event] [int] IDENTITY(1,1) NOT NULL, " +
                " [TextEvent] [varchar](max) NOT NULL, " +
                " [DatetimeEvent] [datetime] NOT NULL," +
                " [ID_User] [int] NOT NULL, " +
                " CONSTRAINT [PK_Journal] PRIMARY KEY CLUSTERED ([ID_Event] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// Создание таблицы логины пользователей
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Login()
        {
            return
                " CREATE TABLE [dbo].[LoginUserTable] " +
                " ( " +
                " [ID_User] [int] NOT NULL, " +
                " [Login_User] [varchar](50) NOT NULL, " +
                " [Password_User] [varchar](50) NOT NULL," +
                "  CONSTRAINT [PK_LoginUserTable] PRIMARY KEY CLUSTERED ([ID_User] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// Создание связующей таблицы должности пользователей 
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Position()
        {
            return
                " CREATE TABLE [dbo].[Position] ( " +
                " [ID_Position] [int] IDENTITY(1,1) NOT NULL, " +
                " [ID_User] [int] NOT NULL, " +
                " [ID_Roly] [int] NOT NULL," +
                "  CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED ([ID_Position] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// Создание таблицы списков ролей пользователей
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Roly()
        {
            return
                " CREATE TABLE [dbo].[Roly] ( " +
                " [ID_Roly] [int] IDENTITY(1,1) NOT NULL, " +
                " [NameRoly] [varchar](max) NOT NULL, " +
                " [allRoly] [bit] NOT NULL, " +


                  " [ReadUser] [bit] NOT NULL, " +
                  " [ReadSotrud] [bit] NOT NULL, " +
                  " [ReadClass] [bit] NOT NULL, " +
                  " [ReadPredmet] [bit] NOT NULL, " +
                  " [CreateAndViewReport] [bit] NOT NULL, " +
                  " [TestReady] [bit] NOT NULL, " +
                  " [CreateTest] [bit] NOT NULL, " +
                  " [CreateGroup] [bit] NOT NULL, " +
                  " [ConnectGroup] [bit] NOT NULL, " +

                   " [AddSotrudForPredmet] [bit] NOT NULL, " +
                   " [DeleteSotrudForPredmet] [bit] NOT NULL, " +

                   " [ViewDataUser] [bit] NOT NULL, " +
                   " [ViewDataSotrud] [bit] NOT NULL, " +
                   " [ViewPrepmet] [bit] NOT NULL, " +
                   " [ViewClass] [bit] NOT NULL, " +


                "  CONSTRAINT [PK_Roly] PRIMARY KEY CLUSTERED ([ID_Roly] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// Создание основной таблицы пользователи
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_UserTable()
        {
            return
                " CREATE TABLE [dbo].[UserTable] ( " +
                " [ID_User] [int] IDENTITY(1,1) NOT NULL, " +
                " [SurnameUser] [varchar](max) NOT NULL, " +
                " [LastnameUser] [varchar](max) NOT NULL, " +
                " [MiddlenameUser] [varchar](max) NULL, " +
                " [DatebirchUser] [date] NOT NULL, " +
                " [GenderUser] [nchar](10) NOT NULL," +
                " [RegistrationdatatimeUser] [varchar](max) NOT NULL, " +
                " [Status] [bit] NOT NULL, " +
                "  CONSTRAINT [PK_UserTable] PRIMARY KEY CLUSTERED ([ID_User] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// Создание таблицы классы пользователей
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Klass()
        {

            return
                    "CREATE TABLE [dbo].[Klass] (" +
                    " [ID_Klass] [int] IDENTITY(1,1) NOT NULL, " +
                    " [NameKlass] [varchar](max) NOT NULL, " +
                    " [ID_Sotrud] [int] NULL, " +
                    "  CONSTRAINT [PK_Klass] PRIMARY KEY CLUSTERED ([ID_Klass] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                    " ";
        }

        /// <summary>
        /// Создание связующей таблицы пользователей и классы
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Klass_User()
        {

            return
                " CREATE TABLE [dbo].[Klass_User] ( " +
                " [IDKlassUser] [int] IDENTITY(1,1) NOT NULL, " +
                " [ID_Klass] [int] NOT NULL, " +
                " [ID_User] [int] NOT NULL, " +
                "  CONSTRAINT [PK_Klass_Sotrud] PRIMARY KEY CLUSTERED ([IDKlassUser] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// Создание таблицы предметы
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Predmet()
        {
            return
                " CREATE TABLE [dbo].[Predmet] ( " +
                " [ID_Predmet] [int] IDENTITY(1,1) NOT NULL, " +
                " [NamePredmet] [varchar](max) NOT NULL, " +
                "  CONSTRAINT [PK_Predmet] PRIMARY KEY CLUSTERED ([ID_Predmet] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                " ";
        }
        /// <summary>
        /// Создание связующей таблицы предметы и пользователи
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Predmet_User()
        {
            return
                " CREATE TABLE [dbo].[PredmetSotrud] ( " +
                " [ID_PredmetUser] [int] IDENTITY(1,1) NOT NULL, " +
                " [ID_Sotrud] [int] NOT NULL, " +
                " [ID_Predmet] [int] NOT NULL, " +
                "  CONSTRAINT [PK_PredmetSotrud] PRIMARY KEY CLUSTERED ([ID_PredmetUser] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                "  ";

        }

        /// <summary>
        /// Создание таблицы тестирование
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Testing()
        {
            return
                " CREATE TABLE [dbo].[Testing] ( " +
                " [ID_Testing] [int] IDENTITY(1,1) NOT NULL, " +
                " [NameTesting] [varchar](max) NOT NULL, " +
                " [CountQuest] [int] NOT NULL," +
                " [ID_CrietingSotrud] [int] NOT NULL, " +
                " [ID_Predmet] [int] NOT NULL, " +
                " [DateCrieting] [datetime] NOT NULL, " +
                " [Description] [varchar](max) NULL," +
                "  CONSTRAINT [PK_Testing] PRIMARY KEY CLUSTERED ([ID_Testing] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                "  ";
        }

        /// <summary>
        /// Создание таблицы вопросы
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Answer()
        {
            return
                  " CREATE TABLE [dbo].[Answer]( " +
                  " [ID_Answer] [int] IDENTITY(1,1) NOT NULL, " +
                  " [ID_Questions] [int] NOT NULL, " +
                  " [Answer] [varchar](max) NULL, " +
                  " [AnswerImage] [varchar](max) NULL, " +
                  " [Image_format] VARCHAR(5) NULL, " +
                  " [IsImaging] [bit] NOT NULL, " +
                  " [Number] [int] NOT NULL, " +
                  "  CONSTRAINT [PK_Answer] PRIMARY KEY CLUSTERED ([ID_Answer] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                  " ";
        }

        /// <summary>
        /// Создание таблицы ответы
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Table_Questions()
        {
            return
                   " CREATE TABLE [dbo].[Questions](  " +
                   " [ID_Quest] [int] IDENTITY(1,1) NOT NULL, " +
                   " [Quest] [varchar](max) NULL, " +
                   " [QuestImage] [varchar](max) NULL, " +
                   " [Complexity] [float] NOT NULL," +
                   " [ID_Testing] [int] NOT NULL, " +
                   " [CorrectAnswer] [int] NOT NULL," +
                   " [IsImaging] [bit] NULL," +
                   " [Image_format] VARCHAR(5) NULL, " +
                   "  CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED ([ID_Quest] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                   "  ";
        }


        /// <summary>
        /// Создание таблицы результаты тестов
        /// </summary>
        /// <returns><TSQL - запрос/returns>
        public static string GetSql_Table_Resultation()
        {
            return
                " CREATE TABLE [dbo].[Resultation](  " +
                " [ID_Result] [int] IDENTITY(1,1) NOT NULL, " +
                " [ID_Test] [int] NULL, " +
                " [ID_User] [int] NOT NULL, " +
                " [CoutCorrectAnswer] [int] NOT NULL, " +
                " [CountNotCorectAnswer] [int] NOT NULL," +
                " [CountQuest] [int] NOT NULL," +
                " [Assessment] [float] NOT NULL, " +
                " [DataTest] [datetime] NOT NULL, " +
                " [IsEarly] [bit] NOT NULL, " +
                "  CONSTRAINT [PK_Resultation] PRIMARY KEY CLUSTERED ([ID_Result] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                " ";
        }

        /// <summary>
        /// TSQL - запрос на редактирование данных во всех таблицах
        /// </summary>
        /// <returns>Возвращает TSQL - запрос на добавление первичных ключей во всех таблицах и связывания их с нужными таблицами</returns>
        public static string GetSql_AlterTable()
        {
            return
                   " ALTER TABLE [dbo].[Journal]  WITH NOCHECK ADD  CONSTRAINT [FK_Journal_UserTable] FOREIGN KEY([ID_User]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Journal] NOCHECK CONSTRAINT [FK_Journal_UserTable] " +
                   " ALTER TABLE [dbo].[LoginUserTable]  WITH NOCHECK ADD  CONSTRAINT [FK_LoginUserTable_UserTable] FOREIGN KEY([ID_User]) REFERENCES [dbo].[UserTable] ([ID_User]) ON DELETE CASCADE NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[LoginUserTable] NOCHECK CONSTRAINT [FK_LoginUserTable_UserTable]" +
                   " ALTER TABLE [dbo].[Position]  WITH NOCHECK ADD  CONSTRAINT [FK_Position_Roly] FOREIGN KEY([ID_Roly]) REFERENCES [dbo].[Roly] ([ID_Roly]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Position] NOCHECK CONSTRAINT [FK_Position_Roly] " +
                   " ALTER TABLE [dbo].[Position]  WITH NOCHECK ADD  CONSTRAINT [FK_Position_UserTable] FOREIGN KEY([ID_User]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Position] NOCHECK CONSTRAINT [FK_Position_UserTable] " +
                   " ALTER TABLE [dbo].[Klass]  WITH NOCHECK ADD  CONSTRAINT [FK_Klass_UserTable] FOREIGN KEY([ID_Sotrud]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Klass] NOCHECK CONSTRAINT [FK_Klass_UserTable] " +
                   " ALTER TABLE [dbo].[Klass_User]  WITH NOCHECK ADD  CONSTRAINT [FK_Klass_Sotrud_Klass] FOREIGN KEY([ID_Klass]) REFERENCES [dbo].[Klass] ([ID_Klass]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Klass_User] NOCHECK CONSTRAINT [FK_Klass_Sotrud_Klass] " +
                   " ALTER TABLE [dbo].[Klass_User]  WITH NOCHECK ADD  CONSTRAINT [FK_Klass_Sotrud_UserTable] FOREIGN KEY([ID_User]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Klass_User] NOCHECK CONSTRAINT [FK_Klass_Sotrud_UserTable] " +
                   " ALTER TABLE [dbo].[PredmetSotrud]  WITH NOCHECK ADD  CONSTRAINT [FK_PredmetSotrud_Predmet] FOREIGN KEY([ID_Predmet]) REFERENCES [dbo].[Predmet] ([ID_Predmet]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[PredmetSotrud] NOCHECK CONSTRAINT [FK_PredmetSotrud_Predmet] " +
                   " ALTER TABLE [dbo].[PredmetSotrud]  WITH NOCHECK ADD  CONSTRAINT [FK_PredmetSotrud_UserTable] FOREIGN KEY([ID_Sotrud]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[PredmetSotrud] NOCHECK CONSTRAINT [FK_PredmetSotrud_UserTable]" +

                   " ALTER TABLE [dbo].[Answer]  WITH NOCHECK ADD  CONSTRAINT [FK_Answer_Questions] FOREIGN KEY([ID_Questions]) REFERENCES [dbo].[Questions] ([ID_Quest]) NOT FOR REPLICATION " +
                   " ALTER TABLE [dbo].[Answer] NOCHECK CONSTRAINT [FK_Answer_Questions]" +

                   " ALTER TABLE [dbo].[Questions]  WITH NOCHECK ADD  CONSTRAINT [FK_Questions_Testing] FOREIGN KEY([ID_Testing]) REFERENCES [dbo].[Testing] ([ID_Testing]) NOT FOR REPLICATION " +
                   " ALTER TABLE [dbo].[Questions] NOCHECK CONSTRAINT [FK_Questions_Testing] " +

                   " ALTER TABLE [dbo].[Testing]  WITH NOCHECK ADD  CONSTRAINT [FK_Testing_Predmet] FOREIGN KEY([ID_Predmet]) REFERENCES [dbo].[Predmet] ([ID_Predmet]) NOT FOR REPLICATION " +
                   " ALTER TABLE [dbo].[Testing] NOCHECK CONSTRAINT [FK_Testing_Predmet] " +
                   " ALTER TABLE [dbo].[Testing]  WITH NOCHECK ADD  CONSTRAINT [FK_Testing_UserTable] FOREIGN KEY([ID_CrietingSotrud]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION  " +
                   " ALTER TABLE [dbo].[Testing] NOCHECK CONSTRAINT [FK_Testing_UserTable] " +
                  
                   " ALTER TABLE [dbo].[Resultation]  WITH NOCHECK ADD  CONSTRAINT [FK_Resultation_Testing] FOREIGN KEY([ID_Test]) REFERENCES [dbo].[Testing] ([ID_Testing]) NOT FOR REPLICATION   " +
                   " ALTER TABLE [dbo].[Resultation] NOCHECK CONSTRAINT [FK_Resultation_Testing] " +
                   " ALTER TABLE [dbo].[Resultation]  WITH NOCHECK ADD  CONSTRAINT [FK_Resultation_UserTable] FOREIGN KEY([ID_User]) REFERENCES [dbo].[UserTable] ([ID_User]) NOT FOR REPLICATION " +
                   " ALTER TABLE [dbo].[Resultation] NOCHECK CONSTRAINT [FK_Resultation_UserTable]" +
                   " ";
        }


        /// <summary>
        /// Создание системного администратора
        /// </summary>
        /// <returns>TSQL - запрос</returns>
        public static string GetSql_Insert_Admin()
        {
            return
               string.Format(" insert into UserTable(SurnameUser,LastnameUser,MiddlenameUser,DatebirchUser,GenderUser,RegistrationdatatimeUser,Status) VALUES ('SYS','ADMINISTRATOR','','{0}','Мужской','{0}','true') ", DateTime.Now) +
               string.Format(" insert into LoginUserTable(ID_User,Login_User,Password_User)  VALUES (1,'admin','21232f297a57a5a743894a0e4a801fc3') ") +
                " insert into Roly(NameRoly,allRoly,ReadUser,ReadSotrud,ReadClass,ReadPredmet,CreateAndViewReport,TestReady,CreateTest,CreateGroup,ConnectGroup,AddSotrudForPredmet,DeleteSotrudForPredmet,ViewDataUser,ViewDataSotrud,ViewPrepmet,ViewClass) " +
                " VALUES ('Системный администратор','true','true','true','true','true','true','true','true','true','true','true','true','true','true','true','true') " +
                " insert into Position(ID_User,ID_roly) Values(1,1) " +
                " insert into Roly(NameRoly,allRoly,ReadUser,ReadSotrud,ReadClass,ReadPredmet,CreateAndViewReport,TestReady,CreateTest,CreateGroup,ConnectGroup,AddSotrudForPredmet,DeleteSotrudForPredmet,ViewDataUser,ViewDataSotrud,ViewPrepmet,ViewClass) " +
                " VALUES ('Пользователь','false','false','false','false','false','false','true','false','false','false','false','false','false','false','false','false') " +
                " insert into Roly(NameRoly, allRoly, ReadUser, ReadSotrud, ReadClass, ReadPredmet, CreateAndViewReport, TestReady, CreateTest, CreateGroup, ConnectGroup, AddSotrudForPredmet, DeleteSotrudForPredmet,ViewDataUser,ViewDataSotrud,ViewPrepmet,ViewClass) " +
                " VALUES ('Учитель','false','true','false','true','true','true','true','true','true','true','false','false','true','true','true','true') ";
        }



        public static string GetCheckBase()
        {
            return
                "";
        }
    }
}
