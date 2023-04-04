

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public abstract class Commands
    {
        abstract public void Execut(string json, InternetClient client);
    }
}
