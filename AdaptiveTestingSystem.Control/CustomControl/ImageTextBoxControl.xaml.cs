using System;
using System.Collections.Generic;
using System.IO;
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
using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.DLL.CScript;

namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для ImageTextBoxControl.xaml
    /// </summary>
    public partial class ImageTextBoxControl : UserControl
    {

        public delegate void ImageViewerHandler(byte[] imageByte);
        public event ImageViewerHandler? ImageView;



        public delegate void DeleteThisHandler(ImageTextBoxControl control);
        public event DeleteThisHandler? Deleting;

        public string Wotemark
        {
            get { return (string)GetValue(WotemarkProperty); }
            set { SetValue(WotemarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Wotemark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WotemarkProperty =
            DependencyProperty.Register("Wotemark", typeof(string), typeof(ImageTextBoxControl), new PropertyMetadata(string.Empty));


        public int WSize
        {
            get { return (int)GetValue(WSizeProperty); }
            set { SetValue(WSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WSizeProperty =
            DependencyProperty.Register("WSize", typeof(int), typeof(ImageTextBoxControl), new PropertyMetadata(18));



        public byte[] ImageData
        {
            get { return (byte[])GetValue(ImageDataProperty); }
            set
            {
                SetValue(ImageDataProperty, value);

                if (value == null) { var mas = new byte[0]; SetValue(ImageDataProperty, mas); return; }

                if (value.Length == 0) ImageQuestionsViewer.Source = null;
                else
                {
                    ImageQuestionsViewer.Source = Converter.ConvertByteArrayToImage(value);
                }
            }
        }

        public static readonly DependencyProperty ImageDataProperty =
            DependencyProperty.Register("ImageData", typeof(byte[]), typeof(ImageTextBoxControl), new PropertyMetadata(new byte[0]));




        public string GetByteImageToString()
        {
           return "";
        }

        public bool IsImaging
        {
            get { return (bool)GetValue(IsImagingProperty); }
            set 
            { 
                SetValue(IsImagingProperty, value);

                imageButton.IsChecked = value;

                if (value)
                {
                    defaultData.Visibility = Visibility.Collapsed;
                    ImageQuestions.Visibility = Visibility.Visible;
                    buttonManager.Visibility = Visibility.Visible;

                }
                else
                {
                    defaultData.Visibility = Visibility.Visible;
                    ImageQuestions.Visibility = Visibility.Collapsed; 
                    buttonManager.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Using a DependencyProperty as the backing store for IsImaging.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsImagingProperty =
            DependencyProperty.Register("IsImaging", typeof(bool), typeof(ImageTextBoxControl), new PropertyMetadata(false));



        public string Extension
        {
            get { return (string)GetValue(ExtensionProperty); }
            set { SetValue(ExtensionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Extension.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExtensionProperty =
            DependencyProperty.Register("Extension", typeof(string), typeof(ImageTextBoxControl), new PropertyMetadata(string.Empty));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Answer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ImageTextBoxControl), new PropertyMetadata(string.Empty));




        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(ImageTextBoxControl), new PropertyMetadata(0));




        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(ImageTextBoxControl), new PropertyMetadata(0));





        public bool IsDeleteButton
        {
            get { return (bool)GetValue(IsDeleteButtonProperty); }
            set { SetValue(IsDeleteButtonProperty, value);

                if (value)
                {
                    BDelete.Visibility = Visibility.Visible;
                  
                }
                else
                {
                    BDelete.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Using a DependencyProperty as the backing store for IsDeleteButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDeleteButtonProperty =
            DependencyProperty.Register("IsDeleteButton", typeof(bool), typeof(ImageTextBoxControl), new PropertyMetadata(false));




        public ImageTextBoxControl()
        {
            InitializeComponent();
        }


        public void SetText(string text)
        {
            defaultData.Text= this.Text = text;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IsImaging = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IsImaging = false;
        }

     

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Выбор картинки";
            openFileDialog.Filter = "Картинки|*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                ImageData = null;
                FileInfo fInfo = new FileInfo(openFileDialog.FileName);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);


                ImageData = br.ReadBytes((int)numBytes);



                //byte[] byteData = System.IO.File.ReadAllBytes(openFileDialog.FileName);

                ////now convert byte[] to Base64
                //string imreBase64Data = Convert.ToHexString(byteData);
                //ImageData = Converter.ToByteArray(imreBase64Data);



                Extension = (System.IO.Path.GetExtension(openFileDialog.FileName)).Replace(".", "").ToLower();
                // ImageQuestionsViewer.Source = new BitmapImage(new Uri(openFileDialog.FileName));

            }
        }

        private void ClearImage_Click(object sender, RoutedEventArgs e)
        {
            ImageQuestionsViewer.Source = null;
            ImageData = null;
            Extension = "";
        }

        private void defaultData_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var obj = (TextBox)sender;
            //this.Text = obj.Text.Trim();
        }

        private void ImageQuestionsViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageView?.Invoke(ImageData);
        }


        public void Clear()
        {
            IsImaging = false;
            ImageQuestionsViewer.Source = null;
            ImageData = null;
            Extension = "";
            defaultData.Text = string.Empty;
            Text= string.Empty;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Deleting?.Invoke(this);
        }
    }
}
