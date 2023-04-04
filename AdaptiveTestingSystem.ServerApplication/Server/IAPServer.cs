using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerApplication.Server
{
    public interface IAPServer
    {
         ServerObject Server { get; set; }
         AppSettings Settings { get; set; }
         Thread ListenThread { get; set; }
         string ErrorMessage { get; set; }
         void Start();
         void Stop();
         void Restart();
         bool IsRunning { get { return Server.IsRunning; } }

         delegate void  ErrorConnectHandler(string error);
         event ErrorConnectHandler? ErrorConnect;

        delegate void StartingToConnectHandler();
        event StartingToConnectHandler? StartingToConnect;

        delegate void ServerRunningHandler();
        event ServerRunningHandler? ServerRunning;

    }
}
