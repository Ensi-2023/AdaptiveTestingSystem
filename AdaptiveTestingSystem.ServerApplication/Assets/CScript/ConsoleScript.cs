using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveTestingSystem.ServerApplication.Assets.CScript.ConsoleCommand;

namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript
{
    public class ConsoleScript
    {
        static readonly List<Commands> _command = new List<Commands>()
        {
            new Command_ServerManagment(){},
            new Command_UserManagment(){}          
        };

        /// <summary>
        /// Парсер команд
        /// </summary>
        /// <param name="text">переданная команда</param>
        /// <param name="server">активный сервер</param>
        /// <param name="onUsePoppup">При взаимодействии с подсказкой запустим проверку на тип команды и если она slowly, то запретим очищать текстовое поле. 
        /// Нужно для задачи команде определённых аргументов</param>
        /// <returns>True - Команда сработала успешно и поле будет очищено. 
        /// False - в 2х случаях: Если команда не опознана и если команда опознана, но нужно не очищать поле.</returns>
        internal static bool ConsoleCommandParser(string text, APServer server, bool onUsePoppup)
        {
            if (text.Trim().Length == 0) return false;
            var command = text.Trim().Split(' ');
            var obj = _command.Find(c => c.ListCommand.Contains(command[0]));


            if (obj == null)
            {
                Logger.Log($"Команда {command[0]} не опознана");
    
                return false;
            }

            if (onUsePoppup)
            {
                if (obj.IsCheckTypeCommand(command))
                {
                    if (obj.CountCalls == 0)
                    {
                        obj.CountCalls++;
                        return false;
                    }
                    else
                    {
                        obj.CountCalls = 0;
                    }
                }
            }

            obj.Command(command, server);
            return true;

        }

    }
}
