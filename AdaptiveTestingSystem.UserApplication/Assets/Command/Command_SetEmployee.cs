using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage._classRoom_subPage_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetEmployee:Commands
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
                Logger.Error($"Command_SetEmployee.Execut вызывал ошибку: {ex.Message}");
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



            var obj = JsonSerializer.Deserialize<List<Data_UserList>>(packet.ToString());
            if (obj == null) return;
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Factory.StartNew(() =>
                {

                    Application.Current.Dispatcher.Invoke(async () =>
                    {

                        if (UIHelper.IsWindowOpen<GUI_AddOrUpdateEmployeeToClassRoom>())
                        {
                            foreach (var item in Application.Current.Windows)
                            {
                                var window = item as GUI_AddOrUpdateEmployeeToClassRoom;
                                if (window != null)
                                {
                                    await window.SetData(obj);
                                    break;
                                }
                            }
                        }
                    });
                });
            });



        }
    }
}
