using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_StatisticGeneral:Commands
    {
        ThreadAcceptData AcceptData;
        public override void Execut(string json, InternetClient client)
        {
            try
            {

                Application.Current.Dispatcher.Invoke(() => {

                    var obj = JsonSerializer.Deserialize<Data_StatisticPacket>(json);

                    if (obj != null)
                    {


                        if (obj.IsCode == Code.ThreadStart)
                        {
                            AcceptData = new ThreadAcceptData();
                            AcceptData.FinishUpload += AcceptData_FinishUpload; 
                            AcceptData.StartCollectingPacket += AcceptData_StartCollectingPacket; 
                            AcceptData.StopUploadPacket += AcceptData_StopUploadPacket;
                            AcceptData.ErrorUpload += AcceptData_ErrorUpload;
                            AcceptData.StatusUpload += AcceptData_StatusUpload;

                        }

                        if (AcceptData != null) AcceptData.Accept(obj);

                    }
                });



            }
            catch (Exception ex)
            {
                Logger.Error($"Command_StatisticGeneral.Execut вызывал ошибку: {ex.Message}");
            }
        }

        private void AcceptData_StatusUpload((double, double) sendmax, bool collection = false)
        {
            var window = CheckUI.GetMainBodyUI() as GUI_Statistic;

            if (window == null)
            {
                Clear();
            }
            else
            {
                if (window.IsCancelUpload()) Clear();
                window.SetInfo(sendmax.Item1, sendmax.Item2);
            }
        }

        private void Clear()
        {
            AcceptData = null;
            ThreadManager.CloseActiveThread();
        }

        private void AcceptData_ErrorUpload(string error)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.error, "Возникла ошибка", error, visibleButton: Visibility.Visible);

        }

        private void AcceptData_StopUploadPacket()
        {
            AcceptData.FinishUpload -= AcceptData_FinishUpload;
            AcceptData.StartCollectingPacket -= AcceptData_StartCollectingPacket;
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData.ErrorUpload -= AcceptData_ErrorUpload;
            AcceptData.StatusUpload -= AcceptData_StatusUpload;
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
            AcceptData.ErrorUpload -= AcceptData_ErrorUpload;
            AcceptData.StatusUpload -= AcceptData_StatusUpload;
            AcceptData = null;


            var obj = JsonSerializer.Deserialize<Data_StatisticGeneral>(packet.ToString());
            if (obj != null)
            {

                var window = CheckUI.GetMainBodyUI() as GUI_Statistic;
                if (window != null)
                {
                    window.SetData(obj,null);
                }
            }
        }
    }
}
