
namespace AdaptiveTestingSystem.Data
{
    public class Enums
    {

        public enum ViewData
        {
            day, month, year
        }

        public enum Code
        {
            Null = 0,
            Delete = 1,
            Insert = 2,
            Update = 3, 
            Error = 4,
            Successfully = 5,

            LoginIsAuthorized = 2321,
            InvalidUserNameOrPassword = 2322,
            RegistrationSuccessful = 2323,
            InvalidLogin = 2324,
            InvalidRegistration = 2325,
            AccountBanned = 2326,
            ErrorGetInfoKlass = 2327,
            ErrorGetInfoKlassNoData = 2328,
            ErrorGetUserList = 2329,
            SuccessfulInsertClassRoom=2330,
            ErrorInsertClassRoom=2331,
            ErrorDeleteClassRoom_GUI_User=2332,
            ErrorDeleteClassRoom=2333,

            SuccessfullDeleteClassRoom_GUI_User = 2334,
            SuccessfullDeleteClassRoom = 2335,




            ////Subject
            ///
            Subject_Insert=5000,
            Subject_Update=5001,
            Subject_Error=5002,
            Subject_Delete=5003,
            Subject_Insert_User=5004,
            Subject_Insert_Successfull = 5005,

            ////GUI CODE
            ///
            GUI_User = 4000,
            GUI_UserModify = 4001,
            GUI_Staff = 4002,
            GUI_UserAll = 4003,
         
            ////Inserting CODE
            ///
            NewUserInsert = 6000,
            NewStaffInsert = 6001,          
            NewUserClassRoomInsert = 6002,          
            ////
            ///User Editing
            User_Edit_Save=7000,
            User_Edit_Error=7001,
            User_Edit_PasswordError=7002,
            ///User Deleting
            User_Delete_Сompleted = 8000,
            User_Delete_Error = 8001,
            User_Delete_WithoutAccess = 8002,

            //User ClassRoom InformatingCode
            NewUserClassRoomError = 6012,
            NewUserClassRoomnSuccessful = 6022,

            //Roly

            Roly_Add_Successfull = 9000,
            Roly_Update_Successfull = 9001,
            Roly_Error = 9002,
            Roly_NameError = 9003,
            Roly_Update_NameError = 9004,


            //Code

            ThreadStart = 10000,
            ThreadEnd = 10001,
            ThreadNext = 10002,
            ThreadStop = 10003,
            NoThreadStart=10004,
            ThreadNewCommand=10005,



            //Server Testing

            WaitClient = 20000,
            StartingTest = 20001,
            StopServer = 20002,
            CreateServer = 20003,
            ViewServer = 20004,
            ErrorCnnectToServer_ServerStart = 20005,
            ErrorCnnectToServer_NotCorrectPassword = 20006,
            ErrorCnnectToServer_NotCorrectServerIndex= 20007,
            ErrorCnnectToServer_ServerNotStart= 20008,
            RemoveConnectedClient= 20009,
            


            //ClientConnectCode
         
            ConnectedToServer = 30002,
            UploadSuccessfull = 30003,
            UploadingData = 30004,
            TestingCompleted = 30005,
            TestingRun = 30006,
            RemoveConnect = 30007,
            KickUserConnect = 30008,
            NewClientConnect = 30009,
            StartTestingUser = 30010,
            StartTestingForAdmin=30011,
        }
    }
}
