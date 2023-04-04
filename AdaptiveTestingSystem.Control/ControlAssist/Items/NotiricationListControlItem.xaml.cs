using AdaptiveTestingSystem.Control.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using AdaptiveTestingSystem.Control.Windows;

namespace AdaptiveTestingSystem.Control.ControlAssist.Items
{
    /// <summary>
    /// Логика взаимодействия для NotiricationListControlItem.xaml
    /// </summary>
    public partial class NotiricationListControlItem : UserControl, INotifyPropertyChanged
    {

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); OnPropertyChanged("IsPressed"); }
        }


        public static readonly DependencyProperty IsPressedProperty;


        static NotiricationListControlItem()
        {
            TimesProperty = DependencyProperty.Register("Times", typeof(string), typeof(NotiricationListControlItem), new PropertyMetadata(DateTime.Now.ToString()));
            IsViewProperty = DependencyProperty.Register("IsView", typeof(bool), typeof(NotiricationListControlItem), new PropertyMetadata(false));
            IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(NotiricationListControlItem), new PropertyMetadata(false));
            IconProperty =  DependencyProperty.Register("IsIcon", typeof(TypeNotification), typeof(NotiricationListControlItem), new PropertyMetadata(TypeNotification.Message));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotiricationListControlItem), new FrameworkPropertyMetadata(typeof(NotiricationListControlItem)));
        }

       

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
                OnPropertyChanged("Title");
            }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value);
                OnPropertyChanged("Message");
            }
        }


        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(NotiricationListControlItem), new PropertyMetadata(string.Empty));




        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NotiricationListControlItem), new PropertyMetadata(string.Empty));





        public string Times
        {
            get { return (string)GetValue(TimesProperty); }
            set { SetValue(TimesProperty, value); }
        }

  
        public static readonly DependencyProperty TimesProperty;


        public TypeNotification IsIcon
        {
            get { return (TypeNotification)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); OnPropertyChanged("IsIcon"); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty;



        public NotiricationListControlItem(TypeNotification icon = TypeNotification.Message)
        {
            InitializeComponent();
            IsIcon = icon;
            Times = DateTime.Now.ToString();
        }

        private void root_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPressed = true;
            IsView = true;    
        }

        private void root_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPressed = false;
        }


        public bool IsView
        {
            get { return (bool)GetValue(IsViewProperty); }
            set { SetValue(IsViewProperty, value); OnPropertyChanged("IsView");
                if (value) { Viewing?.Invoke(this); Index = 2; } 
            }
        }

     
        public static readonly DependencyProperty IsViewProperty;



        public delegate void ViewerHandler(NotiricationListControlItem obj);
        public event ViewerHandler? Viewing;



        public int Index = 1;

    }
}
