
#nullable disable

using AdaptiveTestingSystem.Data.JsonData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public class DBDataMethod
    {

        /// <summary>
        /// Получить индекс роли
        /// </summary>
        /// <param name="value">название роли</param>
        /// <returns>Возвращает Index Роли</returns>
        public static string GetIndexRoly(string value)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT ID_Roly FROM Roly WHERE NameRoly = '{0}'", value), ConnectDataBase.Get());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return reader.GetValue(0).ToString();
                }

                return "";

            }
            catch (Exception ex)
            {

                Logger.Error($"GetIndexRoly: Проризошла критическая ошибка: {ex.Message}");

                return "";
            }
        }


        /// <summary>
        /// Выполняет заданный запрос
        /// </summary>
        /// <param name="sql">пользовательский запрос</param>
        /// <returns>True если запрос выполнился</returns>
        public static async Task<bool> SQLCommandNoTransaction(string sql)
        {
            try
            {              
                SqlCommand command = new SqlCommand(sql, ConnectDataBase.Get());
                command.CommandText = String.Format(sql);

         
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"SQLCommandNoTransaction: Проризошла критическая ошибка: {ex.Message}");
                return false;
            }
        }


        public static async Task<string> SQLCommandReturnFirstStringNoTransaction(string sql)
        {
            try
            {
                SqlCommand command = new SqlCommand(sql, ConnectDataBase.Get());
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        string first = reader.GetValue(0).ToString();
                        reader.Close();
                        return first;
                    }
                }
                else
                {
                    reader.Close();
                    return "";
                }
                reader.Close();
                return "";
            }
            catch (Exception exx) 
            {
   
                Logger.Error($"SQLCommandReturnFirstStringNoTransaction: Проризошла критическая ошибка: {exx.Message}");
                return "false"; 
            }
        }


        /// <summary>
        /// Выполняет поиск записей в таблице. Если записи найдены вернет TRUE
        /// </summary>
        /// <param name="sql">Любой запрос select</param>
        /// <returns>TRUE если записи найдены</returns>
        public static async Task<bool> SQLCommandCheckTablaHasRow(string sql)
        {
            try
            {
                SqlCommand command = new SqlCommand(sql, ConnectDataBase.Get());
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) return true; else return false;
                }
            }
            catch (Exception exx)
            {

                Logger.Error($"SQLCommandCheckTablaHasRow: Проризошла критическая ошибка: {exx.Message}");
                return false;
            }
        }

        /// <summary>
        /// Выполняет заданный вопрос и возвращает какой то результат.
        /// </summary>
        /// <param name="sql">пользовательский запрос</param>
        /// <returns>результат выполнения запроса | false - если запрос не выполнился</returns>
        public static async Task<string> SQLCommandReturnIDNoTransaction(string sql)
        {
            try
            {
                using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    command.CommandText = String.Format(sql);
                    var result = await command.ExecuteScalarAsync();
                    return result == null ? "false" : result.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"SQLCommandReturnIDNoTransaction: Проризошла критическая ошибка. Ошибка:\n{ex.Message}");
                return "false";
            }
        }



        /// <summary>
        /// Получает полную информацию о пользователе в базе данных и возвращает ее в виде объекта
        /// </summary>
        /// <param name="login">логин пользователя для поиска</param>
        /// <returns>Список пользователей</returns>
        public static async Task<Data_Access> GetAccessUser(string login)
        {
            var listAccess = new Data_Access();

            string sql = String.Format($"Select ID_User from LoginUserTable where Login_User = '{login}'");
            SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());
            SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
            if (dataReader.HasRows)
            {

                while (await dataReader.ReadAsync())
                {
                    var index = dataReader.GetValue(0).ToString();
                    var userSql = new SqlCommand($"select * from UserTable where ID_User = '{index}'", ConnectDataBase.Get());
                    var userDataReader = await userSql.ExecuteReaderAsync();
                    if (userDataReader.HasRows)
                    {
                        while (await userDataReader.ReadAsync())
                        {
                            var surname = userDataReader.GetValue(1).ToString();
                            var firstname = userDataReader.GetValue(2).ToString();
                            var middlemane = userDataReader.GetValue(3).ToString();
                            var datebirch = userDataReader.GetValue(4).ToString();
                            var gender = userDataReader.GetValue(5).ToString();
                            var status = ParserVariables.GetBoolean(userDataReader.GetValue(7).ToString());
                            var dateRegistration = userDataReader.GetValue(6).ToString();

                            var sql_Roly = new SqlCommand($"Select ID_Roly from Position where ID_User = {index}", ConnectDataBase.Get());
                            var sql_roly_Reader = await sql_Roly.ExecuteReaderAsync();
                            if (sql_roly_Reader.HasRows)
                            {
                                while (await sql_roly_Reader.ReadAsync())
                                {
                                    var indexRoly = sql_roly_Reader.GetValue(0).ToString();
                                    var sql_access = new SqlCommand($"Select * from Roly where ID_Roly = {indexRoly}", ConnectDataBase.Get());
                                    var sql_access_Reader = await sql_access.ExecuteReaderAsync();
                                    if (sql_access_Reader.HasRows)
                                    {
                                        while (await sql_access_Reader.ReadAsync())
                                        {

                                            listAccess = new Data_Access()                                           
                                            {
                                                IdRoly = ParserVariables.GetInt(indexRoly),
                                                IdUser = ParserVariables.GetInt(index),
                                                NameRoly = sql_access_Reader.GetValue(1).ToString(),
                                                AllRoly = ParserVariables.GetBoolean(sql_access_Reader.GetValue(2).ToString()),
                                                ReadUser = ParserVariables.GetBoolean(sql_access_Reader.GetValue(3).ToString()),
                                                ReadSotrud = ParserVariables.GetBoolean(sql_access_Reader.GetValue(4).ToString()),
                                                ReadClass = ParserVariables.GetBoolean(sql_access_Reader.GetValue(5).ToString()),
                                                ReadPredmet = ParserVariables.GetBoolean(sql_access_Reader.GetValue(6).ToString()),
                                                CreateAndViewReport = ParserVariables.GetBoolean(sql_access_Reader.GetValue(7).ToString()),
                                                TestReady = ParserVariables.GetBoolean(sql_access_Reader.GetValue(8).ToString()),
                                                CreateTest = ParserVariables.GetBoolean(sql_access_Reader.GetValue(9).ToString()),
                                                CreateGroup = ParserVariables.GetBoolean(sql_access_Reader.GetValue(10).ToString()),
                                                ConnectGroup = ParserVariables.GetBoolean(sql_access_Reader.GetValue(11).ToString()),
                                                AddSotrudForPredmet = ParserVariables.GetBoolean(sql_access_Reader.GetValue(12).ToString()),
                                                DeleteSotrudForPredmet = ParserVariables.GetBoolean(sql_access_Reader.GetValue(13).ToString()),

                                                ViewDataUser = ParserVariables.GetBoolean(sql_access_Reader.GetValue(14).ToString()),
                                                ViewDataSotrud= ParserVariables.GetBoolean(sql_access_Reader.GetValue(15).ToString()),
                                                ViewPrepmet = ParserVariables.GetBoolean(sql_access_Reader.GetValue(16).ToString()),

                                                ViewClass = ParserVariables.GetBoolean(sql_access_Reader.GetValue(17).ToString()),

                                                Surname = surname,
                                                Firstname = firstname,
                                                Middlemane = middlemane,
                                                Datebirch = datebirch,
                                                Gender = gender,
                                                DateRegistration = dateRegistration,
                                                Status = status
                                            };
                                            await sql_access_Reader.CloseAsync();
                                            break;
                                        }


                                    }
                                    await sql_roly_Reader.CloseAsync();
                                    break;
                                }
                            }
                            await userDataReader.CloseAsync();
                            break;
                        }
                    }
                    await dataReader.CloseAsync();
                    break;
                }
            }


            return listAccess;
        }



        /// <summary>
        /// Поулчение списка классов
        /// </summary>
        /// <param name="value">
        /// проводит проверку на наличие классного руководителя</param>
        /// <param name="viewer">
        /// не обязательный параметр нужен как маяк для более детального выбора данных для коллекции</param>
        /// <returns>Список классов</returns>
        public static async Task<List<Data_Klass>> GetKlassList(bool value,bool viewer=false)
        {
            var list = new List<Data_Klass>();
            SqlDataReader dataReader = null;
            try
            {
                string sql;


                if (value)
                    sql = $"select * from Klass where ID_Sotrud is null";
                else
                    sql = $"select * from Klass";

                SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());
                dataReader = await sqlCommand.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var newItemKlass = new Data_Klass()
                        {
                            Id = ParserVariables.GetInt(dataReader.GetValue(0).ToString()),
                            IsCode = Code.Null,
                            Name = dataReader.GetValue(1).ToString(),
                        };

                 
                        if (viewer)
                        {
                            var idEmp = dataReader.GetValue(2).ToString();
                            if (idEmp != string.Empty)
                            {
                                if (idEmp != "1")
                                {
                                    var fio = await SQLCommandReturnIDNoTransaction($"select (UT.SurnameUser + ' '+ UT.LastnameUser+ ' ' + UT.MiddlenameUser) as fio From UserTable UT WHERE  UT.ID_User = {idEmp}");
                                    var dateBirch = await SQLCommandReturnIDNoTransaction($"select DatebirchUser From UserTable UT WHERE  UT.ID_User = {idEmp}");
                                    var gender = await SQLCommandReturnIDNoTransaction($"select GenderUser From UserTable UT WHERE  UT.ID_User = {idEmp}");

                                    if (fio != "false")
                                    {
                                        newItemKlass.Employee = fio;
                                        newItemKlass.EmployeeID = int.Parse(idEmp);
                                        newItemKlass.DatebirchEmployee = dateBirch == "false" ? DateTime.Now.ToShortDateString() : dateBirch;
                                        newItemKlass.GenderEmployee = gender == "false" ? "Мужской" : gender;
                                    }
                                }
                             
                            }

                            newItemKlass.Users = await SQLCommandReturnUserClassRoom(newItemKlass.Id);
                            if (newItemKlass.Users != null)
                            {
                                newItemKlass.CountUser = newItemKlass.Users.Count;
                            }
                        }

                        list.Add(newItemKlass);
                    }
                }
                else
                {
                    list.Add(new Data_Klass() { Id = 0, Name = String.Empty, IsCode = Code.ErrorGetInfoKlassNoData });
                }
            }
            catch (Exception ex)
            {
         

                Logger.Error($"GetKlassList вызвал ошибку: {ex.Message}");
                list.Add(new Data_Klass() { Id = 0, Name = String.Empty, IsCode = Code.ErrorGetInfoKlass });
            }

            if (dataReader != null) await dataReader.CloseAsync();

            return list;
        }

        private static async Task<List<Data_UserList>> SQLCommandReturnUserClassRoom(int id)
        {
            var obj = new List<Data_UserList>();
            string sql = $"select ID_User from Klass_User where ID_Klass = {id}";
           
            using (SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            sql = $"select * from UserTable where ID_User = {dataReader.GetValue(0).ToString()}";

                            using (var sqlUserCommand = new SqlCommand(sql, ConnectDataBase.Get()))
                            {
                                using (var readerUser = await sqlUserCommand.ExecuteReaderAsync())
                                {
                                    while (await readerUser.ReadAsync())
                                    {
                                        if (readerUser.HasRows)
                                        {
                                            var surname = readerUser.GetValue(1).ToString();
                                            var firstname = readerUser.GetValue(2).ToString();
                                            var middlemane = readerUser.GetValue(3).ToString();
                                            var datebirch = readerUser.GetValue(4).ToString();
                                            var gender = readerUser.GetValue(5).ToString();

                                            obj.Add(new Data_UserList() 
                                            {
                                                Id = ParserVariables.GetInt(readerUser.GetValue(0).ToString()),
                                                DateBirch= datebirch,
                                                Gender= gender,
                                                Name= surname+" "+firstname+" "+ middlemane
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return obj;
        }

        public static async Task<List<Data_UserList>> GetUserList(Code isRole)
        {
            List<Data_UserList> list = new List<Data_UserList>();
            try
            {
                switch (isRole)
                {
                    case Code.GUI_Staff: list = await GetAllStaff(); break;
                    case Code.GUI_User: list = await GetAllUser(false); break;
                    case Code.GUI_UserModify: list = await GetAllUser(true); break;
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error($"GetUserList: Проризошла критическая ошибка. Ошибка:\n{ex.Message}");
                return list;
            }
        }


        public static async Task<List<Data_UserList>> GetFilterUserList(Code isRole, Date_FilterUser filter)
        {
            List<Data_UserList> list = new List<Data_UserList>();
            try
            {
                switch (isRole)
                {
                    case Code.GUI_Staff: list = await GetAllStaff(); break;
                    case Code.GUI_User: list = await GetAllUser(false); break;
                    case Code.GUI_UserModify: list = await GetAllUser(true); break;
                }

                List<Data_UserList> filterList = new List<Data_UserList>();
                switch (filter.Gender)
                {
                    case 1: filterList = list.FindAll(item => item.Gender.Trim().ToLower() == "мужской"); break;
                    case 2: filterList = list.FindAll(item => item.Gender.Trim().ToLower() == "женский"); break;
                    case 3: filterList = list; break;
                }

                if (filter.IsFilterData)
                {
                    filterList = filterList.FindAll(item=>((DateTime.Parse(item.DateBirch)>=filter.From) && (DateTime.Parse(item.DateBirch)<=filter.To)));
                }

                return filterList;
            }
            catch (Exception ex)
            {
                Logger.Error($"GetUserList: Проризошла критическая ошибка. Ошибка:\n{ex.Message}");
                return list;
            }
        }

        /// <summary>
        /// Ищем в базе данных всех обычных пользователей
        /// </summary>
        /// <param name="customRole">првоерка на кастомную роль</param>
        /// <returns>список пользователей</returns>
        private static async Task<List<Data_UserList>> GetAllUser(bool customRole)
        {
            var list = new List<Data_UserList>();
            SqlCommand sqlCommand = new SqlCommand(StaticTSQLScript.TSQL_UserViewer, ConnectDataBase.Get());
            using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int index = ParserVariables.GetInt(reader.GetValue(0).ToString());
                        if (index == 0) continue;
                        string rolesql = "select" +
                                         " NameRoly " +
                                         " from Roly R " +
                                         " inner join Position P ON P.Id_Roly = R.ID_Roly " +
                                         $" where P.Id_User = {index} SELECT SCOPE_IDENTITY()";

                        var roleString = await SQLCommandReturnIDNoTransaction(rolesql);
                      //Скипаем сисадмина
                        if (roleString.Trim() == "Системный администратор") continue;
                      //Проверка на кастомную роль. Если да, скипаем всех обычных пользователей, если нет то скипаем всю катом роль
                        if (customRole) 
                        {
                             if (roleString.Trim() == "Пользователь") continue; 
                             if (roleString.Trim() == "Учитель") continue; 
                        }
                        else
                             if (roleString.Trim() != "Пользователь") continue;

                        //Проверяем наличие класса у пользователя. Т.к его могут не выбрать при добавлении
                        var index_2 = reader.GetValue(5).ToString() == string.Empty ? 0 : ParserVariables.GetInt(reader.GetValue(5).ToString());
                        var login = await DBSearchMethods.ReturnLogin(index);

                        list.Add(new Data_UserList()
                        {
                            Id = index,
                            IsCode = Code.Null,
                            Name = reader.GetValue(1).ToString().Replace("NULL",""),
                            DateBirch = reader.GetValue(2).ToString(),
                            Gender = reader.GetValue(3).ToString(),
                            RegistrationData = reader.GetValue(4).ToString(),
                            IsTeacher = false,
                            Role = await SQLCommandReturnIDNoTransaction(rolesql),
                            KlassesUser = new List<Data_Klass>()
                                {
                                    new Data_Klass()
                                    {
                                        Id =  index_2,
                                        Name = reader.GetValue(6).ToString(),
                                        IsCode = Code.Null
                                    }
                                },
                            Login = login

                        });

                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Ищем в базе данных всех сотрудников
        /// </summary>
        /// <returns>список сотрудников</returns>
        private static async Task<List<Data_UserList>> GetAllStaff()
        {
            var list = new List<Data_UserList>();
            SqlCommand sqlCommand = new SqlCommand(StaticTSQLScript.TSQL_StaffViewer, ConnectDataBase.Get());
            using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int index = ParserVariables.GetInt(reader.GetValue(0).ToString());
                        if (index == 0) continue;
                        string rolesql = "select" +
                                         " NameRoly " +
                                         " from Roly R " +
                                         " inner join Position P ON P.Id_Roly = R.ID_Roly " +
                                         $" where P.Id_User = {index} SELECT SCOPE_IDENTITY()";

                        var roleString = await SQLCommandReturnIDNoTransaction(rolesql);
                        if (roleString.Trim() == "Системный администратор") continue;

                        if (IsCheck(index) == false)
                        {
                            var login = await DBSearchMethods.ReturnLogin(ParserVariables.GetInt(reader.GetValue(0).ToString()));
                            var klasses = new List<Data_Klass>();

                            using (var command = new SqlCommand($"select * from Klass where ID_Sotrud = {index}", ConnectDataBase.Get()))
                            {
                                using (var read = await command.ExecuteReaderAsync())
                                {
                                    if (read.HasRows)
                                    {
                                        while (await read.ReadAsync())
                                        {
                                            klasses.Add(new Data_Klass()
                                            {
                                                Id = ParserVariables.GetInt(read.GetValue(0).ToString()),
                                                Name = read.GetValue(1).ToString(),
                                                IsCode = Code.Null
                                            });
                                        }
                                    }
                                }
                            }

                                list.Add(new Data_UserList()
                                {
                                    Id = ParserVariables.GetInt(reader.GetValue(0).ToString()),
                                    IsCode = Code.Null,
                                    Name = reader.GetValue(1).ToString().Replace("NULL", ""),
                                    DateBirch = reader.GetValue(2).ToString(),
                                    Gender = reader.GetValue(3).ToString(),
                                    RegistrationData = reader.GetValue(4).ToString(),
                                    IsTeacher = true,
                                    Role = roleString,
                                    KlassesUser = klasses,
                                    Login = login

                                });
                        }
                        else
                        {
                            Add(index, ParserVariables.GetInt(reader.GetValue(5).ToString()), reader.GetValue(6).ToString());
                        }

                    }
                }
            }

            return list;
            #region localmethod

            bool IsCheck(int index)
            {
                var user = list.FirstOrDefault(o => o.Id == index);
                if (user == null)
                    return false;
                else
                    return true;
            }
            void Add(int index, int indexKlass, string nameKlass)
            {
                var user = list.FirstOrDefault(o => o.Id == index);
                user.KlassesUser.Add(new Data_Klass()
                {
                    Id = indexKlass,
                    Name = nameKlass,
                    IsCode = Code.Null
                });
            }
            #endregion
        }

        public static async Task<bool> EditPasswodSysAdmin(string login, string password)
        {
            return await SQLCommandNoTransaction(string.Format("UPDATE LoginUserTable SET Password_User = '{0}' where  Login_User = '{1}'", Encryption.getMd5Hash(password), login));
        }

        public static async Task<bool> EditUserData(string cell, string data,string indexUser)
        {
            return await SQLCommandNoTransaction(string.Format($"UPDATE UserTable SET {cell} = '{data}' WHERE ID_User = {indexUser}"));
        }

        public static async Task<bool> EditUserPassword(string data, string indexUser)
        {
            return await SQLCommandNoTransaction(string.Format($"UPDATE LoginUserTable SET Password_User = '{Encryption.getMd5Hash(data)}' WHERE ID_User = {indexUser}"));
        }

        public static async Task<bool> EditClassRoomEmployee(Data_Klass_UpdateEmployee obj)
        {
            string sql = "select * from UserTable";

            if (obj.IsCode == Code.Update)
            {
                string index = obj.Index_Employee == 0 ? "null" : $"{obj.Index_Employee}";
                sql = $"update Klass set ID_Sotrud={index} where ID_Klass={obj.Index_Class}";
            }
            if (obj.IsCode == Code.Insert) sql = $"update Klass set ID_Sotrud={obj.Index_Employee} where ID_Klass={obj.Index_Class}";

            return await SQLCommandNoTransaction(sql);


        }

        public static async Task<List<Data_Subject>> GetSubjectList()
        {
            var list = new List<Data_Subject>();

            SqlCommand sqlCommand = new SqlCommand("select * from Predmet", ConnectDataBase.Get());
            using (var reader = await sqlCommand.ExecuteReaderAsync())
            { 
                 if(reader.HasRows)
                 {
        
                     while (await reader.ReadAsync())
                     {
                        List<Data_Subject_User> _users = new List<Data_Subject_User>();

                        using (var command = new SqlCommand($" select * from PredmetSotrud where ID_Predmet={reader.GetInt32(0)}",ConnectDataBase.Get()))
                        {
                            using(var pread = await command.ExecuteReaderAsync()) 
                            {
                               if (pread.HasRows)
                               {
                                    while (await pread.ReadAsync())
                                    {
                                        if (pread.GetValue(1) == null) continue;
                                        int index = ParserVariables.GetInt(pread.GetValue(1).ToString());
                                        string fio = await SQLCommandReturnFirstStringNoTransaction($" select (SurnameUser+' '+LastnameUser+' '+MiddlenameUser) as FIO from UserTable where ID_User={index}");
                                        var dateBirch = await SQLCommandReturnIDNoTransaction($"select DatebirchUser From UserTable UT WHERE  UT.ID_User = {index}");
                                        var gender = await SQLCommandReturnIDNoTransaction($"select GenderUser From UserTable UT WHERE  UT.ID_User = {index}");


                                        _users.Add(new Data_Subject_User() { Index = index, Name = fio,DateBirch = dateBirch,Gender = gender });
                                    }
                               }
                            }
                        }


                      

                        list.Add(new Data_Subject() 
                        {
                                Id_data = reader.GetInt32(0),
                                Name_data = reader.GetString(1),                     
                                Users = _users,
                        });
                     }
                 }
            }

            return list;
        }

        public static async Task<List<Data_Roly>> GetRolyList()
        {
            List<Data_Roly> list = new List<Data_Roly>();

            using (var command = new SqlCommand($"select * From Roly", ConnectDataBase.Get()))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Data_Roly() { 
                                Index= ParserVariables.GetInt(reader.GetValue(0).ToString()),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return list;

        }


        public static async Task<List<Data_Roly>> GetRolyList(int index)
        {
            List<Data_Roly> list = new List<Data_Roly>();

            using (var command = new SqlCommand($"select * From Roly R where R.ID_Roly != {index} and R.NameRoly NOT LIKE 'системный администратор'", ConnectDataBase.Get()))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Data_Roly()
                            {
                                Index = ParserVariables.GetInt(reader.GetValue(0).ToString()),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return list;

        }


        public static async Task<Data_RPacket> GetRolyInformationList(int index)
        {
            List<Data_RolyUser> data_RolyUsers= new List<Data_RolyUser>();
            var packet = new Data_RPacket();
            var dataRoly = new Data_RolyInf();

            string sql = "  select UT.ID_User,(UT.SurnameUser+' '+UT.LastnameUser+' '+UT.MiddlenameUser) as fio from UserTable UT " +
                         "  inner join Position P ON P.ID_User = UT.ID_User " +
                         $" where P.ID_Roly = {index} ";


      
            using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using(var reader = await command.ExecuteReaderAsync()) 
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = new Data_RolyUser() {
                                    IndexUser = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                            };

                                                                  
                           data_RolyUsers.Add(data);
                        }
                    }
                }
            }


            using (var dbrolycommand = new SqlCommand($"select * from Roly where ID_Roly = {index}", ConnectDataBase.Get()))
            {
                using (var dbreadroly = await dbrolycommand.ExecuteReaderAsync())
                {
                    if (dbreadroly.HasRows)
                    {
                        while (await dbreadroly.ReadAsync())
                        {
                            dataRoly.Index = index;
                            dataRoly.Name = dbreadroly.GetString(1);
                            dataRoly.ReadUser = dbreadroly.GetBoolean(3);
                            dataRoly.ReadSotrud = dbreadroly.GetBoolean(4);
                            dataRoly.ReadClass = dbreadroly.GetBoolean(5);
                            dataRoly.ReadPredmet = dbreadroly.GetBoolean(6);
                            dataRoly.CreateAndViewReport = dbreadroly.GetBoolean(7);
                            dataRoly.TestReady = dbreadroly.GetBoolean(8);
                            dataRoly.CreateTest = dbreadroly.GetBoolean(9);
                            dataRoly.CreateGroup = dbreadroly.GetBoolean(10);
                            dataRoly.ConnectGroup = dbreadroly.GetBoolean(11);
                            dataRoly.AddSotrudForPredmet = dbreadroly.GetBoolean(12);
                            dataRoly.DeleteSotrudForPredmet = dbreadroly.GetBoolean(13);

                            dataRoly.ViewDataUser = dbreadroly.GetBoolean(14);
                            dataRoly.ViewDataSotrud = dbreadroly.GetBoolean(15);
                            dataRoly.ViewPrepmet = dbreadroly.GetBoolean(16);
                            dataRoly.ViewClass = dbreadroly.GetBoolean(17);



                            break;
                        }
                    }
                }
            }



            packet.Users = data_RolyUsers;
            packet.RolyData = dataRoly;
            return packet;

        }

        public static async Task<List<Data_Testing>> GetTestList()
        {
            List<Data_Testing> List = new List<Data_Testing>();

            try
            {
                string sql = "select T.*,P.NamePredmet,(UT.SurnameUser+' '+UT.LastnameUser+' '+UT.MiddlenameUser) as FIO From Testing T" +
                             " Left join UserTable UT ON UT.ID_User = T.ID_CrietingSotrud" +
                             " Left join Predmet P ON P.ID_Predmet = T.ID_Predmet ";

                using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        { 
                            while(await reader.ReadAsync()) 
                            {
                                var index = reader.GetValue(0);    
                                var nameTesting = reader.GetValue(1);    
                                var countQuest = reader.GetValue(2);    
                                var indexCreater = reader.GetValue(3);    
                                var indexPredmet = reader.GetValue(4);    
                                var dateCreiting = reader.GetValue(5);
                                var descr = reader.GetValue(6);
                                var namePredmet = reader.GetValue(7);
                                var creator = reader.GetValue(8);

                                int ic = 0;
                                int ip = 0;
                                string nc = string.Empty;
                                string np = string.Empty;

                                try
                                {
                                    ic = int.Parse(indexCreater.ToString());
                                }
                                catch
                                {
                                    ic = 0;
                                }

                                try
                                {
                                    ip = int.Parse(indexPredmet.ToString());
                                }
                                catch
                                {
                                    ip = 0;
                                }

                                  if(namePredmet!=null)  np = namePredmet.ToString();
                                  if(creator != null) nc = creator.ToString();
                                  if (dateCreiting == null) dateCreiting = string.Empty;


                                List.Add(new Data_Testing()
                                { 
                                    Index = ParserVariables.GetInt(index.ToString()),
                                    Name = nameTesting.ToString(),
                                    CountQuest = ParserVariables.GetInt(countQuest.ToString()),
                                    DateCrieting = dateCreiting.ToString(),
                                    IndexCreator = ic,
                                    IndexPredmet = ip,
                                    NameCreator = nc,
                                    NamePredmet = np,
                                    Description = dateCreiting.ToString()
                                });



                            }
                        }
                    
                    }
                }
                                
                return List;
            }
            catch(Exception ex) 
            {
                Logger.Error($"GetTestList: Проризошла критическая ошибка. Ошибка:\n{ex.Message}");
                return List;

            }
        }

        public static async Task<List<Data_Question>> GetTestingQuestionList(int index)
        {
             List<Data_Question>  list = new List<Data_Question>();


            string sql = $"select * From Questions where ID_Testing = {index}";
            using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Data_Question() { 
                               Index = reader.GetInt32(0),
                               Question = reader.GetString(1),
                               CorrecrNumber=reader.GetInt32(5),
                               IsImaging = reader.GetBoolean(6),
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
