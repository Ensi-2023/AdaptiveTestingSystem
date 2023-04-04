using AdaptiveTestingSystem.Control.CustomControl;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
using System;
using System.Collections;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject
{
    /// <summary>
    /// Логика взаимодействия для GUI_Subject.xaml
    /// </summary>
    public partial class GUI_Subject : UserControl
    {
        ViewSubjectModel viewSubject;
        public GUI_Subject()
        {
            InitializeComponent();

            DataContext = viewSubject = _Main.Instance.MVVM_Manager.SubjectModel;

            AccessUser();
        }


        private void AccessUser()
        {
            if (_Main.Instance.MyAccount.ReadPredmet)
            {
                readPanel.Visibility = Visibility.Visible;
   
            }
            else
            {
                readPanel.Visibility = Visibility.Collapsed;
            }
        }


        public void Update()
        {
            viewSubject.OnUpdate(true);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                viewSubject.Search(text);
            }
        }


        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            viewSubject.ViewInformation.Execute(row.Item);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }


        private void countView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && viewSubject != null)
            {
                viewSubject.SetCountView(ParserVariables.GetInt(obj.Text));
            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            viewSubject.Update += ViewSubject_Update;
            viewSubject.OverlayShowing += ViewSubject_OverlayShowing;
            viewSubject.OverlayChangeInformation += ViewSubject_OverlayChangeInformation;
            viewSubject.DeleteObjects += ViewSubject_DeleteObjects;
            viewSubject.ViewerInformationSubject += ViewSubject_ViewerInformationSubject; ;
            viewSubject.IsView = true;
            viewSubject.OnUpdate();
        }

        private void ViewSubject_ViewerInformationSubject(MVVM.Model.Subject subject)
        {
            _Main.Instance.Manager.Next(new GUI_Subject_Viewer(subject, viewSubject));
        }


        private void SetDeleteClassRoomList(IList deleteList, out List<MVVM.Model.Subject> list, out List<Data_Subject> commandDeleteList)
        {
            list = deleteList.Cast<MVVM.Model.Subject> ().ToList();
            commandDeleteList = new List<Data_Subject>();
            foreach (var item in list)
            {
                commandDeleteList.Add(new Data_Subject()
                {
                    Id_data= item.Index,
                    Name_data = item.Name
                });
            }
        }


        private void ViewSubject_DeleteObjects(System.Collections.IList deleteList)
        {

            if (deleteList != null)
            {

                if (deleteList.Count == 0) return;

                List<MVVM.Model.Subject> list;
                List<Data_Subject> commandDeleteList;
                SetDeleteClassRoomList(deleteList, out list, out commandDeleteList);

                if (list.Count > 0)
                {
                    if (MessageShow.Show($"Вы действительно желаете удалить #{list.Count} записей?", "Удаление", MessageShow.Type.Question) == true)
                    {
                        var json = new Data_Subject_Delete()
                        {
                            IsCode = Code.Delete,
                            Subject = commandDeleteList,
                            Description = $"Запрос на удаление #{list.Count} записей из таблицы Предметы"
                        };

                        var firstCommand = new Data_FirstCommand()
                        {
                            Command = "Command_DeleteSubject",
                            Json = JsonSerializer.Serialize(json)
                        };

                        _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));
                        viewSubject.DeleteData(deleteList);
                    }
                }




            }


        }



        private void ViewSubject_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Список предметов", subtitle: $"Обработано: {firstValue} из {lastValue}");
        }

        private void ViewSubject_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);
        }

        private async void ViewSubject_Update(bool skipCheck)
        {
            _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: "Список предметов", subtitle: "загрузка...");

            await Task.Delay(250);

            Data_Subject data = new Data_Subject()
            {
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_GetSubjectList", data);
           
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var wind = new GUI_Subject_Changer();
            wind.ShowDialog();

            viewSubject.OnUpdate();
        }

        private void red_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectRoomGrid.SelectedItem != null)
            {
                var wind = new GUI_Subject_Changer((MVVM.Model.Subject)SubjectRoomGrid.SelectedItem);
                wind.ShowDialog();
                viewSubject.OnUpdate();
            }
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {

            viewSubject.Update -= ViewSubject_Update;
            viewSubject.OverlayShowing -= ViewSubject_OverlayShowing;
            viewSubject.OverlayChangeInformation -= ViewSubject_OverlayChangeInformation;
            viewSubject.DeleteObjects -= ViewSubject_DeleteObjects;
            viewSubject.ViewerInformationSubject -= ViewSubject_ViewerInformationSubject;
            viewSubject.IsView = false;

            ThreadManager.CloseActiveThread();

        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            viewSubject.OnUpdate();
        }
    }
}
