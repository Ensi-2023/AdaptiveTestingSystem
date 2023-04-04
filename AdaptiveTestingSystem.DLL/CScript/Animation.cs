#nullable disable
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Animation;

namespace AdaptiveTestingSystem.DLL.CScript
{
    public class Animation: UserControl
    {
        static UIElement UI = null;
        static double To;
        static double From = 0.0;
        static bool IsMinus = false;

        static System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


        public static void AnimatedThickness(UIElement element, Thickness from, Thickness to, TimeSpan duration)
        {
            PowerEase easing = new PowerEase() { Power = 6 };
            easing.EasingMode = EasingMode.EaseOut;
            ThicknessAnimation thickness = new ThicknessAnimation();
            thickness.From = from;
            thickness.To = to;
            thickness.Duration = duration;
            thickness.EasingFunction = easing;
            element.BeginAnimation(MarginProperty, thickness);
        }


        public static void AnimatedOpacity(UIElement element, double from, double to, TimeSpan duration)
        {
            
            
            PowerEase easing = new PowerEase() { Power = 6 };
            easing.EasingMode = EasingMode.EaseOut;
            // анимация для высота
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = from;
            opacityAnimation.To = to;
            opacityAnimation.Duration = duration;
            opacityAnimation.EasingFunction = easing;
            element.BeginAnimation(OpacityProperty, opacityAnimation);
        }

        public static void AnimatedWidth(UIElement element, double from, double to, TimeSpan duration)
        {

            PowerEase easing = new PowerEase() { Power = 6 };
            easing.EasingMode = EasingMode.EaseOut;
            // анимация для высота
            DoubleAnimation widhAnimation = new DoubleAnimation();
            widhAnimation.From = from;
            widhAnimation.To = to;
            widhAnimation.Duration = duration;
            widhAnimation.EasingFunction = easing;
            element.BeginAnimation(WidthProperty, widhAnimation);
        }


        public static void AnimatedHeight(UIElement element, double from, double to, TimeSpan duration)
        {

            PowerEase easing = new PowerEase() { Power = 6 };
            easing.EasingMode = EasingMode.EaseOut;
            // анимация для высота
            DoubleAnimation heightAnimation = new DoubleAnimation();
            heightAnimation.From = from;
            heightAnimation.To = to;
            heightAnimation.Duration = duration;
            heightAnimation.EasingFunction = easing;
            element.BeginAnimation(HeightProperty, heightAnimation);
            
            
        }
 

        public static void AnimatedScrollOffset(UIElement element, double to, TimeSpan duration)
        {
            var obj = element as ScrollViewer;
            if (obj == null) return;
            //// анимация для высота
            ///

            obj.ScrollToVerticalOffset(to);

        }
#nullable enable
        private static void dispatcherTimer_Tick(object? sender, EventArgs e)
        {

            if (IsMinus)
            {
                To--;
                SetOffset(To);
            }
            else
            {
                To++;
                SetOffset(To);
            }

          

        }

        private static void SetOffset(double to)
        {
            var obj = UI as ScrollViewer;
            if (obj == null) return;

            obj.ScrollToVerticalOffset(to);
        }
    }
}
