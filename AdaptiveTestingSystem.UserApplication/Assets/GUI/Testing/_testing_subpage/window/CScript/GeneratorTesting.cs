using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window.CScript
{
    public class GeneratorTesting
    {
        
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;



        public GeneratorTesting()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

        }

        public void Start(TestingItem testing, GUI_ViewCreateQuestions gUI_ViewCreateQuestions,bool isEdit, List<int> deleteQuestion, List<int> deleteAnswer, bool save = false,bool testingoffline = false)
        {
            Generated(testing, gUI_ViewCreateQuestions, isEdit, deleteQuestion, deleteAnswer, save, testingoffline);
        }

       
        private async void Generated(TestingItem testing, GUI_ViewCreateQuestions gui_ViewCreateQuestions, bool isEdit, List<int> deleteQuestion, List<int> deleteAnswer, bool save=false, bool testingoffline = false)
        {

            gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Генерация теста", "");

            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            Logger.Debug($"delete quest: {deleteQuestion.Count}, delete answer: {deleteAnswer.Count}");

            var questing = new List<Data_Question>();
            var test = new Data_Testing()
            {
                Name = testing.NameTest,
                IndexPredmet= testing.IndexPredmet,
                NamePredmet= testing.NamePredmet,
                CountQuest= testing.Questions.Count,
                DateCrieting=DateTime.Now.ToString(),
                IndexCreator=_Main.Instance.MyAccount.ID,
                NameCreator = _Main.Instance.MyAccount.GetName(),
                Description = testing.Description,
                Index = testing.Index,
                DeleteQuestIndex = deleteQuestion,
                DeleteAnswerIndex= deleteAnswer,
            };
            await Task.Delay(10);

            gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Вопрос #1 из {testing.Questions.Count}");
            
            for(int i=0;i< testing.Questions.Count;i++)
            {

                if (gui_ViewCreateQuestions.IsStop) return;

                var quest = testing.Questions[i];
                List<Data_Answer> answer = new List<Data_Answer>();

                for (int j = 0; j < quest.Answer.Count; j++)
                {
                    if (gui_ViewCreateQuestions.IsStop) return;

                    gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Вопрос #{i+1} из {testing.Questions.Count}\nСобираю ответы #{j+1} из {testing.Questions[i].Answer.Count}");
                    
                    var ans = quest.Answer[j];

                    string imageanswhex = string.Empty;

                    if (ans.ImageData != null)
                    {
                        imageanswhex = Convert.ToHexString(ans.ImageData);
                    }

                    if (ans.IsImaging == false)
                    {
                        imageanswhex =string.Empty;
                    }

                    answer.Add(new Data_Answer()
                    {
                        Answer = ans.Text,
                        Image =  imageanswhex,
                        ImageFormat= ans.Extension,
                        IsImaging= ans.IsImaging,
                        Number= ans.Number,
                        Index = ans.Index
                    });

                    await Task.Delay(10);
                }

                string imagehex = string.Empty;

                if (quest.Image!=null) {
                    imagehex = Convert.ToHexString(quest.Image);
                }

                if (quest.IsImaging == false)
                {
                    imagehex = string.Empty;
                }

                questing.Add(new Data_Question()
                {
                    Answer = answer,
                    Complexity = quest.Complexity,
                    CorrecrNumber = quest.CorrectAnswer,
                    Image = imagehex,
                    IsImaging= quest.IsImaging,
                    ImageFormat= quest.Image_format,
                    Question= quest.Question,
                    Index = quest.Index,
                });

                await Task.Delay(10);
            }


            test.Questions= questing;

            if (testingoffline)
            {
                GUI_TestReady testReady = new GUI_TestReady(test);
                gui_ViewCreateQuestions.OverlayShow(false);
                testReady.Show();
                return;
            }


            if (save)
            {
                await SaveData(gui_ViewCreateQuestions, test, false);     
                return;
            }


            if (MessageShow.Show("Сохранить тест в файл ?", "Сохранение", MessageShow.Type.Question) == true)
            {
        
                await SaveData(gui_ViewCreateQuestions, test,isEdit, true);            
                return;
            }
            else
            {
                test.Login = _Main.Instance.MyAccount.Login;
                ThreadSendingTestToServer.SendTest(test, cancelTokenSource, token,isEdit);
            }

        }

        private async Task SaveData(GUI_ViewCreateQuestions gui_ViewCreateQuestions, Data_Testing test,bool isedit,bool sendToServer = false)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Factory.StartNew((() =>
                {
                    string nameFolder;
                    var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        nameFolder = dialog.SelectedPath;
                        string filename = $"{nameFolder}\\exp_test_{DateTime.Now.ToString().Replace('.', '_').Replace(' ', '_').Replace(':', '_')}.xml";
         
                        gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Создаю файл данных");

                        string json = JsonSerializer.Serialize(test);

                        if (test != null)
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data_Testing));
                            Application.Current.Dispatcher.Invoke(() => 
                            {
                                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                                {
                                    xmlSerializer.Serialize(fs, test);
                                }
                            });



                            if (sendToServer)
                            {
                                test.Login = _Main.Instance.MyAccount.Login;
                                ThreadSendingTestToServer.SendTest(test, cancelTokenSource, token, isedit);
                            }

                        } 
                        
                    }

               

                    gui_ViewCreateQuestions.OverlayShow(false);

                  

                }),token);
            });
        }

        public void StartImport(GUI_ViewCreateQuestions gui_ViewCreateQuestions, string file)
        {
            var testing = new Data_Testing();
            var collection = new TestingItem();

            gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Проверка файла", "");

            if (File.Exists(file))
            {

                Application.Current.Dispatcher.Invoke(async () =>
                {
                    await Task.Factory.StartNew((async () => {

   
                        //using (FileStream fstream = File.OpenRead(file))
                        //{
                        //    // выделяем массив для считывания данных из файла


                        //    if (token.IsCancellationRequested)  // проверяем наличие сигнала отмены задачи
                        //    {
                        //        return;     //  выходим из метода и тем самым завершаем задачу
                        //    }

                        //    gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Считываю данные из файла", $"{0} из {fstream.Length}");

                        //    byte[] buffer = new byte[fstream.Length];
                        //    // считываем данные
                        //    await fstream.ReadAsync(buffer, 0, buffer.Length);
                        //    // декодируем байты в строку
                        //    string textFromFile = Encoding.Default.GetString(buffer);

                        //    json += textFromFile;
                        //    gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Считываю данные из файла", $"{json.Length} из {fstream.Length}");


                        //}



                        ///Сделать проверку Index
                    

                        try
                        {
                            // testing = JsonSerializer.Deserialize<Data_Testing>(json);


                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data_Testing));

                            // десериализуем объект
                            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                            {
                                testing = xmlSerializer.Deserialize(fs) as Data_Testing;
                            }


                        }
                        catch (Exception ex)
                        {
                            gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.error, "Произошла ошибка", $"{ex.Message}");
                            return;
                        }

                        await Task.Delay(10);

                        collection.Questions = new List<QuestionItem>();
                        collection.NameTest = testing.Name;
                        collection.IndexPredmet = testing.IndexPredmet;
                        collection.Description = testing.Description;
                        collection.NamePredmet=testing.NamePredmet;
                 

                        Application.Current.Dispatcher.Invoke(()=> {
                        foreach (var obj in testing.Questions)
                        {
                            var answer = new List<ImageTextBoxControl>();

                            foreach (var ans in obj.Answer)
                            {
                             
                                    answer.Add(new ImageTextBoxControl()
                                    {
                                        ImageData = Converter.ToByteArray(ans.Image),
                                        IsImaging = ans.IsImaging,
                                        Text = ans.Answer,
                                        Number = ans.Number,
                                        Index = ans.Index,
                                    });
                                
                            }
                                                        
                            collection.Questions.Add(new QuestionItem()
                            { 
                                Complexity= obj.Complexity,
                                CorrectAnswer=obj.CorrecrNumber,
                                Image= Converter.ToByteArray(obj.Image),
                                Image_format=obj.ImageFormat,
                                Index=obj.Index,
                                IsImaging=obj.IsImaging,
                                Question=obj.Question,
                                Answer = answer,
                                
                            });
                        }
                        });
               

                        gui_ViewCreateQuestions.SetListQuestions(collection);
                        gui_ViewCreateQuestions.OverlayShow(false);



                    }), token);

                });
  

            }
            else
            {
                gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.error, "Произошла ошибка", $"Файл {file} не найден");
            }

            
        }



        public void StartCreateTest(GUI_GeneratorTest generatorTest, Data_Testing data)
        {
            var testing = data;
            var collection = new TestingItem();

   

            if (data!=null)
            {

                Application.Current.Dispatcher.Invoke(async () =>
                {
                    await Task.Factory.StartNew((async () => {

                        _Main.Instance.OverlayShow(true, TypeOverlay.loading, "", "");

                        //using (FileStream fstream = File.OpenRead(file))
                        //{
                        //    // выделяем массив для считывания данных из файла


                        //    if (token.IsCancellationRequested)  // проверяем наличие сигнала отмены задачи
                        //    {
                        //        return;     //  выходим из метода и тем самым завершаем задачу
                        //    }

                        //    gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Считываю данные из файла", $"{0} из {fstream.Length}");

                        //    byte[] buffer = new byte[fstream.Length];
                        //    // считываем данные
                        //    await fstream.ReadAsync(buffer, 0, buffer.Length);
                        //    // декодируем байты в строку
                        //    string textFromFile = Encoding.Default.GetString(buffer);

                        //    json += textFromFile;
                        //    gui_ViewCreateQuestions.OverlayShow(true, TypeOverlay.loading, "Считываю данные из файла", $"{json.Length} из {fstream.Length}");


                        //}




                        await Task.Delay(10);

                        collection.Questions = new List<QuestionItem>();
                        collection.NameTest = testing.Name;
                        collection.IndexPredmet = testing.IndexPredmet;
                        collection.Description = testing.Description;
                        collection.NamePredmet = testing.NamePredmet;


                 

                        Application.Current.Dispatcher.Invoke(() => {
                            foreach (var obj in testing.Questions)
                            {
                                var answer = new List<ImageTextBoxControl>();

                                foreach (var ans in obj.Answer)
                                {

                                    answer.Add(new ImageTextBoxControl()
                                    {
                                        ImageData = Converter.ToByteArray(ans.Image),
                                        IsImaging = ans.IsImaging,
                                        Text = ans.Answer,
                                        Number = ans.Number,
                                        Index= ans.Index,
                                        
                                    });

                                    Logger.Debug($"Quest: #{obj.Index} answer #{ans.Number} {ans.Index}");

                                }

                                collection.Questions.Add(new QuestionItem()
                                {
                                    Complexity = obj.Complexity,
                                    CorrectAnswer = obj.CorrecrNumber,
                                    Image = Converter.ToByteArray(obj.Image),
                                    Image_format = obj.ImageFormat,
                                    Index = obj.Index,
                                    IsImaging = obj.IsImaging,
                                    Question = obj.Question,
                                    Answer = answer,

                                });
                            }

                    
                           
                            generatorTest.nameTesting.Text = testing.Name;
                            generatorTest.EditingPredmet = testing.NamePredmet;
                            generatorTest.TestingItem = collection;


                        


                            generatorTest.Update();


                        });

                       



                    }), token);

                });


            }
            else
            {
                _Main.Instance.OverlayShow(true, TypeOverlay.error, "Произошла ошибка", $"Data null");
            }


        }



        internal void StopThread()
        {
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
            _Main.Instance.Client.CancelSend();
        }

        internal void CreateThread()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        internal void StopThreadData()
        {
            try
            {
                ThreadManager.CloseActiveThread();



                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
                _Main.Instance.Client.CancelSendData();
            }
            catch (Exception ex)
            { 
                
            }
      
        }
    }
}
