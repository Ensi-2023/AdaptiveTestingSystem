using AdaptiveTestingSystem.Control.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class NotificationViewerManager
    {

        private NotificationButton notificationButton;

        private NotiricationListControll controll;
        public NotificationViewerManager(NotiricationListControll list)
        {
            this.controll = list;
            controll.CountNotification += Controll_CountNotification;

         
        }

        public void Add(string content, string title = "", TypeNotification type = TypeNotification.Error)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                controll.Add(content, title, type);
            });

           
        }

        public void SetNotificationButton(NotificationButton button)
        {
            this.notificationButton = button;
        }

        private void Controll_CountNotification(int count)
        {
            if (count >= 99) 
                notificationButton.Count = 99;
            else
                notificationButton.Count = count;
        }
    }
}
