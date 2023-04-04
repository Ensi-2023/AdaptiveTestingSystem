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

namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для LogAnswerControl.xaml
    /// </summary>
    public partial class LogAnswerControl : UserControl
    {

        public Brush ElipseColor
        {
            get { return (Brush)GetValue(ElipseColorProperty); }
            set { SetValue(ElipseColorProperty, value); }
        }

        public static readonly DependencyProperty ElipseColorProperty =
            DependencyProperty.Register("ElipseColor", typeof(Brush), typeof(LogAnswerControl), new PropertyMetadata(Brushes.Transparent));



        public bool IsCorrect
        {
            get { return (bool)GetValue(IsCorrectProperty); }
            set
            { 
                SetValue(IsCorrectProperty, value);

                if (value)
                {
                    AnswerNotCorrectIcon.Visibility = Visibility.Collapsed;
                    AnswerCorrectIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    AnswerNotCorrectIcon.Visibility = Visibility.Visible;
                    AnswerCorrectIcon.Visibility = Visibility.Collapsed;
                }
            
            }
        }



        public string NameUser
        {
            get { return (string)GetValue(NameUserProperty); }
            set { SetValue(NameUserProperty, value); }
        }

        public string CorrectNumber
        {
            get { return (string)GetValue(CorrectNumberProperty); }
            set { SetValue(CorrectNumberProperty, value); }
        }

        public string AnswerNumber
        {
            get { return (string)GetValue(AnswerNumberProperty); }
            set { SetValue(AnswerNumberProperty, value); }
        }




        public string AVGScore
        {
            get { return (string)GetValue(AVGScoreProperty); }
            set { SetValue(AVGScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AVGScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AVGScoreProperty =
            DependencyProperty.Register("AVGScore", typeof(string), typeof(LogAnswerControl), new PropertyMetadata(string.Empty));



        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }


        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(LogAnswerControl), new PropertyMetadata(0));




        public static readonly DependencyProperty AnswerNumberProperty =
            DependencyProperty.Register("AnswerNumber", typeof(string), typeof(LogAnswerControl), new PropertyMetadata(string.Empty));



        public static readonly DependencyProperty CorrectNumberProperty =
            DependencyProperty.Register("CorrectNumber", typeof(string), typeof(LogAnswerControl), new PropertyMetadata(string.Empty));



        public static readonly DependencyProperty NameUserProperty =
            DependencyProperty.Register("NameUser", typeof(string), typeof(LogAnswerControl), new PropertyMetadata(string.Empty));



        public static readonly DependencyProperty IsCorrectProperty =
            DependencyProperty.Register("IsCorrect", typeof(bool), typeof(LogAnswerControl), new PropertyMetadata(false));


        public LogAnswerControl()
        {
            InitializeComponent();
        }
    }
}
