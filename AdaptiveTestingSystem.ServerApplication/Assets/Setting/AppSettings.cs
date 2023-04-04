using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AdaptiveTestingSystem.ServerApplication.Assets.Setting
{
    public class AppSettings
    {
        public delegate void LoadSettingHandler(IPAddress ip, int port, bool onAutoConnect, bool onAutoSaveLog);
        public event LoadSettingHandler? LoadSetting;
        public delegate void SaveSettingHandler(IPAddress ip, int port);
        public event SaveSettingHandler? SaveServerSetting;
        public delegate void ErrorLoadSettingsHandler();
        public event ErrorLoadSettingsHandler? ErrorLoadSetting;
    
        public IPAddress? IP { get; private set; }
        public int Port { get; private set; }
        public bool OnAutoConnectToServer { get; private set; }
        public bool OnAutoSaveLogToServer { get; private set; }


        private bool isError = false;
        private IniFile settingIni;

#nullable disable
        public AppSettings()
        {
            Directory.CreateDirectory("config");
            settingIni = new IniFile("config\\app.ini");

            if (!File.Exists("config\\app.ini"))
            {

            }
        }


        /// <summary>
        /// Задать Ip адрес
        /// </summary>
        /// <param name="_ip">IP адрес сервера</param>
        public void SetIp(IPAddress _ip)
        {
            IP = _ip;
            settingIni.Write("APP", "IpAdress", _ip.ToString());
            Logger.Message($"Настроки IP: {_ip}");
        }
        /// <summary>
        /// Задать порт
        /// </summary>
        /// <param name="_port">Порт сервера</param>
        public void SetPort(int _port)
        {
            Port = _port;
            settingIni.Write("APP", "Port", _port.ToString());

            Logger.Message($"Настроки сохрарены Port:{_port}");
        }
        /// <summary>
        /// Задать авто подключение к серверу при запуске программы
        /// </summary>
        /// <param name="_onAutoConnect">Авто подключение</param>
        public void SetAutoConnect(bool _onAutoConnect)
        {
           
            if(OnAutoConnectToServer!= _onAutoConnect) Logger.Message($"Настроки сохрарены OnAutoConnectToServer:{_onAutoConnect}");
            this.OnAutoConnectToServer = _onAutoConnect;
            settingIni.Write("APP", "AutoConnect", _onAutoConnect.ToString());


        
        }
        /// <summary>
        /// Задать IP адрес и Port
        /// </summary>
        /// <param name="_ip">IP адрес сервера</param>
        /// <param name="_port">Порт сервера</param>
        public void Set(string _ip, int _port)
        {

            if (!(IP.Equals(IPAddress.Parse(_ip))) || (Port != _port))
            {
                SaveServerSetting?.Invoke(IPAddress.Parse(_ip), _port);
            }

            IP = IPAddress.Parse(_ip);
            Port = _port;

            settingIni.Write("APP", "IpAdress", _ip.ToString());
            settingIni.Write("APP", "Port", _port.ToString());

            Logger.Message($"Настроки сохрарены {_ip}:{_port}");
        }
        /// <summary>
        /// Задать IP адрес, порт и авто подключение
        /// </summary>
        /// <param name="_ip">IP адрес сервера</param>
        /// <param name="_port">Порт сервера</param>
        /// <param name="_onAutoConnect">Авто подключение</param>
        public void Set(IPAddress _ip, int _port, bool _onAutoConnect,bool _onAutoSaveLog,bool loging=false)
        {
            IP = _ip;
            Port = _port;
            this.OnAutoConnectToServer = _onAutoConnect;
            this.OnAutoSaveLogToServer = _onAutoSaveLog;
            settingIni.Write("APP", "IpAdress", _ip.ToString());
            settingIni.Write("APP", "Port", _port.ToString());
            settingIni.Write("APP", "AutoConnect", _onAutoConnect.ToString());
            settingIni.Write("APP", "AutoSaveLog", _onAutoSaveLog.ToString());

            if (loging)
            {
                Logger.Message($"Настроки сохрарены IP:{_ip}");
                Logger.Message($"Настроки сохрарены Port:{_port}");
                Logger.Message($"Настроки сохрарены OnAutoConnectToServer:{_onAutoConnect}");
                Logger.Message($"Настроки сохрарены OnAutoSaveLogToServer:{_onAutoSaveLog}");
            }
        }




        /// <summary>
        /// Загрузить настройки
        /// </summary>
        public void Load()
        {
            isError = false;

            try
            {
                OnAutoConnectToServer = ParserVariables.GetBoolean(settingIni.ReadINI("APP", "AutoConnect"));
                Logger.Message($"OnAutoConnectToServer: loaded ({OnAutoConnectToServer})");
            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load.OnAutoConnectToServer: {ex.Message}");
                OnAutoConnectToServer = false;

                Logger.Message($"OnAutoConnectToServer: set (false)");
            }

            try
            {          
                OnAutoSaveLogToServer = ParserVariables.GetBoolean(settingIni.ReadINI("APP", "AutoSaveLog"));
                Logger.Message($"OnAutoSaveLogToServer: loaded ({OnAutoSaveLogToServer})");
            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load.OnAutoSaveLogToServer: {ex.Message}");
                OnAutoSaveLogToServer = false;

                Logger.Message($"OnAutoSaveLogToServer: set (false)");
            }

            try
            {
                IP = ParserVariables.GetIP(settingIni.ReadINI("APP", "IpAdress"));
                Logger.Message($"IP: loaded ({IP})");

            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load.IP: {ex.Message}");
                isError = true;
            }

            try
            {  
                Port = ParserVariables.GetInt(settingIni.ReadINI("APP", "Port"));
                Logger.Message($"Port: loaded ({Port})");
       
            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load.Port: {ex.Message}");
                isError = true;
            }

            //Data Base Setting
            try
            {
                DBSettings settings = new DBSettings();
            }
            catch (Exception ex)
            {
                Logger.Error($"AppSettings.Load (DBSettings.Load): {ex.Message}");
            }




            if (isError)
                ErrorLoadSetting?.Invoke();
            else
            {
                LoadSetting?.Invoke(IP, Port, OnAutoConnectToServer, OnAutoSaveLogToServer);
                Set(IP, Port, OnAutoConnectToServer, OnAutoSaveLogToServer);
            }
        }

        public void SetAutoSaveLog(bool value)
        {
           if(OnAutoSaveLogToServer!=value) Logger.Message($"Настроки сохрарены OnAutoSaveLogToServer:{value}");

            OnAutoSaveLogToServer = value;
            settingIni.Write("APP", "AutoSaveLog", value.ToString());

        
        }
    }
}
