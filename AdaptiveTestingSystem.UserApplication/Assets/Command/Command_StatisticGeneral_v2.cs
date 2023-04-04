//using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui;
//using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
//using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports;
//using System.Threading;

//namespace AdaptiveTestingSystem.UserApplication.Assets.Command
//{
//    public class Command_StatisticGeneral_v2:Commands
//    {

//        ThreadAcceptData AcceptData;

//        public override void Execut(string json, InternetClient client)
//        {
//            try
//            {
//                Application.Current.Dispatcher.Invoke(() => 
//                {
//                    var obj = JsonSerializer.Deserialize<Data_StatisticPacket>(json);
//                    if (obj == null) return;

//                    if (obj != null)
//                    {


//                        if (obj.IsCode == Code.ThreadStart)
//                        {
//                            AcceptData = new ThreadAcceptData();
//                            AcceptData.StatusUpload += ThreadAcceptData_StatusUpload;
//                            AcceptData.ErrorUpload += ThreadAcceptData_ErrorUpload;
//                            AcceptData.FinishUpload += ThreadAcceptData_FinishUpload;
//                        }

//                       if(AcceptData!=null) AcceptData.Accept(obj);
//                    }

//                });

//            }
//            catch (Exception ex)
//            {
//                Logger.Error($"Command_StatisticGeneral.Execut вызывал ошибку: {ex.Message}");
       
//                Clear();
//            }
//        }

//        private void Clear()
//        {
//            AcceptData = null;
//            ThreadManager.CloseActiveThread();
//        }

//        private void ThreadAcceptData_FinishUpload(object packet)
//        {
//            AcceptData.StatusUpload -= ThreadAcceptData_StatusUpload;
//            AcceptData.ErrorUpload -= ThreadAcceptData_ErrorUpload;
//            AcceptData.FinishUpload -= ThreadAcceptData_FinishUpload;


//            var obj = JsonSerializer.Deserialize<Data_StatisticGeneral>(packet.ToString());
//            if (obj != null)
//            {

//                var window = CheckUI.GetMainBodyUI() as GUI_Statistic;
//                if (window != null)
//                {
//                    window.SetData(null,null);
//                }
//                AcceptData = null;
//            }
//        }

//        private void ThreadAcceptData_ErrorUpload(string error)
//        {
//            _Main.Instance.OverlayShow(true, TypeOverlay.error, "Возникла ошибка", error, visibleButton: Visibility.Visible);
//        }

//        private void ThreadAcceptData_StatusUpload((double, double) sendmax,bool collection = false)
//        {
//            var window = CheckUI.GetMainBodyUI() as GUI_Statistic;

//            if (window == null)
//            {
//                Clear();
//            }
//            else
//            {
//                if (window.IsCancelUpload()) Clear();
//                window.SetInfo(sendmax.Item1, sendmax.Item2);
//            }

//        }
//    }
//}
