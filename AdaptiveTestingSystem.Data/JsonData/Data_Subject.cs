using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_Subject: Data_Base
    {
        public int Id_data { get; set; } = 0;
        public string Name_data { get; set; } = string.Empty;
        public string Name_epm_data { get; set; } = string.Empty;

        public List<Data_Subject_User> Users { get; set; } = new List<Data_Subject_User>();

    }



    public class Data_Subject_Delete: Data_Base
    {
        public List<Data_Subject> Subject { get; set; }
        public string Description { get; set; }
    }

    public class Data_Subject_User
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
        public string DateBirch { get; set; } = string.Empty;
    }



    public class Data_Subject_UserDelete: Data_Base
    {
        public List<Data_UserList> Users { get; set; }
        public int Index { get; set; }
    }

}
