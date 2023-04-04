#nullable disable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript
{
    public class Logging
    {
        private static RichTextBox LogBox;
        public static string NameFile { get; private set; }
        private static IniFile LogIni;
        public static void SetupLogBox(Object obj)
        {
            LogBox = (RichTextBox)obj;
            LogBox.Document.Blocks.Clear();
            //Setting.CreateIni();
  
       
            Directory.CreateDirectory("Log");
            NameFile = String.Format(@"Log\\Log_{0}.txt", Main.Instance.TimeStartProgramm.Content.ToString().Replace('.', '_').Replace(' ', '_').Replace(':', '_'));


            LogIni = new IniFile(NameFile);

        }


        /// <summary>
        /// Log
        /// </summary>
        /// <param name="log"></param>
        public static void Send(string log)
        {
            if (log.Trim().Length == 0) return;
            Application.Current.Dispatcher.Invoke(new System.Action(() => {


                Brush brush = Brushes.White;           
                if (log.Contains("[Error]")) brush = Brushes.Red;
                if (log.Contains("[Message]")) brush = Brushes.Green;
                if (log.Contains("[Debug]")) brush = Brushes.Yellow;
                if (log.Contains("[Warning]")) brush = Brushes.Magenta;


                var paragraph = new Paragraph();
                var run = new Run(log)
                {
                    Foreground = brush
                };
                paragraph.Inlines.Add(run);
                LogBox.Document.Blocks.Add(paragraph);

                SendLog(log);
            }
            ));
        }

        static public void SendLog(string log)
        {
            if (Main.Instance.Settings != null && Main.Instance.Settings.OnAutoSaveLogToServer) LogIni.Write("log", String.Format(DateTime.Now + ":" + DateTime.Now.Millisecond), log);
        }

        static public void SendLogNotAutoSave(string log)
        {
            if (Main.Instance.Settings != null && Main.Instance.Settings.OnAutoSaveLogToServer == false) LogIni.Write("AutoLog", "Log (Time to create save: " + DateTime.Now.ToString() + ")", "{\n" + log + "}");
        }

        public static void SaveLog(string log)
        {
            if (Main.Instance.Settings != null && Main.Instance.Settings.OnAutoSaveLogToServer == false) LogIni.Write("AutoLog", "Log (Time to create save: " + DateTime.Now.ToString() + ")", "{\n" + log + "}");
        }
    }
}
