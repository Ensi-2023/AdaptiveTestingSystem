using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Data.JsonData
{


    public class Data_StartTesting: Data_Base
    { 

        public bool IsAdaptive { get; set; }
    }
    public class Data_MultyServerSendAnswer
    {
        public string IndexServer { get; set; } = string.Empty;
        public int IndexQuest { get; set; } = 0;
        public int NumberAnswer { get; set; } = 0;
        public int NumberCorrect { get; set; } = 0;
        public int CountQuest { get; set; }

        public string GUID { get; set; }

        public Data_ResultTesting resultTesting { get; set; } = new Data_ResultTesting();

    }

    public class Data_MultyServerClient: Data_Base
    {
        public string IndexServer { get; set; } = string.Empty;

        public string GUID { get; set; } = string.Empty;
        public int IndexQuest { get; set; } = 0;
        public int IndexAnswer { get; set; } = 0;
        public string NameTest { get; set; } = string.Empty;
        public int IndexTest { get; set; } = 0;
        public string NameClient { get; set; } = string.Empty;
        public bool IsAdaptive { get; set; } = false;

        public int CountQuest { get; set; } = 0;

    }

    public class Data_MultyServer:Data_Base
    {
        public int IndexTest { get; set; } = 0;
        public bool IsAdaptive { get; set; } = false;
        public string Password { get; set; } = string.Empty;  
        public int IndexCreator { get; set; } = 0;
        public string NameTest { get; set; } = string.Empty;
        public int CountQuestForTesting { get; set; } = 0;
    }


    public class Data_PacketServerStatusUser: Data_Base
    {
         public string IndexServer { get; set; }
    }


    public class Data_ListPacketServer: Data_Base
    {
        public bool IsAdmin { get;  set; } = false;
        public List<Data_ListMultyServer> MultyServer { get; set; }
    }

    public class Data_ListMultyServer: Data_Base
    {
        public string IndexServer { get; set; } = string.Empty;
        public int IndexTest { get; set; } = 0;
        public string NameTest { get; set; } = string.Empty;
        public bool IsAdaptive { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public int IndexCreator { get; set; } = 0;
        public string NameCreator { get; set; } = string.Empty;
        public string NamePredmet { get; set; } = string.Empty;
        public int CountUser { get; set; } = 0;
        public int CountQuestForTest { get; set; } = 0;
       
    }

    public class Data_GiveAllUserInTestServer
    {
        public string IndexServer { get; set; } = string.Empty;
    }
}

