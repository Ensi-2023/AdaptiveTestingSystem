﻿using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetAllMultyTest:Commands
    {
        ThreadAcceptData AcceptData;

        public override void Execut(string json, InternetClient client)
        {
            try
            {

                Application.Current.Dispatcher.Invoke(() => {

                    var obj = JsonSerializer.Deserialize<Data_UserList>(json);

                    if (obj != null)
                    {


                        if (obj.IsCode == Code.ThreadStart)
                        {
                            AcceptData = new ThreadAcceptData();
                            AcceptData.FinishUpload += AcceptData_FinishUpload; ;
                            AcceptData.StartCollectingPacket += AcceptData_StartCollectingPacket; ;
                            AcceptData.StopUploadPacket += AcceptData_StopUploadPacket; ;


                        }

                        if (AcceptData != null) AcceptData.Accept(obj);

                    }
                });


         
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetAllMultyTest.Execut вызывал ошибку: {ex.Message}");
            }
        }

        private void AcceptData_StopUploadPacket()
        {
            AcceptData.FinishUpload -= AcceptData_FinishUpload;
            AcceptData.StartCollectingPacket -= AcceptData_StartCollectingPacket;
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData = null;
        }

        private void AcceptData_StartCollectingPacket((double, double) sendmax)
        {

        }

        private void AcceptData_FinishUpload(object packet)
        {
            AcceptData.FinishUpload -= AcceptData_FinishUpload;
            AcceptData.StartCollectingPacket -= AcceptData_StartCollectingPacket;
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData = null;

            var obj = JsonSerializer.Deserialize<Data_ListPacketServer>(packet.ToString());
            if (obj == null) return;
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Factory.StartNew(() => _Main.Instance.MVVM_Manager.ServerBrowserModel.SetCollection(obj.MultyServer));
            });



        }

        private void AdminConnect(Data_ListPacketServer obj)
        {
            _Main.Instance.Manager.Next(new GUI_TestingServerAdminPanel(obj.MultyServer));
            CloseWindowOrOverlay();
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

    }
}


