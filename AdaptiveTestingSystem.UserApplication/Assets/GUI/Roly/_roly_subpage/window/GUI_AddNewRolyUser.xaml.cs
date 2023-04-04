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
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_AddNewRolyUser.xaml
    /// </summary>
    public partial class GUI_AddNewRolyUser : Window
    {
        public static GUI_AddNewRolyUser Instance;
             

        /// <summary>
        /// Добавление новой роли
        /// </summary>
        public GUI_AddNewRolyUser()
        {
            InitializeComponent();
            Instance = this;
            body.Children.Clear();
            body.Children.Add(new page.GUI_page_AddNewRoly());
        }
        /// <summary>
        /// Редактирование роли
        /// </summary>
        /// <param name="index">индекс роли</param>
        public GUI_AddNewRolyUser(int index, GUI_RolyInfViewer gUI_RolyInfViewer)
        {
            InitializeComponent();
            Instance = this;

           
            body.Children.Clear();
            body.Children.Add(new page.GUI_page_UpdateRoly(index, gUI_RolyInfViewer));
        }


        public void OverlayShow(bool show, TypeOverlay typeOverlay = TypeOverlay.nullable, string title = "", string subtitle = "", Visibility visibleButton = Visibility.Collapsed, bool awaiter = false)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                if (Instance == null) return;

                if (awaiter == true) await Task.Delay(250);

                body.IsEnabled = !show;
                this.Overlay.Visibility = show == true ? Visibility.Visible : Visibility.Collapsed;
                this.Overlay.Title = title;
                this.Overlay.SubTitle = subtitle;
                this.Overlay.TOverlay = typeOverlay;
                this.Overlay.ButtonVisible = visibleButton;


                if (title.Trim() == string.Empty && subtitle.Trim() == string.Empty) return;

                if (typeOverlay == TypeOverlay.loading)
                {
                    Logger.Warning($"[{title}] - {subtitle}");
                }

                if (typeOverlay == TypeOverlay.message)
                {
                    Logger.Message($"[{title}] - {subtitle}");
                }

                if (typeOverlay == TypeOverlay.error)
                {
                    Logger.Error($"[{title}] - {subtitle}");
                }


            });
        }

        private void Header_CloseClick()
        {
            Close();
        }

        private void Overlay_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Overlay.Visibility == Visibility.Collapsed) body.IsEnabled = true;
        }
    }
}
