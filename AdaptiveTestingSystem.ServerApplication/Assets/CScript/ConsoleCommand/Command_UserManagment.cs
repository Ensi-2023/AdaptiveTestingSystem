using System;
using System.Collections.Generic;

namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript.ConsoleCommand
{
    public class Command_UserManagment:Commands
    {
        public virtual void Exec(string[]? arg, APServer server) { }
        private static readonly List<Command_UserManagment> managments = new List<Command_UserManagment>()
        {
            new Ban() { Type = TypeCommand.slowlyExec},
            new Unban() { Type = TypeCommand.slowlyExec},
        };



        public override void Command(string[]? arg, APServer server)
        {
            if (arg == null && arg?.Length == 0) return;
#nullable disable
            var obj = managments.Find(c => StringToLowerAndTrim(c.ToString()).Contains(StringToLowerAndTrim(DeleteChar(arg[0], '/'))));

            if (obj == null) return;
            obj.Exec(arg, server);

            this.CountCalls = 0;
        }

        public override bool IsCheckTypeCommand(string[] command)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var obj = managments.Find(c => StringToLowerAndTrim(c.ToString()).Contains(StringToLowerAndTrim(DeleteChar(command[0], '/'))));
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

            if (command.Length > 1)
            {
#nullable disable
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
#nullable enable
        private static bool SLowly(Command_UserManagment? obj)
        {
#nullable disable
            return obj.Type == TypeCommand.slowlyExec;
        }

        public override List<string> ListCommand { get; set; } = new List<string>()
        {
            "/ban",
            "/unban"
        };
    }

    public class Ban : Command_UserManagment
    {
#nullable enable
        public override void Exec(string[]? arg, APServer server)
        {
            var obj = arg;
            if (obj == null) return;
            if (obj.Length > 1)
            {
                try
                {
                    var login = obj[1];
                    server.BannedAccount(login);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Command_UserManagment:  Ban вызвал ошибку: {ex.Message}");
                }
            }
        }
    }

    public class Unban : Command_UserManagment
    {
      
        public override void Exec(string[]? arg, APServer server)
        {
            var obj = arg;
            if (obj == null) return;
            if (obj.Length > 1)
            {
                try
                {
                    var login = obj[1];
                    server.UnBannedAccount(login);

                }
                catch (Exception ex)
                {
                  Logger.Error($"Command_UserManagment:  UnBan вызвал ошибку:{ex.Message}");
                }
            }
        }
    }
}
