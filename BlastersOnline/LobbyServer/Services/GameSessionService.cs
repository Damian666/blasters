using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Game;
using BlastersShared.GameSession;
using BlastersShared.Network.Packets;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.ClientLobby;
using BlastersShared.Network.Packets.Lobby;
using BlastersShared.Services;
using LobbyServer.Network;

namespace LobbyServer
{
    /// <summary>
    /// A service for managing game sessions throughout the lobby lifetime
    /// </summary>
    public class GameSessionService : Service
    {

        /// <summary>
        /// A list of sessions currently in-progress or waiting
        /// </summary>
        private List<GameSession> Sessions { get; set; }

        /// <summary>
        /// Sends a specific user a list of currently active sessions in the lobby.
        /// </summary>
        /// <param name="user">The user to send all the sessions to</param>
        public void SendUserSessions(User user)
        {
            var packet = new SessionListInformationPacket(Sessions);
            ClientNetworkManager.Instance.SendPacket(packet, user.Connection);
        }


        /// <summary>
        /// Adds a user to a given game session is space is available. 
        /// </summary>
        /// <param name="user">The user to add to this game session</param>
        /// <param name="gameSession">The game session this user is to be added to</param>
        public bool AddToSession(User user, GameSession gameSession)
        {

            // Check for reasons joing would be unsuccessful

            if (gameSession.IsFull)
                return false;

            if (gameSession.Users.Contains(user))
                return false;

            if (user.CurrentSession != null)
                return false;

            if (gameSession.InProgress)
                return false;

            // Add the user if it appears safe
            gameSession.Users.Add(user);
            user.CurrentSession = gameSession;


            //TODO; Don't auto start the game; check for wait flags. This will suffice for now, though
            if (gameSession.IsFull || true)
            {
                Sessions.Remove(gameSession);

                gameSession.InProgress = true;

                // We generate a secure token for each user
                foreach (var cUser in gameSession.Users)
                {
                    cUser.SecureToken = Guid.NewGuid();
                    //TODO: Take this parameter from the client
                    // We should also validate it's even legal as it comes from the client
                    cUser.SessionConfig = new UserSessionConfig("FemaleSheet1");
                }

#if DEBUG_MOCK
                foreach (var cUser in gameSession.Users)
                    cUser.SecureToken = Guid.Empty;                                    
#endif

                var appServerService = (AppServerService) ServiceContainer.GetService(typeof (AppServerService));
                var server = appServerService.GetAvailableServer();

                // Notify the app server
                var appServerNotifyPacket = new NotifySessionBeginAppServerPacket(gameSession);
                ClientNetworkManager.Instance.SendPacket(appServerNotifyPacket, server.Connection);

                var endpointInfo = server.Connection.RemoteEndpoint.ToString();

                // We generate a secure token for each user
                foreach (var cUser in gameSession.Users)
                {
                    var packet = new SessionBeginNotificationPacket(cUser.SecureToken, endpointInfo,
                                                                    gameSession.SessionID);
                    ClientNetworkManager.Instance.SendPacket(packet, cUser.Connection);
                }


                Logger.Instance.Log(Level.Info, "The match " + gameSession + " is now underway.");
                Logger.Instance.Log(Level.Info, "The simulation is being completed on: " + server.Name);

            }

            else
            {

                // Send information about the session list again to everyone connected
                //TODO: Only send deltas, but it's not a huge deal right now. Just do it this way
                foreach (var x in ServiceContainer.Users.Values)
                    SendUserSessions(x);

            }


            return true;

        }

        public GameSessionService()
        {
            Sessions = new List<GameSession>();
        
            // Register network callbacks
            PacketService.RegisterPacket<SessionJoinRequestPacket>(ProcessSessionJoinRequest);
            PacketService.RegisterPacket<SessionEndedLobbyPacket>(ProcessSessionEnded);

        }

        private void ProcessSessionEnded(SessionEndedLobbyPacket obj)
        {
            var gameSession = new GameSession();

            // Find the item 
            for (int index = 0; index < Sessions.Count; index++)
            {
                gameSession = Sessions[index];
                if (gameSession.SessionID == obj.SessionID)
                {
                    gameSession.InProgress = false;
                    break;
                }
            }

            Logger.Instance.Log(Level.Info, "The match " + gameSession + " has completed a round. ");
            Logger.Instance.Log(Level.Info,
                                "The winner was " + obj.SessionStatistics.Winner.Name + "; lasting " +
                                obj.SessionStatistics.MatchDuration + "s.");

        }

        private void ProcessSessionJoinRequest(SessionJoinRequestPacket obj)
        {
            // Try and add the user
            var user = ServiceContainer.Users[obj.Sender];
            var session = (from x in Sessions where x.SessionID == obj.SessionID select x);
            var result = AddToSession(user, session.First());

            if (result)
            {
                var packet = new SessionJoinResultPacket(SessionJoinResultPacket.SessionJoinResult.Succesful);
                ClientNetworkManager.Instance.SendPacket(packet, obj.Sender);
            }

            else
            {
                var packet = new SessionJoinResultPacket(SessionJoinResultPacket.SessionJoinResult.Failed);
                ClientNetworkManager.Instance.SendPacket(packet, obj.Sender);
            }


        }

        /// <summary>
        /// Creates a game session and adds it to the list of pending game sessions to begin.
        /// </summary>
        public GameSession CreateSession()
        {
            // Create a default deathpatch session
            var session = GameSession.CreateDefaultDeathmatch();
            Sessions.Add(session);

            //TODO: Sync up network states, send connected players a listing of new session stuff

            Logger.Instance.Log(Level.Info, "A match was succesfully created with the name " + session.Name + ", ID: " + session.SessionID);

            return session;
        }


        public override void PeformUpdate()
        {
            
        }

    }
}
