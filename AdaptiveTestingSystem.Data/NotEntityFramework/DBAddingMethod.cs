using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public class DBAddingMethod
    {
        /// <summary>
        /// Регистарция нового пользователя в программе
        /// </summary>
        /// <param name="obj">передаем регистарционный данные</param>
        /// <returns>возвращает логин и пароль если регистарция удалась</returns>
        public static async Task<Data_Registration> Registration(Data_Registration obj)
        {

            var registration = new Data_Registration();
            registration.IsCode = Code.InvalidRegistration;
            string sql = string.Format(" insert into UserTable(SurnameUser,LastnameUser,MiddlenameUser,DatebirchUser,GenderUser,RegistrationdatatimeUser,Status) " +
                               " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')  SELECT SCOPE_IDENTITY() ",
                               obj.SurnameUser,
                               obj.LastnameUser,
                               obj.MiddlenameUser,
                               obj.DatebirchUser,
                               obj.GenderUser,
                               DateTime.Now, 1);
            try
            {

                string idUser = await DBDataMethod.SQLCommandReturnIDNoTransaction(sql);

                if (idUser != "false")
                {
                    string passEnc = Encryption.getMd5Hash(obj.Password.Trim());


                    sql = String.Format(" insert into LoginUserTable(ID_User,Login_User,Password_User) " +
                        "VALUES ({0},'{1}','{2}')", idUser, obj.Login.Trim(), passEnc);

                    if (await DBDataMethod.SQLCommandNoTransaction(sql))
                    {
                        string indexRoly = DBDataMethod.GetIndexRoly("Пользователь");

                        if (ParserVariables.CountSymbol(indexRoly) > 0)
                        {
                            sql = string.Format("insert into Position(ID_User,ID_Roly) VALUES ({0},{1})", idUser, indexRoly);

                            if (await DBDataMethod.SQLCommandNoTransaction(sql))
                            {

                                registration.Login = obj.Login.Trim();
                                registration.Password = obj.Password.Trim();
                                registration.IsCode = Code.RegistrationSuccessful;
                                return registration;
                            }
                            else
                            {
                                DeleteNewUser(idUser);
                                throw new Exception("Не возможно присвоить роль, пользователь удаляется.");
                            }
                        }
                        else
                        {
                            DeleteNewUser(idUser);
                            throw new Exception("Не возможно проверить роли, пользователь удаляется.");
                        }
                    }
                    else
                    {
                        DeleteNewUser(idUser);
                        throw new Exception("Не возможно присвоить логин, пользователь удаляется");
                    }
                }
                else
                {
                    return registration;
                    throw new Exception("Регистрация в базе данных не удалась!");
                }
            }
            catch (Exception ex)
            {
           

                Logger.Error($"Registration: Ошибка при регистрации пользователя: {ex.Message}\n{obj}");
                return registration;

               
            }

        }

        public static async Task<Data_NewUserInsert> AddNewUserOrStaff(Data_NewUserInsert obj)
        {

            var registration = new Data_NewUserInsert();
      
            if (obj.IsCode == Code.NewUserClassRoomInsert)
                registration.IsCode = Code.NewUserClassRoomError;           
            else 
                registration.IsCode = Code.InvalidRegistration;

            string sql = string.Format(" insert into UserTable(SurnameUser,LastnameUser,MiddlenameUser,DatebirchUser,GenderUser,RegistrationdatatimeUser,Status) " +
                               " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')  SELECT SCOPE_IDENTITY() ",
                               obj.SurnameUser,
                               obj.LastnameUser,
                               obj.MiddlenameUser,
                               obj.DatebirchUser,
                               obj.GenderUser,
                               DateTime.Now, 1);

            try
            {

                string idUser = await DBDataMethod.SQLCommandReturnIDNoTransaction(sql);

                if (idUser != "false")
                {
                    string passEnc = Encryption.getMd5Hash(obj.Password.Trim());


                    sql = String.Format(" insert into LoginUserTable(ID_User,Login_User,Password_User) " +
                        "VALUES ({0},'{1}','{2}')", idUser, obj.Login.Trim(), passEnc);

                    if (await DBDataMethod.SQLCommandNoTransaction(sql))
                    {
                        string indexRoly = String.Empty;
                        if (obj.IsCode ==Code.NewUserInsert || obj.IsCode == Code.NewUserClassRoomInsert) indexRoly = DBDataMethod.GetIndexRoly("Пользователь");
                        if (obj.IsCode ==Code.NewStaffInsert || obj.IsCode == Code.Subject_Insert_User) indexRoly = DBDataMethod.GetIndexRoly("Учитель");

                        if (ParserVariables.CountSymbol(indexRoly) > 0)
                        {
                            sql = string.Format("insert into Position(ID_User,ID_Roly) VALUES ({0},{1})", idUser, indexRoly);

                            if (await DBDataMethod.SQLCommandNoTransaction(sql))
                            {

                                if (obj.IsCode == Code.NewUserClassRoomInsert)
                                {
                                    registration.IsCode = Code.NewUserClassRoomnSuccessful;
                                    registration.Index = ParserVariables.GetInt(idUser);
                                }
                                else
                                    registration.IsCode = Code.RegistrationSuccessful;


                                if (obj.IsCode == Code.Subject_Insert_User)
                                {
                                    sql = $"insert into PredmetSotrud(ID_Sotrud,ID_Predmet) VALUES({idUser},{obj.Subject})";
                                    if (await DBDataMethod.SQLCommandNoTransaction(sql))
                                    {
                                        Logger.Log($"Данные предмета #{obj.Subject} успешно обновлены");

                                        registration.IsCode = Code.Subject_Insert_Successfull;
                                        registration.Index = ParserVariables.GetInt(idUser);
                                    }
                                }


                                if (obj.Klasses.Count > 0)
                                {
                                    foreach (var klasses in obj.Klasses)
                                    {
                                        if (obj.IsCode == Code.NewStaffInsert)
                                        {
                                            sql = $"update Klass set ID_Sotrud = {idUser} where ID_Klass = {klasses}";
                                            if (await DBDataMethod.SQLCommandNoTransaction(sql))
                                            {
                                                Logger.Log($"Данные класса #{klasses} успешно обновлены");
                                            }
                                            continue;
                                        }

                                        if (obj.IsCode == Code.NewUserInsert || obj.IsCode == Code.NewUserClassRoomInsert)
                                        {
                                            sql = $"insert into Klass_User(ID_Klass,ID_User) VALUES ({klasses},{idUser})";
                                            if (await DBDataMethod.SQLCommandNoTransaction(sql))
                                            {
                                                Logger.Log($"Данные класса ученика {obj.SurnameUser} {obj.LastnameUser} успешно обновлены  ID класса: #{klasses}");
                                            }
                                        }

                                    }
                                }

                                return registration;
                            }
                            else
                            {
                                DeleteNewUser(idUser);
                                throw new Exception("Не возможно присвоить роль, пользователь удаляется.");
                            }
                        }
                        else
                        {
                            DeleteNewUser(idUser);
                            throw new Exception("Не возможно проверить роли, пользователь удаляется.");
                        }
                    }
                    else
                    {
                        DeleteNewUser(idUser);
                        throw new Exception("Не возможно присвоить логин, пользователь удаляется");
                    }
                }
                else
                {
                    return registration;
                    throw new Exception("Регистрация в базе данных не удалась!");
                }
            }
            catch (Exception ex)
            {


                Logger.Error($"Registration: Ошибка при регистрации пользователя: {ex.Message}\n{obj}");
                return registration;


            }


        }

        public static async Task<bool> ADDSysAdmin(string login, string password)
        {





            var index = await DBDataMethod.SQLCommandReturnIDNoTransaction(string.Format($"Select * from LoginUserTable where Login_User LIKE '{login}'"));

            if (index != "false")
            {
                throw new Exception("Такой логин уже существует");
            }


            string sql = string.Format(" insert into UserTable(SurnameUser,LastnameUser,MiddlenameUser,DatebirchUser,GenderUser,RegistrationdatatimeUser,Status) " +
                                       " VALUES ('SYS','ADMINISTRATOR','','{0}','Мужской','{1}','true')  SELECT SCOPE_IDENTITY() ", DateTime.Now.ToShortDateString(), DateTime.Now);
            string idUser = await DBDataMethod.SQLCommandReturnIDNoTransaction(sql);

            if (idUser != "false")
            {

                sql = String.Format(" insert into LoginUserTable(ID_User,Login_User,Password_User) " +
                    "VALUES ({0},'{1}','{2}')", idUser, login, Encryption.getMd5Hash(password));

                if (await DBDataMethod.SQLCommandNoTransaction(sql))
                {
                    sql = " insert into Roly(NameRoly,allRoly,ReadUser,ReadSotrud,ReadClass,ReadPredmet,CreateAndViewReport,TestReady,CreateTest,CreateGroup,ConnectGroup,AddSotrudForPredmet,DeleteSotrudForPredmet,ViewDataUser,ViewDataSotrud,ViewPrepmet,ViewClass) " +
                          " VALUES ('Системный администратор','true','true','true','true','true','true','true','true','true','true','true','true','true','true','true','true') " +
                        " " +
                        "  SELECT SCOPE_IDENTITY()";
                    string idRoly = await DBDataMethod.SQLCommandReturnIDNoTransaction(sql);

                    if (idRoly != "false")
                    {
                        sql = string.Format("insert into Position(ID_User,ID_Roly) VALUES ({0},{1})", idUser, idRoly);

                        if (await DBDataMethod.SQLCommandNoTransaction(sql)) return true; else return false;
                    }
                }

            }

            return false;
        }
        private static async void DeleteNewUser(string idUser)
        {
            if (await DBDataMethod.SQLCommandNoTransaction($"delete UserTable where ID_User = {idUser}"))
            {
               Logger.Message($"Удаление ID_User: {idUser} прошло успешно");
            }
            else
            {
               Logger.Message($"Удаление ID_User: {idUser} не удачно.");
            }

        }

        public static async Task<bool> InsertClassRoom(Data_Klass obj)
        {
            if (await DBDataMethod.SQLCommandCheckTablaHasRow($"select * from Klass where NameKlass LIKE '{obj.Name}'") == true) return false;
            string sql = "";
            if (obj.Id != 0)
            {
                sql = $"update Klass set NameKlass='{obj.Name}' where ID_Klass={obj.Id}  SELECT SCOPE_IDENTITY()";
            } 
            else
            {
                sql = $"INSERT INTO Klass (NameKlass) values ('{obj.Name}')  SELECT SCOPE_IDENTITY()";
            }

       

            if (await DBDataMethod.SQLCommandNoTransaction(sql)==true)
            {
                return true;
            }
                        
            return false;
        }

        public static async Task<bool> AddUserToClassRoom(List<Data_UserToClass>? obj)
        {
            try 
            {
                if(obj==null) return false;

                foreach (var item in obj)
                {
                    string sql = $"insert into Klass_User(ID_Klass,ID_User) VALUES ({item.IndexClass},{item.IndexUser})";
                    await DBDataMethod.SQLCommandNoTransaction(sql);
                }
               
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"AddUserToClassRoom: Ошибка при добавлении данных: {ex.Message}\n{obj}");
                return false;
            }
        }

        public static async Task<bool> AddUserToSubject(List<Data_UserToSubject>? obj)
        {
            try 
            {
                if(obj==null) return false;

                foreach (var item in obj)
                {
                    string sql = $"insert into PredmetSotrud(ID_Predmet,ID_Sotrud) VALUES ({item.IndexSubject},{item.IndexUser})";
                    await DBDataMethod.SQLCommandNoTransaction(sql);
                }
               
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"AddUserToClassRoom: Ошибка при добавлении данных: {ex.Message}\n{obj}");
                return false;
            }
        }



        public static async Task<bool> InsertOrUpdateSubject(Data_Subject obj)
        {
        

            if (await DBDataMethod.SQLCommandCheckTablaHasRow($"select * from Predmet where NamePredmet LIKE '{obj.Name_data}'") == true) return false;


            string sql = "";

            if (obj.IsCode == Code.Subject_Insert) sql = $"insert into Predmet(NamePredmet) values ('{obj.Name_data}')";
            if (obj.IsCode == Code.Subject_Update) sql = $"update Predmet set NamePredmet='{obj.Name_data}' where ID_Predmet = {obj.Id_data}";


            if (await DBDataMethod.SQLCommandNoTransaction(sql) == true)
            {
                return true;
            }

            return false;
        }

        public static async Task<bool> InserNewRoly(Data_RolyInf obj)
        {
            string sql = " insert into Roly(NameRoly, allRoly, ReadUser, ReadSotrud, ReadClass, ReadPredmet, CreateAndViewReport, TestReady, CreateTest, CreateGroup, ConnectGroup, AddSotrudForPredmet, DeleteSotrudForPredmet,ViewDataUser,ViewDataSotrud,ViewPrepmet,ViewClass) " +
                         $" VALUES ('{obj.Name}','false'," +
                         $"'{obj.ReadUser}'," +
                         $"'{obj.ReadSotrud}'," +
                         $"'{obj.ReadClass}'," +
                         $"'{obj.ReadPredmet}'," +
                         $"'{obj.CreateAndViewReport}'," +
                         $"'{obj.TestReady}'," +
                         $"'{obj.CreateTest}'," +
                         $"'{obj.CreateGroup}'," +
                         $"'{obj.ConnectGroup}'," +
                         $"'{obj.AddSotrudForPredmet}'," +
                         $"'{obj.DeleteSotrudForPredmet}'), " +
                         $"'{obj.ViewDataUser}'," +
                         $"'{obj.ViewDataSotrud}'," +
                         $"'{obj.ViewPrepmet}'," +
                         $"'{obj.ViewClass}'";




            if (await DBDataMethod.SQLCommandCheckTablaHasRow($"select * From Roly R where R.NameRoly LIKE '{obj.Name}'") == false)
            {
                await DBDataMethod.SQLCommandNoTransaction(sql);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static async Task<bool> UpdateDataRoly(Data_RolyInf obj)
        {
            string upd = $"  Update Roly SET NameRoly = '{obj.Name}'," +
                         $"ReadUser = '{obj.ReadUser}'," +
                         $"ReadSotrud = '{obj.ReadSotrud}'," +
                         $"ReadClass = '{obj.ReadClass}'," +
                         $"ReadPredmet ='{obj.ReadPredmet}'," +
                         $"CreateAndViewReport = '{obj.CreateAndViewReport}'," +
                         $"TestReady = '{obj.TestReady}'," +
                         $"CreateTest = '{obj.CreateTest}'," +
                         $"CreateGroup = '{obj.CreateGroup}'," +
                         $"ConnectGroup = '{obj.ConnectGroup}'," +
                         $"AddSotrudForPredmet = '{obj.AddSotrudForPredmet}'," +
                         $"DeleteSotrudForPredmet = '{obj.DeleteSotrudForPredmet}'," +
                         $"ViewDataUser = '{obj.ViewDataUser}'," +
                         $"ViewDataSotrud = '{obj.ViewDataSotrud}'," +
                         $"ViewPrepmet = '{obj.ViewPrepmet}'," +
                         $"ViewClass = '{obj.ViewClass}' " +

                         $" where ID_Roly = {obj.Index}";



            var str = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction($"select ID_Roly From Roly R where R.NameRoly LIKE '{obj.Name}'");

            if (str.Trim().Length > 0)
            {
                int index = ParserVariables.GetInt(str);

                if (index == obj.Index)
                {
                    await DBDataMethod.SQLCommandNoTransaction(upd);
                    return true;
                }
                else
                    return false;

            }
            else
            {
                await DBDataMethod.SQLCommandNoTransaction(upd);
                return true;
            }
                      
        }

        public static async Task AddNewTesting(Data_Testing testing)
        {
            try
            {

                string sql = "insert into Testing (NameTesting,CountQuest,ID_CrietingSotrud,ID_Predmet,DateCrieting,Description)" +
                       $" VALUES('{testing.Name}',{testing.CountQuest},{testing.IndexCreator},{testing.IndexPredmet},'{testing.DateCrieting}','{testing.Description}')  SELECT SCOPE_IDENTITY()";

                int indextest = 0;
                using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
                {
                    var index = await command.ExecuteScalarAsync();
                    indextest = int.Parse(index.ToString());
                };

                foreach (var item in testing.Questions)
                {
                    await AddQuest(item, indextest);
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"AddNewTesting: Ошибка при добавлении данных: {ex.Message}");
            }
        }

        private static async Task AddQuest(Data_Question item,int indextest)
        {
          string  sql = "insert into Questions (Quest,QuestImage,Complexity,ID_Testing,CorrectAnswer,IsImaging,Image_format)" +
                            $" values ('{item.Question}','{item.Image}',{item.Complexity},{indextest},{item.CorrecrNumber},'{item.IsImaging}','{item.ImageFormat}') SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(sql, ConnectDataBase.Get()))
            {
                var index = await command.ExecuteScalarAsync();

                foreach (var answer in item.Answer)
                {
                    sql = "insert into Answer (ID_Questions,Answer,AnswerImage,Image_format,IsImaging,Number)" +
                          $" values({index},'{answer.Answer}','{answer.Image}','{answer.ImageFormat}','{answer.IsImaging}',{answer.Number}) ";
                    await DBDataMethod.SQLCommandNoTransaction(sql);
                }
            }
        }



        private static async Task QuestionsEditOrInsert(List<Data_Question> question,int indexTest)
        {
            foreach (var quest in question)
            {
                if (quest.Index == 0)
                {
                    await AddQuest(quest, indexTest);
                }
                else
                {
                    string secondString = $"Update Questions Set Quest='{quest.Question}'," +
                        $"QuestImage = '{quest.Image}'," +
                        $"Complexity = {quest.Complexity}," +
                        $"CorrectAnswer = {quest.CorrecrNumber}," +
                        $"IsImaging = '{quest.IsImaging}'," +
                        $"Image_format = '{quest.ImageFormat}' where ID_Quest = {quest.Index}";

                    await DBDataMethod.SQLCommandNoTransaction(secondString);

                    foreach (var answer in quest.Answer)
                    {

                        string lastString;

                        if (answer.Index == 0)
                        {
                            lastString = "insert into Answer (ID_Questions,Answer,AnswerImage,Image_format,IsImaging,Number)" +
                            $" values({quest.Index},'{answer.Answer}','{answer.Image}','{answer.ImageFormat}','{answer.IsImaging}',{answer.Number}) ";
                        }
                        else
                        {
                            lastString = $"update Answer set Answer = '{answer.Answer}'," +
                                $"AnswerImage = '{answer.Image}'," +
                                $"Image_format= '{answer.ImageFormat}'," +
                                $"IsImaging = '{answer.IsImaging}'," +
                                $"Number = {answer.Number} where ID_Answer = {answer.Index}";
                        }

                        await DBDataMethod.SQLCommandNoTransaction(lastString);
                    }

                }
            }
        }

        public static async Task EditTesting(Data_Testing obj)
        {
            try
            {
                string firstSql = $" update Testing set NameTesting='{obj.Name}', CountQuest = {obj.CountQuest},ID_Predmet = {obj.IndexPredmet}, Description ='{obj.Description}' where ID_Testing = {obj.Index} ";
                await DBDataMethod.SQLCommandNoTransaction(firstSql);
                await QuestionsEditOrInsert(obj.Questions, obj.Index);


                if (obj.DeleteAnswerIndex.Count>0)
                foreach (var deleteAnswer in obj.DeleteAnswerIndex)
                {
                        await DBDataMethod.SQLCommandNoTransaction(string.Format($"DELETE FROM Answer WHERE ID_Answer = {deleteAnswer}"));
                }


               if (obj.DeleteQuestIndex.Count > 0)
               foreach (var deleteQuestin in obj.DeleteQuestIndex)
               {
                   await DBDataMethod.SQLCommandNoTransaction(string.Format($"DELETE FROM Questions WHERE ID_Quest = {deleteQuestin}"));
               }

            }
            catch (Exception ex)
            {
                Logger.Error($"EditTesting: Ошибка при редактировании данных: {ex.Message}");
            }
        }

        public static async Task EditQuestion(Data_QuestionEditOrInsert obj)
        {

            var list = new List<Data_Question>
            {
                obj.Question
            };
            await QuestionsEditOrInsert(list, obj.IndexTest);

            if (obj.DeleteAnswerIndex.Count > 0)
                foreach (var deleteAnswer in obj.DeleteAnswerIndex)
                {
                    await DBDataMethod.SQLCommandNoTransaction(string.Format($"DELETE FROM Answer WHERE ID_Answer = {deleteAnswer}"));
                }
        }

        public static async Task AddToResultTable(Data_TestRun obj)
        {

            string sql = $" insert into Resultation (ID_User,ID_Test,DataTest,CoutCorrectAnswer,CountNotCorectAnswer,CountQuest,Assessment,IsEarly) " +
                         $" VALUES({obj.IndexUser},{obj.Index},'{obj.DateTimeTest}',{obj.CountCorrect},{obj.CountNotCorrect},{obj.Count},{obj.Assessment},'{obj.IsEarly}') ";

            await DBDataMethod.SQLCommandNoTransaction(sql);
                      
        }
    }
}
