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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdaptiveTestingSystem.Control.ControlAssist.Items;
using AdaptiveTestingSystem.Control.Windows;

namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для NotiricationListControll.xaml
    /// </summary>
    public partial class NotiricationListControll : UserControl
    {
   
        private async void Closed()
        {
            Animation.AnimatedOpacity(root, root.Opacity, 0, TimeSpan.FromMilliseconds(100));
            Animation.AnimatedWidth(body, body.ActualWidth, 0, TimeSpan.FromMilliseconds(250));

            await Task.Delay(300);
            this.Visibility = Visibility.Collapsed;
        }

        public void Open()
        {
            this.Visibility = Visibility.Visible;
            Animation.AnimatedOpacity(root, 0, 1, TimeSpan.FromMilliseconds(100));
            Animation.AnimatedWidth(body, 0, 350, TimeSpan.FromMilliseconds(250));
            BubbleSort(notification);

        }

        private void BubbleSort(StackPanel array)
        {
       
              
                var task = Task.Run(() =>
                {

                    Application.Current.Dispatcher.Invoke(async() =>
                    {
                        overlayUpdate.Visibility = Visibility.Visible;
                        var obj = new List<NotiricationListControlItem>();
                        var obj2 = new List<NotiricationListControlItem>();


                        for (int i = 0; i < array.Children.Count; i++)
                        {
                            var item = array.Children[i] as NotiricationListControlItem;
                            if (item != null)
                            {

                                if (item.IsView==false)
                                {
                                    obj.Add(item);
                                }
                                else
                                    obj2.Add(item);
                            }

                            await Task.Delay(1);
                        }


                        array.Children.Clear();

                        foreach (var ob in obj)
                        {
                            array.Children.Add(ob);
                        }

                        foreach (var ob in obj2)
                        {
                            array.Children.Add(ob);
                        }


                        obj.Clear();
                        obj2.Clear();

                        overlayUpdate.Visibility = Visibility.Collapsed;

                    });
                });

        
       
        }


        public void Add(string content, string title = "", TypeNotification type = TypeNotification.Message)
        {
            NewNotification(content, title, type);
        }


        public delegate void ViewerHandler(int count);
        public event ViewerHandler? CountNotification;


        private void NewNotification(string content, string title, TypeNotification type)
        {

            var obj = new NotiricationListControlItem(type) { Message = content, Title = title };
            obj.Viewing += Obj_Viewing;
            notification.Children.Add(obj);

            CalculateNotification();
        }

        private void CalculateNotification()
        {
              int sum = 0;

            if (notification.Children.Count != 0)
            {

                for (int i = 0; i < notification.Children.Count; i++)
                {
                    var obj = notification.Children[i] as NotiricationListControlItem;
                    if (obj != null)
                    {
                        if (obj.IsView == false) sum++;
                    }
                }
            }

            CountNotification?.Invoke(sum);
        }

        private void Obj_Viewing(NotiricationListControlItem obj)
        {
            obj.Viewing -= Obj_Viewing;
            CalculateNotification();
        }

        public NotiricationListControll()
        {
            InitializeComponent();
        }

        private void openedButton_Click(object sender, RoutedEventArgs e)
        {
             Closed();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (notification.Children.Count != 0)
            {
                for (int i = 0; i < notification.Children.Count; i++)
                {
                    var obj = notification.Children[i] as NotiricationListControlItem;
                    if (obj != null)
                    {
                        obj.IsView = true;
                    }
                }
            }
            CalculateNotification();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
     
            RemoveView(notification);

    ; 
        }

        private void RemoveView(StackPanel array)
        {

            var task = Task.Run(() =>
            {

                Application.Current.Dispatcher.Invoke(async () =>
                {
                    overlayUpdate.Visibility = Visibility.Visible;
                    var obj = new List<NotiricationListControlItem>();
            


                    for (int i = 0; i < array.Children.Count; i++)
                    {
                        var item = array.Children[i] as NotiricationListControlItem;
                        if (item != null)
                        {

                            if (item.IsView==false)
                            {
                                obj.Add(item);
                              
                            }
                      
                        }

                        await Task.Delay(1);
                    }


                    array.Children.Clear();

                    foreach (var ob in obj)
                    {
                        array.Children.Add(ob);
                    }
                    overlayUpdate.Visibility = Visibility.Collapsed;
                    CalculateNotification();
                });
            });



        }

        private void overlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Closed();
            }
        }

        private void sortedButton_Click(object sender, RoutedEventArgs e)
        {
            BubbleSort(notification);
        }

      
    }
}
