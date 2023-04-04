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

namespace AdaptiveTestingSystem.Control.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowImageViewer.xaml
    /// </summary>
    public partial class WindowImageViewer : Window
    {
        public static readonly DependencyProperty dependencyImageData;

        static WindowImageViewer()
        {

            dependencyImageData = DependencyProperty.Register("ImageData", typeof(byte[]), typeof(WindowImageViewer));

        }
        public byte[] ImageData
        {
            get { return (byte[])GetValue(dependencyImageData); }
            set
            {
                SetValue(dependencyImageData, value);

                if (value == null) ImageQuestionsViewer.Source = null; else { ImageQuestionsViewer.Source =  Converter.ConvertByteArrayToImage(value); }
            }
        }



        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;
        public WindowImageViewer(byte[] image)
        {
            InitializeComponent();
            ImageData = image;

            Width = System.Windows.SystemParameters.PrimaryScreenWidth - 200;
            Height = System.Windows.SystemParameters.PrimaryScreenHeight - 100;
        }

        private void root_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }

        private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(scrollViewer);
            hOff = scrollViewer.HorizontalOffset;
            vOff = scrollViewer.VerticalOffset;
            scrollViewer.CaptureMouse();
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (scrollViewer.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(scrollViewer).X));
                scrollViewer.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(scrollViewer).Y));
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.ReleaseMouseCapture();
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //  scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + e.Delta);
            //  scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + e.Delta);

            var element = ImageQuestionsViewer;
            var position = e.GetPosition(element);
            var transform = element.LayoutTransform as MatrixTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1); // choose appropriate scaling factor

            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            element.LayoutTransform = new MatrixTransform(matrix);




        }

        private void Header_CloseClick()
        {
            Close();
        }
    }
}
