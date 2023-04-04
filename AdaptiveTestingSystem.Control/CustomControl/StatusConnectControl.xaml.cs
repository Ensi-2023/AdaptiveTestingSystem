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
using static AdaptiveTestingSystem.Data.Enums;

namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для StatusConnectControl.xaml
    /// </summary>
    public partial class StatusConnectControl : UserControl
    {
        public Code IsCode
        {
            get { return (Code)GetValue(IsCodeProperty); }
            set { SetValue(IsCodeProperty, value); }
        }
            
        public static readonly DependencyProperty IsCodeProperty =
            DependencyProperty.Register("IsCode", typeof(Code), typeof(StatusConnectControl), new PropertyMetadata(Code.ConnectedToServer));

        public Brush BarColor
        {
            get { return (Brush)GetValue(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarColorProperty =
            DependencyProperty.Register("BarColor", typeof(Brush), typeof(StatusConnectControl), new PropertyMetadata(Brushes.GreenYellow));


        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(StatusConnectControl), new PropertyMetadata(Brushes.GreenYellow));

        public StatusConnectControl()
        {
            InitializeComponent();
        }
    }
}
