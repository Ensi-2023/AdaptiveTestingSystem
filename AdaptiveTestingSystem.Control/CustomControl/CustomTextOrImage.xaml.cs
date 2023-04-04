using AdaptiveTestingSystem.Control.Themes;
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
    /// Логика взаимодействия для CustomTextOrImage.xaml
    /// </summary>
    public partial class CustomTextOrImage : UserControl
    {
        public Brush CardColor
        {
            get { return (Brush)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        public static readonly DependencyProperty CardColorProperty =
            DependencyProperty.Register("CardColor", typeof(Brush), typeof(CustomTextOrImage), new PropertyMetadata(Brushes.Transparent));


        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }


        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(string), typeof(CustomTextOrImage), new PropertyMetadata("0"));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CustomTextOrImage), new PropertyMetadata(string.Empty));


        public int GetIndex()
        {
            return Index;
        }






        public Visibility VisibleUI
        {
            get { return (Visibility)GetValue(VisibleUIProperty); }
            private  set { SetValue(VisibleUIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleUI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleUIProperty =
            DependencyProperty.Register("VisibleUI", typeof(Visibility), typeof(CustomTextOrImage), new PropertyMetadata(Visibility.Visible));



        public bool ViewButtonAndNumber
        {
            get { return (bool)GetValue(ViewButtonProperty); }
            set
            {
                SetValue(ViewButtonProperty, value);
                if (value)
                {

                    SetValue(VisibleUIProperty, Visibility.Visible);
                }
                else
                {

                    SetValue(VisibleUIProperty, Visibility.Collapsed);
         
                }
            }
        }


        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeiProperty); }
            set { SetValue(ImageHeiProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHei.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeiProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(CustomTextOrImage), new PropertyMetadata(70.0));



        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(CustomTextOrImage), new PropertyMetadata(70.0));





        public static readonly DependencyProperty ViewButtonProperty =
            DependencyProperty.Register("ViewButton", typeof(bool), typeof(CustomTextOrImage), new PropertyMetadata(true));



        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(CustomTextOrImage), new PropertyMetadata(0));




        public bool IsImaging
        {
            get { return (bool)GetValue(IsImagingProperty); }
            set
            {
                SetValue(IsImagingProperty, value);
             

                if (value)
                {
                    valueText.Visibility = Visibility.Collapsed;
                    ImageQuestionsViewer.Visibility = Visibility.Visible;

                }
                else
                {
                    valueText.Visibility = Visibility.Visible;
                    ImageQuestionsViewer.Visibility = Visibility.Collapsed;
                }
            }
        }


        // Using a DependencyProperty as the backing store for IsImaging.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsImagingProperty =
            DependencyProperty.Register("IsImaging", typeof(bool), typeof(CustomTextOrImage), new PropertyMetadata(false));


        public byte[] ImageData
        {
            get { return (byte[])GetValue(ImageDataProperty); }
            set
            {
            
                SetValue(ImageDataProperty, value);
                if (value == null) return;
                if (value.Length == 0) ImageQuestionsViewer.Source = null;
                else
                {
                   ImageQuestionsViewer.Source = Converter.ConvertByteArrayToImage(value);
                }
            }
        }

        public static readonly DependencyProperty ImageDataProperty =
            DependencyProperty.Register("ImageData", typeof(byte[]), typeof(CustomTextOrImage), new PropertyMetadata(new byte[0]));

        public delegate void ImageViewerHandler(byte[] imageByte);
        public event ImageViewerHandler? ImageView;


        public string Image_Format
        {
            get { return (string)GetValue(Image_FormatProperty); }
            set { SetValue(Image_FormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image_Format.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Image_FormatProperty =
            DependencyProperty.Register("Image_Format", typeof(string), typeof(CustomTextOrImage), new PropertyMetadata(string.Empty));







        public delegate void DeleteHandler(CustomTextOrImage control);
        public event DeleteHandler? Delete;


        public delegate void EditingHandler(CustomTextOrImage control);
        public event EditingHandler? Editing;

        public CustomTextOrImage()
        {
            InitializeComponent();
        }



        private void ImageQuestionsViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageView?.Invoke(ImageData);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(this);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Editing?.Invoke(this);
        }
    }
}
