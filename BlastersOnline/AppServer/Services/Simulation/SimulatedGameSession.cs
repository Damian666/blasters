using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppServer.Network;
using BlastersShared;
using BlastersShared.Game;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using BlastersShared.GameSession;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.Client;

namespace AppServer.Services.Simulation
{
    /// <summary>
    /// A simulated game session that is being completed on this application server.
    /// </summary>
    public class SimulatedGameSession
    {

        /// <summary>
        /// The game session is being simulated.
        /// </summary>
        public GameSession Session { get; set; }

        // A list of tokens still waiting to be loaded
        private readonly List<Guid> _secureTokensWaiting = new List<Guid>();

        // A stopwatch is useful for timing the match
        Stopwatch _timer = new Stopwatch();

        /// <summary>
        /// The simulation state is the state of the game currently being played.
        /// </summary>
        private SimulationState _simulationState;

        // Events
        public delegate void SessionEndedHandler(SimulatedGameSession session, SessionEndStatistics e);
        public event SessionEndedHandler SessionEnded;
        protected virtual void OnSessionEnded(SimulatedGameSession session, SessionEndStatistics e)
        {
            SessionEndedHandler handler = SessionEnded;
            if (handler != null) handler(session, e);
        }

        public SimulatedGameSession(GameSession session)
        {
            Session = session;

            

            SetupSimulation();

        }

        /// <summary>
        /// Handles movement for a particular instance
        /// </summary>
        public void HandleMovement(NotifyMovementPacket notifyMovementPacket)
        {

            Entity sender = null;
            foreach (var entity in _simulationState.Entities)
            {
                var playerComponent = (PlayerComponent)entity.GetComponent(typeof(PlayerComponent));
                var connection = playerComponent.Connection;

                if (connection == notifyMovementPacket.Sender)
                    sender = entity;
            }

            foreach (var player in _simulationState.Entities)
            {

                // Grab the connection
                var playerComponent = (PlayerComponent)player.GetComponent(typeof(PlayerComponent));
                var connection = playerComponent.Connection;

                if (connection != notifyMovementPacket.Sender)
                {
                    var broadcastPacket = new MovementRecievedPacket(notifyMovementPacket.Velocity,
                                                                     notifyMovementPacket.Location, sender.ID);

                    ClientNetworkManager.Instance.SendPacket(broadcastPacket, connection); 

                }


            }

        }


        /// <summary>
        /// Verifies a user is okay to enter the game and loading has been complete.
        /// </summary>
        /// <returns></returns>
        public bool VerifyGameLoad(NotifyLoadedGamePacket obj)
        {

            if (_secureTokensWaiting.Contains(obj.SecureToken))
            {
                // Verify this player has loaded the game
                _secureTokensWaiting.Remove(obj.SecureToken);

                ulong id = FindUser(obj);

                // Once a player has loaded, it's okay to send them them the game state
                var packet = new SessionSendSimulationStatePacket(_simulationState, id);
                ClientNetworkManager.Instance.SendPacket(packet, obj.Sender);

                // The minute we know this entity is good, use it

                return true;
            }

            return false;

        }


        private ulong FindUser(NotifyLoadedGamePacket obj)
        {

            foreach (var user in _simulationState.Entities)
            {
                var comp = ((PlayerComponent) user.GetComponent(typeof (PlayerComponent)));

                if (comp.SecureToken == obj.SecureToken)
                {
                    comp.Connection = obj.Sender;
                    return user.ID;
                }
            }

            throw new Exception("A user with the given secure token key could not be found. Sync issue?");
        }

        /// <summary>
        /// Sets the simulation up by configurating parameters and the like
        /// </summary>
        private void SetupSimulation()
        {

            // Add everyones token
            foreach (var user in Session.Users)
                _secureTokensWaiting.Add(user.SecureToken);

            // The state can be generated based on the session information passed down
            _simulationState = SimulationStateFactory.CreateSimulationState(Session);


            // Start the game timer immediately
            //TODO: Don't start the timer until everyone is loaded
            _timer.Start();

        }


        /// <summary>
        /// Performs updates on the simulation state
        /// </summary>
        public void PerformUpdate()
        {
            // Check to see if the timer is expired

            if (_timer.Elapsed.TotalSeconds > Session.Configuration.MaxPlayers * 25)
                TerminateSession();

        }

        /// <summary>
        ///  A function for cleaning up the simulation.
        /// Used for calculating a winner as well.
        /// </summary>
        private void TerminateSession()
        {

            //TODO: Implement a solver for finding out the winner of the match effectively
            // In most cases, this is the last player standing but not always

            // Let subscribers know this game is finished
            var result = new SessionEndStatistics(Session.Users[0], _timer.Elapsed.TotalSeconds);
            OnSessionEnded(this, result);


        }


    }
}
