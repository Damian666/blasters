﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersGame.Services.Movement;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleGame;
using PuzzleGame.Network;

namespace BlastersGame.Services
{
    /// <summary>
    /// The movement service is response for moving entities along
    /// </summary>
    public class MovementService : Service
    {

        private readonly ulong _idToMonitor;
        private Entity _player = null;

        private Dictionary<ulong, EntityInterpolator> _entityInterpolators = new Dictionary<ulong, EntityInterpolator>();

        // Timing related info (amount of updates sent per seconds i.e 0.1 is 10FPS )
        const float MOVEMENT_RATE = 0.1f;
        private float _lastReaction = 0f;

        public MovementService(ulong idToMonitor)
        {
            _idToMonitor = idToMonitor;
        }

        public override void Initialize()
        {

            // Hook into networks events
            PacketService.RegisterPacket<MovementRecievedPacket>(MovementRecieved);

            // Query for the player we want

            foreach (var entity in ServiceManager.Entities)
            {
                if (entity.ID == _idToMonitor)
                {
                    _player = entity;
                    continue;
                }

                var transformComponent = (TransformComponent)entity.GetComponent(typeof(TransformComponent));
                var interpolator = new EntityInterpolator(transformComponent);
                _entityInterpolators.Add(entity.ID, interpolator);
            }


        }

        private void MovementRecieved(MovementRecievedPacket obj)
        {
            Entity player = null;

            foreach (var entity in ServiceManager.Entities)
            {
                if (entity.ID == obj.EntityID)
                {
                    player = entity;
                    continue;
                }
            }

            var transformComponent = (TransformComponent)player.GetComponent(typeof(TransformComponent));

            if(obj.Velocity.X < 0)
                transformComponent.DirectionalCache = DirectionalCache.Left;
            else if(obj.Velocity.X > 0)
                transformComponent.DirectionalCache = DirectionalCache.Right;
            else if(obj.Velocity.Y > 0)
                transformComponent.DirectionalCache = DirectionalCache.Down;
            else if(obj.Velocity.Y < 0)
                transformComponent.DirectionalCache = DirectionalCache.Up;
            


            // Retreieve an interpolator
            var interpolator = _entityInterpolators[obj.EntityID];
            interpolator.ResetProgress(obj.Location);

        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            // Do nothing
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var entity in ServiceManager.Entities)
            {

                // Local and remote entities are treated differently
                if (entity.ID == _idToMonitor)
                    ProcessLocalPlayer(entity, gameTime);
                else
                    ProcessRemoteEntity(entity, gameTime);



            }

        }

        private void ProcessLocalPlayer(Entity entity, GameTime gameTime)
        {
            // Local players can be moved automatically, then report their status if needed
            var transformComponent = (TransformComponent)entity.GetComponent(typeof(TransformComponent));
            transformComponent.LocalPosition += transformComponent.Velocity;


            if (transformComponent.Velocity.X < 0)
                transformComponent.DirectionalCache = DirectionalCache.Left;
            else if (transformComponent.Velocity.X > 0)
                transformComponent.DirectionalCache = DirectionalCache.Right;
            else if (transformComponent.Velocity.Y > 0)
                transformComponent.DirectionalCache = DirectionalCache.Down;
            else if (transformComponent.Velocity.Y < 0)
                transformComponent.DirectionalCache = DirectionalCache.Up;


            if ((_lastReaction > MOVEMENT_RATE && transformComponent.Velocity != Vector2.Zero) || transformComponent.Velocity != transformComponent.LastVelocity)
            {

                // Alert the server out this change in events if needed
                var packet = new NotifyMovementPacket(transformComponent.Velocity, transformComponent.LocalPosition);
                NetworkManager.Instance.SendPacket(packet);

                // Reset reaction timer
                _lastReaction = 0f;

            }

            // Increment reaction timer
            _lastReaction += (float)gameTime.ElapsedGameTime.TotalSeconds;


        }

        private void ProcessRemoteEntity(Entity entity, GameTime gameTime)
        {
            // Interpolate
            foreach (var entityInterpolator in _entityInterpolators)
                entityInterpolator.Value.PeformInterpolationStep(gameTime, MOVEMENT_RATE);

        }

        public override void HandleInput(InputState inputState)
        {

        }






    }
}
