#nullable disable

namespace AdaptiveTestingSystem.Data.JsonData
{   
    public class Data_Registration: Data_Base
    {
        public string SurnameUser { get; set; }
        public string LastnameUser { get; set; }
        public string MiddlenameUser { get; set; }
        public string DatebirchUser { get; set; }
        public string GenderUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public List<int> Klasses { get; set; }

 
    }


    public class Data_NewUserInsert: Data_Base
    {
        public string SurnameUser { get; set; }
        public string LastnameUser { get; set; }
        public string MiddlenameUser { get; set; }
        public string DatebirchUser { get; set; }
        public string GenderUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public List<int> Klasses { get; set; }
        public int Subject { get; set; }  = 0;

        public int Index { get; set; }

    }

}
