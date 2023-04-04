using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_mini_mvvm;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_RolyChanger.xaml
    /// </summary>
    public partial class GUI_RolyChanger : Window
    {
        public static GUI_RolyChanger Instance;

        VM_Roly mv_Roly;


        System.Collections.IList userList;
        int _index;
        public GUI_RolyChanger(System.Collections.IList list, int index)
        {
            InitializeComponent();
            Instance = this;

            mv_Roly = new VM_Roly();    
            userList = list;
            _index = index;

            DataContext = mv_Roly;
        }

        private void Header_CloseClick()
        {
            Close();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            Update();  
        }

        private async void Update()
        {
            OverlayShow(true, TypeOverlay.loading, "Загружаю роли...");

            await Task.Delay(250);

            var command = new Data_Roly()
            { 
                Index = _index,
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_GetRolyListWithoutThisIndex", command);

        }

        public void OverlayShow(bool show, TypeOverlay typeOverlay = TypeOverlay.nullable, string title = "", string subtitle = "", Visibility visibleButton = Visibility.Collapsed, bool awaiter = false)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                if (Instance == null) return;

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


        public void SetData(List<Data_Roly> list)
        {
            mv_Roly.SetData(list);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                mv_Roly.Search(text);
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void rolyChanger_Click(object sender, RoutedEventArgs e)
        {

            if (listViewBox.SelectedItems.Count > 0)
            {
                var obj = listViewBox.SelectedItem as MV_Roly;
                if (obj != null)
                {
                    if (MessageShow.Show("Изменить роль пользователю'ям ? ","Изменение роли",MessageShow.Type.Question) == true) 
                    {

                        List<Data_RolyUser> users = new List<Data_RolyUser>();



                        foreach (var item in userList)
                        {
                            users.Add(new Data_RolyUser() {IndexUser = (item as MV_User).Index});
                        }

                        if (users.Count == 0) return;

                        var command = new Data_UpdateUserRoly()
                        { 
                           IndexRoly= obj.Index,
                           Users = users
                        };

                        var packet = new Data_FirstCommand()
                        {
                            Command = "Command_ChangerRolyUser",
                            Json = JsonSerializer.Serialize(command)
                        };

                        _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
                        DialogResult = true;
                        Close();
                    }
                
                }
            }
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadManager.CloseActiveThread();
        }
    }
}
