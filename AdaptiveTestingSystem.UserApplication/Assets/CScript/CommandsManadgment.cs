using AdaptiveTestingSystem.UserApplication.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class CommandsManadgment
    {

        static private bool IsUpload { get;  set; }
        static private string Command { get; set; } = string.Empty;


        private static readonly List<Commands> CommandClass = new List<Commands>()
        {
            new Command_Authorization(),
            new Command_SetGUID(),
            new Command_Registration(),
            new Command_SetUserList(),
            new Command_User_Insert_klassList(),
            new Command_User_Insert_ClassRoomInsert(),
            new Command_DeleteClassRoom(),
            new Command_NewUserInsert(),
            new Command_DisconnectClass(),
            new Command_UserEdit(),
            new Command_UserEdit_Delete(),
            new Command_SetKlassList(),
            new Command_SetUserNoClassRoom(),
            new Command_SetUserInformationByIndex(),
            new Command_SetEmployee(),
            new Command_SubjectUpdateData(),
            new Command_SetSubjectList(),
            new Command_SetUserToSubject(),
            new Command_SetUserRoly(),
            new Command_InfAddNewRoly(),
            new Command_SetRolyInformationList(),
            new Command_SetRolyListWithoutThisIndex(),
            new Command_SetAllTest(),
            new Command_SetQuestionThisTesting(),
            new Command_SetSubjectListInGeneratorTest(),
            new Command_SetFreeSocket(),
            new Command_SuccessfullyOrErrorAddingTest(),
            new Command_ApendTestingData(),
            new Command_ViewQuestData(),
            new Command_SuccessfullyOrErrorAddingOrEditQuest(),
            new Command_SetRandomNameAndIndexTest(),
            new Command_SetAllMultyTest(),
            new Command_SetAllTestForCreateServer(),
            new Command_ConnectToTestingServer(),
            new Command_ClientManagerForActiveTestingServer(),
            new Command_SetStatusClientForConnectTestServer(),
            new Command_SetAnswerClientForActiveTestServer(),
            new Command_StartTesting(),
            new Command_SetUserResultTesting(),
            new Command_StatisticGeneral(),
            new Command_ConnectAdminTestServer(),
            new Command_StatisticCustom(),
        };


        private static Type? GetTypeCommand(string command)
        {
            return Type.GetType($"AdaptiveTestingSystem.UserApplication.Assets.Command.{command}");
        }


        static public void Parse(string json, InternetClient client)
        {
            try
            {
                //if (IsUpload)
                //{
                //    try
                //    {
                //        var jsons = JsonSerializer.Deserialize<Data_FirstCommand>(json);
                //        if (jsons != null)
                //        {
                //            if (jsons.IsCloseThread)
                //            {
                //                IsUpload = false;
                //            }
                //        }
                //    }
                //    catch { }

                //    var searchclass = CommandClass.Find(o => o.GetType() == GetTypeCommand(Command));
                //    if (searchclass != null)
                //    {

                //    }
                //}
                //else
                {
                    var jsonDes = JsonSerializer.Deserialize<Data_FirstCommand>(json);
                    if (jsonDes == null) return;

                    if (jsonDes.IsUpload)
                    {
                        Command = jsonDes.Command;
                    }
                
                    var _class = CommandClass.Find(o => o.GetType() == GetTypeCommand(jsonDes.Command));
                    if (_class != null)
                    {
                        _class.Execut(jsonDes.Json, client);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Warning($"Ошибка обрабротки команды\n{ex.Message}");
                
            }

        }
    }
}
