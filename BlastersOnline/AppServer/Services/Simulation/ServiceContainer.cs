using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
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

        public ServiceContainer(SimulationState simulationState)
        {
            _simulationState = simulationState;
        }


        private List<Service> _services = new List<Service>();

        public void AddService(Service service)
        {
            service.ServiceManager = this;
            service.Initialize();
            _services.Add(service);
        }

        public delegate void EntityAddedDelegate(Entity entity);
        public event EntityAddedDelegate EntityAdded;

        protected virtual void OnEntityAdded(Entity entity)
        {
            EntityAddedDelegate handler = EntityAdded;
            if (handler != null) handler(entity);
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

        /// <summary>
        /// A list of entities contained in this service system.
        /// </summary>
        public List<Entity> Entities
        {
            get { return _simulationState.Entities; }
        }




        public void UpdateService(float deltaTime)
        {
            foreach (var service in _services)
                service.Update(deltaTime);
        }



    }
}
