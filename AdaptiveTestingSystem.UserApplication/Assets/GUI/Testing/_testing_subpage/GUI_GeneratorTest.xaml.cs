using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window.CScript;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_GeneratorTest.xaml
    /// </summary>
    /// 

    public class QuestionItem
    {
        public int Index { get; set; } = 0;
        public string Question { get; set; } = string.Empty;
        public byte[] Image { get; set; }
        public bool IsImaging { get; set; } = false;
        public int Complexity { get; set; } = 0;
        public string Image_format { get; set; } = string.Empty;
        public int CorrectAnswer { get; set; } = 0;

        public List<ImageTextBoxControl> Answer { get; set; }
    }

    public class TestingItem
    {
        public int Index { get; set; } = 0;
        public string NameTest { get; set; } = string.Empty;
        public string NamePredmet { get; set; } = string.Empty;
        public int IndexPredmet { get; set; } = 0;
        public int IndexEmp { get; private set; } = _Main.Instance.MyAccount.ID;
        public List<QuestionItem> Questions { get; set; } = new List<QuestionItem>();
        public string Description { get; set; } = string.Empty;
    }

    public partial class GUI_GeneratorTest : UserControl
    {

     

        public TestingItem TestingItem { get; set; }
        public object FileFolder_recovery { get; private set; }
        public bool IsEdit { get; private set; } = false;
        public string EditingPredmet { get; set; } = string.Empty;
        public int EditingIndex { get; set; } = 0;
        public int IndexTest { get; private set; } = 0;

        private GeneratorTesting GeneratorTesting;

        public List<int> DeleteAnswer = new List<int>();
        public List<int> DeleteQuestion = new List<int>();


        public GUI_GeneratorTest()
        {
            InitializeComponent();
            IndexTest = 0;
            TestingItem = new TestingItem();
            TestingItem.Questions = new List<QuestionItem>();
        }

        public GUI_GeneratorTest(bool isEditing,int index)
        {
            InitializeComponent();
            TestingItem = new TestingItem();

            IndexTest = index;
            TestingItem.Questions = new List<QuestionItem>();
            this.IsEdit = isEditing;
            SendServerForEditServer(index);
        }

        private void SendServerForEditServer(int index)
        {

            Data_TestingView view = new Data_TestingView()
            {
                Index = index,
                
            };

            ThreadManager.Send("Command_ViewTestingData", view);

            _Main.Instance.OverlayShow(true, TypeOverlay.loading);

        }

        private void ImageTextBoxControl_ImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }


        private void SetAnswer(ImageTextBoxControl control = null)
        {

            if (answerList.Children.Count == 10)
            {
              //  _Main.Instance._Notification.Add("Ошибка","Достигнуто максимальное количество ответов",TypeNotification.Error);
              //  return;
            }


            deleteTest.Visibility = Visibility.Visible;
            ImageTextBoxControl imageText = new ImageTextBoxControl() { Margin = new Thickness(5) };
            imageText.IsDeleteButton = true;
            imageText.ImageView += ImageTextBoxControl_ImageView;
            imageText.Deleting += ImageText_Deleting;
            imageText.Number = (answerList.Children.Count + 1);
            imageText.Wotemark = $"Напишите ответ #{(answerList.Children.Count + 1)}";
            imageText.Name = $"answer_{(answerList.Children.Count + 1)}";


            if (control != null)
            {
                imageText.Number = control.Number;
                imageText.Wotemark = control.Wotemark;
                imageText.IsImaging = control.IsImaging;
                imageText.ImageData = control.ImageData;
                imageText.Text = control.Text;
                imageText.Index = control.Index;
                imageText.Extension = control.Extension;
            }
            
            
            TextBlock text = new TextBlock()
            {
                Text = $"#{answerList.Children.Count + 1} ",
                FontSize = 25,
                Foreground = (Brush)this.FindResource("DefaultTextForegroud"),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(13, 0, 0, 0),
           
            };

            Border border = new Border() { Height = 2, Background = (Brush)this.FindResource("BorderColor"), Margin = new Thickness(10) };

            Grid gridgrid = new Grid()
            {
                ColumnDefinitions = { new ColumnDefinition() { Width = GridLength.Auto }, new ColumnDefinition() { } },
                RowDefinitions = { new RowDefinition() { Height = GridLength.Auto }, new RowDefinition() { Height = GridLength.Auto } }
            };


            Grid.SetColumn(text, 0);
            Grid.SetColumn(imageText, 1);
            Grid.SetRow(text, 0);
            Grid.SetRow(imageText, 0);
            Grid.SetRow(border, 1);
            Grid.SetColumnSpan(border, 2);
            Grid.SetColumn(border, 0);

            gridgrid.Children.Add(text);
            gridgrid.Children.Add(imageText);
            gridgrid.Children.Add(border);

            answerList.Children.Add(gridgrid);
            SetCorrectAnsw();
        }

        private void ImageText_Deleting(ImageTextBoxControl control)
        {

            if (MessageShow.Show("Удалить этот ответ?", "Удалить", MessageShow.Type.Question) == false) return;

            Logger.Debug($"delete request: {control.Name} {control.Index}");

            foreach (var item in answerList.Children)
            {
                if ((item as Grid) != null)
                {
                    foreach (var obj in (item as Grid).Children)
                    {
                        var answer = obj as ImageTextBoxControl;

                        if (answer != null)
                        {

                            if (control.Index != answer.Index) break;
                            answer.ImageView -= ImageTextBoxControl_ImageView;
                            answer.Deleting -= ImageText_Deleting;

                            var itemAnsw = CorrectAnsw.Items[CorrectAnsw.Items.Count - 1];
                            if (answer.Index != 0)
                            {
                                Logger.Debug($"delete list add: {answer.Index}");
                                DeleteAnswer.Add(answer.Index);
                            }
                            CorrectAnsw.Delete(itemAnsw);
                            answerList.Children.Remove((item as Grid));
                            ChangeNameTextBlock();
                            return;
                        }
                    }


                }
            }

         

        }

        private void ChangeNameTextBlock()
        {
            for (int i = 0; i < answerList.Children.Count; i++)
            {
                var grid = answerList.Children[i] as Grid;
                if (grid != null)
                {
                    foreach (var textBlock in grid.Children)
                    {
                        var seach = textBlock as TextBlock;
                        if (seach != null)
                        {
                            seach.Text = $"#{i + 1} ";
                            break;
                        }
                    }

                }
            }
        }

        private void addAnsw_Click(object sender, RoutedEventArgs e)
        {
            SetAnswer();
        }

        private void SetCorrectAnsw()
        {
            if (answerList.Children.Count > 0)
            {
                CorrectAnsw.Insert(new PopupItemControl() {Caption=(answerList.Children.Count).ToString()});
            }
        }

        private void deleteAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (answerList.Children.Count > 0)
            {
                var item = answerList.Children[answerList.Children.Count - 1];
                if (item != null)
                {


                    foreach (var obj in (item as Grid).Children)
                    { 
                        var answer = obj as ImageTextBoxControl;
                        if (answer != null)
                        {
                            answer.ImageView -= ImageTextBoxControl_ImageView; 
                            answer.Deleting -= ImageText_Deleting;
                            if (answer.Index != 0) 
                            {
                                Logger.Debug($"delete list add: {answer.Index}");
                                DeleteAnswer.Add(answer.Index);
                            }
                            break;
                        }
                    }

                    answerList.Children.Remove(item);
                    if(answerList.Children.Count==0) deleteTest.Visibility = Visibility.Collapsed;
                }
                var itemAnsw = CorrectAnsw.Items[CorrectAnsw.Items.Count - 1];

                CorrectAnsw.Delete(itemAnsw);
            }
            else { CorrectAnsw.ClearItems();  } 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EditingPredmet = string.Empty;
            GeneratorTesting = new GeneratorTesting();
            DeleteAnswer = new List<int>();
            DeleteQuestion = new List<int>();
            
            Logger.Message("GUI_GeneratorTest Loadeds");

            if (IsEdit==false)  Update();
        }


        public async void Update()
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Проверка данных", subtitle: $"Ожидайте...");
            await Task.Delay(150);

            Logger.Debug("Update");

            Data_FirstCommand command = new Data_FirstCommand()
            {
                Command = "Command_CheckForPresencePredmet",
               
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(command));

        }
        internal void SetDataPacket(List<Data_Subject> obj)
        {
            contentAnsw.IsEnabled= true;
            buttonManager.IsEnabled= true;    
            if (obj.Count==0) 
            {
                _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка", "В базе данных нет предметов, добавьте их", visibleButton: Visibility.Visible);
                contentAnsw.IsEnabled= false;
                buttonManager.IsEnabled= false;
                return;
            }

            Predmet.ClearItems();

            foreach (var item in obj) 
            {
                Predmet.Insert(new PopupItemControl() { Caption = item.Name_data,Index=item.Id_data});
            }


            if (EditingPredmet.Trim() != string.Empty)
            {
                Predmet.SelectedText = EditingPredmet;
            }

            _Main.Instance.OverlayShow(false);
        }

        private void addNewPredmet_Click(object sender, RoutedEventArgs e)
        {
            var wind = new GUI_Subject_Changer();
            wind.ShowDialog();
            Update();
        }

        private void AddAnswerToTest_Click(object sender, RoutedEventArgs e)
        {
            List<ImageTextBoxControl> list = GetAnswer();
   
            string error = string.Empty;
            string name = nameTesting.Text;
            string predmet = Predmet.Text;
            string correct = CorrectAnsw.Text;


            error += CheckFilledQuestion();

            if (name.Trim() == string.Empty)
            {
                error += "Не заполнено название теста\n";
            }

            if (correct.Trim() == string.Empty)
            {
                error += "Не выбран правильный ответ\n";
            }

            if (predmet.Trim() == string.Empty)
            {
                error += "Не выбран предмет\n";
            }

            if (list.Count == 0)
            {
                error += "Добавьте ответы на вопросы (минимум 3)\n";
            }
            else if (list.Count < 3)
            {
                error += "Мало ответов на вопрос, добавьте минимум 3 ответа\n";
            }
            else
            {
                error += CheckCompletion(list);
            }

            if (error.Trim().Length > 0)
            {
                MessageShow.Show($"---Произошла ошибка---\n\n{error}", "Ошибка", MessageShow.Type.Error); 
                return;
            }

            var selectPredmet = Predmet.Items[Predmet.SelectedIndex] as AdaptiveTestingSystem.Control.Themes.PopupItemControl;


            TestingItem.IndexPredmet = selectPredmet.Index;
            TestingItem.NameTest = name;
            TestingItem.NamePredmet = Predmet.Text;
            TestingItem.Index = IndexTest;


            TestingItem.Questions.Add(new QuestionItem()
            {
                Answer = list,
                Complexity = (int)complex.Value,
                CorrectAnswer = int.Parse(correct),
                Image = questionPanel.ImageData,
                Image_format =questionPanel.Extension,
                IsImaging = questionPanel.IsImaging,
                Question = questionPanel.IsImaging==true?"Картинка":questionPanel.Text,
                Index = IsEdit? EditingIndex:0
            });


            if (IsEdit)
            {                
                DeleteQuestion.Remove(DeleteQuestion.Find(o => o == EditingIndex));
            }

            Logger.Debug($"Edit: #{EditingIndex} Test: #{TestingItem.Index}");

            ClearData();
        }

        private string CheckCompletion(List<ImageTextBoxControl> list)
        {
            string error = string.Empty;
            foreach (var item in list)
            {
                if (item.IsImaging)
                {
                    if (item.ImageData.Length == 0)
                    {
                        error += $"Не заполнен ответ #{item.Number}\n";
                        continue;
                    }               
                }
                else 
                {
                    if (item.Text.Trim() == string.Empty)
                    {
                        error += $"Не заполнен ответ #{item.Number}\n";
                        continue;
                    }
                }
                

            }
            return error;
        }

        private string CheckFilledQuestion(bool nullable = false)
        {
            string error = string.Empty;


            if (nullable)
            {

                if (questionPanel.IsImaging)
                {
                    if (questionPanel.ImageData.Length != 0)
                        error = $"0";
                }
                else
                {
                    if (questionPanel.Text.Trim() != string.Empty)
                    {
                        error = $"0";
                    }
                }
                return error;
            }
            
            if (questionPanel.IsImaging)
            {
                if (questionPanel.ImageData.Length == 0)
                    error = $"Не запронен вопрос\n";
            }
            else
            {
                if (questionPanel.Text.Trim() == string.Empty)
                {
                    error = $"Не запронен вопрос\n";
                }
            }

            return error;
        }

        private void ClearData()
        {
            questionPanel.Clear();
            answerList.Children.Clear();
            CorrectAnsw.ClearItems();
            EditingIndex = 0;
            complex.Value = 0;
        }

        private List<ImageTextBoxControl> GetAnswer()
        {
            var list = new List<ImageTextBoxControl>();

            if (answerList.Children.Count == 0)
            {
                return list;
            }

            for (int i = 0; i < answerList.Children.Count; i++)
            {
                var item = answerList.Children[i] as Grid;

                if (item != null)
                {
                    foreach (var obj in item.Children)
                    { 
                        var answer = obj as ImageTextBoxControl;
                        if (answer != null)
                        {
                            list.Add(answer);
                            break;
                        }
                    }
                }
                 
            }

            return list;

        }

        private void contentAnsw_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        public bool IsEditingData()
        {
            string error = string.Empty;

          
            error += CheckFilledQuestion(true);
            

            if (CorrectAnsw.Text.Trim() != string.Empty)
            {
                error += "1";
            }

            if (answerList.Children.Count > 0)
            {
                error += "2";
            }

            if ((int)complex.Value != 0)
            {
                error += "3";
            }

            if (error.Trim() != string.Empty) return true;
            return false;
        }

        private void checkQuestions_Click(object sender, RoutedEventArgs e)
        {

                    
            var wind = new GUI_ViewCreateQuestions(TestingItem,this, GeneratorTesting);
            wind.ShowDialog();
        }

        public void SetQuest(QuestionItem question)
        {
            ClearData();


            if (question.Index != 0) DeleteQuestion.Add(question.Index);


            complex.Value = question.Complexity;

            questionPanel.SetText(question.Question);
            questionPanel.ImageData = question.Image;
            questionPanel.IsImaging = question.IsImaging;
            questionPanel.Extension = question.Image_format;

            EditingIndex = question.Index;

            foreach (var item in question.Answer)
            {
                SetAnswer(item);
            }
            CorrectAnsw.SelectedText = question.CorrectAnswer.ToString();
            TestingItem.Questions.Remove(question);
        }

        internal void RemoveQuest(QuestionItem item)
        {
            if(item.Index!=0) DeleteQuestion.Add(item.Index);
            TestingItem.Questions.Remove(item);
        }

        private void clearAnswer_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        internal void SetTest(Data_Testing obj)
        {
            if(obj != null)  GeneratorTesting.StartCreateTest(this, obj);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadManager.CloseActiveThread();
        }
    }
}
