#nullable disable
namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    /// <summary>
    /// Выдает GUID новому пользователю, если тот прошел проверку на hash
    /// </summary>
    public class Command_GUID:Commands
    {
        private readonly string vertyHash = "0295b28d454780feb6f195075d5c6ae5";

        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var _parse = JsonSerializer.Deserialize<Data_GiveGuid>(json);          
                var input = (_parse.Identity);
                if (Encryption.verifyMd5Hash(input, vertyHash))
                {
                    Data_GiveGuid guid = new Data_GiveGuid()
                    {
                        Date = DateTime.Now.ToString(),
                        GUID = client.GuidClient,
                        Identity = ""
                    };

                    Data_FirstCommand command = new Data_FirstCommand()
                    {
                        Command = "Command_SetGUID",
                        Json = JsonSerializer.Serialize<Data_GiveGuid>(guid)
                    };

                    Logger.Log($"Command_GUID ({client.IP}:{client.Port}) выдаю GUID:{client.GuidClient}");
                    activeServer.Send(JsonSerializer.Serialize<Data_FirstCommand>(command), client.GuidClient);
                }
                else client.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GUID ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }

        }
    }
}
