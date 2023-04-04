using AdaptiveTestingSystem.Control.CustomControl.TestingControl;
using AdaptiveTestingSystem.Control.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static AdaptiveTestingSystem.Data.Enums;


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui
{
    /// <summary>
    /// Логика взаимодействия для GUI_TestingRun.xaml
    /// </summary>
    /// 

    public class Score
    { 
        public int ScoreUser { get; set; }    
        public double StartAVG { get; set; }    
    }


    public partial class GUI_TestingRun : UserControl, INotifyPropertyChanged
    {
        public double CountAssessment2 { get; set; } = 0;
        public double CountAssessment3 { get; set; } = 0;
        public double CountAssessment4 { get; set; } = 0;
        public double CountAssessment5 { get; set; } = 0;
        public double AVG { get; set; } = 0;

        public int CountQuestingUser { get; set; } = 0;

        Data_MultyServerClient _MultyServerClient;

        public bool IsActive { get; private set; } = false;
 
        public bool IsUpload { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public bool? IsAdaptive { get; set; } = false;
        private int IndexTest { get; set; }
        private DispatcherTimer TimeUpdate;

        private int timeTimer;

        public int TimerTime
        {
            get { return timeTimer; }
            set { timeTimer = value; OnPropertyChanged("TimerTime"); }
        }

        public bool IsOffline { get; private set; }
        public Data_Testing Data { get; private set; }
        public Data_Testing AdaptiveData { get; private set; }
        public Data_Testing NormalyData { get; private set; }
        public Data_TestRun TestRun { get; private set; }

        private Score Score { get; set; } = null;

        private int countQuest = 0;

        public int CountQuest
        {
            get { return countQuest; }
            set { countQuest = value; OnPropertyChanged("CountQuest"); }
        }

        public bool MultyPlayer { get; private set; } = false;

        private CustomTextOrImage QuestionActive = null;
        private bool IsSendUpload = false;

        public GUI_TestingRun(int indexTest,double value)
        {
            InitializeComponent();
            IndexTest = indexTest;
            CountQuestingUser = (int)value;
        }

        public GUI_TestingRun(Data_Testing data)
        {
            InitializeComponent();
            SendData(data, true);
        }

        public GUI_TestingRun(Data_MultyServerClient data)
        {
            InitializeComponent();
            this.MultyPlayer = true;
            this.IndexTest = data.IndexTest;
            this._MultyServerClient = data;
            this.IsAdaptive = data.IsAdaptive;
            this.CountQuestingUser = data.CountQuest;
                   
        }

        public void SetupTimer()
        {
            if (TimeUpdate != null) { TimeUpdate.Stop(); TimeUpdate.Tick -= timeReconnectr_Tick; }

            TimerTime = 120;
            TimeUpdate = new DispatcherTimer();
            TimeUpdate.Tick += new EventHandler(timeReconnectr_Tick);
            TimeUpdate.Interval = TimeSpan.FromSeconds(1);
            TimeUpdate.Start();
        }


        private void timeReconnectr_Tick(object sender, EventArgs e)
        {
        
            if (TimerTime <= 0)
            {
                TimeUpdate.Stop();
                TimeUpdate.Tick -= (timeReconnectr_Tick);
                SendAnswerQuest(false);
            }

            TimerTime--;

        }

        private async void SendAnswerQuest(bool correct = true)
        {
            TimeUpdate.Stop();


            int correctNumber = int.Parse(QuestionActive.Number);
            var correctAnswer = await CheckSelectAnswer(correctNumber, correct);

            if (correctAnswer.Item1 == 0)
            {
                _Main.Instance._Notification.Add("", "Выберите ответ", TypeNotification.Error);
                 return;
            }

            var questingAnswer = new Data_TestRun_Qeust()
            {
                Index = QuestionActive.Index,
                IndexCorrectAnswer= correctAnswer.Item2,
                IndexAnswer = correctAnswer.Item1
            };

            TestRun.List_Qeusts.Add(questingAnswer);

            var answer = new Data_TestRun_Answer();

            if (MultyPlayer)
            {
                correctAnswer.Item3 = QuestionActive.Index;
                SendDataMultyPlayer(correctAnswer);
            }


            ClearAnswerComponent();

            if (Quest.Children.Count > 0)
            {
                (Quest.Children[0] as CustomTextOrImage).ImageView -= ThisImageView;
                Quest.Children.Clear();
            }
         
            NextQuest();
        }

        private void SendDataMultyPlayer((int, int,int) correctAnswer)
        {

            Data_ResultTesting resultTesting = new Data_ResultTesting()
            {
                avg = AVG,
                CountAssessment2=(int)CountAssessment2,
                CountAssessment3= (int)CountAssessment3,
                CountAssessment4= (int)CountAssessment4,
                CountAssessment5= (int)CountAssessment5,
            };


            Data_MultyServerSendAnswer data_Multy = new Data_MultyServerSendAnswer()
            {
                IndexServer = _MultyServerClient.IndexServer,
                NumberAnswer = correctAnswer.Item1,
                NumberCorrect = correctAnswer.Item2,
                IndexQuest = correctAnswer.Item3,
                resultTesting= resultTesting,
                CountQuest = int.Parse(maxQuest.Text)
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_GetAnswerClientForActiveTestServer",
                Json = JsonSerializer.Serialize(data_Multy)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));

        }

        private async Task<(int, int,int)> CheckSelectAnswer(int correctNumber, bool correct = true)
        {
            int indexAnswer = 0;
            int indexCorrect = 0;


            foreach (AnswerControl item in answerComponent.Children)
            {
                if (correct)
                {
                    if (item.IsSelect)
                    {
                        indexAnswer = item.Index;
                        break;
                    }
                }
                else
                {
                    var number = item.Number.Replace("#", string.Empty);
                    if (int.Parse(number) == correctNumber) continue;
                    indexAnswer = item.Index;
                    break;
                }

                await Task.Delay(20);
            }

            if (indexAnswer == 0)
            {
                return (indexAnswer, indexCorrect,0);
            }

            foreach (AnswerControl item in answerComponent.Children)
            {
                var number = item.Number.Replace("#", string.Empty);
                if (int.Parse(number) == correctNumber)
                {
                    indexCorrect = item.Index;
                    break;
                }
            }


            return (indexAnswer, indexCorrect,0);
        }

        private async void NextQuest()
        {
            Data_Question quest = null;

            if (IsAdaptive == true)
            {
                if (AdaptiveData.Questions.Count > 0)
                {
                    CountQuest++;
                    SetupTimer();

                    Random random = new Random();
                    quest = AdaptiveData.Questions[random.Next(0, AdaptiveData.Questions.Count)];
                    CustomTextOrImage questionControl = CreateQuest(quest);
                    questionControl.ImageView += ThisImageView;
                    QuestionActive = questionControl;
                    Quest.Children.Add(questionControl);
                    await CreateAnswer(quest);
                    AdaptiveData.Questions.Remove(quest);
                }
                else
                {
                    SendResult();
                }
            }
            else
            {
                if (NormalyData.Questions.Count > 0)
                {
                    CountQuest++;
                    SetupTimer();

                    Random random = new Random();
                    quest = NormalyData.Questions[random.Next(0, NormalyData.Questions.Count)];
                    CustomTextOrImage questionControl = CreateQuest(quest);
                    questionControl.ImageView += ThisImageView;
                    QuestionActive = questionControl;
                    Quest.Children.Add(questionControl);
                    await CreateAnswer(quest);
                    NormalyData.Questions.Remove(quest);
                }
                else
                {
                    SendResult();
                }
            }
        }

        private async Task CreateAnswer(Data_Question quest)
        {
            foreach (var item in quest.Answer)
            {
                var answer = new AnswerControl()
                {
                    ImageData = Converter.ToByteArray(item.Image),
                    Answer = item.Answer,
                    Index = item.Index,
                    Number = (answerComponent.Children.Count + 1).ToString(),
                    IsImaging = item.IsImaging,
                    ImageHeight = 100,
                    ImageWidth = answerComponent.ActualWidth - 70,
                };

                answer.ImageView += ThisImageView;

                answerComponent.Children.Add(answer);

                await Task.Delay(50);
            }
        }

        private CustomTextOrImage CreateQuest(Data_Question quest)
        {
            return new CustomTextOrImage()
            {
                ImageData = Converter.ToByteArray(quest.Image),
                IsImaging = quest.IsImaging,
                Title = quest.IsImaging ? "Картинка" : quest.Question,
                Number = quest.CorrecrNumber.ToString(),
                Index = quest.Index,
                Image_Format = quest.ImageFormat,
                ImageHeight = 100,
                ImageWidth = Quest.ActualWidth - 70,
                ViewButtonAndNumber = false,
                Style = this.TryFindResource("NoMouseOverBGChange") as Style
            };
        }

        private void SendResult(bool isEarly = false)
        {

            if (MultyPlayer)
            {
                SetStatus(Code.TestingCompleted);
            }          
           
            overlay.Visibility = Visibility.Visible;
            Test.Visibility = Visibility.Collapsed;

            descr.Text = "Тест окончен";
            TestRun.Count = int.Parse(maxQuest.Text);
            TestRun.CountAnswer = TestRun.List_Qeusts.Count;
            TestRun.IsEarly = isEarly;

            GUI_TestReady.Instance.SetUI(new GUI_TestResult(TestRun, IsOffline));
            

        }

        private void ClearAnswerComponent()
        {
            if (answerComponent.Children.Count == 0) return;


            foreach (AnswerControl item in answerComponent.Children)
            { 
               item.ImageView -= ThisImageView;
            }
             answerComponent.Children.Clear();
        }

        private void ThisImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
          
            if (IsOffline) return;
            
            SendInfo(true,Visibility.Collapsed);
            StartViewData();         
        }

        private async void SendServerForEditServer(int index)
        {

            await Task.Delay(500);

            Data_TestingView view = new Data_TestingView()
            {
                Index = index,
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_ViewTestingData", view);
        }

        public void SendInfo(double size, double maxsize)
        {

            double c = maxsize / size;
            double perc = 100 / c;
            progress.Value = (perc);
            IsUpload = true;



            if (IsSendUpload == false && MultyPlayer)
            {
                SetStatus(Code.UploadingData);
                IsSendUpload = true;
            }


            //Command_SetStatusUserForActiveTestServer

        }

        private void SetStatus(Code code)
        {
            Data_PacketServerStatusUser view = new Data_PacketServerStatusUser()
            {
                IndexServer = _MultyServerClient.IndexServer,
                IsCode = code
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_SetStatusUserForActiveTestServer",
                Json = JsonSerializer.Serialize(view)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
        }

        public void SendData(Data_Testing data,bool isOffline = false)
        {
         

            IsActive = false;
            IsUpload = false;
            IsOffline = isOffline;
       
            Data = data;
            AdaptiveData = new Data_Testing();
            NormalyData = new Data_Testing();
            TestRun = new Data_TestRun()
            {
                Index = data.Index,
                List_Qeusts = new List<Data_TestRun_Qeust>()
                { 
                    
                },
                IndexUser = _Main.Instance.MyAccount.ID,
                IsEarly = false
            };

            maxQuest.Text = CountQuestingUser.ToString();
            CountQuest = 0;

            if (MultyPlayer == false)
            {
                progress.Visibility = Visibility.Collapsed;
                titlePercent.Visibility = Visibility.Collapsed;
                descr.Text = "Загрузка завершена, формирую тест";
                overlay.Visibility = Visibility.Collapsed;
                Test.Visibility = Visibility.Visible;        
                GenerateQuest();
            }
            else
            {
                progress.Visibility = Visibility.Collapsed;
                titlePercent.Visibility = Visibility.Collapsed;
                descr.Text = "Загрузка завершена, ожидаю команды к запуску теста";
                SetStatus(Code.UploadSuccessfull);
            }
        }

        public void StartMulty(bool isAdaptive)
        { 
             IsAdaptive = isAdaptive;
             GenerateQuest();
   
        }

        private async void GenerateQuest()
        {
            Random random = new Random();
            IsActive = true;
            if (IsAdaptive == true)
            {
                IsActive = true;
                progress.Visibility = Visibility.Visible;
                titlePercent.Visibility = Visibility.Visible;
                descr.Text = "Генерирую вопросы";


                int score = 100;
                Score = new Score()
                {
                    ScoreUser = score
                };

                AdaptiveData.Questions = new List<Data_Question>();

                List<Data_Question> list = null;
                if (AVG < 3) { score = 25; list = Data.Questions.FindAll(o => o.Complexity < 6); }
                else if (AVG >= 3 && AVG < 4) { score = 45; list = Data.Questions.FindAll(o => (o.Complexity >= 2 && o.Complexity < 7)); }
                else if (AVG >= 4 && AVG < 5) { score = 55; list = Data.Questions.FindAll(o => (o.Complexity >= 4 && o.Complexity < 8)); }
                else { score = 85; list = Data.Questions.FindAll(o => (o.Complexity >= 6 && o.Complexity <= 10)); }

                Score.ScoreUser = score;
                Score.StartAVG = AVG;

    

                if (list.Count >= CountQuestingUser)
                {
                    await GenerationQuestList(list, random);
                }
                else
                {
                    while (list.Count < CountQuestingUser)
                    {
                        if (IsActive == false) return;

                        var item = Data.Questions[random.Next(Data.Questions.Count)];
                        if (item != null)
                        {
                            if (list.Find(o => o == item) == null)
                            {
                                list.Add(item);
                                continue;
                            }
                        }
                    }

                    await GenerationQuestList(list, random);
                }

            }
            else
            {
                List<Data_Question> list = null;
                list = Data.Questions.FindAll(o => o.Complexity > 0);
                NormalyData.Questions = new List<Data_Question>();
                await GenerationQuestList(list, random);
            }


            progress.Visibility = Visibility.Collapsed;
            titlePercent.Visibility = Visibility.Collapsed;
            descr.Text = "";
            overlay.Visibility = Visibility.Collapsed;
            Test.Visibility = Visibility.Visible;
      

            SetupTimer();
            NextQuest();
        }

        private async Task GenerationQuestList(List<Data_Question> list, Random random)
        {
 
            for (int i = 0; i < CountQuestingUser; i++)
            {
                while (IsActive)
                {
                    var quest = list[random.Next(list.Count)];
                    if (quest != null)
                    {
                        if (IsAdaptive==true)
                        {

                            if (AdaptiveData.Questions.Find(o => o == quest) == null)
                            {
                                AdaptiveData.Questions.Add(quest);
                                double c = CountQuestingUser / AdaptiveData.Questions.Count;
                                double perc = 100 / c;
                                progress.Value = (perc);


                                break;
                            }
                        }
                        else
                        {
                            if (NormalyData.Questions.Find(o => o == quest) == null)
                            {
                                NormalyData.Questions.Add(quest);
                                double c = CountQuestingUser / NormalyData.Questions.Count;
                                double perc = 100 / c;
                                progress.Value = (perc);


                                break;
                            }
                        }
                    }

                    await Task.Delay(25);
                }


                await Task.Delay(25);
            }
        }

        public void StartViewData()
        {
            progress.IsIndeterminate = true;
            progress.Value = 0;
            progress.Maximum = 100;
            progress.Minimum = 0;
            titlePercent.Visibility = Visibility.Visible;
            Test.Visibility = Visibility.Collapsed;


            Data_ResultTesting user = new Data_ResultTesting()
            {
                Index = _Main.Instance.MyAccount.ID
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_GetUserResultTesting",
                Json = JsonSerializer.Serialize(user)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));

        }

        internal void SendInfo(bool isind = false,Visibility title = Visibility.Visible)
        {
            progress.IsIndeterminate = isind;
            progress.Visibility= Visibility.Visible;
            overlay.Visibility= Visibility.Visible;
            descr.Text = "Загрузка данных. Ожидайте...";
           /// titlePercent.Visibility = title;
            progress.Value = 0;
            ErrorButtons.Visibility = Visibility.Collapsed;
        }

        public void SetError()
        {
            descr.Text = "Произошла ошибка, повторите загрузку";
            ErrorButtons.Visibility= Visibility.Visible;
            progress.Visibility = Visibility.Collapsed;
            titlePercent.Visibility = Visibility.Collapsed;
 
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            GUI_TestReady.Instance.Close();
        }

        private void errorButton_Click(object sender, RoutedEventArgs e)
        {
            progress.IsIndeterminate = true;
            progress.Visibility = Visibility.Visible;
            overlay.Visibility = Visibility.Visible;
            descr.Text = "Загрузка данных. Ожидайте...";
            progress.Value = 0;
            ErrorButtons.Visibility = Visibility.Collapsed;

            StartViewData();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        private void NextQuest_Click(object sender, RoutedEventArgs e)
        {
            SendAnswerQuest();
        }

        private void cancelTest_Click(object sender, RoutedEventArgs e)
        {
            SendResult(true);
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {

            if (TimeUpdate != null)
            {
                TimeUpdate.Stop();
                TimeUpdate.Tick -= (timeReconnectr_Tick);
            }
            
        }

        public void UploadDataTest(Data_ResultTesting data_ResultTesting)
        {
            CountAssessment2 = data_ResultTesting.CountAssessment2;
            CountAssessment3 = data_ResultTesting.CountAssessment3;
            CountAssessment4 = data_ResultTesting.CountAssessment4;
            CountAssessment5 = data_ResultTesting.CountAssessment5;

                     
            try
            {
                AVG = ((5 * CountAssessment5) + (4 * CountAssessment4) + (3 * CountAssessment3) + (2 * CountAssessment2)) / (CountAssessment2 + CountAssessment3 + CountAssessment4 + CountAssessment5);

                AVG = Math.Round(AVG, 2);
            } catch { AVG = 0; }



            SendServerForEditServer(IndexTest);
        }


        public void ReSumScore((int, int, int, int) score)
        {
            CountAssessment2 += score.Item1;
            CountAssessment3 += score.Item2;
            CountAssessment4 += score.Item3;
            CountAssessment5 += score.Item4;

            try
            {
                AVG = ((5 * CountAssessment5) + (4 * CountAssessment4) + (3 * CountAssessment3) + (2 * CountAssessment2)) / (CountAssessment2 + CountAssessment3 + CountAssessment4 + CountAssessment5);
                AVG = Math.Round(AVG, 2);
            } catch
            {
                AVG = 0;
            }

        }
    }
}
