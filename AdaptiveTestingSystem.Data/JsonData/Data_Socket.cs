#nullable disable
using System.Net;
namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_CreateSocket
    {
        public string GUID { get; set; }
        public bool IsTCP { get; set; }
        public bool IsUDP { get; set; }
    }

    public class Data_SendClientSocket: Data_Base
    {
        public string IP { get; set; }
        public int Port { get; set; }
       
    }
}
