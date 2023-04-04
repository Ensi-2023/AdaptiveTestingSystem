#nullable disable

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    /// <summary>
    /// Вызывается при отключении пользователя от профиля.
    /// </summary>
    public class Command_AuthorizedUserDisconnect:Commands
    {
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Disconnect>(json);
                activeServer.DeleteInListAuthorizationGUID(obj.GUI);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_AuthorizedUserDisconnect вызвал ошибку: {ex.Message}");
            }
        }
    }
}
