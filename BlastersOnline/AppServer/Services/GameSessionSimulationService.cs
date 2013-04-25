using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServer.Network;
using AppServer.Services.Simulation;
using BlastersShared.GameSession;
using BlastersShared.Services;
using BlastersShared.Network.Packets.Lobby;

namespace AppServer.Services
{
    /// <summary>
    /// The game session service on an app server simulates games.
    /// The service also acts as a proxy for all active games going on.
    /// </summary>
    public class GameSessionSimulationService : Service
    {

        public GameSessionSimulationService()
        {
            PacketService.RegisterPacket<NotifySessionBeginAppServerPacket>(ProccessIncomingSession);
        }

        private void ProccessIncomingSession(NotifySessionBeginAppServerPacket obj)
        {
            var simulatedGame = new SimulatedGameSession(obj.Session);
            ActiveGameSessions.Add(simulatedGame);
        }

        /// <summary>
        /// The active simulations going on through this service
        /// </summary>
        public List<SimulatedGameSession> ActiveGameSessions { get; set; }

        public override void PeformUpdate()
        {
            // Update the network and the like, perform event updates, too
            ClientNetworkManager.Instance.Update();

            // Simulate
            foreach (var simulatedGameSession in ActiveGameSessions)
                simulatedGameSession.PerformUpdate();
        }


    }
}
