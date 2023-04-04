using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Control.Windows
{
    public class MessageShow
    {
        public enum Type
        {
            Error = 0,
            Message,
            Question
        }


        public static void Show()
        {
            var obj = new MessageGUI();
            obj.ShowDialog();
        }


        public static bool? Show(string message)
        {
            var obj = new MessageGUI(message, "");
            return obj.ShowDialog();
        }

        public static bool? Show(string message, string title)
        {
            var obj = new MessageGUI(message, title);
            return obj.ShowDialog();
        }

        public static bool? Show(string message, string title, Type type)
        {
            var obj = new MessageGUI(message, title, type);
            return obj.ShowDialog();
        }
    }
}