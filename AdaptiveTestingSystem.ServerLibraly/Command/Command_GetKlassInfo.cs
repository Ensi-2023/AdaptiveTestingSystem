#nullable disable
namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetKlassInfo:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {


                var obj = JsonSerializer.Deserialize<Data_Klass>(json);

                var list = await DBDataMethod.GetKlassList(obj.IsCheking);

                var comman = new Data_FirstCommand()
                {
                    Command = "Command_User_Insert_klassList",
                    Json = JsonSerializer.Serialize(list)
                };

                Send(client, activeServer, comman);
                
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetKlassInfo вызвал ошибку: {ex.Message}");
            }
        }
    }
}
