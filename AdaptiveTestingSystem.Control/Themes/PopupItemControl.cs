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

    public class PopupItemControl : ContentControl
    {


        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }




        public string HiddenField
        {
            get { return (string)GetValue(HiddenFieldProperty); }
            set { SetValue(HiddenFieldProperty, value); }
        }




        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }


        public HorizontalAlignment DescriptionHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(DescriptionHorizontalAlignmentProperty); }
            set { SetValue(DescriptionHorizontalAlignmentProperty, value); }
        }




        public VerticalAlignment DescriptionVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(DescriptionVerticalAlignmentProperty); }
            set { SetValue(DescriptionVerticalAlignmentProperty, value); }
        }

        public VerticalAlignment CommandVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(CommandVerticalAlignmentProperty); }
            set { SetValue(CommandVerticalAlignmentProperty, value); }
        }


        public HorizontalAlignment CommandHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(CommandHorizontalAlignmentProperty); }
            set { SetValue(CommandHorizontalAlignmentProperty, value); }
        }


        public Brush CommandForeground
        {
            get { return (Brush)GetValue(CommandForegroundProperty); }
            set { SetValue(CommandForegroundProperty, value); }
        }

        public Brush DescriptionForeground
        {
            get { return (Brush)GetValue(DescriptionForegroundProperty); }
            set { SetValue(DescriptionForegroundProperty, value); }
        }

        public int CommandFontSize
        {
            get { return (int)GetValue(CommandFontSizeProperty); }
            set { SetValue(CommandFontSizeProperty, value); }
        }



        public int DecriptionFontSize
        {
            get { return (int)GetValue(DecriptionFontSizeProperty); }
            set { SetValue(DecriptionFontSizeProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty;
        public static readonly DependencyProperty DescriptionProperty;
        public static readonly DependencyProperty DescriptionHorizontalAlignmentProperty;
        public static readonly DependencyProperty DescriptionVerticalAlignmentProperty;
        public static readonly DependencyProperty CommandVerticalAlignmentProperty;
        public static readonly DependencyProperty CommandHorizontalAlignmentProperty;
        public static readonly DependencyProperty CommandForegroundProperty;
        public static readonly DependencyProperty DescriptionForegroundProperty;
        public static readonly DependencyProperty CommandFontSizeProperty;
        public static readonly DependencyProperty DecriptionFontSizeProperty;
        public static readonly DependencyProperty HiddenFieldProperty;
        public static readonly DependencyProperty IndexProperty;


        static PopupItemControl()
        {
            DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PopupItemControl), new PropertyMetadata(""));
            DescriptionHorizontalAlignmentProperty = DependencyProperty.Register("DescriptionHorizontalAlignment", typeof(HorizontalAlignment), typeof(PopupItemControl), new PropertyMetadata(HorizontalAlignment.Right));
            DescriptionVerticalAlignmentProperty = DependencyProperty.Register("DescriptionVerticalAlignment", typeof(VerticalAlignment), typeof(PopupItemControl), new PropertyMetadata(VerticalAlignment.Center));
            CommandVerticalAlignmentProperty = DependencyProperty.Register("CommandVerticalAlignment", typeof(VerticalAlignment), typeof(PopupItemControl), new PropertyMetadata(VerticalAlignment.Center));
            CommandHorizontalAlignmentProperty = DependencyProperty.Register("CommandHorizontalAlignment", typeof(HorizontalAlignment), typeof(PopupItemControl), new PropertyMetadata(HorizontalAlignment.Left));
            CommandForegroundProperty = DependencyProperty.Register("CommandForeground", typeof(Brush), typeof(PopupItemControl), new PropertyMetadata(Brushes.White));
            DescriptionForegroundProperty = DependencyProperty.Register("DescriptionForeground", typeof(Brush), typeof(PopupItemControl), new PropertyMetadata(Brushes.White));
            CommandFontSizeProperty = DependencyProperty.Register("CommandFontSize", typeof(int), typeof(PopupItemControl), new PropertyMetadata(13));
            DecriptionFontSizeProperty = DependencyProperty.Register("DecriptionFontSize", typeof(int), typeof(PopupItemControl), new PropertyMetadata(13));
            CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(PopupItemControl), new PropertyMetadata(""));
            IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(PopupItemControl), new PropertyMetadata(0));
            HiddenFieldProperty = DependencyProperty.Register("HiddenField", typeof(string), typeof(PopupItemControl), new PropertyMetadata(string.Empty));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupItemControl), new FrameworkPropertyMetadata(typeof(PopupItemControl)));
        }
    }
}
