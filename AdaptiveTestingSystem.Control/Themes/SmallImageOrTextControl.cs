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

namespace AdaptiveTestingSystem.Control.Themes
{
 
    public class SmallImageOrTextControl : System.Windows.Controls.Control
    {

        public delegate void ImageViewerHandler(byte[] imageByte);
        public event ImageViewerHandler? ImageView;

        static SmallImageOrTextControl()
        {
            TextSizeProperty = DependencyProperty.Register("TextSize", typeof(double), typeof(SmallImageOrTextControl), new PropertyMetadata(20.0));
            TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SmallImageOrTextControl), new PropertyMetadata(string.Empty));
            IsImageProperty =  DependencyProperty.Register("IsImage", typeof(bool), typeof(SmallImageOrTextControl), new PropertyMetadata(false));
            ImageDataProperty = DependencyProperty.Register("ImageData", typeof(byte[]), typeof(SmallImageOrTextControl), new PropertyMetadata(null));
            ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(SmallImageOrTextControl), new PropertyMetadata(70.0));
            ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(SmallImageOrTextControl), new PropertyMetadata(70.0));
            ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(SmallImageOrTextControl), new PropertyMetadata(null));
            ImageDataStringProperty =  DependencyProperty.Register("ImageDataString", typeof(string), typeof(SmallImageOrTextControl), new PropertyMetadata(string.Empty));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SmallImageOrTextControl), new FrameworkPropertyMetadata(typeof(SmallImageOrTextControl)));
        }



        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set
            {
                SetValue(TextSizeProperty, value);
            }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty;
        public static readonly DependencyProperty TextSizeProperty;


        public byte[] ImageData
        {
            get { return (byte[])GetValue(ImageDataProperty); }
            set 
            {
                SetValue(ImageDataProperty, value);
                Image = Converter.ConvertByteArrayToImage(value);
            }
        }

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public static readonly DependencyProperty ImageProperty;

        public string ImageDataString
        {
            get { return (string)GetValue(ImageDataStringProperty); }
            set { SetValue(ImageDataStringProperty, value);

            }
        }

        // Using a DependencyProperty as the backing store for ImageDataString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageDataStringProperty;



        // Using a DependencyProperty as the backing store for ImageData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageDataProperty;


        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set
            {
                SetValue(ImageHeightProperty, value);
       
            }
        }

        // Using a DependencyProperty as the backing store for ImageHei.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty;



        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty ;


        public bool IsImage
        {
            get { return (bool)GetValue(IsImageProperty); }
            set { SetValue(IsImageProperty, value); Console.WriteLine($"{value}"); }
        }

        // Using a DependencyProperty as the backing store for IsImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsImageProperty;

        Image GetImage;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var textBlock = GetTemplateChild("PART_Title") as TextBlock;
            var image = GetTemplateChild("PART_Image") as Image;
            if (image != null)
            {
                GetImage = image;
                GetImage.MouseLeftButtonUp += GetImage_MouseLeftButtonUp;
            }
        }

        private void GetImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageView?.Invoke(Converter.ToByteArray(ImageDataString));
        }

        ~SmallImageOrTextControl()
        {
            GetImage.MouseLeftButtonUp -= GetImage_MouseLeftButtonUp;
        }

    }
}
