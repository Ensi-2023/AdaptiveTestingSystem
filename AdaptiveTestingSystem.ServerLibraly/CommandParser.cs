#nullable disable
using AdaptiveTestingSystem.ServerLibraly.Command;
using AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand;

namespace AdaptiveTestingSystem.ServerLibraly
{
    public class CommandParser
    {

        private readonly List<Commands> _commands = new List<Commands>()
        {
            new Command_AuthorizationUser(),    
            new Command_GUID(),
            new Command_AuthorizedUserDisconnect(),
            new Command_GetKlassInfo(),
            new Command_GetUserList(),
            new Command_Registration(),
            new Command_InsertClassRoom(),
            new Command_DeleteClassRoom(),
            new Command_NewUserInsert(),
            new Command_DeleteUser(),
            new Command_GetFilterUserList(),
            new Command_UserEdit(),
            new Command_UserEdit_Delete(),
            new Command_GetCLassRoomList(),
            new Command_DeleteUserInClassRoom(),
            new Command_GetUserNoClassRoom(),
            new Command_UserToClassRoom(),
            new Command_GetUserInformationByIndex(),
            new Command_GetEmployee(),
            new Command_ClassRoom_UpdateEmployee(),
            new Command_SubjectInsertOrUpdate(),
            new Command_GetSubjectList(),
            new Command_DeleteSubject(),
            new Command_DeleteUserInSubject(),
            new Command_GetUserToSubject(),
            new Command_UserToSubject(),
            new Command_GetRolyList(),
            new Command_InsertNewRoly(),
            new Command_GetUserInRoly(),
            new Command_GetRolyListWithoutThisIndex(),
            new Command_ChangerRolyUser(),
            new Command_DeleteRoly(),
            new Command_UpdateDataUserRoly(),
            new Command_GetAllTest(),
            new Command_GetQuestionThisTesting(),
            new Command_DeleteTest(),
            new Command_DeleteQuestionFromTest(),
            new Command_CheckForPresencePredmet(),
            new Command_GetFreeSocket(),
            new Command_ApendTestingData(),
            new Command_ViewTestingData(),
            new Command_QuestionsDataViewer(),
            new Command_ApendQuestingData(),
            new Command_SendResultTestig(),
            new Command_ReturnRandomNameAndIndexTest(),
            new Command_AddMultyServerTesting(),
            new Command_GetMultyServerTesting(),
            new Command_GetAllTestForCreateServer(),
            new Command_ConnectToServerThisIndexServer(),
            new Command_DisconnectUserForActiveTestServer(),
            new Command_GiveAllUserinActiveTestServerThisIndex(),
            new Command_SetStatusUserForActiveTestServer(),
            new Command_AllowConnectingToTesetServer(),
            new Command_GetAnswerClientForActiveTestServer(),
            new Command_StartTestingUser(),
            new Command_CloseActiveTestingServerThisIndex(),
            new Command_GetUserResultTesting(),
            new Command_StatisticGeneral(),
            new Command_StatisticCustom(),
     
        };


        private Type _getTypeCommand(string command)
        {
            return Type.GetType($"AdaptiveTestingSystem.ServerLibraly.Command.{command}");
        }




        public void Parse(string message,ClientObject client,ServerObject activeServer)
        {
            try
            {              
                if (client.CountNotCorrectCommand <= 0)
                {
                    Logger.Warning($"У клиента {client.GuidClient} закончились попытки обработки команд. Отключаю его от сервера...");
                    client.Close();
                    return;
                }

                var obj = JsonSerializer.Deserialize<Data_FirstCommand>(message);

                if (obj == null) return;
                var obd = _commands.Find(o => o.GetType() == _getTypeCommand(obj.Command));
                if (obd == null) { client.CountNotCorrectCommand--; return; }
                client.CountNotCorrectCommand = 3;
                var json = obj.Json;
                obd.Execut(json, client, activeServer);

                if (obj.Command == "Command_ApendTestingData") return;
                if (obj.Command == "Command_ViewTestingData") return;
                Logger.Warning($"{client.GuidClient}: ({client.IP}:{client.Port}) вызывает функцию {obj.Command}.Execut");

            }
            catch
            {
                return;
            }
        }
    }
}
