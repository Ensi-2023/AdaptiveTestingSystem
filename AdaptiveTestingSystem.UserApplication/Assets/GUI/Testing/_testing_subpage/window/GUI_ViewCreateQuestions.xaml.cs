using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window.CScript;
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
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_ViewCreateQuestions.xaml
    /// </summary>
    public partial class GUI_ViewCreateQuestions : Window
    {
        public static GUI_ViewCreateQuestions Instanse;


        public TestingItem _testingItem;
        GUI_GeneratorTest _gui_GeneratorTest;
        GeneratorTesting GeneratorTesting;

        public bool IsStop { get; private set; }

        public GUI_ViewCreateQuestions(TestingItem testingItem, GUI_GeneratorTest gUI_GeneratorTest, GeneratorTesting generator)
        {
            InitializeComponent();
            Instanse = this;
            _testingItem = testingItem;
            _gui_GeneratorTest= gUI_GeneratorTest;
            GeneratorTesting = generator;
          
        }

        private void Header_CloseClick()
        {
            Close();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            GeneratorTesting.CreateThread();

            SetListQuestions();
        }

        public async void SetListQuestions()
        {
            foreach (var item in _testingItem.Questions)
            {
                CustomTextOrImage custom = new CustomTextOrImage();

                custom.Number = (body.Children.Count+1).ToString();
                custom.Title = item.Question;
                custom.ImageData = item.Image;
                custom.IsImaging = item.IsImaging;
                custom.Index = item.Index;
                custom.Image_Format = item.Image_format;

                if (item.IsImaging) custom.Title = "Картинка";

                custom.Editing += Custom_Editing;
                custom.Delete += Custom_Delete;
                custom.ImageView += Custom_ImageView;

                body.Children.Add(custom);
                await Task.Delay(30);
            }
        }

        public void SetListQuestions(TestingItem testing)
        {

            Application.Current.Dispatcher.Invoke(() => {


                body.Children.Clear();

                foreach (var item in testing.Questions)
                {
                    CustomTextOrImage custom = new CustomTextOrImage();

                    custom.Number = (body.Children.Count+1).ToString();
                    custom.Title = item.Question;
                    custom.ImageData = item.Image;
                    custom.IsImaging = item.IsImaging;
                    custom.Index = item.Index;
                    custom.Image_Format = item.Image_format;

                    if (item.IsImaging) custom.Title = "Картинка";

                    custom.Editing += Custom_Editing;
                    custom.Delete += Custom_Delete;
                    custom.ImageView += Custom_ImageView;

                    body.Children.Add(custom);
                }

                _gui_GeneratorTest.nameTesting.Text = testing.NameTest;
                _gui_GeneratorTest.Predmet.Text = testing.NamePredmet;
                _testingItem = testing;
                _gui_GeneratorTest.TestingItem= testing;
                description.Text = testing.Description;
           

        });
        }


        private async void SearchQuestions(string text)
        {
            this.IsEnabled = false;
            body.Children.Clear();

            foreach (var item in _testingItem.Questions)
            {
                if (item.Question.ToLower().Trim().Contains(text.ToLower().Trim()))
                {

                    CustomTextOrImage custom = new CustomTextOrImage();
                    custom.Number = (body.Children.Count + 1).ToString();
                    custom.Title = item.Question;
                    custom.ImageData = item.Image;
                    custom.IsImaging = item.IsImaging;
                    custom.Image_Format = item.Image_format;

                    if (item.IsImaging) custom.Title = "Картинка";

                    custom.Editing += Custom_Editing;
                    custom.Delete += Custom_Delete;
                    custom.ImageView += Custom_ImageView;

                    body.Children.Add(custom);
                    await Task.Delay(10);
                }
            }
            this.IsEnabled = true;
        }

        private void Custom_ImageView(byte[] imageByte)
        {
            ImageViewer.Show(imageByte);
        }

        private void Custom_Delete(CustomTextOrImage control)
        {
            var item = _testingItem.Questions.Find(o=>o.Index==control.GetIndex());
            if (item != null)
            {
                if (MessageShow.Show("Удалить вопрос ?\nВосстановить будет невозможно", "", MessageShow.Type.Question) == true)
                {
                   // _testingItem.Questions.Remove(item);


                    _gui_GeneratorTest.RemoveQuest(item);

                    control.Editing -= Custom_Editing;
                    control.Delete -= Custom_Delete;
                    control.ImageView -= Custom_ImageView;

                    body.Children.Remove(control);
                }
            }
        }

        private void Custom_Editing(CustomTextOrImage control)
        {
            var item = _testingItem.Questions.Find(o => o.Index == control.GetIndex());


            Logger.Debug($"Edit quest: #{item.Index}");

            if (item != null)
            {
                if (MessageShow.Show("Редактировать вопрос ?\nНе забудьте повторно его сохранить.", "", MessageShow.Type.Question) == true)
                {

                    _gui_GeneratorTest.SetQuest(item);
                                    
                    control.Editing -= Custom_Editing;
                    control.Delete -= Custom_Delete;
                    control.ImageView -= Custom_ImageView;

                    body.Children.Remove(control);

                    Close();

                }
            }

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                SearchQuestions(text);
            }
        }

        public void OverlayShow(bool show, TypeOverlay typeOverlay = TypeOverlay.nullable, string title = "", string subtitle = "", Visibility visibleButton = Visibility.Collapsed, bool awaiter = false)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
           
                if (awaiter == true) await Task.Delay(250);

                body.IsEnabled = !show;
                this.Overlay.Visibility = show == true ? Visibility.Visible : Visibility.Collapsed;
                this.Overlay.Title = title;
                this.Overlay.SubTitle = subtitle;
                this.Overlay.TOverlay = typeOverlay;
                this.Overlay.ButtonVisible = visibleButton;


                if (title.Trim() == string.Empty && subtitle.Trim() == string.Empty) return;
                if (typeOverlay == TypeOverlay.loading)
                {
                    Logger.Warning($"[{title}] - {subtitle}");
                }

                if (typeOverlay == TypeOverlay.message)
                {
                    Logger.Message($"[{title}] - {subtitle}");
                }

                if (typeOverlay == TypeOverlay.error)
                {
                    Logger.Error($"[{title}] - {subtitle}");
                }

            });
        }

        private void Overlay_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Overlay.Visibility == Visibility.Collapsed) body.IsEnabled = true;
        }

        private async void AddToTest_Click(object sender, RoutedEventArgs e)
        {

            if (_gui_GeneratorTest.IsEditingData())
            {
                _Main.Instance._Notification.Add("Удалите или добавьте редактируемый вопрос в список");
                return;
            }


            if (_testingItem.Questions.Count < 5)
            {
                _Main.Instance._Notification.Add("Добавьте как минимум 5 вопросов");
                return;
            }


            if (MessageShow.Show("Начать генерацию теста ? ", "", MessageShow.Type.Question) == true)
            {
                IsStop = false;
                OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Ожидаю выделения порта передачи.");
          
                await Task.Delay(1000);

                if (IsStop) return;

                StartSendToDB();
            }
        }


        public void StartSendToDB()
        {
            IsStop = false;
            _testingItem.Description = description.Text;
            GeneratorTesting.Start(_testingItem, this, _gui_GeneratorTest.IsEdit, _gui_GeneratorTest.DeleteQuestion, _gui_GeneratorTest.DeleteAnswer);
        }

        private void descCreate_Click(object sender, RoutedEventArgs e)
        {
            Animation.AnimatedHeight(Description, Description.ActualHeight, 250, TimeSpan.FromMilliseconds(150));
            descCreate.Visibility= Visibility.Collapsed;    
        }

        private void dascClose_Click(object sender, RoutedEventArgs e)
        {
            Animation.AnimatedHeight(Description, Description.ActualHeight, 0, TimeSpan.FromMilliseconds(150));
            descCreate.Visibility = Visibility.Visible;
        }

        private void exPort_Click(object sender, RoutedEventArgs e)
        {
            if (_gui_GeneratorTest.IsEditingData())
            {
                _Main.Instance._Notification.Add("Удалите или добавьте редактируемый вопрос в список");
                return;
            }


            if (_testingItem.Questions.Count < 5)
            {
                _Main.Instance._Notification.Add("Добавьте как минимум 5 вопросов");
                return;
            }


            if (MessageShow.Show("Тест будет сгенерирован, начать экспорт?", "", MessageShow.Type.Question) == true)
            {
                _testingItem.Description = description.Text;
                GeneratorTesting.Start(_testingItem, this, false, _gui_GeneratorTest.DeleteQuestion, _gui_GeneratorTest.DeleteAnswer, true);
            }
        }

        private void imPort_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Выбор теста";
            openFileDialog.Filter = "Тесты|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                GeneratorTesting.StartImport( this, openFileDialog.FileName);
            }
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            GeneratorTesting.StopThreadData();
        }

        private void testTesting_Click(object sender, RoutedEventArgs e)
        {
            if (_gui_GeneratorTest.IsEditingData())
            {
                _Main.Instance._Notification.Add("Удалите или добавьте редактируемый вопрос в список");
                return;
            }


            if (_testingItem.Questions.Count < 5)
            {
                _Main.Instance._Notification.Add("Добавьте как минимум 5 вопросов");
                return;
            }


            if (MessageShow.Show("Тест будет сгенерирован, проверить тест?", "", MessageShow.Type.Question) == true)
            {
                _testingItem.Description = description.Text;
                GeneratorTesting.Start(_testingItem, this, false, _gui_GeneratorTest.DeleteQuestion, _gui_GeneratorTest.DeleteAnswer,testingoffline:true);
            }
        }

        private void Overlay_OverlayThreadStop()
        {
            ThreadManager.CloseActiveThread();

            IsStop = true;
        }
    }
}
