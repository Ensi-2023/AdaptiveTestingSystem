using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.DLL.CScript
{
    public class ThemeManager
    {

        public delegate void ThemeChangedHandler(string themeStyle);
        public event ThemeChangedHandler? ThemeChanged;

        public string Theme { get; private set; }



        public ThemeManager() { }
 
        public ThemeManager(string style)
        {
            Set(style);
            Theme = style;
        }

        public void Set(string style)
        {
            try
            {
                // определяем путь к файлу ресурсов 
                var uri = new Uri($"AdaptiveTestingSystem.Control;component/Themes/{style}.xaml", UriKind.Relative);
                // загружаем словарь ресурсов
                ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                // очищаем коллекцию ресурсов приложения
                Application.Current.Resources.Clear();
                // добавляем загруженный словарь ресурсов
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);

                ThemeChanged?.Invoke(style);
                Theme = style;

            }
            catch
            {
                throw new Exception("Ошибка применение темы\nВыбрана тема по умолчанию");
            }

        }

        public void Default()
        {
            // определяем путь к файлу ресурсов
            var uri = new Uri($"AdaptiveTestingSystem.Control;component/Themes/BlackTheme.xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);

            ThemeChanged?.Invoke("BlackTheme");
        }

    }
}
