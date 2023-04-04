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
    /// Логика взаимодействия для AccountInformationCard.xaml
    /// </summary>
    public partial class AccountInformationCard : UserControl
    {


        public delegate void AccountExitHandler();
        public event AccountExitHandler? AccountExit;


        public delegate void AccountViewProfilHandler();
        public event AccountViewProfilHandler? AccountViewProfil;


        public delegate void AccountExitProgrammHandler();
        public event AccountExitProgrammHandler? AccountExitProgramm;


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(AccountInformationCard), new PropertyMetadata(false));



        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            private set { SetValue(IsPressedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPressed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register("IsPressed", typeof(bool), typeof(AccountInformationCard), new PropertyMetadata(false));



        public static RoutedEvent ClickEvent =
        EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AccountInformationCard));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected virtual void OnClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent, this);
        
            RaiseEvent(args);
            IsOpen = popupContent.IsOpen = !popupContent.IsOpen;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            OnClick();

            SetValue(IsPressedProperty, false);
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            SetValue(IsPressedProperty, true);
        }


        public string AccountName
        {
            get { return (string)GetValue(AccountNameProperty); }
            set { SetValue(AccountNameProperty, value); 

                try
                {
                    if (value == string.Empty) return;
                    string[] textName = value.Split(' ');
                    string str = $"{textName[0][0]}{textName[1][0]}";
                    SetValue(IconTextProperty, str.ToUpper().Trim());
          
                }
                catch (Exception ex)
                {
                    Logger.Error($"AccountInformationCard.AccountName произошла ошибка: {ex.Message}");
                    if (value == string.Empty) return;    
                    string str = $"{value[0]}";
                    SetValue(IconTextProperty, str.ToUpper().Trim());
          
                }

            }
        }

        public static readonly DependencyProperty AccountNameProperty =
            DependencyProperty.Register("AccountName", typeof(string), typeof(AccountInformationCard), new PropertyMetadata(string.Empty));


        public string RolyName
        {
            get { return (string)GetValue(RolyNameProperty); }
            set { SetValue(RolyNameProperty, value); }
        }

     
        public static readonly DependencyProperty RolyNameProperty =
            DependencyProperty.Register("RolyName", typeof(string), typeof(AccountInformationCard), new PropertyMetadata(string.Empty));


        public Brush CardColor
        {
            get { return (Brush)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        public static readonly DependencyProperty CardColorProperty =
            DependencyProperty.Register("CardColor", typeof(Brush), typeof(AccountInformationCard), new PropertyMetadata(Brushes.Transparent));




        public string IconText
        {
            get { return (string)GetValue(IconTextProperty); }
            private set { SetValue(IconTextProperty, value); }
        }

   
        public static readonly DependencyProperty IconTextProperty =
            DependencyProperty.Register("IconText", typeof(string), typeof(AccountInformationCard), new PropertyMetadata(string.Empty));


   


        public AccountInformationCard()
        {
            InitializeComponent();
        }

        private void MyProfil_Click(object sender, RoutedEventArgs e)
        {
            popupContent.IsOpen = IsOpen = false;
            AccountViewProfil?.Invoke();
        }

        private void AccountExit_Click(object sender, RoutedEventArgs e)
        {
            AccountExit?.Invoke();
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            AccountExitProgramm?.Invoke();
        }
    }
}
