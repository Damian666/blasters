using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BlastersGame;
using BlastersGame.Levels;

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

        public ServiceContainer(SimulationState simulationState, ContentManager contentManager, GraphicsDevice device)
        {
            _contentManager = contentManager;
            _simulationState = simulationState;
            GraphicsDevice = device;
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


        public GraphicsDevice GraphicsDevice { get; set; }

        public Camera2D Camera { get; set; }
        public Map Map { get; set; }

        private List<Service> _services = new List<Service>();
        private ContentManager _contentManager;

        public void AddService(Service service)
        {
            service.ServiceManager = this;
            service.ContentManager = _contentManager;
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


        private List<Entity> _toRemove = new List<Entity>();

        /// <summary>
        /// Removes an entity from the server container, also fires off an event to notify.
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(Entity entity)
        {
            _toRemove.Add(entity);
        }

        public void RemoveEntityByID(ulong entityID)
        {
            foreach (var entity in Entities)
            {
                if (entity.ID == entityID)
                {
                    _toRemove.Add(entity);
                    return;
                }
            }

            throw new KeyNotFoundException("The given entity was not found on the client. Double request sent? ");
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

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw everything
            foreach (var service in _services)
                service.Draw(spriteBatch);

        }

        public void UpdateService(GameTime gameTime)
        {
            foreach (var service in _services)
                service.Update(gameTime);
        }

        public void UpdateInput(InputState inputState)
        {

            foreach (var toRemove in _toRemove)
            {
                Entities.Remove(toRemove);
                OnEntityRemoved(toRemove);
            }
            _toRemove.Clear();

            foreach (var service in _services)
                service.HandleInput(inputState);
        }


    }
}
