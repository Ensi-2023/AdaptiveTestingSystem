using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand
{
    public class IC_Socket
    {

        List<SocketObject> socketObjects;
        ServerObject activeServer;


        public IC_Socket(ServerObject server)
        {
           this.activeServer = server;
            socketObjects = new List<SocketObject>();         
        }

        public void DeleteAllSocket()
        {
            activeServer.DeleteAllSocket();
        }

        public void DeleteSocket(int port)
        {
            activeServer.DeleteSocket(port);
        }

        public async Task<SocketObject> StartSocket(int port = 6754)
        {
            var socket = await activeServer.SetSocket(port);           
            var item  = socketObjects.FirstOrDefault(x => x.Port== socket.Port);
            if(item==null)
                socketObjects.Add(socket);


             await Task.Factory.StartNew(() => 
             {
                  Application.Current.Dispatcher.Invoke(()=> 
                  {                    
                     ActiveSocket active = new ActiveSocket(activeServer);
                     activeServer.Tasks.Add(active);
                     active.Start(socket);
                  });
             });

           
            return socket;
        }

        public void ViewAllSocket()
        {
            activeServer.ViewSocket();
        }
    }
}
