using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage._classRoom_subPage_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetUserNoClassRoom:Commands
    {

        ThreadAcceptData AcceptData;
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => {

                    var obj = JsonSerializer.Deserialize<Data_UserViewer>(json);

                    if (obj != null)
                    {


                        if (obj.IsCode == Code.ThreadStart)
                        {
                            AcceptData = new ThreadAcceptData();
                            AcceptData.FinishUpload += AcceptData_FinishUpload; 
                            AcceptData.StartCollectingPacket += AcceptData_StartCollectingPacket; 
                            AcceptData.StopUploadPacket += AcceptData_StopUploadPacket; 
                                                      

                        }

                        if (AcceptData != null) AcceptData.Accept(obj);

                    }
                });

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetUserNoClassRoom.Execut вызывал ошибку: {ex.Message}");
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


            var list = JsonSerializer.Deserialize<Data_UserViewer>(packet.ToString());

            if (list == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {

                if (UIHelper.IsWindowOpen<GUI_AddNewUserToClassRoom>())
                {
                    foreach (var item in Application.Current.Windows)
                    {
                        var window = item as GUI_AddNewUserToClassRoom;
                        if (window != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {

                                var userViewer = window.body.Children[0] as GUI_ANUtCR_UView;
                                if (userViewer == null) return;
                                userViewer.SetData(list.UserList);

                            });

                            break;
                        }
                    }
                }
            });

        } 
    }
}
