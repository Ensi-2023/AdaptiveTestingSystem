#nullable disable
namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    /// <summary>
    /// Срабатывает при авторизации пользователей. Выдает роль
    /// </summary>
    public class Command_AuthorizationUser:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Authoriz>(json);

                //Проверка наличия пользователя в базе данных
                if (await DBSearchMethods.IsCheckLoginAndPassword(obj.Login.Trim(), obj.Password.Trim()))
                {
                    //Проверка на доступ к приложению
                    var banned = await DBSearchMethods.IsCheckInformationBannedAccount(obj.Login);
                    if (banned)
                    {
                        var _obj = new Data_Authoriz()
                        {
                            Login = obj.Login,
                            Password = "ok",
                            IsVerified = true,
                            IsCode = Code.AccountBanned
                        };

                        var command = new Data_FirstCommand()
                        {
                            Command = "Command_Authorization",
                            Json = JsonSerializer.Serialize(_obj)
                        };
                        Send(client, activeServer, command);
                    }
                    else
                    {
                        var access = await DBDataMethod.GetAccessUser(obj.Login);
                        //Проверка на авторизованного пользователя. Авторизованный ПК отключает от программы.
                        if (await activeServer.IsCheckAuthorizedClient(obj.Login))
                        {

                            var guid = await activeServer.IsReturnAuthorizedGUID(obj.Login);
                            var disc = new Data_Disconnect()
                            {
                                GUI = guid,
                                IsCode = Code.LoginIsAuthorized
                            };


                            var comm = new Data_FirstCommand()
                            {
                                Command = "Command_DisconnectClass",
                                Json = JsonSerializer.Serialize(disc)
                            };

                            activeServer.Send(JsonSerializer.Serialize(comm), guid);
                            activeServer.DeleteInListAuthorizationGUID(guid);
                        }

                        //Если все проверки прошли успешно авторизовываем пользователя
                        activeServer.AddInAuthorizatedUserList(client.GuidClient, obj.Login);
                       
                        var _obj = new Data_Authoriz()
                        {
                            Login = obj.Login,
                            Password = "ok",
                            IsVerified = true,
                            IsCode = Code.Null,
                            AccessRights = access
                        };

                        var command = new Data_FirstCommand()
                        {
                            Command = "Command_Authorization",
                            Json = JsonSerializer.Serialize(_obj)
                        };

                        client.Name = obj.Login;
                        Send(client, activeServer, command);
                    }
                }
                else
                {
                    //Если не нашли показываем пользователю ошибку
                    var _obj = new Data_Authoriz()
                    {
                        Login = obj.Login,
                        Password = "no",
                        IsVerified = false,
                        IsCode = Code.InvalidUserNameOrPassword
                    };

                    var command = new Data_FirstCommand()
                    {
                        Command = "Command_Authorization",
                        Json = JsonSerializer.Serialize(_obj)
                    };

                    Send(client, activeServer, command);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_AuthorizationUser вызвал ошибку авторизации: {ex.Message}");
                client.Close();
            }
        }
    }
}
