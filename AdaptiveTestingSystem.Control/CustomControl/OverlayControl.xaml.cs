using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AdaptiveTestingSystem.Control.CustomControl
{
    public partial class OverlayControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); timeTimer = 60; }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(OverlayControl), new PropertyMetadata(string.Empty));



        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); timeTimer = 60; }
        }

        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.Register("SubTitle", typeof(string), typeof(OverlayControl), new PropertyMetadata(string.Empty));



        public TypeOverlay TOverlay
        {
            get { return (TypeOverlay)GetValue(TOverlayProperty); }
            set { SetValue(TOverlayProperty, value); }
        }

        public static readonly DependencyProperty TOverlayProperty =
            DependencyProperty.Register("TOverlay", typeof(TypeOverlay), typeof(OverlayControl), new PropertyMetadata(TypeOverlay.loading, CallBackOverlay));




        public Visibility ButtonVisible
        {
            get { return (Visibility)GetValue(ButtonVisibleProperty); }
            set { SetValue(ButtonVisibleProperty, value); }
        }


        public static readonly DependencyProperty ButtonVisibleProperty =
            DependencyProperty.Register("ButtonVisible", typeof(Visibility), typeof(OverlayControl), new PropertyMetadata(Visibility.Collapsed));



        private static void CallBackOverlay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as OverlayControl;
            if (obj == null) return;


            switch ((TypeOverlay)e.NewValue)
            {
                case TypeOverlay.loading:
                    obj.errorTemplated.Visibility = Visibility.Collapsed;
                    obj.messageTemplated.Visibility = Visibility.Collapsed;
                    obj.loadingTemplated.Visibility = Visibility.Visible;

                    break;
                case TypeOverlay.error:
                    obj.errorTemplated.Visibility = Visibility.Visible;

                    obj.loadingTemplated.Visibility = Visibility.Collapsed;
                    obj.messageTemplated.Visibility = Visibility.Collapsed;
                    break;
                case TypeOverlay.message:

                    obj.errorTemplated.Visibility = Visibility.Collapsed;
                    obj.loadingTemplated.Visibility = Visibility.Collapsed;
                    obj.messageTemplated.Visibility = Visibility.Visible;

                    break;
                default:

                    obj.errorTemplated.Visibility = Visibility.Collapsed;
                    obj.loadingTemplated.Visibility = Visibility.Collapsed;
                    obj.messageTemplated.Visibility = Visibility.Collapsed;
                    obj.ButtonVisible = Visibility.Collapsed;

                    break;
            }
        }

        public OverlayControl()
        {
            InitializeComponent();
        }


        private DispatcherTimer TimeUpdate;
        private int timeTimer;


        public delegate void OverlayCloseHandler();
        public event OverlayCloseHandler? OverlayThreadStop;


        public void SetupTimer()
        {
            if (TimeUpdate != null) { TimeUpdate.Stop(); TimeUpdate.Tick -= (timeReconnectr_Tick); }

            timeTimer = 60;
            TimeUpdate = new DispatcherTimer();
            TimeUpdate.Tick += new EventHandler(timeReconnectr_Tick);
            TimeUpdate.Interval = TimeSpan.FromSeconds(1);
            TimeUpdate.Start();
        }


        private void TimerStop()
        {
            TimeUpdate.Stop();
            TimeUpdate.Tick -= (timeReconnectr_Tick);

        }

        private void timeReconnectr_Tick(object sender, EventArgs e)
        {

            if (timeTimer <= 0)
            {

                ButtonVisible = Visibility.Visible;
            }

            timeTimer--;

        }

        private void root_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as UserControl).Visibility == Visibility.Visible)
            {
                Animation.AnimatedOpacity(this, 0, 1, TimeSpan.FromMilliseconds(750));
                SetupTimer();
            }
            else
            {
                TOverlay = TypeOverlay.nullable;
                TimerStop();
            }
        }

        private void CLosedOverlay_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility=Visibility.Collapsed;
            OverlayThreadStop?.Invoke();
        }

    }
}
