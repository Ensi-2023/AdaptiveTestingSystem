
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class AppSettings
    {
        public event SettingLoadedHandler? SettingLoaded;
        public event ErrorLoadSettingsHandler? ErrorLoadSetting;
        public IPAddress? IP { get; private set; }
#nullable disable
        private static IniFile _settingIni;
        private bool _isError = false;

        public delegate void SettingLoadedHandler();
        public delegate void ErrorLoadSettingsHandler();

        public int Port { get; private set; }
        public string ThemeName { get; private set; }

        public AppSettings()
        {
            Directory.CreateDirectory("config");
            _settingIni = new IniFile("config\\app.ini");

            if (!File.Exists("config\\app.ini"))
            {

            }
        }

        public void Load()
        {
            _isError = false;
            try
            {
                IP = ParserVariables.GetIP(_settingIni.ReadINI("APP", "IpAdress"));
                Logger.Message($"IP: loaded ({IP})");
            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load.IP: {ex.Message}");
                _isError = true;
            }

            try
            {
                Port = ParserVariables.GetInt(_settingIni.ReadINI("APP", "Port"));
                Logger.Message($"Port: loaded ({Port})");
            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load.Port: {ex.Message}");
                _isError = true;
            }

            ThemeName = _settingIni.ReadINI("APP", "Theme");
            if (ThemeName.Trim().Length == 0)
            { ThemeName = "BlackTheme"; ThemeSet("BlackTheme"); }


            Logger.Message($"Theme: loaded ({ThemeName})");


            if (_isError)
                ErrorLoadSetting?.Invoke();
            else
            {
                SettingLoaded?.Invoke();
                Set(IP.ToString(), Port);
            }
        }

        public void Set(string _ip, int _port)
        {
            _settingIni.Write("APP", "IpAdress", _ip.ToString());
            _settingIni.Write("APP", "Port", _port.ToString());      
        }

        public void ThemeSet(string name)
        {
            _settingIni.Write("APP", "Theme", name);
            Logger.Message($"Theme update: {name}");

        }

 
    }
}
