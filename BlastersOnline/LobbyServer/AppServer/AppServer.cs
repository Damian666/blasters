using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace LobbyServer.AppServer
{

    /// <summary>
    /// Represents a single application server
    /// </summary>
    public class AppServer
    {

        /// <summary>
        /// The connection to this specific app-server.
        /// This should likely be on the same subnet and network; although not required.
        /// </summary>
        public NetConnection Connection { get; set; }

        /// <summary>
        /// The name of this application server
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The current CPU load of this app server; used to determine where to place the next game on
        /// </summary>
        public int CPULoad { get; set; }

        public AppServer(NetConnection connection, string name, int cpuLoad)
        {
            Connection = connection;
            Name = name;
            CPULoad = cpuLoad;
        }

    }
}
