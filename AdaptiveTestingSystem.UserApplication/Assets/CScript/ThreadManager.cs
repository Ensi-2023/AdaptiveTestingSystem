using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class ThreadManager
    {

        public static bool IsStop { get; private set; } = false;

        private static string _command { get; set; } = string.Empty;
        private static object _serializeObj { get; set; } = null;
        public static void Send(string nameCommand, object objSerialize)
        {
            _command = nameCommand;
            _serializeObj = objSerialize;
            (_serializeObj as Data_Base).IsCode = Code.ThreadStart;
            IsStop = false;

            var firstCommand = new Data_FirstCommand()
            {
                Command = nameCommand,
                Json = JsonSerializer.Serialize(objSerialize)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));
        }


        public static void GoNextPacket()
        {
            if (_serializeObj != null)
            {

                (_serializeObj as Data_Base).IsCode = Code.ThreadNext;

                var firstCommand = new Data_FirstCommand()
                {
                    Command = _command,
                    Json = JsonSerializer.Serialize(_serializeObj)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));
            
            }
        }
        public static void Clear()
        {
            _command = string.Empty;
            _serializeObj = null;
        }
        public static (string, object) GetActiveObj()
        {
            return (_command, _serializeObj);
        }
        public async static void CloseActiveThread()
        {
            try
            {
                if (_serializeObj != null)
                {
                    IsStop = true;

                    await Task.Delay(250);
                    var obj = _serializeObj as Data_Base;
                    if (obj != null)
                    {
                        (_serializeObj as Data_Base).IsCode = Code.ThreadEnd;

                        var firstCommand = new Data_FirstCommand()
                        {
                            Command = _command,
                            Json = JsonSerializer.Serialize(_serializeObj)
                        };
                                              
                        _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));
                        Clear();
                    }
                }
            }
            catch (Exception exx)
            {
                Logger.Error(exx.Message);
            }
        }
    }
}
