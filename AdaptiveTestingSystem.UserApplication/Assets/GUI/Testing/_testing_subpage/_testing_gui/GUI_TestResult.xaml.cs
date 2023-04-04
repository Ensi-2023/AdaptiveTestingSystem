using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui
{
    /// <summary>
    /// Логика взаимодействия для GUI_TestResult.xaml
    /// </summary>
    public partial class GUI_TestResult : UserControl
    {
        Data_TestRun data_Result;
        bool IsOffline { get; set; } = false;
        public GUI_TestResult(Data_TestRun testRun, bool isOffline = false)
        {
            InitializeComponent();
            data_Result = testRun;
            overlay.Visibility= Visibility.Visible;
            IsOffline=isOffline;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            GUI_TestReady.Instance.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {    
            CreateResult();
        }

        private void SendResultToServer(int assessment, int corretQuest, int notCorretQuest)
        {
            if (IsOffline) return;


            data_Result.Assessment = assessment;
            data_Result.DateTimeTest = DateTime.Now.ToString();
            data_Result.CountCorrect = corretQuest;
            data_Result.CountNotCorrect = notCorretQuest;

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_SendResultTestig",
                Json = JsonSerializer.Serialize(data_Result)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
        }

        private async void CreateResult()
        {
            await Task.Delay(3000);
            overlay.Visibility = Visibility.Collapsed;

            double correct = 0;
            double nocorrect;

            if (data_Result.CountAnswer > 0) {

                foreach (var item in data_Result.List_Qeusts)
                {
                    if (item.IndexAnswer == item.IndexCorrectAnswer)
                        correct++;      
                }

                nocorrect = data_Result.Count - correct;
            }
            else
                nocorrect = data_Result.Count;

            double c = data_Result.Count / correct;
            double perc = 100 / c;

            resultUI.Visibility = Visibility.Visible;

            corretQuest.Text = correct.ToString();
            notCorretQuest.Text = nocorrect.ToString();
            allQuest.Text = data_Result.Count.ToString();

            int scoreResult;
            if (perc <= 30)
            {
                scoreResult = 2;
            }
            else if (perc > 30 && perc <= 50) scoreResult = 3;
            else if (perc > 50 && perc <= 80) scoreResult = 4;
            else scoreResult = 5;


            SendResultToServer(scoreResult, (int)correct, (int)nocorrect);

            switch (scoreResult)
            {
                case 2: Set("Ты очень плохо знаешь материал", "2", (Brush)this.TryFindResource("TestPerc30")); break;
                case 3: Set("Ты плохо знаешь материал", "3", (Brush)this.TryFindResource("TestPerc50")); break;
                case 4: Set("Ты хорошо знаешь материал", "4", (Brush)this.TryFindResource("TestPerc80")); break;
                case 5: Set("Ты отлично знаешь материал", "5", (Brush)this.TryFindResource("TestPerc100")); break;
            }
        }

        private void Set(string _desc, string _score, Brush _brush)
        {
            score.Text = _score.ToString();
            desc.Text = _desc;
            Body.Background= _brush;
            score.Foreground= _brush;
        }
    }
}
