namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    /// <summary>
    /// Тут происходит взаимодействие серверной части и базы данных. В любой из команд
    /// При переходе на Entity заменять методы именно в командах 
    /// </summary>

    public abstract class Commands
    {
        abstract public void Execut(string json,ClientObject client,ServerObject activeServer);

        public void Send(ClientObject client, ServerObject activeServer, Data_FirstCommand command)
        {
            activeServer.Send(JsonSerializer.Serialize(command), client.GuidClient);
        }
    }
}
