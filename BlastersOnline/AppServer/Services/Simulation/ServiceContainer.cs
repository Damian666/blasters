using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppServer.Network;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
using BlastersShared.Network.Packets.AppServer.BlastersShared.Network.Packets.AppServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppServer.Services.Simulation
{
    /// <summary>
    /// A service container manages a bunch of game related services and entity management.   
    /// </summary>
    public class ServiceContainer
    {
        /// <summary>
        /// The simulation state contains information about the current game world.
        /// </summary>
        private SimulationState _simulationState;

        /// <summary>
        /// A read-only copy of the simulation state which can be used to access game state specific things.
        /// </summary>
        public SimulationState SimulationState { get { return _simulationState; } }

        public ServiceContainer(SimulationState simulationState)
        {
            _simulationState = simulationState;
        }


        private List<SimulationService> _services = new List<SimulationService>();

        public void AddService(SimulationService service)
        {
            service.ServiceManager = this;
            service.Initialize();
            _services.Add(service);
        }


        public delegate void EntityEvent(Entity entity);

        public event EntityEvent EntityAdded;
        public event EntityEvent EntityRemoved;

        protected virtual void OnEntityRemoved(Entity entity)
        {
            EntityEvent handler = EntityRemoved;
            if (handler != null) handler(entity);
        }

        protected virtual void OnEntityAdded(Entity entity)
        {
            EntityEvent handler = EntityAdded;
            if (handler != null) handler(entity);
        }


        public Entity RetrieveEntityByID(ulong userID)
        {
            foreach (var entity in Entities)
            {
                if (entity.ID == userID)
                    return entity;
            }

            return null;
        }

        /// <summary>
        /// Adds an entity to the server container, also fires off events for notifications.
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            OnEntityAdded(entity);
        }

        private List<Entity> _toRemove = new List<Entity>(); 

        /// <summary>
        /// Removes an entity from the server container, also fires off an event to notify.
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(Entity entity)
        {
            _toRemove.Add(entity);
            OnEntityRemoved(entity);
        }

        /// <summary>
        /// A list of entities contained in this service system.
        /// </summary>
        public List<Entity> Entities
        {
            get { return _simulationState.Entities; }
        }




        public void UpdateService(double deltaTime)
        {

            foreach (var toRemove in _toRemove)
            {
                Entities.Remove(toRemove);
            }
            _toRemove.Clear();
           

            foreach (var service in _services)
                service.Update(deltaTime);
        }



    }
}
