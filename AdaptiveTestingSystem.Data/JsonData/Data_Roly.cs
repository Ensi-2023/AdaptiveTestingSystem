using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_Roly:Data_Base
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }


    public class Data_RolyInf: Data_Base
    {
        public int Index { get; set; } 
        public string Name { get; set; } = string.Empty;

        public bool ReadUser { get; set; }
        public bool ReadSotrud { get; set; }
        public bool ReadClass { get; set; }
        public bool ReadPredmet { get; set; }
        public bool CreateAndViewReport { get; set; }
        public bool TestReady { get; set; }
        public bool CreateTest { get; set; }
        public bool CreateGroup { get; set; }
        public bool ConnectGroup { get; set; }
        public bool AddSotrudForPredmet { get; set; }
        public bool DeleteSotrudForPredmet { get; set; }


        public bool ViewDataUser { get; set; }
        public bool ViewDataSotrud { get; set; }
        public bool ViewPrepmet { get; set; }
        public bool ViewClass { get; set; }

      
    }

    public class Data_RolyUser
    { 
        public int IndexUser { get; set; }
        public string Name { get; set; }

    }

    public class Data_UpdateUserRoly
    {
        public int IndexRoly { get; set; }
        public List<Data_RolyUser> Users { get; set; } = new List<Data_RolyUser>();
    }

    public class Data_RPacket
    { 
        public List<Data_RolyUser> Users { get; set; } = new List<Data_RolyUser>();
        public Data_RolyInf RolyData { get; set; }
    }
}
