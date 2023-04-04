#nullable disable
using AdaptiveTestingSystem.Control.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;

namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для NotificationControll.xaml
    /// </summary>
    public partial class NotificationControll : UserControl
    {

        private static List<StyleSetup> StyleNotification { get; set; }
        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(NotificationControll), new PropertyMetadata(Brushes.Transparent));

        public Brush BarColor
        {
            get { return (Brush)GetValue(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarColorProperty =
            DependencyProperty.Register("BarColor", typeof(Brush), typeof(NotificationControll), new PropertyMetadata(Brushes.GreenYellow));



        public Brush NotificationBackground
        {
            get { return (Brush)GetValue(NotificationBackgroundProperty); }
            set { SetValue(NotificationBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationBackgroundProperty =
            DependencyProperty.Register("NotificationBackground", typeof(Brush), typeof(NotificationControll), new PropertyMetadata(Brushes.White));




        public string NotificationTitle
        {
            get { return (string)GetValue(NotificationTitleProperty); }
            set { SetValue(NotificationTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTitleProperty =
            DependencyProperty.Register("NotificationTitle", typeof(string), typeof(NotificationControll), new PropertyMetadata("NULL"));



        public string NotificationText
        {
            get { return (string)GetValue(NotificationTextProperty); }
            set { SetValue(NotificationTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTextProperty =
            DependencyProperty.Register("NotificationText", typeof(string), typeof(NotificationControll), new PropertyMetadata("NULL"));




        public Brush NotificationTitleForeground
        {
            get { return (Brush)GetValue(NotificationTitleForegroundProperty); }
            set { SetValue(NotificationTitleForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationTitleForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTitleForegroundProperty =
            DependencyProperty.Register("NotificationTitleForeground", typeof(Brush), typeof(NotificationControll), new PropertyMetadata(Brushes.Black));





        public Brush NotificationIconColor
        {
            get { return (Brush)GetValue(NotificationIconColorProperty); }
            set { SetValue(NotificationIconColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationIconColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationIconColorProperty =
            DependencyProperty.Register("NotificationIconColor", typeof(Brush), typeof(NotificationControll), new PropertyMetadata(Brushes.Black));




        public Brush NotificationTextForeground
        {
            get { return (Brush)GetValue(NotificationTextForegroundProperty); }
            set { SetValue(NotificationTextForegroundProperty, value); }
        }




        public PackIconMaterialKind Icon
        {
            get { return (PackIconMaterialKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconMaterialKind), typeof(NotificationControll));





        public TypeNotification TypeNotification
        {
            get { return (TypeNotification)GetValue(TypeNotificationProperty); }
            set { SetValue(TypeNotificationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeNotification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeNotificationProperty =
            DependencyProperty.Register("TypeNotification", typeof(TypeNotification), typeof(NotificationControll), new PropertyMetadata(TypeNotification.Questions));





        // Using a DependencyProperty as the backing store for NotificationTextForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTextForegroundProperty =
            DependencyProperty.Register("NotificationTextForeground", typeof(Brush), typeof(NotificationControll), new PropertyMetadata(Brushes.Black));


        BackgroundWorker bw;
        Notification GetNotification;
      


        public NotificationControll(Notification notification)
        {
            InitializeComponent();
            GetNotification = notification;
            SetStyle();
        }

        private void SetStyle()
        {
            StyleNotification = new List<StyleSetup>()
            {
                new StyleSetup()
                {
                    BarColor = (Brush)TryFindResource("NotificationBarColor_Message"),
                    BorderColor = (Brush)TryFindResource("NotificationBorderColor_Message"),
                    NotificationBackground = (Brush)TryFindResource("NotificationBackground_Message"),
                    NotificationTextForeground = (Brush)TryFindResource("NotificationTextForeground_Message"),
                    NotificationTitleForeground = (Brush)TryFindResource("NotificationTitleForeground_Message"),
                    NotificationIconColor = (Brush)TryFindResource("NotificationIconColor_Message"),
                    Type = TypeNotification.Message,
                    Icon = PackIconMaterialKind.Message
                },
                 new StyleSetup()
                {
                    BarColor = (Brush)TryFindResource("NotificationBarColor_Error"),
                    BorderColor = (Brush)TryFindResource("NotificationBorderColor_Error"),
                    NotificationBackground = (Brush)TryFindResource("NotificationBackground_Error"),
                    NotificationTextForeground = (Brush)TryFindResource("NotificationTextForeground_Error"),
                    NotificationTitleForeground = (Brush)TryFindResource("NotificationTitleForeground_Error"),
                    NotificationIconColor = (Brush)TryFindResource("NotificationIconColor_Error"),
                    Type = TypeNotification.Error,
                    Icon = PackIconMaterialKind.CloseThick
                },
                  new StyleSetup()
                {
                    BarColor = (Brush)TryFindResource("NotificationBarColor_Warning"),
                    BorderColor = (Brush)TryFindResource("NotificationBorderColor_Warning"),
                    NotificationBackground = (Brush)TryFindResource("NotificationBackground_Warning"),
                    NotificationTextForeground = (Brush)TryFindResource("NotificationTextForeground_Warning"),
                    NotificationTitleForeground = (Brush)TryFindResource("NotificationTitleForeground_Warning"),
                    NotificationIconColor = (Brush)TryFindResource("NotificationIconColor_Warning"),
                    Type = TypeNotification.Warning,
                    Icon = PackIconMaterialKind.Alert
                },
                   new StyleSetup()
                {
                    BarColor = (Brush)TryFindResource("NotificationBarColor_Questions"),
                    BorderColor = (Brush)TryFindResource("NotificationBorderColor_Questions"),
                    NotificationBackground = (Brush)TryFindResource("NotificationBackground_Questions"),
                    NotificationTextForeground = (Brush)TryFindResource("NotificationTextForeground_Questions"),
                    NotificationTitleForeground = (Brush)TryFindResource("NotificationTitleForeground_Questions"),
                    NotificationIconColor = (Brush)TryFindResource("NotificationIconColor_Questions"),
                    Type = TypeNotification.Questions,
                    Icon = PackIconMaterialKind.CommentQuestion
                },
            };

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetupStyle(TypeNotification);
        }


        private void SetupStyle(TypeNotification type)
        {
            var styles = StyleNotification.Find(o => o.Type == type);
            if (styles == null) return;
            BarColor = styles.BarColor;
            BorderColor = styles.BorderColor;
            NotificationBackground = styles.NotificationBackground;
            NotificationTextForeground = styles.NotificationTextForeground;
            NotificationTitleForeground = styles.NotificationTitleForeground;
            NotificationIconColor = styles.NotificationIconColor;
            Icon = styles.Icon;


        }



        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.RunWorkerAsync();


            SystemSounds.Beep.Play();
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            for (int i = 0; i < 100; i++)
            {
                bw?.ReportProgress(i);
                Thread.Sleep(20);
            }


        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void WidenObject(int newWidth, int newHeight, int opacity, TimeSpan duration)
        {
            DoubleAnimation animationW = new DoubleAnimation(newWidth, duration);
            DoubleAnimation animationH = new DoubleAnimation(newHeight, duration);
            DoubleAnimation animationO = new DoubleAnimation(opacity, duration);
            this.BeginAnimation(Rectangle.OpacityProperty, animationO);
            this.BeginAnimation(Rectangle.WidthProperty, animationW);
            this.BeginAnimation(Rectangle.HeightProperty, animationH);
        }


        async void Close()
        {
            bw?.Dispose();
            WidenObject(0, 0, 0, TimeSpan.FromMilliseconds(200));
            await Task.Delay(350);

            GetNotification.Delete(this);
        }

        private void UserControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
    #nullable enable
    class StyleSetup
    {
        public Brush? BarColor { get; set; }
        public Brush? BorderColor { get; set; }
        public Brush? NotificationBackground { get; set; }
        public Brush? NotificationTitleForeground { get; set; }
        public Brush? NotificationTextForeground { get; set; }
        public Brush? NotificationIconColor { get; set; }
        public TypeNotification? Type { get; set; }
        public PackIconMaterialKind Icon { get; set; }
    }
}
