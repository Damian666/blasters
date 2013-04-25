using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppServer.Network;
using BlastersShared;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Services;

namespace AppServer.Services
{
    /// <summary>
    /// A service responsible for communicating with the lobby periodically and answering calls.
    /// </summary>
    public class LobbyCommunicatorService : Service
    {


        public LobbyCommunicatorService()
        {

            // Get a random server name
            string serverName = new Random().NextString(18).ToUpper();
        
            LobbyServerNetworkManager.Instance.Connect();


            // Wait about a second init
            Thread.Sleep(5000);



            var callback = new Action(delegate
                {

                    // Send the 'add me to the cluster, please' packet
                    var packet = new ClusterAddPacket(Global.PrivateKey);
                    LobbyServerNetworkManager.Instance.SendPacket(packet);
                });

            
            Thread.Sleep(2500);

            callback.Invoke();
           

        }





        /// <summary>
        /// Intitiates a handshake with the lobby server and broadcasts its existance.
        /// </summary>
        private void HandshakeWithLobby()
        {
            
        }


        public override void PeformUpdate()
        {
            // Update the network and the like, perform event updates, too
            LobbyServerNetworkManager.Instance.Update();

        }
    }
}
