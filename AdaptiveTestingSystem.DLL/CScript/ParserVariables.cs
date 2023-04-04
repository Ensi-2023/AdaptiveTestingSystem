
using System.Net;

namespace AdaptiveTestingSystem.DLL.CScript
{
    public class ParserVariables
    {
        public static bool GetBoolean(string value)
        {

            try
            {
                return bool.Parse(value);
            }
            catch
            {
                throw new Exception($"Ошибка преобразования: {value} в boolean");
            }
        }


        public static int CountSymbol(string value)
        {
            return value.Trim().Length;
        }

        public static DateTime GetDate(string value)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch
            {


           

                throw new Exception($"Ошибка преобразования: {value} в DateTime");

           
            }
        }

        public static int GetInt(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                        
                throw new Exception($"Ошибка преобразования: {value} в int");
          
            }
        }

        public static IPAddress? GetIP(string value)
        {
            try
            {
                return IPAddress.Parse(value);
            }
            catch 
            {
                throw new Exception($"Ошибка преобразования: {value} в IPAddress");    
            }
        }
    }
}
