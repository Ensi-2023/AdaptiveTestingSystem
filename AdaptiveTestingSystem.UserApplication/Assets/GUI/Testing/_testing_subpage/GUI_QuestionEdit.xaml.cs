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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_QuestionEdit.xaml
    /// </summary>
    public partial class GUI_QuestionEdit : UserControl
    {
        Data_Question Data { get; set; }
        public List<int> DeleteAnswer = new List<int>();
        public TestingItem TestingItem { get; set; }

        private bool _isInsert { get; set; }
        public int IndexTest { get; private set; }

        public GUI_QuestionEdit(Data_Question data,int indexTest, bool isInsert = false)
        {
            InitializeComponent();
            Data = data;
            _isInsert = isInsert;
            IndexTest = indexTest;
            if (_isInsert)
                RetryData.Visibility = Visibility.Collapsed;
        }

        private void addAnsw_Click(object sender, RoutedEventArgs e)
        {
            SetAnswer();
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

        private void SetAnswer(Data_Answer data = null)
        {

            deleteTest.Visibility = Visibility.Visible;
            ImageTextBoxControl imageText = new ImageTextBoxControl() { Margin = new Thickness(5) };
            imageText.IsDeleteButton = true;
            imageText.ImageView += ImageTextBoxControl_ImageView;
            imageText.Deleting += ImageText_Deleting;
            imageText.Number = (answerList.Children.Count + 1);
            imageText.Wotemark = $"Напишите ответ #{answerList.Children.Count + 1}";
            imageText.Name = $"answer_{(answerList.Children.Count + 1)}";
            imageText.Index = 0;

            if (data != null)
            {
                imageText.Number = data.Number;
                imageText.IsImaging = data.IsImaging;
                imageText.ImageData = Converter.ToByteArray(data.Image);
                imageText.Text = data.Answer;
                imageText.Index = data.Index;
                imageText.Extension = data.ImageFormat;
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

        private void SetCorrectAnsw()
        {
            if (answerList.Children.Count > 0)
            {
                CorrectAnsw.Insert(new PopupItemControl() { Caption = (answerList.Children.Count).ToString() });
            }
        }

        private void deleteTest_Click(object sender, RoutedEventArgs e)
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
                    if (answerList.Children.Count == 0) deleteTest.Visibility = Visibility.Collapsed;
                }
                var itemAnsw = CorrectAnsw.Items[CorrectAnsw.Items.Count - 1];

                CorrectAnsw.Delete(itemAnsw);
            }
            else { CorrectAnsw.ClearItems(); }
        }
        private void ImageTextBoxControl_ImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }

        private void contentAnsw_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        private void RetryData_Click(object sender, RoutedEventArgs e)
        {
            SetQuest();
        }

        private void ClearData()
        {
            questionPanel.Clear();
            answerList.Children.Clear();
            CorrectAnsw.ClearItems();
            complex.Value = 0;
        }

        public void SetQuest()
        {
            ClearData();

            if (_isInsert) return;
            complex.Value = Data.Complexity;
            questionPanel.SetText(Data.Question);
            questionPanel.ImageData = Converter.ToByteArray(Data.Image);
            questionPanel.IsImaging = Data.IsImaging;
            questionPanel.Extension = Data.ImageFormat;


            foreach (var item in Data.Answer)
            {
                SetAnswer(item);
            }
            CorrectAnsw.SelectedText = Data.CorrecrNumber.ToString();
        }

        private List<Data_Answer> GetAnswer()
        {
            var list = new List<Data_Answer>();

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
                            var qqq = new Data_Answer()
                            {
                                Answer = answer.Text,
                                ImageFormat = answer.Extension,
                                Index = answer.Index,
                                IsImaging = answer.IsImaging,
                                Number = answer.Number 
                            };

                            if (answer.ImageData != null)
                            {
                                qqq.Image = Convert.ToHexString(answer.ImageData);
                            }

                            list.Add(qqq);

                            break;
                        }
                    }
                }

            }

            return list;

        }

        private void AddAnswerToTest_Click(object sender, RoutedEventArgs e)
        {

            TestingItem = new TestingItem();

            List<Data_Answer> list = GetAnswer();

            string error = string.Empty;

            string correct = CorrectAnsw.Text;


            error += CheckFilledQuestion();

           
            if (correct.Trim() == string.Empty)
            {
                error += "Не выбран правильный ответ\n";
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

            var quest= new Data_Question()
            {
                Answer = list,
                Complexity = (int)complex.Value,
                CorrecrNumber = int.Parse(correct),
                Image = Convert.ToHexString(questionPanel.ImageData) ,
                ImageFormat = questionPanel.Extension,
                IsImaging = questionPanel.IsImaging,
                Question = questionPanel.IsImaging == true ? "Картинка" : questionPanel.Text,
                Index = _isInsert ?0 : Data.Index
            };

            Data_QuestionEditOrInsert packet = new Data_QuestionEditOrInsert();
            packet.Question = quest;
            packet.IndexTest = IndexTest;
            packet.DeleteAnswerIndex = DeleteAnswer;


            if (MessageShow.Show("Отправить данные на сервер ? ", "", MessageShow.Type.Question) == true)
            {

                _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Генерация вопроса", $"Ожидаю выделения порта передачи.");
                ThreadSendingTestToServer.StartSendEditOrInsertQuest(packet, _isInsert);
            }

            Logger.Debug($"Edit: #{Data.Index} ");
         
        }


        private string CheckCompletion(List<Data_Answer> list)
        {
            string error = string.Empty;
            foreach (var item in list)
            {
                if (item.IsImaging)
                {
                    if (item.Image.Length == 0)
                    {
                        error += $"Не заполнен ответ #{item.Number}\n";
                        continue;
                    }
                }
                else
                {
                    if (item.Answer.Trim() == string.Empty)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetQuest();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadSendingTestToServer.CancelThread();
        }
    }
}
