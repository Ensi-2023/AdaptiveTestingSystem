
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript.ConsoleCommand
{
    public class Command_ServerManagment:Commands
    {
        public virtual void Exec(string[]? arg, APServer server) { }
#nullable disable
        private static readonly List<Command_ServerManagment> managments = new List<Command_ServerManagment>()
        {
            new Start() { Type = TypeCommand.slowlyExec},
            new Stop() { Type = TypeCommand.fastExec},
            new Restart(){ Type = TypeCommand.fastExec},
            new Socket(){ Type=TypeCommand.fastExec,
                Argument=new List<ListCommand>()
                {
                    new ListCommand()
                    {
                        Name="view",Type=TypeCommand.fastExec
                    },
                    new ListCommand()
                    {
                        Name="port",Type=TypeCommand.slowlyExec
                    }
                }
            }
        };

        public override bool IsCheckTypeCommand(string[] command)
        {

            var obj = managments.Find(c => StringToLowerAndTrim(c.ToString()).Contains(StringToLowerAndTrim(DeleteChar(command[0], '/'))));

            if (command.Length > 1)
            {
                if (obj.Argument == null) return SLowly(obj);
                try
                {
                    var objSearch = obj.Argument.Find(c => StringToLowerAndTrim(c.Name).Contains(StringToLowerAndTrim(command[1])));
                    if (objSearch == null) throw new Exception();
                    return objSearch.Type == TypeCommand.slowlyExec;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return SLowly(obj);
            }

        }

        private static bool SLowly(Command_ServerManagment obj)
        {
            return obj.Type == TypeCommand.slowlyExec;
        }

        public override List<string> ListCommand { get; set; } = new List<string>()
        {
            "/start",
            "/stop",
            "/restart",
            "/socket",
        };

#nullable enable
        public override void Command(string[]? arg, APServer server)
        {
            if (arg == null && arg?.Length == 0) return;
#nullable disable
            var obj = managments.Find(c => StringToLowerAndTrim(c.ToString()).Contains(StringToLowerAndTrim(DeleteChar(arg[0], '/'))));

            if (obj == null) return;
            obj.Exec(arg, server);

            this.CountCalls = 0;
        }
    }
    public class Start : Command_ServerManagment
    {
#nullable enable
        public override void Exec(string[]? arg, APServer server)
        {

            if (server.Server!=null && server.IsRunning) { Logger.Error($"Основной сервер уже запущен!"); return; }

            var obj = arg;
            if (obj == null) return;
            if (obj.Length < 2) { server.Start(); }
            else
            {
                try
                {
                    var ip = ConvertToIP(obj[1]);
                    var port = int.Parse(obj[2]);

                    Main.Instance.Settings.Set(ip.ToString(), port);
                    server.Start();
                }
                catch (Exception ex)
                {
                  Logger.Error($"Command_ServerManagment.Start вызвал ошибку {ex.Message}");
                }
            }

        }
    }

    public class Stop : Command_ServerManagment
    {
        public override void Exec(string[]? arg, APServer server)
        {
            server.Stop();
        }
    }
    public class Restart : Command_ServerManagment
    {
        public override void Exec(string[]? arg, APServer server)
        {
            server.Restart();
        }

    }

    public class Socket : Command_ServerManagment
    {

        public override async void Exec(string[]? arg, APServer server)
        {
#nullable disable
            var obj = arg;
            if (obj == null) return;
            if (obj.Length < 2)
            {
                string help = "\n####### Справка Socket #######\n/socket view - показать все активные сокеты" +
                    "\n/socket port 0000 - запустить сокет на порте '0000'\n" +
                    "/socket - вызвать справку";

         
                Logger.Message(help);

            }
            else
            {
                try
                {
                    switch (arg[1])
                    {
                        case "view": server.Server.IC_Socket.ViewAllSocket(); break;
                        case "deleteall": server.Server.IC_Socket.DeleteAllSocket(); break;
                        case "delete": 
                            {
                                var port = int.Parse(arg[2]);
                                server.Server.IC_Socket.DeleteSocket(port);
                            } break;
                        case "port":
                            try
                            {
                                var port = int.Parse(arg[2]);
                                await server.Server.IC_Socket.StartSocket(port);
                            }
                            catch
                            {
                              Logger.Error($"Порт для сокета указан не корректно");
                            }

                            break;
                        default:
                            {
                           
                                Logger.Error($"Команда {GetCommand(arg)} не опознана");
                                return;
                            }

                    }
                }
                catch (Exception ex)
                {
                  Logger.Error($"Command_ServerManagment.Socket вызвал ошибку {ex.Message}");
                }

            }
        }

        private string GetCommand(string[] arg)
        {
            string str = "";
            for (int i = 0; i < arg.Length; i++)
            {
                str += string.Format($"{arg[i]} ");
            }

            return str;
        }
    }
}

