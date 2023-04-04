#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace AdaptiveTestingSystem.Control.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessageGUI.xaml
    /// </summary>
    public partial class MessageGUI : Window
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageGUI), new PropertyMetadata(string.Empty));


        public MessageShow.Type TypeMessage
        {
            get { return (MessageShow.Type)GetValue(TypeMessageProperty); }
            set
            {
                SetValue(TypeMessageProperty, value);
                switch (value)
                {
                    case MessageShow.Type.Error:

                        PART_Panel.Style = (Style)TryFindResource("BorderErrorPanel");
                        PART_Button_Yes.Content = "OK";
                        PART_Button_no.Visibility = Visibility.Collapsed;
                        PART_Button_Yes.Background = (Brush)TryFindResource("MessageShowButtonBackground_Error");
                        PART_Icon.Foreground = (Brush)TryFindResource("MessageShowIconBackground_Error");
                        PART_Icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.AlertCircle;
                        PART_Content.Foreground = (Brush)TryFindResource("MessageShowForeground_Error");
                        PART_Button_Yes.Foreground = (Brush)TryFindResource("MessageShowButtonForeground_Error");
                        PART_Button_no.Foreground = (Brush)TryFindResource("MessageShowButtonForeground_Error");
                        break;
                    case MessageShow.Type.Message:
                        PART_Panel.Style = (Style)TryFindResource("BorderMessagePanel");
                        PART_Button_Yes.Content = "OK";
                        PART_Button_no.Visibility = Visibility.Collapsed;
                        PART_Button_Yes.Background = (Brush)TryFindResource("MessageShowButtonBackground_Message");
                        PART_Icon.Foreground = (Brush)TryFindResource("MessageShowIconBackground_Message");
                        PART_Content.Foreground = (Brush)TryFindResource("MessageShowForeground_Message");
                        PART_Icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.Message;
                        PART_Button_Yes.Foreground = (Brush)TryFindResource("MessageShowButtonForeground_Message");
                        PART_Button_no.Foreground = (Brush)TryFindResource("MessageShowButtonForeground_Message");
                        break;
                    case MessageShow.Type.Question:
                        PART_Panel.Style = (Style)TryFindResource("BorderQuestionPanel");
                        PART_Button_Yes.Content = "Да";
                        PART_Button_no.Visibility = Visibility.Visible;
                        PART_Button_Yes.Background = (Brush)TryFindResource("MessageShowButtonBackground_Questions");
                        PART_Button_no.Background = (Brush)TryFindResource("MessageShowButtonBackground_Questions");
                        PART_Icon.Foreground = (Brush)TryFindResource("MessageShowIconBackground_Questions");
                        PART_Icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.FrequentlyAskedQuestions;
                        PART_Content.Foreground = (Brush)TryFindResource("MessageShowForeground_Questions");
                        PART_Button_Yes.Foreground = (Brush)TryFindResource("MessageShowButtonForeground_Questions");
                        PART_Button_no.Foreground = (Brush)TryFindResource("MessageShowButtonForeground_Questions");

                        break;
                }
            }
        }

        public static readonly DependencyProperty TypeMessageProperty = DependencyProperty.Register("TypeMessage", typeof(MessageShow.Type), typeof(MessageGUI), new PropertyMetadata(MessageShow.Type.Message));

        public MessageGUI()
        {
            InitializeComponent();
        }

        public MessageGUI(string message)
        {
            InitializeComponent();
            Message = message;
        }

        public MessageGUI(string message, string title)
        {
            InitializeComponent();
            Message = message;
            Title = title;
        }

        public MessageGUI(string message, string title, MessageShow.Type type)
        {
            InitializeComponent();
            Message = message;
            Title = title;
            TypeMessage = type;
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
     
        }

        private void PART_Button_no_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void PART_Button_Yes_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
            Close();
        }
    }
}
