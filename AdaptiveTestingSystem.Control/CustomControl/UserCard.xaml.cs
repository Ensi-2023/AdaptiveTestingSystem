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
    /// Логика взаимодействия для UserCard.xaml
    /// </summary>
    public partial class UserCard : UserControl
    {
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

      
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(UserCard), new PropertyMetadata(0));


        public string NameUser
        {
            get { return (string)GetValue(NameUserProperty); }
            set { SetValue(NameUserProperty, value); }
        }


        public static readonly DependencyProperty NameUserProperty =
            DependencyProperty.Register("NameUser", typeof(string), typeof(UserCard), new PropertyMetadata(string.Empty));



        public string DateBirch
        {
            get { return (string)GetValue(DateBirchProperty); }
            set {

                var date = value;
                try
                {
                    date = DateTime.Parse(date).ToShortDateString();
                }
                catch 
                {
                    date = DateTime.Now.ToShortDateString();
                }
                
                SetValue(DateBirchProperty, date); }
        }

    
        public static readonly DependencyProperty DateBirchProperty =
            DependencyProperty.Register("DateBirch", typeof(string), typeof(UserCard), new PropertyMetadata(string.Empty));


        public string Gender
        {
            get { return (string)GetValue(GenderProperty); }
            set { SetValue(GenderProperty, value); }
        }

      
        public static readonly DependencyProperty GenderProperty =
            DependencyProperty.Register("Gender", typeof(string), typeof(UserCard), new PropertyMetadata(string.Empty));




        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(UserCard), new PropertyMetadata(string.Empty));


        public UserCard()
        {
            InitializeComponent();
        }
    }
}
