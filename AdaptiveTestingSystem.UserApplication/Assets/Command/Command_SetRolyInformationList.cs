using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetRolyInformationList:Commands
    {
        ThreadAcceptData AcceptData;

        public override void Execut(string json, InternetClient client)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => {

                    var obj = JsonSerializer.Deserialize<Data_Roly>(json);

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
                Logger.Error($"Command_SetRolyInformationList.Execut вызывал ошибку: {ex.Message}");
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



            var obj = JsonSerializer.Deserialize<Data_RPacket>(packet.ToString());
            if (obj == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {

                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_RolyInfViewer;
                if (guiUID != null)
                {
                    guiUID.SetDataPacket(obj);
                }
            });



        }
    }
}



