#nullable disable
namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_Code: Data_Base
    { 
    }


    public class Data_Authoriz: Data_Base
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }

        public Data_Access AccessRights { get; set; } = new Data_Access();
    }

    public class Data_Disconnect: Data_Base
    {
        public string GUI { get; set; }

    }

    public class Data_Access
    {
        public int IdUser { get; set; }
        public int IdRoly { get; set; }
        public string NameRoly { get; set; }
        public bool AllRoly { get; set; }
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


        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Middlemane { get; set; }
        public string Datebirch { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
        public string DateRegistration { get; set; }
    }

    public class Data_GiveGuid
    {
        public string GUID { get; set; }
        public string Date { get; set; }
        public string Identity { get; set; }
    }

    public class Data_UserList: Data_Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public List<Data_Klass> KlassesUser { get; set; }
        public string DateBirch { get; set; }
        public string RegistrationData { get; set; }
        public string Role { get; set; }

        public bool IsTeacher { get; set; }
        public bool IsModify { get; set; }
        public string Login{ get; set; }

        public bool IsUpdateOrInsert { get; set; }
    }

    public class Data_UserViewer: Data_Base
    {
        public List<Data_UserList> UserList { get; set; } = new List<Data_UserList>();
        public bool IsFilter { get; set; } = false;
        public Date_FilterUser FilterUser { get; set; } = null;
    }

    public class Data_UserPacket:Data_Base
    {
        public Code IsRoleUser { get; set; }
        public bool IsFilter { get; set; } = false;
        public Date_FilterUser FilterUser { get; set; } = null;
    }
}

    public class Data_DeleteUser: Data_Base
    { 
        public int Id { get; set; }

        public List<Data_DeleteUser> Users { get; set; } = new List<Data_DeleteUser>();
    }

    public class Date_FilterUser
    {
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
        public bool IsFilterData { get; set; } = false;
        /// <summary>
        /// 1 - Male; 2 - Female; 3 - All
        /// </summary>
        public int Gender { get; set; } = 3;
    }


    public class Data_UserEdit
    {
        public int IndexUser { get; set; } = 0;
        public bool IsEditFIO { get; set; } = false;    
        public bool IsEditDateBirch { get; set; } = false;
        public bool IsEditGender { get; set; } = false;
        public bool IsEditPassword { get; set; } = false;
        public string FIO { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateBirch { get; set; } = string.Empty;

        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }


    public class Data_UserEdit_Save: Data_Base
    {

    }

    public class Data_UserEdit_Delete: Data_Base
    {

        public int IndexDeletedUser { get; set; }
        public int IndexUser { get; set; }

    }

    public class Data_UserToClass: Data_Base
    { 

        public int IndexUser { get; set; }
        public int IndexClass { get; set; }
    }


    public class Data_UserToSubject: Data_Base
    {
 
        public int IndexUser { get; set; }
        public int IndexSubject { get; set; }
    }


