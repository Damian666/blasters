using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame.Services
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

        public ServiceContainer(SimulationState simulationState, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _simulationState = simulationState;
        }

        private List<Service> _services = new List<Service>();
        private ContentManager _contentManager;

        public void AddService(Service service)
        {
            service.ServiceManager = this;
            service.ContentManager = _contentManager;
            service.Initialize();
            _services.Add(service);
        }

        /// <summary>
        /// A list of entities contained in this service system.
        /// </summary>
        public List<Entity> Entities
        {
            get { return _simulationState.Entities; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw everything
            foreach (var service in _services)
                service.Draw(spriteBatch);                
            
        }


    }
}
