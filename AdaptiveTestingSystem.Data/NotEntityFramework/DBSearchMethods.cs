#nullable disable

using System;
using System.Data.Common;
using System.Reflection;
using System.Security.Policy;

namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public class DBSearchMethods
    {
        /// <summary>
        /// Поиск логина и сопоставление hash паролей в базе данных
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль в виде строки</param>
        /// <returns>Вертер True если данные верны</returns>
        public static async Task<bool> IsCheckLoginAndPassword(string login, string password)
        {
            try
            {
                string sql = String.Format($"Select * from LoginUserTable where Login_User = '{login}'");
                SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());

                SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var hash = dataReader.GetValue(2).ToString();
                        if (Encryption.verifyMd5Hash(password, hash))
                        {
                            await dataReader.CloseAsync();
                            return true;
                        }
                        else
                        {
                            await dataReader.CloseAsync();
                            return false;
                        }
                    }
                }

                await dataReader.CloseAsync();
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string> ReturnLogin(int id)
        {
            string sql = String.Format($"Select Login_User from LoginUserTable where ID_User = {id}");
          
            SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());
  
            using (var dataReader = await sqlCommand.ExecuteReaderAsync())
            {
                while (await dataReader.ReadAsync())
                {
                    if (dataReader.HasRows)
                    {
                        return dataReader.GetValue(0).ToString();
                    }
                }
            }
                
            return string.Empty;
        }

        public static async Task DeleteUser(List<Data_DeleteUser> list)
        {
            try
            {
                Logger.Warning($"Начинаю удаление #{list.Count} пользователей");

                foreach (var item in list)
                {
                    await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM Position WHERE ID_User = {0}", item.Id));
                    await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM UserTable WHERE ID_User = {0}", item.Id));
                    await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM LoginUserTable WHERE ID_User = {0}", item.Id));

                    Logger.Log($"Пользователь Index#{item.Id} удален");
                }
                Logger.Warning($"Удаление завершено.");
            }
            catch (Exception ex)
            {
                Logger.Error($"DeleteUser вызвал ошибку: {ex.Message}");
            }
        }

        public static async Task<Data_Klass_Delete> DeleteClassRoom(Data_Klass_Delete obj)
        {
            Data_Klass_Delete data = new Data_Klass_Delete();
            data.IsCode = obj.IsCode;
            data.Klasses = new List<Data_Klass>();
            string description = string.Empty;

            Logger.Warning($"{obj.Description}");
                  
            if(obj.IsCode == Code.GUI_User || obj.IsCode == Code.Delete)
            {
                foreach (var item in obj.Klasses)
                {
                    int index = item.Id;
                    string sql = $"select * from Klass_User where ID_Klass = {index}";
                    if (await DBDataMethod.SQLCommandCheckTablaHasRow(sql) == false)
                    {
                        sql = $"delete Klass where ID_Klass = {index}";
                        if (await DBDataMethod.SQLCommandNoTransaction(sql))
                        {
                            Logger.Log($"DeleteClassRoom: Класс {item.Name} удален");
                            description += $"Класс {item.Name} удален.\n";
                        }
                    }
                    else
                    {
                        data.Klasses.Add(new Data_Klass() 
                        {
                            Id = index,
                            Name = item.Name,
                            IsCode = Code.ErrorDeleteClassRoom_GUI_User
                        });

                        Logger.Error($"Класс {item.Name} не был удален. Очистите список учащихся");
                        description += $"Класс {item.Name} не был удален. Очистите список учащихся\n";
                    }


                }
            }

            data.Description = description;
            return data;
        }


        public static async Task<bool> DeleteSubject(Data_Subject_Delete obj)
        {
            try
            {
                if (obj.IsCode == Code.Delete)
                {
                    foreach (var item in obj.Subject)
                    {
                        int index = item.Id_data;
                        await DBDataMethod.SQLCommandNoTransaction($"delete from PredmetSotrud where ID_Predmet = {index}");
                        await DBDataMethod.SQLCommandNoTransaction($"delete from Predmet where ID_Predmet = {index}");
                    }
                }

                return true;

            } catch (Exception ex) 
            {
                Logger.Error($"DBSearchMethods.DeleteSubject вызвал ошибку: {ex.Message}");
                return false;
            }
         
        }


        /// <summary>
        /// Поиск логина и проверка на доступность к функциям программы
        /// </summary>
        /// <param name="login">логин</param>
        /// <returns>Вернет True если доступ есть</returns>
        public static async Task<bool> IsCheckInformationBannedAccount(string login)
        {
            string sql = String.Format($"Select ID_User from LoginUserTable where Login_User = '{login}'");
            bool isBanned = false;
            SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());
            SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    var index = dataReader.GetValue(0).ToString();
                    var userSql = new SqlCommand($"select Status from UserTable where ID_User = '{index}'", ConnectDataBase.Get());
                    var userDataReader = await userSql.ExecuteReaderAsync();

                    if (userDataReader.HasRows)
                    {
                        while (await userDataReader.ReadAsync())
                        {
                            var banned = ParserVariables.GetBoolean(userDataReader.GetValue(0).ToString());
                            return !banned;
                        }
                    }

                    break;
                }
            }

            return isBanned;
        }

        /// <summary>
        /// Закрывает доступ к программе определенному пользователю. Поиск происходит по логину
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <returns>возвращает True если аккаунт забанен</returns>
        public static async Task<bool> BannedAccount(string login)
        {
            string sql = String.Format($"Select ID_User from LoginUserTable where Login_User = '{login}'");
            bool isBanned = false;
            SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());
            SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    var index = dataReader.GetValue(0).ToString();
                    var userSql = $"update UserTable set status=0 where ID_User = '{index}'";
                    return await DBDataMethod.SQLCommandNoTransaction(userSql);
                }
            }
            return isBanned;
        }
        /// <summary>
        /// Откртывает доступ к программе определенному пользователю. Поиск происходит по логину
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <returns>возвращает True если аккаунт разбанен</returns>
        public static async Task<bool> UnBannedAccount(string login)
        {
            string sql = String.Format($"Select ID_User from LoginUserTable where Login_User = '{login}'");
            bool isBanned = false;
            SqlCommand sqlCommand = new SqlCommand(sql, ConnectDataBase.Get());
            SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    var index = dataReader.GetValue(0).ToString();
                    var userSql = $"update UserTable set status=1 where ID_User = '{index}'";
                    return await DBDataMethod.SQLCommandNoTransaction(userSql);
                }
            }

            sqlCommand.Dispose();
            return isBanned;
        }



        public static async Task<bool> UserAccessVerification(int indexUser,string cells)
        {
            string indexRoly = $"SELECT ID_Roly FROM Position WHERE ID_User = {indexUser}";
            SqlCommand sqlCommand = new SqlCommand(indexRoly, ConnectDataBase.Get());
            using (var dataReader = await sqlCommand.ExecuteReaderAsync())
            {
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var index = dataReader.GetValue(0).ToString();
                        string access = $"SELECT {cells} FROM Roly WHERE ID_Roly = {index}";

                        using (var newSqlCommand = new SqlCommand(access, ConnectDataBase.Get()))
                        {
                            using (var reader = await newSqlCommand.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    { 
                                        var data = (bool)reader.GetValue(0);
                                        if(data) return true;
                                        else return false;
                                    }

                                }
                                else return false;
                            }
                        }            
                    }
                }
                else return false;
            }

            sqlCommand.Dispose();

            return false;
        }          



        public static async Task<bool> DeleteSysAdmin(string login, string password)
        {
            string idUser = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction(string.Format("select ID_User from LoginUserTable where Login_User = '{0}'", login));
            string idRoly = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction(string.Format("Select ID_Roly from Position where ID_User={0}", idUser));
            bool deleteRoly = await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM Roly WHERE ID_Roly = {0}", idRoly));
            bool deletePosition = await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM Position WHERE ID_User = {0}", idUser));
            bool deleteUser = await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM UserTable WHERE ID_User = {0}", idUser));
            bool deleteLoginUser = await DBDataMethod.SQLCommandNoTransaction(string.Format("DELETE FROM LoginUserTable WHERE ID_User = {0}", idUser));
            if (deleteRoly && deletePosition && deleteUser && deleteLoginUser) return true; else return false;
        }

        public static async Task<bool> VertySysAdminPassword(string password, string login)
        {
            try
            {
                string sql = String.Format("Select * from LoginUserTable where Login_User ='{0}'", login);
                SqlCommand command = new SqlCommand(sql, ConnectDataBase.Get());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        string hash = reader.GetValue(2).ToString();

                        if (Encryption.verifyMd5Hash(password, hash))
                        {

                            return true;
                        }
                        else
                        {

                            return false;
                        }
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"DBSearchMethods.VertySysAdminPassword вызвал ошибку: {ex.Message}");
                return false;
            }

        }

        public static async Task<bool> DeleteUserInClassRoom(Data_Klass_UserDelete obj)
        {
            string sql="";
            try
            {
                foreach (var user in obj.Users)
                {
                    sql += $" DELETE FROM Klass_User where ID_Klass={obj.Index} and ID_User={user.Id} \n";
                }

                using (var newSqlCommand = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    var read = await newSqlCommand.ExecuteNonQueryAsync();                   
                }

                return true;
            }
            catch(Exception ex)
            {
                Logger.Error($"DBSearchMethods.DeleteUserInClassRoom вызвал ошибку: {ex.Message}");
            }
            return false;
        }


        public static async Task<bool> DeleteUserInSubject(Data_Subject_UserDelete obj)
        {
            string sql = "";
            try
            {
                foreach (var user in obj.Users)
                {
                    sql += $" DELETE FROM PredmetSotrud where ID_Predmet={obj.Index} and ID_Sotrud={user.Id} \n";
                }

                using (var newSqlCommand = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    var read = await newSqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"DBSearchMethods.DeleteUserInSubject вызвал ошибку: {ex.Message}");
            }
            return false;
        }



        public static async Task<List<Data_UserList>> GetUserNoClassRoom()
        {
            try
            {
                var list = new List<Data_UserList>();

                string sql = "select UT.ID_User, UT.SurnameUser,UT.LastnameUser,UT.MiddlenameUser,UT.DatebirchUser,UT.GenderUser, R.NameRoly from UserTable UT " +
                    " Inner join Position P on P.ID_User = UT.ID_User " +
                    " Inner Join Roly R on R.ID_Roly = P.ID_Roly " +
                    " where R.NameRoly!='системный администратор' and R.NameRoly !='учитель'";

                using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                string searchsql = $"select * from Klass_User where ID_User = {reader.GetValue(0)}";

                                if (await DBDataMethod.SQLCommandCheckTablaHasRow(searchsql)==false)
                                {
                                    list.Add(new Data_UserList()
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = $"{reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}",
                                        DateBirch = reader.GetValue(4).ToString(),
                                        Gender = reader.GetString(5),
                                    });
                                }
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error($"DBSearchMethods.GetUserNoClassRoom вызвал ошибку: {ex.Message}");
                return null;
            }
        }


        public static async Task<List<Data_UserList>> GetUserNoSubject(int indexSubject)
        {
            try
            {
                var list = new List<Data_UserList>();

                string sql = "select UT.ID_User, UT.SurnameUser,UT.LastnameUser,UT.MiddlenameUser,UT.DatebirchUser,UT.GenderUser, R.NameRoly from UserTable UT " +
                    " Inner join Position P on P.ID_User = UT.ID_User " +
                    " Inner Join Roly R on R.ID_Roly = P.ID_Roly " +
                    " where R.NameRoly!='системный администратор' and R.NameRoly = 'учитель'";

                using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                string searchsql = $"select * from PredmetSotrud where ID_Sotrud = {reader.GetValue(0)} and ID_Predmet = {indexSubject}";

                                if (await DBDataMethod.SQLCommandCheckTablaHasRow(searchsql) == false)
                                {
                                    list.Add(new Data_UserList()
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = $"{reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}",
                                        DateBirch = reader.GetValue(4).ToString(),
                                        Gender = reader.GetString(5),
                                    });
                                }
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error($"DBSearchMethods.GetUserNoSubject вызвал ошибку: {ex.Message}");
                return null;
            }
        }


        public static async Task<Data_UserList> GetUserInformationByIndex(int id)
        {
            var obj = new Data_UserList();

            string sql= " select UT.ID_User, UT.SurnameUser,UT.LastnameUser,UT.MiddlenameUser,UT.DatebirchUser,UT.GenderUser, R.NameRoly,LUT.Login_User from UserTable UT " +
                        " Inner join LoginUserTable LUT on LUT.ID_User = UT.ID_User  " +
                        " Inner join Position P on P.ID_User = UT.ID_User " +
                        " Inner Join Roly R on R.ID_Roly = P.ID_Roly " +
                        $" where  UT.ID_User = {id}";

            using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync()) 
                        {
                            obj.Id = id;
                            obj.Name = $"{reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}";
                            obj.DateBirch = reader.GetValue(4).ToString();
                            obj.Gender = reader.GetString(5);
                            obj.Role= reader.GetString(6);
                            obj.Login= reader.GetString(7);
                            break;
                        }
                    }
                }
            } 
                return obj;
        }

        public static async Task<List<Data_UserList>> GetListEmployee(Data_UserList obj)
        {
            var list = new List<Data_UserList>();


            string sql;

            if (obj.IsUpdateOrInsert == true)
            {
                sql = $"select  ID_User,(UT.SurnameUser + ' '+ UT.LastnameUser+ ' ' + UT.MiddlenameUser) as fio, DatebirchUser,GenderUser from UserTable UT where ID_User != {obj.Id}";
            }
            else
            {
                sql = $"select  ID_User,(UT.SurnameUser + ' '+ UT.LastnameUser+ ' ' + UT.MiddlenameUser) as fio, DatebirchUser,GenderUser from UserTable UT where ID_User != 1";
            }


            using (var command = new SqlCommand(sql,ConnectDataBase.Get())) 
            {
                using(var reader = await command.ExecuteReaderAsync()) 
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync()) 
                        {

                            var index = reader.GetInt32(0);
                            string checkRole =
                                " select R.NameRoly from Position P " +
                                " inner join UserTable UT on UT.ID_User = P.ID_Position " +
                                " inner join Roly R on R.ID_Roly = P.ID_Roly " +
                                $" where UT.ID_User = {index} ";

                            var role = await DBDataMethod.SQLCommandReturnIDNoTransaction(checkRole);
                            if (role.Trim().ToLower() != "учитель") continue;
                            list.Add(new Data_UserList()
                            {
                                Id = index,
                                Name = reader.GetValue(1).ToString(),                              
                                DateBirch = reader.GetValue(2).ToString(),
                                Gender = reader.GetValue(3).ToString(),
                            });
                        }
                    }
                    
                }
            }
            return list;
        }

        public static async Task<int> SearchIndexSubject(string name_data)
        {
            string id = await DBDataMethod.SQLCommandReturnIDNoTransaction($"select ID_Predmet from Predmet where NamePredmet='{name_data}'");
            return ParserVariables.GetInt(id);
        }

        public static async Task ChangedDataUserRoly(Data_UpdateUserRoly obj)
        {   
            foreach(var item in obj.Users)
            {     
                await DBDataMethod.SQLCommandReturnIDNoTransaction($" UPDATE Position SET ID_Roly={obj.IndexRoly} where ID_User = {item.IndexUser}");
            }
        }

        public static async Task DeleteTest(List<Data_Testing> list)
        {

            foreach (var item in list)
            {
                int index = item.Index;

                using (var command = new SqlCommand($"select ID_Quest from Questions where ID_Testing = {index}", ConnectDataBase.Get()))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                await DBDataMethod.SQLCommandNoTransaction($"delete from Answer Where ID_Questions = {reader.GetValue(0)}");
                            }
                        }
                    }
                }

                await DBDataMethod.SQLCommandNoTransaction($"delete from Questions Where ID_Testing = {index}");
                await DBDataMethod.SQLCommandNoTransaction($"delete from Testing Where ID_Testing = {index}");
            }
        }

        public static async Task<Data_Testing> GetSearchTestByIndex(int index)
        {
            Data_Testing data = null;

            try 
            {
                using(var firstCommand = new SqlCommand($"select * from Testing WHERE ID_Testing = {index}",ConnectDataBase.Get())) 
                {
                    using (var firstReader = await firstCommand.ExecuteReaderAsync())
                    {
                        if (firstReader.HasRows)
                        {
                            data = new Data_Testing();
                            while (await firstReader.ReadAsync())
                            {
                                data.Index = index;
                                data.Name = firstReader.GetValue(1).ToString();
                                data.CountQuest = firstReader.GetInt32(2);                                              
                                data.DateCrieting = firstReader.GetValue(5).ToString();
                                data.Description = firstReader.GetValue(6).ToString();


                                var ic = firstReader.GetValue(3);
                                if (ic != null)
                                {
                                    data.IndexCreator = firstReader.GetInt32(3);
                                    data.NameCreator = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select (UT.SurnameUser+ ' ' + ut.LastnameUser+ ' '+ut.MiddlenameUser) as fio from UserTable UT where ut.ID_User = {firstReader.GetInt32(3)} ");
                                }

                                var ip = firstReader.GetValue(4);
                                if (ip != null)
                                {
                                    data.IndexPredmet = firstReader.GetInt32(4);
                                    data.NamePredmet = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select NamePredmet from Predmet where ID_Predmet = {firstReader.GetInt32(4)}");
                                }

                                var questions= new List<Data_Question>();

                                using(var secondCommand = new SqlCommand($"select * From Questions where ID_Testing= {index}",ConnectDataBase.Get())) 
                                {
                                    using(var secondReader = await secondCommand.ExecuteReaderAsync()) 
                                    {
                                        if (secondReader.HasRows)
                                        {
                                            while (await secondReader.ReadAsync())
                                            {
                                                var quest = new Data_Question();
                                                var answers = new List<Data_Answer>();

                                                quest.Index = secondReader.GetInt32(0);
                                                quest.Question = secondReader.GetValue(1).ToString();
                                                quest.Image = secondReader.GetValue(2).ToString();
                                                quest.ImageFormat = secondReader.GetValue(7).ToString();
                                                quest.Complexity= (int)secondReader.GetDouble(3);
                                                quest.CorrecrNumber= secondReader.GetInt32(5);
                                                quest.IsImaging= secondReader.GetBoolean(6);

                                                using (var lastCommand = new SqlCommand($"select * From Answer where ID_Questions = {secondReader.GetInt32(0)}", ConnectDataBase.Get()))
                                                { 
                                                   using(var lastReader = await lastCommand.ExecuteReaderAsync()) 
                                                    {
                                                        if (lastReader.HasRows)
                                                        {
                                                            while (await lastReader.ReadAsync())
                                                            {
                                                                var answer = new Data_Answer();
                                                                answer.Index = lastReader.GetInt32(0);                                                             
                                                                answer.Answer = lastReader.GetValue(2).ToString();                                                                      
                                                                answer.Image = lastReader.GetValue(3).ToString();
                                                                answer.ImageFormat = lastReader.GetValue(4).ToString();                                                  
                                                                answer.IsImaging = lastReader.GetBoolean(5);
                                                                answer.Number= lastReader.GetInt32(6);
                                                                answers.Add(answer);
                                                            }
                                                        }
                                                    }
                                                }

                                                quest.Answer = answers;
                                                questions.Add(quest);
                                            }
                                        }
                                    }
                                }

                                data.Questions = questions;
                                break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex) 
            {
                Logger.Error($"DBSearchMethods.GetSearchTestByIndex вызвал ошибку: {ex.Message}");
            }

            return data;
        }

        public static async Task<Data_Question> GetQuestData(int index)
        {
            Data_Question packet = null;

            using (var command = new SqlCommand($"select * from Questions where ID_Quest = {index}", ConnectDataBase.Get()))
            {
                using(var reader = await command.ExecuteReaderAsync()) 
                {
                    if (reader.HasRows)
                    {
                        packet = new Data_Question();

                        while (await reader.ReadAsync()) 
                        {
                            var listAnswer = new List<Data_Answer>();

                            packet.Index= reader.GetInt32(0);
                            packet.Question = reader.GetString(1);
                            packet.Image = reader.GetString(2);
                            packet.Complexity=(int)reader.GetDouble(3);
                            packet.CorrecrNumber=reader.GetInt32(5);
                            packet.IsImaging= reader.GetBoolean(6);
                            packet.ImageFormat=reader.GetString(7);


                            using(var lastCommand = new SqlCommand($"select * From Answer where ID_Questions = {index}",ConnectDataBase.Get())) 
                            {
                                using (var lastREader = await lastCommand.ExecuteReaderAsync())
                                { 
                                    if(lastREader.HasRows) 
                                    {
                                        while (await lastREader.ReadAsync())
                                        {
                                            Data_Answer answer = new Data_Answer()
                                            {
                                                Answer = lastREader.GetString(2),
                                                Index = lastREader.GetInt32(0),
                                                Image= lastREader.GetString(3),
                                                ImageFormat=lastREader.GetString(4),
                                                IsImaging= lastREader.GetBoolean(5),
                                                Number = lastREader.GetInt32(6)
                                            };

                                            listAnswer.Add(answer);
                                        }
                                    }
                                }
                            }

                            packet.Answer = listAnswer;
                        }
                    }
                }
            }

          return packet;
        }

        public static async Task<(string,int)> GetRandomNameAndIndexTest()
        { 
            string sql = "SELECT TOP 1 NameTesting,ID_Testing FROM Testing ORDER BY NEWID()";

            using (var sqlCommand = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            return (reader.GetValue(0).ToString(), reader.GetInt32(1));
                        }
                    }
                }
            }

            return ("", 0);
        }

        public static async Task<(string,string)> GetNameSotrudAndNamePredmet(int indexCreator, int indexTest)
        {
            string nameSotrud = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select (UT.SurnameUser+ ' ' + ut.LastnameUser+ ' '+ut.MiddlenameUser) as fio from UserTable UT where ut.ID_User = {indexCreator} ");
            string namePredmet = "";


            using (var firstCommand = new SqlCommand($"select ID_Predmet from Testing WHERE ID_Testing = {indexTest}", ConnectDataBase.Get()))
            {
                using (var firstReader = await firstCommand.ExecuteReaderAsync())
                {
                    if (firstReader.HasRows)
                    {
                        while (await firstReader.ReadAsync())
                        {
                            namePredmet = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select NamePredmet from Predmet where ID_Predmet = {firstReader.GetInt32(0)}");
                        }
                    }
                }
            }


         return (nameSotrud, namePredmet);
        }

    

        public static async Task<List<Data_AllTestForSB>> GetAllTestForCreateServer()
        {
            var data_s = new List<Data_AllTestForSB>();

            string sql = "select ID_Testing,NameTesting,ID_CrietingSotrud,ID_Predmet From Testing";

            using (var sqlCommand = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            data_s.Add(new Data_AllTestForSB() 
                            {
                                Index= reader.GetInt32(0),
                                NameTest= reader.GetString(1),
                                MaxQuestCountForUser = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select Count(*) From Questions where ID_Testing = {reader.GetInt32(0)}"),
                                NameCreate = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select (UT.SurnameUser+ ' ' + ut.LastnameUser+ ' '+ut.MiddlenameUser) as fio from UserTable UT where ut.ID_User = {reader.GetValue(2)}"),
                                NamePredmet = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select NamePredmet from Predmet where ID_Predmet = {reader.GetValue(3)}")
                        });
                        }
                    }
                }
            }


            return data_s;

        }

        public static async Task<Data_ResultTesting> GetUserResultTesting(int index)
        {
         
            var a2 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 2 and ID_User= {index}");
            var a3 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 3 and ID_User= {index}");
            var a4 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 4 and ID_User= {index}");
            var a5 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 5 and ID_User= {index}");

            Data_ResultTesting data = new Data_ResultTesting()
            {
                CountAssessment2 = double.Parse(a2),
                CountAssessment3 = double.Parse(a3),
                CountAssessment4 = double.Parse(a4),
                CountAssessment5 = double.Parse(a5),
            };

            return data;
        }

        public static async Task<Data_StatisticGeneral> GetGeneralStatistic()
        {
            var statistic = new Data_StatisticGeneral();

            statistic.AllScoreTests = new List<Data_AllScoreTest> ();
            statistic.ClassroomScore_generals=new List<Data_ClassroomScore_general>();
            statistic.AverageScores5ClassRoom = new List<Data_5ClassRoomForAverageScore>();
            statistic.MostTested3Subjects = new List<Data_3MostTestedSubject>();
            statistic.OneMostActiveUser = new Data_MostActiveUser();
            statistic.MostActiveUsers =new  List<Data_MostActiveUser>();

            //Данные обовсех тестах
            using (var command = new SqlCommand("select * from Testing", ConnectDataBase.Get())) 
            {
                using (var read = await command.ExecuteReaderAsync())
                {
                    if (read.HasRows)
                    {
                        while (await read.ReadAsync())
                        {
                    
                            var index = read.GetValue(0);
                            var name = read.GetValue(1);

                            var a2 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 2 and ID_Test= {index}");
                            var a3 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 3 and ID_Test= {index}");
                            var a4 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 4 and ID_Test= {index}");
                            var a5 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 5 and ID_Test= {index}");

                            var _stat = new Data_AllScoreTest()
                            {
                                TestName = name.ToString(),
                                Count_Score2_general = double.Parse(a2),
                                Count_Score3_general = double.Parse(a3),
                                Count_Score4_general = double.Parse(a4),
                                Count_Score5_general = double.Parse(a5),
                            };

                            statistic.AllScoreTests.Add(_stat);

                        }
                    }
                }
            }

            //Данные по классам
            using (var command = new SqlCommand("select * from Klass", ConnectDataBase.Get()))
            {
                using (var readClassRoom = await command.ExecuteReaderAsync())
                {
                    if (readClassRoom.HasRows)
                    {
                        while (await readClassRoom.ReadAsync())
                        {
                            var indexClass = readClassRoom.GetValue(0);
                            var nameClass = readClassRoom.GetValue(1);

                            int count2 = 0;
                            int count3 = 0;
                            int count4 = 0;
                            int count5 = 0;

                            using (var userClass = new SqlCommand($"select * from Klass_User where ID_Klass = {indexClass}", ConnectDataBase.Get()))
                            {
                                using (var readUserClass = await userClass.ExecuteReaderAsync())
                                {
                                    if (readUserClass.HasRows)
                                    {
                                        while (await readUserClass.ReadAsync())
                                        { 
                                            var indexUser  = readUserClass.GetValue(2);

                                            var a2 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 2 and ID_User= {indexUser}");
                                            var a3 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 3 and ID_User= {indexUser}");
                                            var a4 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 4 and ID_User= {indexUser}");
                                            var a5 = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select  Count(*) from Resultation where Assessment = 5 and ID_User= {indexUser}");

                                            count2 += int.Parse(a2);
                                            count3 += int.Parse(a3);
                                            count4 += int.Parse(a4);
                                            count5 += int.Parse(a5);
                                        }

                                        statistic.ClassroomScore_generals.Add(new Data_ClassroomScore_general()
                                        {
                                            ClassRoomName = nameClass.ToString(),
                                            Count_Score2= count2,
                                            Count_Score3= count3,
                                            Count_Score4= count4,
                                            Count_Score5 = count5,
                                        });

                                    }
                                }
                            }
                        }
                    }
                }
            }

            //5 лучших классов по среднему баллу
            if (statistic.ClassroomScore_generals.Count > 0)
            {
                foreach (var item in statistic.ClassroomScore_generals)
                {

                    double avg = (((5 * (double)item.Count_Score5) + (4 * (double)item.Count_Score4) + (3 * (double)item.Count_Score3) +
                        (2 * (double)item.Count_Score2)) / ((double)item.Count_Score5 + (double)item.Count_Score4 + (double)item.Count_Score3 + (double)item.Count_Score2));

                    var data_5ClassRoomFor = new Data_5ClassRoomForAverageScore()
                    {
                        AverageScore = avg,
                        ClassRoomName = item.ClassRoomName,
                    };

                    statistic.AverageScores5ClassRoom.Add(data_5ClassRoomFor);
                }

                var sort = statistic.AverageScores5ClassRoom.OrderByDescending(x => x.AverageScore);
                var list = new List<Data_5ClassRoomForAverageScore>();
                if (sort.Count() > 5)
                {
                    for (int i = 0; i < sort.Count(); i++)
                    {
                        if (i > 4)
                        {
                            break;
                        }

                        list.Add(sort.ToArray()[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < sort.Count(); i++)
                    {
                       list.Add(sort.ToArray()[i]);
                    }
                }


                statistic.AverageScores5ClassRoom = list;
            }

            //3 самых тестируемых предмета
            string sql = "select top 3 R.ID_Test,Count(*) as ct from Resultation R " + 
                         " Inner join Testing T ON T.ID_Testing = R.ID_Test " +
                         " Group by R.ID_Test Order by ct DESC";

            using (var  predmetCommand = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (var readpredmetCommand = await predmetCommand.ExecuteReaderAsync())
                {
                    if (readpredmetCommand.HasRows)
                    {
                        while (await readpredmetCommand.ReadAsync())
                        {
                            var indexTest = readpredmetCommand.GetValue(0);               
                            string sqlPredmet = $"select P.NamePredmet from Testing T Inner Join Predmet P ON P.ID_Predmet = T.ID_Predmet where T.ID_Testing = {indexTest}";
                            var predmetName = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction(sqlPredmet);
                            var subj = new Data_3MostTestedSubject()
                            { 
                                Count = int.Parse(readpredmetCommand.GetValue(1).ToString()),
                                SubjectName = predmetName,
                            };

                            statistic.MostTested3Subjects.Add(subj);
                        }
                    }
                }
            }


            //Поиск самого результативных 20 пользователtей, по лучшему балу и после прохождения 20 тестов. 

            sql = " select top 20 ID_User,avg(Assessment) as av,count(Id_user) as ct from Resultation " +
                  " group by ID_User Having count(Id_user) >= 20 Order by ct,av ";

            using (var sqlMostOneUser = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                using (var readeruser = await sqlMostOneUser.ExecuteReaderAsync())
                {
                    if (readeruser.HasRows)
                    {
                        while (await readeruser.ReadAsync())
                        { 
                           var indexUser = readeruser.GetValue(0);
                           var nameCreate = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select (UT.SurnameUser+ ' ' + ut.LastnameUser+ ' '+ut.MiddlenameUser) as fio from UserTable UT where ut.ID_User = {indexUser}");

                           var data_MostActiveUser = new Data_MostActiveUser()
                           { 
                                Index = int.Parse(indexUser.ToString()),
                                Name = nameCreate,
                                UserDataInMonth = await GetAllResultInAllMountInThisYear(indexUser)
                           };

                            statistic.MostActiveUsers.Add(data_MostActiveUser);
                        }
                    }
                }
            }

            if (statistic.MostActiveUsers.Count > 0)
               statistic.OneMostActiveUser = statistic.MostActiveUsers[0];

                return statistic;
        }

        private static async Task<List<Data_UserDataInMonth>> GetAllResultInAllMountInThisYear(object indexUser)
        {
            List<Data_UserDataInMonth> list = new List<Data_UserDataInMonth>();
            for (int i = 0; i < 12; i++)
            {

                Data_UserDataInMonth data_UserData = new Data_UserDataInMonth();
                var mount = i.ToString().Length > 1 ? $"{i + 1}" : $"0{i+1}";

                string sql = $"select Assessment from Resultation where DataTest >= '01.{mount}.{DateTime.Now.Year}' and DataTest <= '{DateTime.DaysInMonth(DateTime.Now.Year, i + 1)}.{mount}.{DateTime.Now.Year}' and ID_User = {indexUser}";

                using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        double score2 = 0;
                        double score3 = 0;
                        double score4 = 0;
                        double score5 = 0;

                        if (reader.HasRows)
                        {
                       
                            while (await reader.ReadAsync())
                            {                             
                                switch (reader.GetDouble(0))
                                {
                                    case 2: score2++; break;
                                    case 3: score3++; break;
                                    case 4: score4++; break;
                                    case 5: score5++; break;
                                }
                            }
                        }

                        double divider = score5+score4+score3+score2;
                        double divisible = (5 * score5) + (4* score4) + (3*score3) + (2* score2);
                        double avg = 0;
                        if (divider != 0)
                        {
                            avg = divisible / divider;
                        }

                        data_UserData.AVG = avg;
                        data_UserData.Count_Score2 = score2;
                        data_UserData.Count_Score3 = score3;
                        data_UserData.Count_Score4 = score4;
                        data_UserData.Count_Score5 = score5;
                        data_UserData.Dates = new Data_Month(1, i + 1, DateTime.Now.Year);
                    }
                }

                list.Add(data_UserData);
                await Task.Delay(10);
            }


            return list;
        }

        public static async Task<Data_StatisticCustom> GetCustomStatistic(int index)
        {
            var data_Statistics = new Data_StatisticCustom();
            data_Statistics.data_AllUsers = new List<Data_AllUserPacket>();

            var userList = await DBDataMethod.GetUserList(Code.GUI_User);
            var userModifyList = await DBDataMethod.GetUserList(Code.GUI_UserModify);

            AddToListUser(data_Statistics, userList);
            AddToListUser(data_Statistics, userModifyList);

            if(index==0) return data_Statistics;








            else return data_Statistics;
        }

        private static void AddToListUser(Data_StatisticCustom data_Statistics, List<Data_UserList> userList)
        {
            foreach (var item in userList)
            {
                var search = data_Statistics.data_AllUsers.FirstOrDefault(o => o.Index == item.Id);
                if (search != null) continue;

                data_Statistics.data_AllUsers.Add(new Data_AllUserPacket()
                {
                    Index = item.Id,
                    Gender = item.Gender,
                    Name = item.Name,
                    DateBirch = item.DateBirch,
                });
            }
        }
    }
}
