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

namespace AdaptiveTestingSystem.Control.CustomControl.TestingControl
{


    public partial class AnswerControl : UserControl
    {
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
            DependencyProperty.Register("ImageData", typeof(byte[]), typeof(AnswerControl), new PropertyMetadata(new byte[0]));

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
            DependencyProperty.Register("IsImaging", typeof(bool), typeof(AnswerControl), new PropertyMetadata(false));


        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(AnswerControl), new PropertyMetadata(0));



        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeiProperty); }
            set { SetValue(ImageHeiProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHei.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeiProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(AnswerControl), new PropertyMetadata(70.0));



        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(AnswerControl), new PropertyMetadata(70.0));



        public Brush CardColor
        {
            get { return (Brush)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        public static readonly DependencyProperty CardColorProperty =
            DependencyProperty.Register("CardColor", typeof(Brush), typeof(AnswerControl), new PropertyMetadata(Brushes.Transparent));



        public Brush BgCardColor
        {
            get { return (Brush)GetValue(BgCardColorProperty); }
            set { SetValue(BgCardColorProperty, value); }
        }

        public static readonly DependencyProperty BgCardColorProperty =
            DependencyProperty.Register("BgCardColor", typeof(Brush), typeof(AnswerControl), new PropertyMetadata(Brushes.Transparent));



        public Brush SubTextForeground
        {
            get { return (Brush)GetValue(SubTextForegroundProperty); }
            set { SetValue(SubTextForegroundProperty, value); }
        }

        public static readonly DependencyProperty SubTextForegroundProperty =
            DependencyProperty.Register("SubTextForeground", typeof(Brush), typeof(AnswerControl), new PropertyMetadata(Brushes.Transparent));


        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }


        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(string), typeof(AnswerControl), new PropertyMetadata("0"));


        public string Answer
        {
            get { return (string)GetValue(AnswerProperty); }
            set
            {
                SetValue(AnswerProperty, value);
            }
        }


        public static readonly DependencyProperty AnswerProperty =
            DependencyProperty.Register("Answer", typeof(string), typeof(AnswerControl), new PropertyMetadata(string.Empty));



        public bool IsSelect
        {
            get { return (bool)GetValue(IsSelectProperty); }
            set { SetValue(IsSelectProperty, value); }
        }

        public static readonly DependencyProperty IsSelectProperty =
            DependencyProperty.Register("IsSelect", typeof(bool), typeof(AnswerControl), new PropertyMetadata(false));




        public bool IsLetDeselect
        {
            get { return (bool)GetValue(IsLetDeselectProperty); }
            set { SetValue(IsLetDeselectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLetDeselect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLetDeselectProperty =
            DependencyProperty.Register("IsLetDeselect", typeof(bool), typeof(AnswerControl), new PropertyMetadata(false));





        public int GroupIndex
        {
            get { return (int)GetValue(GroupIndexProperty); }
            set { SetValue(GroupIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupIndexProperty =
            DependencyProperty.Register("GroupIndex", typeof(int), typeof(AnswerControl), new PropertyMetadata(0));


        public delegate void ImageViewerHandler(byte[] imageByte);
        public event ImageViewerHandler? ImageView;

        public AnswerControl()
        {
            InitializeComponent();
        }

        private void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (IsSelect)
            {
                if (IsLetDeselect == false) return;

                this.IsSelect = false;
                return;
            }

            var panel = this.Parent as Panel;
            if (panel != null)
            {
                foreach(var item in panel.Children)
                {
                    var child = (item as AnswerControl);
                    if (child != null)
                    {
                        if (child == this) continue;
                        if (this.GroupIndex == child.GroupIndex) child.IsSelect = false;
                    }          
                }
            }        

            IsSelect = !IsSelect;
        }

        private void ImageQuestionsViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageView?.Invoke(ImageData);
        }
    }
}
