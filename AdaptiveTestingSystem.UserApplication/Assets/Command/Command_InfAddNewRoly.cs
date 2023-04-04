using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window._subpage_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window.page;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_InfAddNewRoly:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_RolyInf>(json);
                switch (obj.IsCode)
                {
                    case Code.Roly_Add_Successfull: SuccessfullAddnewRoly();  break;
                    case Code.Roly_NameError:
                        ErrorNameRoly();
                        break;

                    case Code.Roly_Update_Successfull: SuccessfullUpdateRoly(); break;
                    case Code.Roly_Update_NameError: ErrorNameRoly(); break;
                    
                }
           
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_InfAddNewRoly.Execut вызывал ошибку: {ex.Message}");
            }
        }

        private void SuccessfullUpdateRoly()
        {
            Application.Current.Dispatcher.Invoke(() => {

                if (UIHelper.IsWindowOpen<GUI_AddNewRolyUser>())
                {
                    foreach (var item in Application.Current.Windows)
                    {
                        var window = item as GUI_AddNewRolyUser;
                        if (window != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {

                                window.OverlayShow(false);

                                var guiUID = window.body.Children[0] as GUI_page_UpdateRoly;
                               
                                if (guiUID != null)
                                {
                                    guiUID.SaveData();
                                    window.Close();
                                    return;
                                }

                               

                            });

                            break;
                        }
                    }
                }

            
            });
        }

        private void SuccessfullAddnewRoly()
        {
            Application.Current.Dispatcher.Invoke(() => {

                if (UIHelper.IsWindowOpen<GUI_AddNewRolyUser>())
                {
                    foreach (var item in Application.Current.Windows)
                    {
                        var window = item as GUI_AddNewRolyUser;
                        if (window != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                               
                                window.OverlayShow(false);
                                window.Close();
                            });

                            break;
                        }
                    }
                }

                _Main.Instance._Notification.Add("","Новая роль добавлена",TypeNotification.Message);

            });


        }

        private void ErrorNameRoly()
        {
            Application.Current.Dispatcher.Invoke(() => {

                if (UIHelper.IsWindowOpen<GUI_AddNewRolyUser>())
                {
                    foreach (var item in Application.Current.Windows)
                    {
                        var window = item as GUI_AddNewRolyUser;
                        if (window != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                               window.OverlayShow(true, TypeOverlay.error, "Ошибка добавления", "Данная роль уже существует", visibleButton: Visibility.Visible);
                            });

                            break;
                        }
                    }
                }
            });
        }
    }
}
