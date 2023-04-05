using AdaptiveTestingSystem.Data.NotEntityFramework;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    /// <summary>
    /// Не хочу это пока что изменять. Работает и ладно. 21.03.2023
    /// </summary>
    public class Command_ApendTestingData : Commands
    {

        private GUI_TestReady _GUI_TestReady;
        ThreadAcceptData AcceptData;    

        public override void Execut(string json, InternetClient client)
        {
            try
            {

                Application.Current.Dispatcher.Invoke(() => {

                    var obj = JsonSerializer.Deserialize<Data_SendTesting>(json);

                    if (obj != null)
                    {
                        

                        if (obj.IsCode == Code.ThreadStart)
                        {
                            AcceptData = new ThreadAcceptData();
                            AcceptData.StatusUpload += ThreadAcceptData_StatusUpload;
                            AcceptData.ErrorUpload += ThreadAcceptData_ErrorUpload;
                            AcceptData.FinishUpload += ThreadAcceptData_FinishUpload;
                            AcceptData.StartCollectingPacket += AcceptData_StartCollectingPacket;
                            AcceptData.StopUploadPacket += AcceptData_StopUploadPacket;

                            SearchWindow();
                            
                        }

                        if(AcceptData!=null)  AcceptData.Accept(obj);

                    }
                });
            }

            catch (Exception ex)
            {
                Logger.Error($"Command_ApendTestingData.Execut вызывал ошибку: {ex.Message}");
            } 
        }

        private void AcceptData_StopUploadPacket()
        {
            AcceptData.StatusUpload -= ThreadAcceptData_StatusUpload;
            AcceptData.ErrorUpload -= ThreadAcceptData_ErrorUpload;
            AcceptData.FinishUpload -= ThreadAcceptData_FinishUpload;
            AcceptData.StartCollectingPacket -= AcceptData_StartCollectingPacket;
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData = null;

            _Main.Instance.OverlayShow(false);
            _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add("Прервано пользователем", "Загрузка", TypeNotification.Warning);
            _Main.Instance._Notification.Add("", "Прервано пользователем", TypeNotification.Warning);
            _Main.Instance.Manager.Back();

        }

        private void AcceptData_StartCollectingPacket((double, double) sendmax)
        {
            ThreadAcceptData_StatusUpload((sendmax.Item1, sendmax.Item2),true);
        }

        private void SearchWindow()
        {
            if (UIHelper.IsWindowOpen<GUI_TestReady>())
            {
                foreach (var item in Application.Current.Windows)
                {
                    var window = item as GUI_TestReady;
                    if (window != null)
                    {

                        if (window.Uid == _Main.Instance.UI_TestReady.Uid)
                        {
                            _GUI_TestReady = window;
                            break;
                        }
                    }
                }
            }
        }

        private  async void ThreadAcceptData_FinishUpload(object packet)
        {
            AcceptData.StatusUpload -= ThreadAcceptData_StatusUpload;
            AcceptData.ErrorUpload -= ThreadAcceptData_ErrorUpload;
            AcceptData.FinishUpload -= ThreadAcceptData_FinishUpload;
            AcceptData.StartCollectingPacket -= AcceptData_StartCollectingPacket; 
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData = null;

            if (packet == null) return;

            await Task.Delay(250);
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Testing>(packet.ToString());

                if (obj != null)
                {
              
                    if (_GUI_TestReady != null)
                    {
                        _GUI_TestReady.SetTest(obj);
                        _GUI_TestReady = null;
                    }
                    else
                    {
                        _GUI_TestReady = null;
                        var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_GeneratorTest;
                        if (guiUID != null)
                        {
                            guiUID.SetTest(obj);
                            return;
                        }
                        var uiTest = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;

                        if (uiTest != null)
                        {
                            uiTest.SetTest(obj);
                            return;
                        }

                    }
               
                }
             }
            catch (Exception ex)
            {
                Logger.Error($"Command_ApendTestingData.CreateTest вызывал ошибку: {ex.Message}");
                if (_GUI_TestReady != null)
                {
                    _GUI_TestReady.SetError();
                }
                else
                {
                    _Main.Instance.OverlayShow(false);
                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add(ex.Message, "Ошибка", TypeNotification.Error);
                    _Main.Instance._Notification.Add("", "Возникла ошибка", TypeNotification.Error);
                    _Main.Instance.Manager.Back();

                }

                ThreadManager.CloseActiveThread();
            }

        }


        private void ThreadAcceptData_ErrorUpload(string error)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.error, "Возникла ошибка", error, visibleButton: Visibility.Visible);
        }

        private void ThreadAcceptData_StatusUpload((double, double) sendmax, bool collection = false)
        {
         
            if (_GUI_TestReady != null)
            {
                bool active = _GUI_TestReady.SetInfo(sendmax.Item1, sendmax.Item2);
                if (active == false)
                {
                    ThreadManager.CloseActiveThread();
                    return;
                }
            }
            else
            {
                _GUI_TestReady = null;

                if (collection == true)
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Данные", $"Ожидаю обработки", visibleButton: Visibility.Collapsed);

                else
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Данные", $"Загруженно {sendmax.Item1} из {sendmax.Item2} кб.", visibleButton: Visibility.Visible);

                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_GeneratorTest;
                var uiAdminTest = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
                if (guiUID == null && uiAdminTest == null)
                {
                    ThreadManager.CloseActiveThread();                 
                    Logger.Debug("Send cancel Upload");
                    return;
                }
            }


        }


    }
}

