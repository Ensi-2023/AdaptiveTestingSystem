using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_NewUserInsert:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_NewUserInsert>(json);

                switch (obj.IsCode)
                {
                    case Code.RegistrationSuccessful:

                        Application.Current.Dispatcher.Invoke(() => 
                        {
                            _Main.Instance._Notification.Add("Добавление", "Добавление успешно!", TypeNotification.Message);
                            _Main.Instance.OverlayShow(false);

                            var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Users_Insert;
                            if (guiUID == null)
                            {
                                
                                _Main.Instance.Manager.Back();
                                return;
                            }

                            guiUID.Clear();
                        });

                        break;
                    case Code.InvalidLogin:

                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка добавления", "Такой логин уже зарегистрирован", visibleButton: Visibility.Visible);
                      
                        break;

                    case Code.InvalidRegistration:

                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка добавления", "Возникла непредвиденная ошибка. Попробуйте попозже", visibleButton: Visibility.Visible);
                        break;


                    case Code.NewUserClassRoomError:
                    

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _Main.Instance.OverlayShow(false);
                            _Main.Instance._Notification.Add("Ошибка добавления", "Такой логин уже зарегистрирован", TypeNotification.Error);                          
                        });

                        break;

                    case Code.NewUserClassRoomnSuccessful:
                        _Main.Instance.OverlayShow(false);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _Main.Instance._Notification.Add("Добавление", "Добавление успешно!", TypeNotification.Message);
                         

                                if (UIHelper.IsWindowOpen<GUI_AddNewUserToClassRoom>())
                                {
                                    foreach (var item in Application.Current.Windows)
                                    {
                                        var window = item as GUI_AddNewUserToClassRoom;
                                        if (window != null)
                                        {
                                            Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                var guiUID = window.body.Children[0] as GUI_Users_Insert;
                                                if (guiUID == null)
                                                {                                              
                                                    return;
                                                }



                                                var userviewer= (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_ClassRoom_Viewer;
                                                if (userviewer != null)
                                                {
                                                    guiUID.Clear(userviewer, obj.Index);
                                                    return;
                                                }



                                                guiUID.Clear();

                                            });

                                            break;
                                        }
                                    }
                                }
                            

                        });

                        break;

                    case Code.Subject_Insert_Successfull:
                        _Main.Instance.OverlayShow(false);


                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _Main.Instance._Notification.Add("Добавление", "Добавление успешно!", TypeNotification.Message);


                            if (UIHelper.IsWindowOpen<GUI_AddNewUserToSubject>())
                            {
                                foreach (var item in Application.Current.Windows)
                                {
                                    var window = item as GUI_AddNewUserToSubject;
                                    if (window != null)
                                    {
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            var guiUID = window.body.Children[0] as GUI_Users_Insert;
                                            if (guiUID == null)
                                            {
                                                return;
                                            }

                                            var userviewer = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Subject_Viewer;
                                            if (userviewer != null)
                                            {
                                                guiUID.Clearv2(userviewer, obj.Index);
                                                return;
                                            }



                                            guiUID.Clear();

                                        });

                                        break;
                                    }
                                }
                            }


                        });


                        break;

                    default:
                        throw new Exception($"Код {obj.IsCode} не опознан");
                }


            }
            catch (Exception ex)
            {
                Logger.Error($"Command_Registration.Execut вызвал ошибку: {ex.Message}");
            }
        }

    }
}
