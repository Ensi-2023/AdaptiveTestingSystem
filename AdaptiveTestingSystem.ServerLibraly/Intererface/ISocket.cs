using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Intererface
{
    public interface ISocket
    {
        public int Port { get; set; }
        public IPAddress IPAddressAddress { get; set; }
        public int TimeLife { get; set; }
        public int CountConnect { get; set; }

        public int MaxConnect { get; set; }
    }
}
