using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._custom_component
{
    /// <summary>
    /// Логика взаимодействия для ViewUserDataTest.xaml
    /// </summary>
    /// 

    //public class Model
    //{ 
    //    public int Index { get; set; }
    //    public string Text { get; set; }
    //    public bool IsImageQuest { get; set; }
    //    public string ImageQuestString { get; set; }
    //    public string AnswerUser { get; set; }
    //    public string AnswerCorrect { get; set; }
    //    public string ImageAnswerUserstring { get; set; }
    //    public string ImageAnswerCorrectString { get; set; }
    //    public bool IsImageAnswerUser { get; set; }
    //    public bool IsImageAnswerCorrect { get; set; }

    //}

    public partial class ViewUserDataTest : UserControl
    {
        public string GUID { get; private set; }
        Data_Testing _dataTesting { get; set; }
        List<Data_MultyServerSendAnswer> _serverSendAnswers { get; set; }
        GUI_TestingServerAdminPanel gui_TestingServer = null;
        VM_ResultDataTesting vm_ResultData;
            


        public ViewUserDataTest(string guid,Data_Testing data,List<Data_MultyServerSendAnswer> serverSendAnswers, GUI_TestingServerAdminPanel gUI_TestingServerAdminPanel)
        {
            InitializeComponent();
            GUID=guid;
            _dataTesting = data;
            _serverSendAnswers= serverSendAnswers;
            gui_TestingServer = gUI_TestingServerAdminPanel;

            vm_ResultData = new VM_ResultDataTesting();
            DataContext = vm_ResultData;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GeneratorV2();
        }

        private async void GeneratorV2()
        {
            if (_serverSendAnswers.Count == 0) return;
           
            double correctAnswer = 0;
            double notcorrectAnswer = 0;

            Overlay.Visibility = Visibility.Visible;

            double maxRow = _serverSendAnswers[0].CountQuest;

            for (int j = 0; j < _serverSendAnswers.Count; j++)
            {

                var quest = _serverSendAnswers[j];
                var searchQuest = _dataTesting.Questions.Find(o => o.Index == quest.IndexQuest);

        
                if (searchQuest != null)
                {
                    var answer = searchQuest.Answer.Find(p => p.Index == quest.NumberAnswer);
                    var correct = searchQuest.Answer.Find(p => p.Index == quest.NumberCorrect);
            
                    if (answer == null && correct == null) continue;
                    if (quest.NumberCorrect == quest.NumberAnswer) correctAnswer++; else notcorrectAnswer++;



                    MV_ResultDataTest dataTest = new MV_ResultDataTest() 
                    {
                        Index = j+1,
                        Quest = searchQuest.Question,
                        IsQuestImage= searchQuest.IsImaging,
                        QuestImage = searchQuest.Image,
                        AnswerUser = answer.Answer,
                        IsAnswerImage = answer.IsImaging,
                        AnswerImage = answer.Image,
                        CorrectAnswer = correct.Answer,
                        CorrectImage= correct.Image,
                        IsCorrectAnswerImage = correct.IsImaging
                    };

                    vm_ResultData.AddOne(dataTest);
                    await Task.Delay(20);
                }
            }


            Overlay.Visibility = Visibility.Collapsed;


            double c = maxRow / correctAnswer;
            double perc = 100 / c;

            int scoreResult = 0;
            if (perc <= 30)
            {
                scoreResult = 2;
            }
            else if (perc > 30 && perc <= 50) scoreResult = 3;
            else if (perc > 50 && perc <= 80) scoreResult = 4;
            else scoreResult = 5;


            correctQuestAnswer.Text = correctAnswer.ToString();
            notcorrectQuestAnswer.Text = notcorrectAnswer.ToString();
            scoreTest.Text = scoreResult.ToString();

            allQuest.Text = maxRow.ToString();


        }

        //private async void GenerationResult()
        //{
        //    // ImageQuestionsViewer.Source = Converter.ConvertByteArrayToImage(value);

        //    int correctAnswer = 0;
        //    int notcorrectAnswer = 0;

        //    Overlay.Visibility= Visibility.Visible;

        //    int maxRow = _serverSendAnswers.Count;
        //   // body
        //    Grid grid = new Grid()
        //    {
        //        ColumnDefinitions = { new ColumnDefinition() { }, new ColumnDefinition() { Width = GridLength.Auto }, new ColumnDefinition() { Width = GridLength.Auto } },
        //    };


        //    RowDefinition titleRow= new RowDefinition() { Height = GridLength.Auto };
        //    grid.RowDefinitions.Add(titleRow);

        //    var title1 = CreateTextBlock("Вопрос");
        //    var title2 = CreateTextBlock("Ответил");
        //    var title3 = CreateTextBlock("Правильный ответ");

        //    title1.HorizontalAlignment= HorizontalAlignment.Left;


        //    Grid.SetColumn(title1, 0);
        //    Grid.SetColumn(title2, 1);
        //    Grid.SetColumn(title3, 2);


        //    Border border = new Border()
        //    { 
        //        CornerRadius = new CornerRadius(5),
        //        Background = (Brush)this.TryFindResource("DefaultPopupPanelBackground"),
        //    };

        //    Grid.SetColumn(border, 0);
        //    Grid.SetColumnSpan(border, 3);

        //    Header.Children.Add(border);
        //    Header.Children.Add(title1);
        //    Header.Children.Add(title2);
        //    Header.Children.Add(title3);


        //    for (int i = 0; i < maxRow; i++)
        //    {
        //        RowDefinition rowDefinition = new RowDefinition() { Height=GridLength.Auto};
        //        grid.RowDefinitions.Add(rowDefinition);
        //    }

        //    for (int j = 0; j < _serverSendAnswers.Count; j++)
        //    { 
        //        var quest = _serverSendAnswers[j];
        //        var searchQuest = _dataTesting.Questions.Find(o=>o.Index == quest.IndexQuest);
        //        if (searchQuest != null)
        //        {

        //            if (searchQuest.IsImaging)
        //            {
        //                ImageViewComponent imageView = new ImageViewComponent()
        //                {
        //                    ImageData = Converter.ToByteArray(searchQuest.Image),
        //                    VerticalAlignment= VerticalAlignment.Center,
        //                    HorizontalAlignment= HorizontalAlignment.Left,
        //                    Width =110,
        //                    Height=110,
        //                    Margin = new Thickness(13)
        //                };

        //                imageView.ImageView += ImageView_ImageView;

        //                Grid.SetRow(imageView, j+1);               
        //                Grid.SetColumn(imageView, 0);
        //                grid.Children.Add(imageView);

        //            }
        //            else
        //            {
        //                TextBlock text = new TextBlock()
        //                {
        //                    Foreground = this.TryFindResource("DefaultTextForegroud") as Brush,
        //                    Text = searchQuest.Question,
        //                    TextWrapping = TextWrapping.Wrap,
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    HorizontalAlignment = HorizontalAlignment.Left,
        //                    FontSize =18,
        //                    Margin = new Thickness(13),
        //                };

        //                Grid.SetRow(text, j+1);
        //                Grid.SetColumn(text, 0);
        //                grid.Children.Add(text);
        //            }


        //            var answer = searchQuest.Answer.Find(p => p.Index == quest.NumberAnswer);
        //            var correct = searchQuest.Answer.Find(p => p.Index == quest.NumberCorrect);

        //            if (answer == null && correct == null) continue;

        //            if (quest.NumberCorrect == quest.NumberAnswer)
        //            {
        //                correctAnswer++;

        //                if (answer.IsImaging)
        //                {
        //                    ImageViewComponent imageView = CreateImageAnswer(answer);

        //                    imageView.ImageView += ImageView_ImageView;
        //                    ImageViewComponent imageView_2 = CreateImageAnswer(answer);
        //                    imageView_2.ImageView += ImageView_ImageView;

        //                    Grid.SetRow(imageView, j+1);
        //                    Grid.SetRow(imageView_2, j + 1);
        //                    Grid.SetColumn(imageView, 1);
        //                    Grid.SetColumn(imageView_2, 2);

        //                    grid.Children.Add(imageView);
        //                    grid.Children.Add(imageView_2);
        //                }
        //                else
        //                {
        //                    TextBlock text = CreateTextBlock(answer);
        //                    TextBlock text2 = CreateTextBlock(answer);

        //                    Grid.SetRow(text, j+1);
        //                    Grid.SetRow(text2, j+1);
        //                    Grid.SetColumn(text, 1);
        //                    Grid.SetColumn(text2, 2);
        //                    grid.Children.Add(text);
        //                    grid.Children.Add(text2);
        //                }
        //            }
        //            else
        //            {
        //                notcorrectAnswer++;
        //                SetNoteQuallyAnswer(j, answer, correct, grid);
        //            }
        //        }
        //        await Task.Delay(20);
        //        CreateBorder(j, grid);
        //    }
        //    bodyddd.Children.Clear();
        //    bodyddd.Children.Add(grid);
        //    Overlay.Visibility = Visibility.Collapsed;


        //    double c = maxRow / correctAnswer;
        //    double perc = 100 / c;

        //    int scoreResult = 0;
        //    if (perc <= 30)
        //    {
        //        scoreResult = 2;
        //    }
        //    else if (perc > 30 && perc <= 50) scoreResult = 3;
        //    else if (perc > 50 && perc <= 80) scoreResult = 4;
        //    else scoreResult = 5;


        //    correctQuestAnswer.Text = correctAnswer.ToString();
        //    notcorrectQuestAnswer.Text = notcorrectAnswer.ToString();
        //    scoreTest.Text = scoreResult.ToString();

        //    allQuest.Text = maxRow.ToString();

        //}

        private void CreateBorder(int j, Grid grid)
        {
            Border border = new Border()
            {
                Height=1,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = (Brush)this.TryFindResource("BorderColor"),
            };

            Grid.SetColumn(border, 0);
            Grid.SetRow(border, j+1);
            Grid.SetColumnSpan(border, 3);

            grid.Children.Add(border);

        }

        //private static ImageViewComponent CreateImageAnswer(Data_Answer? answer)
        //{
        //    return new ImageViewComponent()
        //    {
        //        ImageData = Converter.ToByteArray(answer.Image),
        //        ImageHeight = 110,
        //        ImageWidth = 110,
        //        VerticalAlignment = VerticalAlignment.Center,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        Margin = new Thickness(13),
        //    };
        //}

        private TextBlock CreateTextBlock(Data_Answer? answer)
        {
            return new TextBlock()
            {
                Foreground = this.TryFindResource("DefaultTextForegroud") as Brush,
                Text = answer.Answer,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                Margin = new Thickness(13),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,

            };
        }


        private TextBlock CreateTextBlock(string title)
        {
            return new TextBlock()
            {
                Foreground = this.TryFindResource("DefaultTextForegroud") as Brush,
                Text = title,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                Margin = new Thickness(13),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
        }

        //private void SetNoteQuallyAnswer(int j, Data_Answer? answer, Data_Answer? correct,Grid grid)
        //{
        //    if (answer.IsImaging)
        //    {
        //        ImageViewComponent imageView = new ImageViewComponent()
        //        {
        //            ImageData = Converter.ToByteArray(answer.Image),
        //            ImageHeight = 110,
        //            ImageWidth = 110,
        //            VerticalAlignment = VerticalAlignment.Center,
        //            HorizontalAlignment = HorizontalAlignment.Center,
        //            Margin = new Thickness(13),
        //        };

        //        imageView.ImageView += ImageView_ImageView;

        //        Grid.SetRow(imageView, j+1);
        //        Grid.SetColumn(imageView, 1);
        //        grid.Children.Add(imageView);
        //    }
        //    else
        //    {
        //        TextBlock text = new TextBlock()
        //        {
        //            Foreground = this.TryFindResource("DefaultTextForegroud") as Brush,
        //            Text = answer.Answer,
        //            TextWrapping = TextWrapping.Wrap,
        //            FontSize = 18,
        //            VerticalAlignment = VerticalAlignment.Center,
        //            HorizontalAlignment = HorizontalAlignment.Center,
        //            Margin = new Thickness(13),
        //        };


        //        Grid.SetRow(text, j+1);
        //        Grid.SetColumn(text, 1);
        //        grid.Children.Add(text);
        //    }


        //    if (correct.IsImaging)
        //    {
        //        ImageViewComponent imageView = new ImageViewComponent()
        //        {
        //            ImageData = Converter.ToByteArray(correct.Image),
        //            ImageHeight = 110,
        //            ImageWidth = 110,
        //            VerticalAlignment = VerticalAlignment.Center,
        //            HorizontalAlignment = HorizontalAlignment.Center,
        //            Margin = new Thickness(13),
        //        };

        //        imageView.ImageView += ImageView_ImageView;

        //        Grid.SetRow(imageView, j+1);
        //        Grid.SetColumn(imageView, 2);
        //        grid.Children.Add(imageView);
        //    }
        //    else
        //    {
        //        TextBlock text = new TextBlock()
        //        {
        //            Foreground = this.TryFindResource("DefaultTextForegroud") as Brush,
        //            Text = correct.Answer,
        //            TextWrapping = TextWrapping.Wrap,
        //            FontSize = 18,
        //            VerticalAlignment = VerticalAlignment.Center,
        //            HorizontalAlignment = HorizontalAlignment.Center,
        //            Margin = new Thickness(13),
        //        };


        //        Grid.SetRow(text, j+1);
        //        Grid.SetColumn(text, 2);
        //        grid.Children.Add(text);
        //    }
        //}

        private void ImageView_ImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            GUID = null;
            _dataTesting = null;
            _serverSendAnswers = null;
            gui_TestingServer = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gui_TestingServer.Back();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        private void ImageViewComponent_ImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }
    }
}
