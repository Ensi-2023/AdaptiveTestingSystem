using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetSubjectList:Commands
    {
        ThreadAcceptData AcceptData;

        public override void Execut(string json, InternetClient client)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    var obj = JsonSerializer.Deserialize<Data_Klass>(json);

                    if (obj != null)
                    {


                        if (obj.IsCode == Code.ThreadStart)
                        {
                            AcceptData = new ThreadAcceptData();
                            AcceptData.FinishUpload += AcceptData_FinishUpload; 
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
                Logger.Error($"Command_SetSubjectList.Execut вызывал ошибку: {ex.Message}");
            }
        }
        private void AcceptData_ErrorUpload(string error)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.error, "Возникла ошибка", error, visibleButton: Visibility.Visible);

        }

        private void AcceptData_StatusUpload((double, double) sendmax, bool collection = false)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Данные", $"Загруженно {sendmax.Item1} из {sendmax.Item2} кб.", visibleButton: Visibility.Visible);
        }

        private void AcceptData_StopUploadPacket()
        {
            AcceptData.FinishUpload -= AcceptData_FinishUpload;
            AcceptData.ErrorUpload -= AcceptData_ErrorUpload;
            AcceptData.StatusUpload -= AcceptData_StatusUpload;
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData = null;
        }


        private void AcceptData_FinishUpload(object packet)
        {
            AcceptData.FinishUpload -= AcceptData_FinishUpload;
            AcceptData.ErrorUpload -= AcceptData_ErrorUpload;
            AcceptData.StatusUpload -= AcceptData_StatusUpload;
            AcceptData.StopUploadPacket -= AcceptData_StopUploadPacket;
            AcceptData = null;

            var obj = JsonSerializer.Deserialize<List<Data_Subject>>(packet.ToString());
            if (obj == null) return;
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Factory.StartNew(() => _Main.Instance.MVVM_Manager.SubjectModel.SetCollection(obj));
            });

        }
    }
}



