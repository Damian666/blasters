﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.GameSession;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Services;
using LobbyServer.Network;

namespace LobbyServer
{
    /// <summary>
    /// Manages the application services that can server Blasters Online games, used to 
    /// </summary>
    public class AppServerService : Service
    {

        /// <summary>
        /// A list of application servers currently connected to this service.
        /// </summary>
        private List<AppServer.AppServer> ApplicationServers { get; set; }

        public AppServerService()
        {
            ApplicationServers = new List<AppServer.AppServer>();

            RegisterNetworkCallbacks();
        }


        public override void PeformUpdate()
        {
            ClientNetworkManager.Instance.Update();
        }

        private void RegisterNetworkCallbacks()
        {



            PacketService.RegisterPacket<ClusterAddPacket>(ProcessClusterAddRequest);
        }

        private void ProcessClusterAddRequest(ClusterAddPacket obj)
        {
            var key = obj.PrivatePassword;

            // If the keys match
            if (key == Global.PrivateKey)
            {
                var appServer = new AppServer.AppServer(obj.Sender, "Ares", 0);            
                ApplicationServers.Add(appServer);
                Lobby.PrintLine(ConsoleColor.Green, string.Format("The application server {0} has joined the cluster.", appServer.Name));
            }
            else
            {
                obj.Sender.Disconnect("INVALID TOKEN");
            }
      

        }

        /// <summary>
        /// Proccesses a <see cref="GameSession"/> and sends it to the approtiate application server.
        /// </summary>
        public void ProcessGameSessionRequest(GameSession gameSession)
        {

            
        }


        public AppServer.AppServer GetAvailableServer()
        {
            return ApplicationServers[0];
        }

  
    }
}
