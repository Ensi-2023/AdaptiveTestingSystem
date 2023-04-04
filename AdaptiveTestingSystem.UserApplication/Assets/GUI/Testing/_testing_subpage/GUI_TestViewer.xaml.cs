using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_mini_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm;
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
    /// Логика взаимодействия для GUI_TestViewer.xaml
    /// </summary>
    public partial class GUI_TestViewer : UserControl
    {

        VM_QuestionViewer mvvm_QuestionViewer;
        private int _index { get; set; }
        public string _desc;
        public GUI_TestViewer(int index, string nameTest,string predmet,string desc= "")
        {
            InitializeComponent();


            _index = index;
            testName.Text= nameTest;
            predmetName.Text=predmet;

            mvvm_QuestionViewer = new VM_QuestionViewer();
            DataContext = mvvm_QuestionViewer;
            _desc= desc;

        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                mvvm_QuestionViewer.Search(text);
            }
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            mvvm_QuestionViewer.ViewInformation(row.Item, _index);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Загрузка данных теста..");

            var obj = new Data_FirstCommand()
            {
                Command = "Command_GetQuestionThisTesting",
                Json = JsonSerializer.Serialize(new Data_Testing() { Index = _index })
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(obj));
        }

        internal async void SetDataPacket(List<Data_Question> obj)
        {
            if (mvvm_QuestionViewer != null) await mvvm_QuestionViewer.SetData(obj);
        }

        private async void AnswerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as DataGrid;
            if (obj.SelectedItems.Count > 0)
            {

                deleteAnswd.Visibility = Visibility.Visible;
                Animation.AnimatedOpacity(deleteAnswd, deleteAnswd.Opacity, 1, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                Animation.AnimatedOpacity(deleteAnswd, deleteAnswd.Opacity, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(170);
                deleteAnswd.Visibility = Visibility.Collapsed;

            }
        }

        private async void deleteAnswd_Click(object sender, RoutedEventArgs e)
        {
            if (AnswerGrid.SelectedItems.Count > 0)
            {
                if (MessageShow.Show("Удалить вопросы ?\nВосстановить будет невозможно", "", MessageShow.Type.Question) == true)
                {


                    var items = AnswerGrid.SelectedItems;
                    var list = new List<Data_Question>();

                    foreach (MV_Question item in items)
                    {
                        list.Add(new Data_Question() { Index = item.Index });
                    }

                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Ожидайте..");


                    var packet = new Data_DeleteQuestions()
                    {
                        Questions = list,
                        Index = _index
                    };

                    var obj = new Data_FirstCommand()
                    {
                        Command = "Command_DeleteQuestionFromTest",
                        Json = JsonSerializer.Serialize(packet)
                    };

                    _Main.Instance.Client.Send(JsonSerializer.Serialize(obj));
                    await Task.Delay(250);

                    _Main.Instance.OverlayShow(false);
                    Update();

                }


            }
        }

        private void descView_Click(object sender, RoutedEventArgs e)
        {
            if(_desc.Trim().Length>0) MessageShow.Show($"-----Описание теста-----\n\n\n{_desc}","Описание теста",MessageShow.Type.Message);
        }

     

        private async void deleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Удалить тест этот ?\nВосстановить будет невозможно", "", MessageShow.Type.Question) == true)
            {
                _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Ожидайте..");

                List<Data_Testing> commandDeleteList = new List<Data_Testing>()
                { 
                    new Data_Testing() { Index = _index}
                };

                var obj = new Data_FirstCommand()
                {
                    Command = "Command_DeleteTest",
                    Json = JsonSerializer.Serialize(commandDeleteList)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(obj));
                await Task.Delay(250);

                _Main.Instance.OverlayShow(false);
                _Main.Instance.Manager.Back();
            }
        }

        private void editAnsw_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI_GeneratorTest(true,_index));
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void addAnsw_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI_QuestionEdit(new Data_Question(), _index, true));
        }
    }
}
