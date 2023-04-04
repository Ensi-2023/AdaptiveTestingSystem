using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_QuestViewer.xaml
    /// </summary>
    public partial class GUI_QuestViewer : UserControl
    {

        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;

        private List<CustomTextOrImage> answerList { get; set; }
        public Data_Question Data { get; set; }
        public int IndexQuest { get; private set; }
        public int IndexTest { get; private set; }
        public GUI_QuestViewer(int index,int indexTest)
        {
            InitializeComponent();
            IndexQuest= index;
            IndexTest= indexTest;
    
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            _Main.Instance.OverlayShow(true,TypeOverlay.loading,"Просмотр данных вопроса","Ожидайте...");
            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_QuestionsDataViewer",
                Json = JsonSerializer.Serialize(new Data_QuestDataViewer() { Index = IndexQuest})
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                Search(text);
            }
        }

        private async void Search(string text)
        {
            Body.Children.Clear();
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Поиск", "Ожидайте...");
            foreach (CustomTextOrImage item in answerList)
            {
                if (text.Trim() == string.Empty)
                {
                    Body.Children.Add(item);
                    await Task.Delay(50);
                    continue;
                }

                if (item.IsImaging)
                {
                    if (item.Title.Trim().ToLower().Contains(text.Trim().ToLower()))
                    {
                        Body.Children.Add(item);
                        await Task.Delay(50);
                        continue;
                    }
                }
                else
                {
                    if (item.Title.Trim().ToLower().Contains(text.Trim().ToLower()))
                    {
                        Body.Children.Add(item);
                        await Task.Delay(50);

                    }
                }
            }
            _Main.Instance.OverlayShow(false);
            searchBox.Focus();  
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        public async void SetTest(Data_Question obj)
        {
            answerList = new List<CustomTextOrImage>();
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            Body.Children.Clear();
            Data = obj;
            QuestionViewer.IsImaging = Data.IsImaging;
            QuestionViewer.ImageData = Converter.ToByteArray(Data.Image);
            QuestionViewer.Title = Data.Question;
            QuestionViewer.ImageHeight = 120;
            QuestionViewer.ImageWidth = QuestionViewer.ActualWidth-50;
            QuestionViewer.ImageView += QuestionViewer_ImageView;

            await SetAnswer(token);
         
            Logger.Message($"Data Accept: {obj.Index} {obj.Answer.Count}");
        }

        private async Task SetAnswer(CancellationToken token)
        {
            foreach (var item in Data.Answer)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                CustomTextOrImage answerControl = new CustomTextOrImage()
                {
                    ImageData = Converter.ToByteArray(item.Image),
                    IsImaging = item.IsImaging,
                    Title = item.IsImaging?"Картинка" : item.Answer,
                    Number = (Body.Children.Count + 1).ToString(),
                    Index = item.Index,
                    Image_Format = item.ImageFormat,
                    ImageHeight = 80,
                    ImageWidth = Body.ActualWidth-70,
                };


                answerControl.SizeChanged += AnswerControl_SizeChanged;
                answerControl.ImageView += QuestionViewer_ImageView;
                answerList.Add(answerControl);
                Body.Children.Add(answerControl);
                await Task.Delay(50);

            }
        }

        private void AnswerControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var item = sender as CustomTextOrImage;
            if (item != null)
            { 
                item.ImageWidth= Body.ActualWidth - 70;
            }
        }

        private void QuestionViewer_ImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }

        private void DeleteProperty(CustomTextOrImage control)
        {
            control.ImageView -= QuestionViewer_ImageView;
            control.SizeChanged -= AnswerControl_SizeChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
         
            DeleteProperty(QuestionViewer);

            foreach (var item in Body.Children)
            {
                var control = item as CustomTextOrImage;
                if (control != null)
                {
                    DeleteProperty(control);
                }
            }

            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
            }
        }

        private void QuestionViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            QuestionViewer.ImageWidth = QuestionViewer.ActualWidth - 50;
        }

        private async void deleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Удалить вопрос ?\nВосстановить будет невозможно", "", MessageShow.Type.Question) == true)
            {
                var list = new List<Data_Question>()
                {
                    new Data_Question() { Index = IndexQuest }
                };

                _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Ожидайте..");

                var packet = new Data_DeleteQuestions()
                {
                    Questions = list,
                    Index = IndexTest
                };

                var obj = new Data_FirstCommand()
                {
                    Command = "Command_DeleteQuestionFromTest",
                    Json = JsonSerializer.Serialize(packet)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(obj));
                await Task.Delay(250);

                _Main.Instance.OverlayShow(false);
                _Main.Instance.Manager.Back();

            }

        }

        private void editAnsw_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI_QuestionEdit(Data, IndexTest));
        }

        private void addAnsw_Click(object sender, RoutedEventArgs e)
        {
 
        }
    }
}
