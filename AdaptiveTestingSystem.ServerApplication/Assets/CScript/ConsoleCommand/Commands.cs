#nullable disable
using System;
using System.Collections.Generic;


namespace AdaptiveTestingSystem.ServerApplication.Assets.CScript.ConsoleCommand
{
    public enum TypeCommand
    {
        fastExec, slowlyExec
    }

    public class ArgumentListCommand
    {
        public string Name { get; set; }
        public TypeCommand Type { get; set; }
    }

    public class ListCommand
    {
        public string Name { get; set; }
        public TypeCommand Type { get; set; }
#nullable enable
        public List<ArgumentListCommand>? Argument { get; set; }
#nullable disable
    }


    public abstract class Commands
    {
        /// <summary>
        /// Тип команды
        /// </summary>
        public TypeCommand Type { get; set; }
        public int CountCalls = 0;
        public abstract List<string> ListCommand { get; set; }
#nullable enable
        public List<ListCommand>? Argument { get; set; }
#nullable disable
        /// <summary>
        /// Проверяет тип команды
        /// </summary>
        /// <param name="command">Заданная команда</param>
        /// <returns>True - Если команда медленная
        ///  False - Если команда быстрая
        /// </returns>
        public abstract bool IsCheckTypeCommand(string[] command);
        /// <summary>
        /// Передаёт команду с аргументами или без
        /// </summary>
        /// <param name="arg">Команда</param>
        /// <param name="server">Активный сервер</param>
#nullable enable
        public abstract void Command(string[]? arg, APServer server);
#nullable disable
        /// <summary>
        /// Переводит строку в нижний регистр и удаляет все пробелы
        /// </summary>
        /// <param name="str">строковое значение</param>
        /// <returns>возвращает изменённую строку</returns>
        public static string StringToLowerAndTrim(string str)
        {
            return str.Trim().ToLower();
        }
        /// <summary>
        /// Удаляет заданный символ из строки
        /// </summary>
        /// <param name="str">Строковое значение</param>
        /// <param name="symbol">Удаляемый символ</param>
        /// <returns>Возвращает изменённую строку</returns>
        public static string DeleteChar(string str, char symbol)
        {
            return str.Replace(symbol, ' ').Trim();
        }

        /// <summary>
        /// Конвертация строки в IP
        /// </summary>
        /// <param name="ip">строковое значение IP</param>
        /// <returns>сконвертированый IP адрес</returns>
        /// <exception cref="Exception">Ошибка конвертации строки в IP адрес</exception>
        public static IPAddress ConvertToIP(string ip)
        {
            try
            {
                return IPAddress.Parse(ip);
            }
            catch
            {
                throw new Exception($"[{DateTime.Now}] - Ошибка конвертации строки {ip} в IP адрес");
            }
        }
    }
}