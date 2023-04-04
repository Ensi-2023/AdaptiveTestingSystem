#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace AdaptiveTestingSystem.DLL.CScript
{
    public class PageManager
    {
        private List<UserControl> Page;
        private Grid Main;

        public List<UserControl> Get
        {
            get { return Page; }
        }


        public delegate void InformationPageHandler(int count, string title);
        public event InformationPageHandler? InformationPage;

        public PageManager(Grid contentWindow)
        {
            Main = contentWindow;
            Page = new List<UserControl>();
        }

        public string GetInfoOpenPage()
        {
            string info = string.Empty;
            foreach (UserControl page in Page.ToList())
            {
                info += $"{wpf.WindowsAssist.GetUCTitle(page)}\n";
            }
            return info.Trim();
        }

        public void FirstPage()
        {
            UserControl first = Page.First();
            SetPage(first);
            Page = new List<UserControl>();
            Add(first);
            InformationPage?.Invoke(Page.Count, wpf.WindowsAssist.GetUCTitle(first));

        }

        public void LastPage()
        {
            UserControl last = Page.Last();
            Page.Remove(Page.Find(o => ParserVariables.GetInt(o.Uid) == ParserVariables.GetInt(last.Uid)));
            SetPage(last);
            InformationPage?.Invoke(Page.Count, wpf.WindowsAssist.GetUCTitle(last));
        }

        public void Back()
        {
            var lastPage = Page.Last();

            if (Page.Count >= 3)
            {
                Page.Remove(lastPage);
                lastPage = Page.Last();
                SetPage(lastPage);
                InformationPage?.Invoke(Page.Count, wpf.WindowsAssist.GetUCTitle(lastPage));
            }
            else
            {
                Page.Remove(lastPage);
                FirstPage();
            }
        }

        public void SetTitle(UserControl? page)
        {
            InformationPage?.Invoke(Page.Count, wpf.WindowsAssist.GetUCTitle(page));
        }

        public virtual void Next(UserControl? next)
        {

        
            //if (next != null)
            //{
            //    if (CheckPage(next))
            //    {
            //        var list = new List<UserControl>();
            //        foreach (UserControl page in Page.ToList())
            //        {
            //            if (ParserVariables.GetInt(page.Uid) == ParserVariables.GetInt(next.Uid))
            //            {
            //              return;
            //            }
            //            else
            //                list.Add(page);

            //        }
            //        Page = list;
            //        LastPage();
            //    }
            //}
            Application.Current.Dispatcher.Invoke(() => { 

            if (next != null)
            {
                if (CheckPage(next)) return;
                
               var CurrentPage = Page.Last();
               if (CurrentPage != null)
               {
                   Add(CurrentPage);
                   Add(next);
                   SetPage(next);
               }

                
            }
            });
        }

        private bool CheckPage(UserControl check)
        {
            foreach (var it in Page.ToList())
            {
                if (it.Uid == check.Uid)
                {
                    return true;
                }
            }
            return false;
        }

        private void Add(UserControl control)
        {
            //var item = Page.Where(p => p==control);

            UserControl? item = null;

            foreach (var it in Page)
            {
                if (it == control)
                {
                    item = it;
                    return;
                }
            }

            if (item != null) return;
            Page.Add(control);
        }

        public void SetPage(UserControl page)
        {
            Main.Children.Clear();
            Main.Children.Add(page);
            InformationPage?.Invoke(Page.Count, wpf.WindowsAssist.GetUCTitle(page));
        }

        public void SetFirstPage(UserControl page)
        {
            Add(page);
            Main.Children.Clear();
            Main.Children.Add(page);
            InformationPage?.Invoke(Page.Count, wpf.WindowsAssist.GetUCTitle(page));
        }

        private UserControl Search(int index)
        {
            foreach (var item in Page)
            {
                if (Page.IndexOf(item) == index) return item;
            }

            return Page.First();
        }

        public void Home()
        {
            FirstPage();
        }
    }
}
