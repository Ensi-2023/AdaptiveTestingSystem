using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_User_Insert_klassList:Commands
    {
        public override async void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_Klass>>(json);


                if (obj[0].IsCode == Code.ErrorGetInfoKlass)
                {
                    Logger.Error("Ошибка получения данных");
                    _Main.Instance.ShowMessage("Ошибка получения данных о классах", "Данные о классах", TypeNotification.Error);

                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Ошибка получения данных о классах", type: TypeNotification.Error);
                    Clear();
                    return;
                }


                if (obj[0].IsCode == Code.ErrorGetInfoKlassNoData)
                {
                    Logger.Log("В базе данных нет записей о свободных классах");
                    _Main.Instance.ShowMessage("В базе данных нет записей о свободных классах", "Данные о классах", TypeNotification.Error);

                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"В базе данных нет записей о свободных классах", type: TypeNotification.Error);
                    Clear();
                    return;
                }



                await Set(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Clear()
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Users_Insert;
                if (guiUID == null) return;
                guiUID.studentKlasses.Children.Clear();
                guiUID.KlassesList.Clear();
                guiUID.DeleteKlassData.IsEnabled = false;
            });

        }

        private async Task Set(List<Data_Klass>? obj)
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Users_Insert;
                if (guiUID == null)
                {
                    return;
                }

                guiUID.Overlay(true);

                guiUID.studentKlasses.Children.Clear();
                guiUID.KlassesList.Clear();
                guiUID.DeleteKlassData.IsEnabled = true;

                foreach (var item in obj.ToList())
                {
                    guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Users_Insert;
                    if (guiUID == null || ParserVariables.GetInt(guiUID.Uid) != 31)
                    {
                        break;
                    }

                    if (guiUID.student.IsChecked == true)
                    {

                        var newCheck = new CustomRadioButton() { Content = item.Name, ID = item.Id };
                        guiUID.studentKlasses.Children.Add(newCheck);
                        guiUID.KlassesList.Add(newCheck);
                    }
                    else
                    {
                        var newCheck = new CustomCheckBox() { Content = item.Name, ID = item.Id };
                        guiUID.studentKlasses.Children.Add(newCheck);
                        guiUID.KlassesList.Add(newCheck);
                    }
                    await Task.Delay(5);
                }


                if (guiUID != null)
                {
                    guiUID.Overlay(false);
                }

            });
        }
    }
}
