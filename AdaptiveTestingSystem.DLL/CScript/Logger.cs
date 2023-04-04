using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.DLL.CScript
{
    static public class Logger
    {
        static public void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Debug] [{DateTime.Now}] - {message}");
            Console.ResetColor();
        }

        static public void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[Log] [{DateTime.Now}] - {message}");
            Console.ResetColor();
        }

        static public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] [{DateTime.Now}] - {message}");
            Console.ResetColor();
        }

        static public void Message(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Message] [{DateTime.Now}] - {message}");
            Console.ResetColor();
        }

        static public void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Warning] [{DateTime.Now}] - {message}");
            Console.ResetColor();
        }
    }
}
