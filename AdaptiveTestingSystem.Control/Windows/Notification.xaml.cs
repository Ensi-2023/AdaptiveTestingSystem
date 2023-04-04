using AdaptiveTestingSystem.Control.CustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.Control.Windows
{
    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    /// 
    public enum TypeNotification
    {
        Message,
        Error,
        Questions,
        Warning
    }

    public partial class Notification : Window
    {
        static Notification Instance;


        public Notification()
        {
            InitializeComponent();
            Instance = this;
        }

      

        public void Add(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Instance == null)
                {
                    Instance = new Notification();
                }


                var primaryMonitorArea = SystemParameters.WorkArea;
                Instance.Left = primaryMonitorArea.Right - Width - 10;
                Instance.Top = primaryMonitorArea.Bottom - Height - 10;


                Instance.NotofocationChild.Children.Add(
                new NotificationControll(this)
                {
                    NotificationTitle = "",
                    NotificationText = message,
                    TypeNotification = TypeNotification.Message
                });



                Instance.Topmost = true;
                Instance.Show();
            });
        }

        public void Add(string title, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Instance == null)
                {
                    Instance = new Notification();
                }



                Instance.NotofocationChild.Children.Add(
                new NotificationControll(this)
                {
                    NotificationTitle = title,
                    NotificationText = message,
                    TypeNotification = TypeNotification.Message
                });



                Instance.Topmost = true;
                Instance.Show();
            });
        }


        public void Add(string title, string message, TypeNotification type)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                //StartGUI.Account.Set(obj.AccessRights[0], obj.Name);
                //StartGUI.Instance.SetChilden(new MainGUI());

                if (Instance == null)
                {
                    Instance = new Notification();
                }


                var primaryMonitorArea = SystemParameters.WorkArea;
                Instance.Left = primaryMonitorArea.Right - Width - 10;
                Instance.Top = primaryMonitorArea.Bottom - Height - 10;

                Instance.NotofocationChild.Children.Add(
                new NotificationControll(this)
                {
                    NotificationTitle = title,
                    NotificationText = message,
                    TypeNotification = type
                });


                Instance.Topmost= true;
                Instance.Show();

            });


        }

        private Brush SetTitleForeground(TypeNotification type)
        {
            return Brushes.DarkGray;
        }

        private Brush SetTextForeground(TypeNotification type)
        {
            return Brushes.Black;
        }

        private Brush SetBC(TypeNotification type)
        {
            return Brushes.BlueViolet;
        }

        private Brush SetBG(TypeNotification type)
        {
            return Brushes.LimeGreen;
        }

        public void Delete(NotificationControll notifocation)
        {

            try
            {
                if (Instance == null) return;

                Instance.NotofocationChild.Children.Remove(notifocation);

                if (Instance.NotofocationChild.Children.Count == 0)
                {
                    Instance.Close();
                    Instance = null;
                }
            }catch(Exception ex) { Logger.Error(ex.Message); }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var primaryMonitorArea = SystemParameters.WorkArea;
            Instance.Left = primaryMonitorArea.Right - Width - 10;
            Instance.Top = primaryMonitorArea.Bottom - Height - 10;
        }
    }
}
