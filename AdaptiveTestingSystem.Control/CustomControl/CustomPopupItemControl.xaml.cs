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
   
    public partial class CustomPopupItemControl : UserControl
    {
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(CustomPopupItemControl), new PropertyMetadata(""));



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(CustomPopupItemControl), new PropertyMetadata(""));





        public HorizontalAlignment DescriptionHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(DescriptionHorizontalAlignmentProperty); }
            set { SetValue(DescriptionHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty DescriptionHorizontalAlignmentProperty =
            DependencyProperty.Register("DescriptionHorizontalAlignment", typeof(HorizontalAlignment), typeof(CustomPopupItemControl), new PropertyMetadata(HorizontalAlignment.Right));



        public VerticalAlignment DescriptionVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(DescriptionVerticalAlignmentProperty); }
            set { SetValue(DescriptionVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty DescriptionVerticalAlignmentProperty =
            DependencyProperty.Register("DescriptionVerticalAlignment", typeof(VerticalAlignment), typeof(CustomPopupItemControl), new PropertyMetadata(VerticalAlignment.Center));



        public VerticalAlignment CommandVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(CommandVerticalAlignmentProperty); }
            set { SetValue(CommandVerticalAlignmentProperty, value); }
        }

         public static readonly DependencyProperty CommandVerticalAlignmentProperty =
            DependencyProperty.Register("CommandVerticalAlignment", typeof(VerticalAlignment), typeof(CustomPopupItemControl), new PropertyMetadata(VerticalAlignment.Center));




        public HorizontalAlignment CommandHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(CommandHorizontalAlignmentProperty); }
            set { SetValue(CommandHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty CommandHorizontalAlignmentProperty =
            DependencyProperty.Register("CommandHorizontalAlignment", typeof(HorizontalAlignment), typeof(CustomPopupItemControl), new PropertyMetadata(HorizontalAlignment.Left));





        public Brush CommandForeground
        {
            get { return (Brush)GetValue(CommandForegroundProperty); }
            set { SetValue(CommandForegroundProperty, value); }
        }
         public static readonly DependencyProperty CommandForegroundProperty =
            DependencyProperty.Register("CommandForeground", typeof(Brush), typeof(CustomPopupItemControl), new PropertyMetadata(Brushes.White));




        public Brush DescriptionForeground
        {
            get { return (Brush)GetValue(DescriptionForegroundProperty); }
            set { SetValue(DescriptionForegroundProperty, value); }
        }

        public static readonly DependencyProperty DescriptionForegroundProperty =
            DependencyProperty.Register("DescriptionForeground", typeof(Brush), typeof(CustomPopupItemControl), new PropertyMetadata(Brushes.White));




        public int CommandFontSize
        {
            get { return (int)GetValue(CommandFontSizeProperty); }
            set { SetValue(CommandFontSizeProperty, value); }
        }

        public static readonly DependencyProperty CommandFontSizeProperty =
            DependencyProperty.Register("CommandFontSize", typeof(int), typeof(CustomPopupItemControl), new PropertyMetadata(13));



        public int DecriptionFontSize
        {
            get { return (int)GetValue(DecriptionFontSizeProperty); }
            set { SetValue(DecriptionFontSizeProperty, value); }
        }

       
        public static readonly DependencyProperty DecriptionFontSizeProperty =
            DependencyProperty.Register("DecriptionFontSize", typeof(int), typeof(CustomPopupItemControl), new PropertyMetadata(13));


        public CustomPopupItemControl()
        {
            InitializeComponent();
        }
    }
}
