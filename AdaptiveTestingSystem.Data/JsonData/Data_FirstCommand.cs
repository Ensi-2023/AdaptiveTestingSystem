#nullable disable
namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_FirstCommand
    {
        public string Command { get; set; } = string.Empty;
        public string Json { get; set; } = string.Empty;
        public bool IsUpload { get; set; } = false;
        public bool IsCloseThread { get; set; } = false;
    }

    public class Data_Base
    {
        public string IdentityCommand { get; set; } = string .Empty;
        public string PacketIndex { get; set; }
        public Code IsCode { get; set; }
        public byte[] Data { get; set; } = new byte[0];
        public int SizePacket { get; set; }
    }
}
