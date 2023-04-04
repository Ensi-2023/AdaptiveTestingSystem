using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public enum WindowButton
        {
            All,
            Close,
            ColoseAndMinimize
        }

        private Window _window;

        public delegate void MinimizeClickHandler();
        public event MinimizeClickHandler? MinimizeClick;

        public delegate void MaximizeClickHandler();
        public event MaximizeClickHandler? MaximizeClick;

        public delegate void CLoseClickHandler();
        public event CLoseClickHandler? CloseClick;

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(HeaderControl), new PropertyMetadata(string.Empty));


        public Brush PanelColor
        {
            get { return (Brush)GetValue(PanelColorProperty); }
            set { SetValue(PanelColorProperty, value); }
        }

        public static readonly DependencyProperty PanelColorProperty =
            DependencyProperty.Register("PanelColor", typeof(Brush), typeof(HeaderControl), new PropertyMetadata(Brushes.Transparent));



        public WindowButton HeaderButton
        {
            get { return (WindowButton)GetValue(HeaderButtonProperty); }
            set { SetValue(HeaderButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderButtonProperty =
            DependencyProperty.Register("HeaderButton", typeof(WindowButton), typeof(HeaderControl), new PropertyMetadata(WindowButton.All,WindowButtonPropertyChange));

        private static void WindowButtonPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as HeaderControl;
            if (obj == null) return;

            switch ((WindowButton)e.NewValue)
            {
                case WindowButton.All:
                    obj.FormMinimize.Visibility = Visibility.Visible;
                    obj.FormMaximize.Visibility = Visibility.Visible;
                    break;
                case WindowButton.Close:
                    obj.FormMinimize.Visibility = Visibility.Collapsed;
                    obj.FormMaximize.Visibility = Visibility.Collapsed;
                    break;
                case WindowButton.ColoseAndMinimize:
                    obj.FormMinimize.Visibility = Visibility.Visible;
                    obj.FormMaximize.Visibility = Visibility.Collapsed;
                    break;
            }

        }

        public HeaderControl()
        {
            InitializeComponent();
            Loaded += HeaderControl_Loaded;
            MouseDoubleClick += HeaderControl_MouseDoubleClick;
        }

        private void HeaderControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_window.WindowState == WindowState.Normal)
            {
                _window.WindowState = WindowState.Maximized;
                _window.Margin = new Thickness(10);
            }
            else
            {
                _window.WindowState = WindowState.Normal;
                _window.Margin = new Thickness(0, 0, 0, 0);
            }
        }


        private void HeaderControl_Loaded(object sender, RoutedEventArgs e)
        {
            _window = UIHelper.FindParent(this);
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
          if (e.ChangedButton == MouseButton.Left)
            {         
                _window.Cursor = Cursors.Cross;
                _window.DragMove();   
                _window.Cursor = Cursors.Arrow;
            }
        }

        private void FormMinimize_Click(object sender, RoutedEventArgs e)
        {
            MinimizeClick?.Invoke();
        }

        private void FormMaximize_Click(object sender, RoutedEventArgs e)
        {
            MaximizeClick?.Invoke();
        }

        private void FormClose_Click(object sender, RoutedEventArgs e)
        {
            CloseClick?.Invoke();
        }
    }
}
