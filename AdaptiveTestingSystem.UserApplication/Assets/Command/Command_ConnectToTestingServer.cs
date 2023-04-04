using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_ConnectToTestingServer:Commands
    {
        public override void Execut(string json, InternetClient client)
        { 
            try
            {
                var obj = JsonSerializer.Deserialize<Data_MultyServerClient>(json);
                if (obj == null) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    switch (obj.IsCode)
                    {
                        case Code.ErrorCnnectToServer_ServerStart: ErrorMessage("Ошибка подключениея сервер уже запущен"); { break; }
                        case Code.ConnectedToServer: { ConnectToServer(obj); break; }
                        case Code.RemoveConnect: { RemoveClient(obj); break; }
                        case Code.StartingTest: { break; }
                        case Code.ErrorCnnectToServer_NotCorrectPassword: { ErrorMessage("Не верный пароль"); break; }
                        case Code.ErrorCnnectToServer_NotCorrectServerIndex: { ErrorMessage("Не верный индекс сервера"); break; }
                        case Code.NewClientConnect: { NewClientForServer(obj); break; }
                        case Code.ErrorCnnectToServer_ServerNotStart: { ErrorMessage("На данный момент не возможно подключиться\nПопробуйте позднее"); break; }
                        case Code.RemoveConnectedClient: {  CloseTestWindow(); break; }
                            
                            
                            
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetAllMultyTest.Execut вызывал ошибку: {ex.Message}");
            }
        }

        private void CloseTestWindow()
        {
            if (UIHelper.IsWindowOpen<GUI_TestReady>())
            {
                foreach (var item in Application.Current.Windows)
                {
                    var window = item as GUI_TestReady;
                    if (window != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var child = window.body.Children[0] as GUI_TestingRun;
                            if (child != null)
                            {
                                ErrorMessage("Сервер завершил работу, подключение разорвано");
                                window.Close();
                            }
                        });

                        return;
                    }
                }
            }
        }

        private void RemoveClient(Data_MultyServerClient obj)
        {
            Logger.Debug($"desconnect: {obj.GUID}");


            var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
            if (guiUID != null)
            {
                guiUID.DesconnectUser(obj);
                return;
            }
        }

        private void NewClientForServer(Data_MultyServerClient obj)
        {

            Logger.Debug($"connect: {obj.NameClient}");

            var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
            if (guiUID != null)
            {
                guiUID.Connect(obj);
                return;
            }
        }

        private void ErrorMessage(string message)
        {
            if (UIHelper.IsWindowOpen<GUI_FastConnect>())
            {
                foreach (var item in Application.Current.Windows)
                {
                    var window = item as GUI_FastConnect;
                    if (window != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            window.Overlay.Visibility = Visibility.Collapsed;
                            _Main.Instance._Notification.Add("", $"{message}", TypeNotification.Error);

                        });

                        return;
                    }
                }
            }
            else
            {
                _Main.Instance._Notification.Add("", $"{message}", TypeNotification.Error);
                _Main.Instance.OverlayShow(false);
            }
        }


        private void CloseWindowOrOverlay()
        {
            if (UIHelper.IsWindowOpen<GUI_FastConnect>())
            {
                foreach (var item in Application.Current.Windows)
                {
                    var window = item as GUI_FastConnect;
                    if (window != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            window.Close();

                        });

                        return;
                    }
                }
            }
            else
            {
                _Main.Instance.OverlayShow(false);
            }
        }

        private void ConnectToServer(Data_MultyServerClient obj)
        {
            CloseWindowOrOverlay();
            Logger.Debug("test");
            _Main.Instance.Manager.Home();
            _Main.Instance.UI_TestReady = new GUI_TestReady(obj);
            _Main.Instance.UI_TestReady.Uid = (Guid.NewGuid().ToString());
            _Main.Instance.Hide();
            Logger.Debug("tes2");
            _Main.Instance.UI_TestReady.ShowDialog();


        }
    }
}
