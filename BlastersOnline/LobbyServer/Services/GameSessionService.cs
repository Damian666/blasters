using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Game;
using BlastersShared.GameSession;
using BlastersShared.Models;
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
        private ObservableCollection<GameSession> Sessions { get; set; }

        /// <summary>
        /// Sends a specific user a list of currently active sessions in the lobby.
        /// </summary>
        /// <param name="user">The user to send all the sessions to</param>
        public void SendUserSessions(User user)
        {
            var packet = new SessionListInformationPacket(Sessions.ToList());
            ClientNetworkManager.Instance.SendPacket(packet, user.Connection);
        }

        /// <summary>
        /// Sends a specific user a list of currently active sessions in the lobby.
        /// </summary>
        /// <param name="user">The user to send all the sessions to</param>
        public void SendUserUpdatedSession(User user, GameSession session)
        {

            var stubList = new List<GameSession>();
            stubList.Add(session);

            var packet = new SessionListInformationPacket(stubList);
            ClientNetworkManager.Instance.SendPacket(packet, user.Connection);
        }


        public void SendUserOnlineList(User user)
        {

            var nameList = new List<string>();
            foreach (var player in ServiceContainer.Users)
                nameList.Add(player.Value.Name);

            var packet = new NotifyUsersOnlinePacket(nameList);
            ClientNetworkManager.Instance.SendPacket(packet, user.Connection);
        }

        public void SendToUsersOnlineList()
        {
            //TODO: Send to interested parties only (not engaged in a game, not in-store etc)

            foreach (var user in ServiceContainer.Users.Values)
            {
                SendUserOnlineList(user);
            }
        }


        public GameSessionService()
        {
            Sessions = new ObservableCollection<GameSession>();

            Sessions.CollectionChanged += SessionsOnCollectionChanged;

            // Register network callbacks
            PacketService.RegisterPacket<SessionJoinRequestPacket>(ProcessSessionJoinRequest);
            PacketService.RegisterPacket<SessionEndedLobbyPacket>(ProcessSessionEnded);
            PacketService.RegisterPacket<SessionCreateRequestPacket>(ProcessSessionCreate);
            PacketService.RegisterPacket<SessionLeaveRequest>(ProcessSessionLeave);

        }

        private void SessionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            // A LINQ where to find all users who are not in a game
            var idleUsers = ServiceContainer.Users.Values.Where(x => x.CurrentSession == null).ToList();
            // Session we're going to kill off
            var session = (GameSession)notifyCollectionChangedEventArgs.NewItems[0];

            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var user in idleUsers)
                        SendUserUpdatedSession(user, session);
                    break;

                case NotifyCollectionChangedAction.Remove:

                    // The client should parse max ID as preparing to remove
                    session.SessionID = uint.MaxValue;

                    foreach (var user in idleUsers)
                        SendUserUpdatedSession(user, session);
                    break;
            }
        }


        /// <summary>
        /// Adds a user to a given game session is space is available. 
        /// </summary>
        /// <param name="user">The user to add to this game session</param>
        /// <param name="gameSession">The game session this user is to be added to</param>
        public bool AddToSession(User user, GameSession gameSession)
        {

            // Check for reasons joing would be unsuccessful

            // This case was added to accomodate for the change from session.First()
            //   to session.FirstOrDefault() (Default being null)
            if (gameSession == null)
                return false;

            if (gameSession.IsFull)
                return false;

            if (gameSession.Users.Contains(user))
                return false;

            if (user.CurrentSession != null)
                return false;

            if (gameSession.InProgress)
                return false;

            //TODO: When room creation logic is in, comment this back in
            // If noone is in this room, it's being destroyed
            //   if (gameSession.Users.Count == 0)
            //       return false;


            // Add the user if it appears safe
            user.CurrentSession = gameSession;
            gameSession.Users.Add(user);
          


            //TODO; Don't auto start the game; check for wait flags. This will suffice for now, though
            if (gameSession.IsFull)
            {


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

        private void CheckCompletion(GameSession gameSession)
        {

            if (!gameSession.IsFull || gameSession.InProgress)
                return;

            // We wait, and then tell everyone it's OK to enter again

            //Sessions.Remove(gameSession);

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

            var appServerService = (AppServerService)ServiceContainer.GetService(typeof(AppServerService));
            var server = appServerService.GetAvailableServer();

            // Notify the app server
            var appServerNotifyPacket = new NotifySessionBeginAppServerPacket(gameSession);
            ClientNetworkManager.Instance.SendPacket(appServerNotifyPacket, server.Connection);

            var endpointInfo = server.Connection.RemoteEndpoint.ToString();

            //endpointInfo = "99.235.224.52:7798";

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



        private void ProcessSessionLeave(SessionLeaveRequest obj)
        {

            // Retrieve the user
            var user = ServiceContainer.Users[obj.Sender];

            // Get the users session
            var session = user.CurrentSession;

            // Remove this user
            session.Users.Remove(user);

            // If this was the last user, this room is now extinct
            session.Users.CollectionChanged -= UsersOnCollectionChanged;
        }

        private void ProcessSessionCreate(SessionCreateRequestPacket obj)
        {
            // Retrieve the user that wants to enter
            var user = ServiceContainer.Users[obj.Sender];

            // If the user is in a session, they shouldn't be able to make one
            if (user.CurrentSession != null)
            {
                Logger.Instance.Log(Level.Warn, user.Name + " has attempted to create a match but was already in one.");
                return;
            }

            // Generate a new session
            var generatedSession = CreateSession();
            Sessions.Add(generatedSession);

            // Update all the sessions
            foreach (var x in ServiceContainer.Users.Values)
                SendUserSessions(x);

            // Add the user into the session
            AddToSession(user, generatedSession);

        }

        private void UsersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {

            // Any user that isn't in a game but in the lobby is probably interested in this change of events
            // A LINQ where to find all users who are not in a game
            var idleUsers = ServiceContainer.Users.Values.Where(x => x.CurrentSession == null).ToList();

            // Grab the current session
            var user = (User)notifyCollectionChangedEventArgs.NewItems[0];
            var session = user.CurrentSession;

            // Construct a packet with the new user amount
            var packet = new SessionUpdatePacket((byte) session.Users.Count);

            // Determine the event
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    foreach (var idleUser in idleUsers)
                        ClientNetworkManager.Instance.SendPacket(packet, idleUser.Connection);
                    break;

            }

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

            Thread.Sleep(1500);

            // See if we should play again

            gameSession.InProgress = false;

            CheckCompletion(gameSession);

        }

        private void ProcessSessionJoinRequest(SessionJoinRequestPacket obj)
        {

            if (!ServiceContainer.Users.ContainsKey(obj.Sender))
                return;

            // Try and add the user
            var user = ServiceContainer.Users[obj.Sender];
            var session = (from x in Sessions where x.SessionID == obj.SessionID select x);
            var result = AddToSession(user, session.FirstOrDefault());

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

            Thread.Sleep(1500);


            CheckCompletion(session.FirstOrDefault());

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

            // Create the event handler
            session.Users.CollectionChanged += UsersOnCollectionChanged;

            return session;
        }


        public override void PeformUpdate()
        {

        }


        internal void ActivateSession(GameSession demoSession)
        {
            CheckCompletion(demoSession);
        }
    }
}
