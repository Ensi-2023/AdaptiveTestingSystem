using System.Data.SqlClient;


string ConnectionString = $"Data Source = ARKADY\\SQLEXPRESS; Initial Catalog = newBdChageResult; Integrated Security = True; MultipleActiveResultSets=True";



List<int> ints = new List<int>()
{
    1,2,3,4,5
};


Random random= new Random();

Console.WriteLine("Обновление базы данных");
void Help()
{

    Console.WriteLine("Доступные Команды: ");
    Console.WriteLine("Клонировать вопросы: --вопросы 30 [клонировать вопросы 30 раз]");
    Console.WriteLine("Клонировать результаты: --результаты 30 [клонировать результаты 30 раз]");
}

Help();

while (true)
{
    Console.Write("> ");
    string comm = Console.ReadLine();
    if (comm.Trim().ToLower() == "выход") return;
    if (comm.Trim().ToLower() == "help") Help();
    if (comm.Trim().ToLower().Contains("--вопросы"))
    {
        int count = 0;
        try
        {   
            var mas = comm.Trim().ToLower().Split(' ');
            count = int.Parse(mas[1]);
        } catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
            continue;
        }
        SqlConnection sqlConnection = new SqlConnection(ConnectionString);
        sqlConnection.Open();
        while (count > 0)
        {

            var index = ints[random.Next(ints.Count)];

            Console.WriteLine($"Выбран вопрос #{index}");

            using (SqlCommand command = new SqlCommand($"select * from Questions where ID_Quest = {index}", sqlConnection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            string sql = "insert into Questions (Quest,QuestImage,Complexity,ID_Testing,CorrectAnswer,IsImaging,Image_format)" +
                             $" values ('{reader.GetValue(1)}','{reader.GetValue(2)}',{random.Next(4, 8)},{reader.GetValue(4)},1,'false','') SELECT SCOPE_IDENTITY()";


                            Console.WriteLine($"Добавляю вопрос #{index}");

                            SqlCommand sendCommand = new SqlCommand(sql, sqlConnection);
                            var indexNewQuest = await sendCommand.ExecuteScalarAsync();


                            using (SqlCommand answerCommand = new SqlCommand($"select * from Answer where ID_Questions = {index}", sqlConnection))
                            {
                                using (var answerReader = await answerCommand.ExecuteReaderAsync())
                                {
                                    if (answerReader.HasRows)
                                    {
                                        while (await answerReader.ReadAsync())
                                        {
                                            string lastString = "insert into Answer (ID_Questions,Answer,AnswerImage,Image_format,IsImaging,Number)" +
                                                                $" values({indexNewQuest},'{answerReader.GetValue(2)}','{answerReader.GetValue(3)}','{answerReader.GetValue(4)}'" +
                                                                $",'{answerReader.GetValue(5)}',{answerReader.GetValue(6)}) ";



                                            sendCommand = new SqlCommand(lastString, sqlConnection);
                                            await sendCommand.ExecuteNonQueryAsync();


                                            Console.WriteLine($"Добавляю ответ #{answerReader.GetValue(6)}");
                                        }
                                    }
                                }

                            }


                        }
                    }
                }
            }


            count--;
            await Task.Delay(30);
        }
        sqlConnection.Close();
    }
    if (comm.Trim().ToLower().Contains("--результаты"))
    {
        int count = 0;
        try
        {
            var mas = comm.Trim().ToLower().Split(' ');
            count = int.Parse(mas[1]);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            continue;
        }
        SqlConnection sqlConnection = new SqlConnection(ConnectionString);
        sqlConnection.Open();
        int countsRequest = 0;
        while (count > 0)
        {

            if (countsRequest == 10) break;

            string sql = $"select * from Resultation where ID_Result = {ints[random.Next(1, ints.Count)]}";

            using (var selectResult = new SqlCommand(sql, sqlConnection))
            {
                using (var reader = await selectResult.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        countsRequest = 0;

                        while (await reader.ReadAsync())
                        {
                        

                            var idtest = reader.GetValue(1);
                            var iduser = reader.GetValue(2);

                            var correctAnsw = reader.GetValue(3);
                            var notcorrectAnsw = reader.GetValue(4);
                            var countquest = reader.GetValue(5);
                            var score = reader.GetValue(6);
                            var date = reader.GetValue(7);
                            var isearly = reader.GetValue(8);


                            sql = "insert into Resultation(ID_Test,ID_User,CoutCorrectAnswer,CountNotCorectAnswer,CountQuest,Assessment,DataTest,IsEarly) " +
                                         $" VALUES ({idtest},{random.Next(2,4)},{correctAnsw},{notcorrectAnsw},{countquest},{random.Next(2,6)},'{date}','{isearly}')";

                            SqlCommand sendCommand = new SqlCommand(sql, sqlConnection);
                            await sendCommand.ExecuteScalarAsync();

                            Console.WriteLine($"Клонирование удалось, осталось: {count} раз");

                        }


                    }
                    else
                    {
                        
                        countsRequest++;
                        Console.WriteLine($"Попытка сгенерировать запрос #{countsRequest}");
                        continue; 
                    }
                }
            }


           count--;  
        }
    }
}