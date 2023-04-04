#nullable disable
namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_Registration:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Registration>(json);

                if (await DBSearchMethods.IsCheckLoginAndPassword(obj.Login, obj.Password) == false)
                {
                    var reg = await DBAddingMethod.Registration(obj);
                    if (reg != null)
                    {
                        Send(client, activeServer, SuccessfulRegistration(reg));
                    }
                }
                else
                {
                    Send(client, activeServer, SendInvalid());
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_Registration ({client.IP}:{client.Port}) вызвал ошибку при регистрации: {ex.Message}");
                client.Close();
            }
        }

        private Data_FirstCommand SendInvalid()
        {
            var reg = new Data_Registration()
            {
                IsCode = Enums.Code.InvalidLogin
            };

            var command = new Data_FirstCommand()
            {
                Command = "Command_Registration",
                Json = JsonSerializer.Serialize(reg)
            };

            return command;
        }
        private Data_FirstCommand SuccessfulRegistration(Data_Registration obj)
        {
            var command = new Data_FirstCommand()
            {
                Command = "Command_Registration",
                Json = JsonSerializer.Serialize(obj)
            };

            return command;
        }




    }
}
