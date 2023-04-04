using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetUserList : Commands
    {

        ThreadAcceptData AcceptData;
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_UserPacket>(json);
                if (obj != null)
                {
              
                    if (obj.IsCode == Code.ThreadStart)
                    {
                        AcceptData = new ThreadAcceptData();
                        AcceptData.StatusUpload += ThreadAcceptData_StatusUpload;
                        AcceptData.ErrorUpload += ThreadAcceptData_ErrorUpload;
                        AcceptData.FinishUpload += ThreadAcceptData_FinishUpload;
                    }

                   if(AcceptData!=null) AcceptData.Accept(obj);
                }
                else
                {
                    _Main.Instance.OverlayShow(false);
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetUserList.Execut вызывал ошибку: {ex.Message}");
                Clear();
            }
        }

        private void Clear()
        {
            AcceptData = null;
            ThreadManager.CloseActiveThread();
        }

        private void ThreadAcceptData_FinishUpload(object packet)
        {
            AcceptData.StatusUpload -= ThreadAcceptData_StatusUpload;
            AcceptData.ErrorUpload -= ThreadAcceptData_ErrorUpload;
            AcceptData.FinishUpload -= ThreadAcceptData_FinishUpload;
            _Main.Instance.OverlayShow(false);

            AcceptData = null;

            if (packet == null) return;

            var obj = JsonSerializer.Deserialize<Data_UserViewer>(packet.ToString());
            if (obj == null) return;
            Application.Current.Dispatcher.Invoke(async () =>
            {
                switch (obj.IsCode)
                {
                    case Code.GUI_Staff: await Task.Factory.StartNew(() => _Main.Instance.MVVM_Manager.StaffModel.SetCollection(obj)); break;
                    case Code.GUI_User: await Task.Factory.StartNew(() => _Main.Instance.MVVM_Manager.UserModel.SetCollection(obj)); break;
                    case Code.GUI_UserAll: break;
                    case Code.GUI_UserModify: await Task.Factory.StartNew(() => _Main.Instance.MVVM_Manager.ModifyUserModel.SetCollection(obj)); break;
                }

            });

        }

        private void ThreadAcceptData_ErrorUpload(string error)
        {
            _Main.Instance.OverlayShow(true,TypeOverlay.error,"Возникла ошибка",error,visibleButton:Visibility.Visible);
        }

        private void ThreadAcceptData_StatusUpload((double, double) sendmax,bool collection = false)
        {
            _Main.Instance.OverlayShow(true,TypeOverlay.loading,"Данные",$"Загруженно {sendmax.Item1} из {sendmax.Item2} кб.",visibleButton:Visibility.Visible);
        }
    }
}

