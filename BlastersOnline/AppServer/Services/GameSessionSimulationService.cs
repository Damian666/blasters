using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServer.Network;
using AppServer.Services.Simulation;
using BlastersShared;
using BlastersShared.GameSession;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.Client;
using BlastersShared.Services;
using BlastersShared.Network.Packets.Lobby;
using Lidgren.Network;

namespace AppServer.Services
{
    /// <summary>
    /// The game session service on an app server simulates games.
    /// The service also acts as a proxy for all active games going on.
    /// </summary>
    public class GameSessionSimulationService : Service
    {
        /// <summary>
        /// The routing table provides a quick lookup of where users are in the simulation.
        /// This way, players can simply send packets to the gateway and have them routed accordingly.
        /// </summary>
        private Dictionary<NetConnection, SimulatedGameSession> _routingTable = new Dictionary<NetConnection, SimulatedGameSession>(); 

        public GameSessionSimulationService()
        {
            PacketService.RegisterPacket<NotifySessionBeginAppServerPacket>(ProccessIncomingSession);
            PacketService.RegisterPacket<NotifyLoadedGamePacket>(ProcessGameLoaded);

            ActiveGameSessions = new List<SimulatedGameSession>();
        }

        private void ProcessGameLoaded(NotifyLoadedGamePacket obj)
        {
            var result = GetUserSession(obj.SecureToken);

            if (result != null)
            {
                // Tell the game simulator they're good to go
                result.VerifyGameLoad(obj);

                // Add the user to the routing table
                _routingTable.Add(obj.Sender, result);

                Logger.Instance.Log(Level.Debug, "Session " + result.Session.SessionID + ":" + " A user has succesfully passed their token.");

                // Assign that user the proper connection
                foreach (var user in result.Session.Users)
                {
                    if (user.SecureToken == obj.SecureToken)
                    {
                        user.Connection = obj.Sender;
                    }
                }
                    
                

            }
        }

        private void ProccessIncomingSession(NotifySessionBeginAppServerPacket obj)
        {
            var simulatedGame = new SimulatedGameSession(obj.Session);
            ActiveGameSessions.Add(simulatedGame);
            _routingTable.Remove(obj.Sender);

            // Log the event that this session begun
            Logger.Instance.Log(Level.Info,  "Session " + simulatedGame.Session.SessionID + ":" + " Started a simulation job on this application server.");
            simulatedGame.SessionEnded += SimulatedGame_SessionEnded;




        }

        private List<SimulatedGameSession> _removeQueue = new List<SimulatedGameSession>();

        private void SimulatedGame_SessionEnded(SimulatedGameSession session, SessionEndStatistics e)
        {
            _removeQueue.Add(session);

            // Kill any event handler
            session.SessionEnded -= SimulatedGame_SessionEnded;

            // Alert the lobby server the match has ended
            var packet = new SessionEndedLobbyPacket(session.Session.SessionID ,e);
            LobbyServerNetworkManager.Instance.SendPacket(packet);

            Logger.Instance.Log(Level.Info, "Session " + session.Session.SessionID + ":" + " Completed a simulation job on this application server.");

            // Remove from the router table and notify game is over
            foreach (var user in session.Session.Users)
            {
                _routingTable.Remove(user.Connection);

                // Send the packet
                var userPacket = new SessionEndedLobbyPacket(session.Session.SessionID, e);
                ClientNetworkManager.Instance.SendPacket(userPacket, user.Connection);

            }
            
                
            

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

            foreach (var toRemove in _removeQueue)
                ActiveGameSessions.Remove(toRemove);


        }

        private SimulatedGameSession GetUserSession(NetConnection connection)
        {
            return _routingTable[connection];
        }

        private SimulatedGameSession GetUserSession(Guid secureToken)
        {
           
            // This is a much slower, brute find
            // Use only if the identity of the user is unknown

            foreach (var simulatedGameSession in ActiveGameSessions)
            {
                foreach (var user in simulatedGameSession.Session.Users)
                {
                    if (user.SecureToken == secureToken)
                        return simulatedGameSession;
                }   
            }

            return null;

        }

    }
}
