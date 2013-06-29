using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppServer.Network;
using AppServer.Services.Simulation.Services;
using BlastersShared;
using BlastersShared.Game;
using BlastersShared.Game.Components;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using BlastersShared.GameSession;
using BlastersShared.Network.Packets;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.AppServer.BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.Client;
using Microsoft.Xna.Framework;

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
        /// The total amount of time that had happened then
        /// </summary>
        private double _totalThen = 0f;

        /// <summary>
        /// The simulation state is the state of the game currently being played.
        /// </summary>
        private SimulationState _simulationState;

        private ServiceContainer _serviceContainer;

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

        public void HandleBombRequest(RequestPlaceBombPacket request)
        {

            var sender = RetrieveSender(request);

            if (sender == null)
                return;

            var transformComponent = (TransformComponent)sender.GetComponent(typeof(TransformComponent));
            var bombModifier = (BombCountModifierComponent)sender.GetComponent(typeof(BombCountModifierComponent));

            if (bombModifier.Amount == bombModifier.CurrentBombCount)
                return;
            else
                bombModifier.CurrentBombCount++;

            Vector2 location = transformComponent.LastLocalPosition;
            location = request.CurrentPosition;
            location += transformComponent.Size * new Vector2(0.5f, 0.875f);
            // TODO: Remove hardcoded 32s

            location = new Vector2(32 * (int)(location.X / 32), 32 * (int)(location.Y / 32));


            var bomb = EntityFactory.CreateBomb(location, sender);
            AddEntity(bomb);
        }

        private void AddEntity(Entity entity)
        {
            // Add entity to the list
            _simulationState.Entities.Add(entity);
            var packet = new EntityAddPacket(entity);


            foreach (var player in _simulationState.Entities)
            {
                // Grab the connection
                var playerComponent = (PlayerComponent)player.GetComponent(typeof(PlayerComponent));

                if (playerComponent == null)
                    continue;

                var connection = playerComponent.Connection;


                ClientNetworkManager.Instance.SendPacket(packet, connection);
            }
        }

        private void NoSyncAddEntity(Entity entity)
        {
            var packet = new EntityAddPacket(entity);


            foreach (var player in _simulationState.Entities)
            {
                // Grab the connection
                var playerComponent = (PlayerComponent)player.GetComponent(typeof(PlayerComponent));

                if (playerComponent == null)
                    continue;

                var connection = playerComponent.Connection;


                ClientNetworkManager.Instance.SendPacket(packet, connection);
            }
        }


        /// <summary>
        /// Handles movement for a particular instance
        /// </summary>
        public void HandleMovement(NotifyMovementPacket notifyMovementPacket)
        {
            var sender = RetrieveSender(notifyMovementPacket);

            if (sender == null)
                return;

            var transformComponent = (TransformComponent)sender.GetComponent(typeof(TransformComponent));


            transformComponent.LastLocalPosition = transformComponent.LocalPosition;
            transformComponent.LocalPosition = notifyMovementPacket.Location;
            transformComponent.ServerPosition = notifyMovementPacket.Location;

            foreach (var player in _simulationState.Entities)
            {

                // Grab the connection
                var playerComponent = (PlayerComponent)player.GetComponent(typeof(PlayerComponent));

                if (playerComponent == null)
                    continue;


                var connection = playerComponent.Connection;

                if (connection != notifyMovementPacket.Sender)
                {
                    var broadcastPacket = new MovementRecievedPacket(notifyMovementPacket.Velocity,
                                                                     notifyMovementPacket.Location, sender.ID);

                    ClientNetworkManager.Instance.SendPacket(broadcastPacket, connection);

                }


                foreach (var powerup in _simulationState.Entities)
                {
                    if (powerup.HasComponent(typeof (PowerUpCollectionComponent)))
                    {

                        var transformPoweurp = (TransformComponent) powerup.GetComponent(typeof(TransformComponent));

                        Rectangle playerBox = new Rectangle((int) (transformComponent.LocalPosition.X + 13), (int) (transformComponent.LocalPosition.Y + 45), 24, 20);
                        Rectangle powerupBox = new Rectangle((int) transformPoweurp.LocalPosition.X, (int) transformPoweurp.LocalPosition.Y, 32, 32);


                        if (playerBox.Intersects(powerupBox))
                        {
                            // Kill thy entity
                            _serviceContainer.RemoveEntity(powerup);

                            // Increase bombs
                            var power = (PowerUpCollectionComponent) powerup.GetComponent(typeof (PowerUpCollectionComponent));
                            var playerPower = (BombCountModifierComponent) player.GetComponent(typeof(BombCountModifierComponent));

                            playerPower.Strength += power.PowerUps[0].Strength;


                        }


                    }

                }


            }



            // Now that you've moved, should check to see if any poweurps need to be awarded

   


        }

        private Entity RetrieveSender(Packet packet)
        {
            Entity sender = null;
            foreach (var entity in _simulationState.Entities)
            {
                var playerComponent = (PlayerComponent)entity.GetComponent(typeof(PlayerComponent));

                if (playerComponent == null)
                    continue;

                var connection = playerComponent.Connection;



                if (connection == packet.Sender)
                    sender = entity;
            }
            return sender;
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
                var comp = ((PlayerComponent)user.GetComponent(typeof(PlayerComponent)));

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

            _serviceContainer = new ServiceContainer(_simulationState);

            var detonationService = new DetonationService();
            var powerupService = new PowerupService();

            // Add services we might need
            _serviceContainer.AddService(detonationService);
            _serviceContainer.AddService(powerupService);

            _serviceContainer.EntityRemoved += _serviceContainer_EntityRemoved;
            _serviceContainer.EntityAdded += _serviceContainer_EntityAdded;

            // Start the game timer immediately
            //TODO: Don't start the timer until everyone is loaded
            _timer.Start();

        }

        void _serviceContainer_EntityAdded(Entity entity)
        {
            NoSyncAddEntity(entity);
        }

        void _serviceContainer_EntityRemoved(Entity entity)
        {
            var packet = new EntityRemovePacket(entity.ID);


            foreach (var player in _simulationState.Entities)
            {
                // Grab the connection
                var playerComponent = (PlayerComponent)player.GetComponent(typeof(PlayerComponent));

                if (playerComponent == null)
                    continue;

                var connection = playerComponent.Connection;


                ClientNetworkManager.Instance.SendPacket(packet, connection);
            }

        }

        /// <summary>
        /// Performs updates on the simulation state
        /// </summary>
        public void PerformUpdate()
        {

            double deltaTime = (_timer.Elapsed.TotalSeconds - _totalThen);
            _totalThen = _timer.Elapsed.TotalSeconds;

            // Check to see if the timer is expired
            if (_timer.Elapsed.TotalSeconds > Session.Configuration.MaxPlayers * 125)
                TerminateSession();

            // If the game has started, run the simulation
            if (_timer.IsRunning)
                _serviceContainer.UpdateService(deltaTime);

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
