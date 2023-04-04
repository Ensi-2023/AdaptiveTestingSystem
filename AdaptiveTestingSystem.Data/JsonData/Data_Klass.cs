#nullable disable

using Microsoft.VisualBasic.ApplicationServices;

namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_Klass: Data_Base
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }

        public bool IsCheking { get; set; }


        public string Employee { get; set; }
        public int EmployeeID { get; set; }
        public int CountUser { get; set; }
        public List<Data_UserList> Users { get; set; }

        public string DatebirchEmployee { get; set; } = DateTime.Now.ToShortDateString();

        public string GenderEmployee { get; set; }


    }

    public class Data_Klass_Delete: Data_Base
    { 
        public List<Data_Klass> Klasses { get; set; }

        public string Description { get; set; }
        
    }


    public class Data_Klass_UserDelete: Data_Base
    { 
        public List<Data_UserList> Users { get; set; }
   
        public int Index { get; set; }
    }


    public class Data_Klass_UpdateEmployee: Data_Base
    { 

        public int Index_Class { get; set; }
        public int Index_Employee { get; set; }
    }
}
