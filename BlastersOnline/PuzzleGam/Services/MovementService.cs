using System;
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
using BlastersGame;
using BlastersGame.Network;
using BlastersGame.Levels;
using TiledSharp;
using BlastersGame.Components;
using BlastersShared.Services.Sprites;
using C3.XNA;

namespace BlastersGame.Services
{
    /// <summary>
    /// The movement service is response for moving entities along
    /// </summary>
    public class MovementService : Service
    {
        private readonly ulong _idToMonitor;

        private readonly Dictionary<ulong, EntityInterpolator> _entityInterpolators = new Dictionary<ulong, EntityInterpolator>();

        // Timing related info (amount of updates sent per seconds i.e 0.1 is 10FPS )
        const float MovementRate = 0.1f;
        private float _lastReaction;

        public Map Map { get; set; }
        public Dictionary<string, SpriteDescriptor> SpriteDescriptorLookup { get; set; }

        public MovementService(ulong idToMonitor)
        {
            _idToMonitor = idToMonitor;
        }

        public override void Initialize()
        {
            // Hook into networks events
            PacketService.RegisterPacket<MovementRecievedPacket>(MovementRecieved);

            // Query for the players we don't want
            foreach (var entity in ServiceManager.Entities)
            {
                if (entity.ID == _idToMonitor)
                {
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
                }
            }

            if (player != null)
            {
                var transformComponent = (TransformComponent)player.GetComponent(typeof(TransformComponent));

                if (obj.Velocity.X < 0)
                    transformComponent.DirectionalCache = DirectionalCache.Left;
                else if (obj.Velocity.X > 0)
                    transformComponent.DirectionalCache = DirectionalCache.Right;
                else if (obj.Velocity.Y > 0)
                    transformComponent.DirectionalCache = DirectionalCache.Down;
                else if (obj.Velocity.Y < 0)
                    transformComponent.DirectionalCache = DirectionalCache.Up;
            }

            // Retreieve an interpolator
            var interpolator = _entityInterpolators[obj.EntityID];
            interpolator.ResetProgress(obj.Location);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, ServiceManager.Camera.GetTransformation());
            
            foreach (var entity in ServiceManager.Entities)
            {
                // Local players can be moved automatically, then report their status if needed
                var skinComponent = (SkinComponent) entity.GetComponent(typeof (SkinComponent));
                // TODO: Make it so we can get the sprite descriptors from the sprite service.
                var spriteDescriptor = SpriteDescriptorLookup[skinComponent.SpriteDescriptorName];
                var transformComponent = (TransformComponent) entity.GetComponent(typeof (TransformComponent));

                Rectangle bbox = new Rectangle(
                    (int) (transformComponent.LocalPosition.X + spriteDescriptor.BoundingBox.X),
                    (int) (transformComponent.LocalPosition.Y + spriteDescriptor.BoundingBox.Y),
                    spriteDescriptor.BoundingBox.Width,
                    spriteDescriptor.BoundingBox.Height);
           
                spriteBatch.DrawRectangle(bbox, Color.White, 2f);
        


            }

            foreach (TmxLayer layer in Map.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    // TODO: Sucks. Temporary, incomplete code. Needs to get fixed.
                    if (tile.GID == 7)
                    {
                        Rectangle tileRect = new Rectangle(tile.X*32, 35+tile.Y*32, 32, 32);
                        spriteBatch.DrawRectangle(tileRect, Color.Red, 3f);

                    }

                }
            }
            spriteBatch.End();


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
            var skinComponent = (SkinComponent)entity.GetComponent(typeof(SkinComponent));
            // TODO: Make it so we can get the sprite descriptors from the sprite service.
            var spriteDescriptor = SpriteDescriptorLookup[skinComponent.SpriteDescriptorName];
            
            var transformComponent = (TransformComponent)entity.GetComponent(typeof(TransformComponent));
            ServiceManager.Camera.Move(transformComponent.LocalPosition);

            var movementModifierComponent = (MovementModifierComponent)entity.GetComponent(typeof(MovementModifierComponent));

            // Determine the movement bonus multiplier
            float movementBonus = 1.0f;
            if (movementModifierComponent != null)
                movementBonus = movementModifierComponent.Bonus;

            // Apply the multiplier to the velocity and move the position
            Vector2 nextPosition = transformComponent.LocalPosition;
            nextPosition += transformComponent.Velocity * movementBonus;

            // Clamp the x and y so the player won't keep walking offscreen
            float nextX = MathHelper.Clamp(nextPosition.X, 0, 640 - transformComponent.Size.X);
            float nextY = MathHelper.Clamp(nextPosition.Y, 35, 600 - transformComponent.Size.Y);

            // TODO: This is shitty. Needs to be redone.
            if (transformComponent.Velocity.LengthSquared() != 0)
            {
                if (Map != null)
                {
                    foreach (TmxLayer layer in Map.Layers)
                    {
                        foreach (TmxLayerTile tile in layer.Tiles)
                        {
                            // TODO: Sucks. Temporary, incomplete code. Needs to get fixed.
                            if (tile.GID == 7)
                            {
                                Rectangle tileRect = new Rectangle(tile.X * 32, 35 + tile.Y * 32, 32, 32);
                                
                                Rectangle xBBox = new Rectangle(
                                      (int)nextX + spriteDescriptor.BoundingBox.X,
                                      (int)transformComponent.LocalPosition.Y + spriteDescriptor.BoundingBox.Y,
                                      (int)spriteDescriptor.BoundingBox.Width,
                                      (int)spriteDescriptor.BoundingBox.Height);

                                if (tileRect.Intersects(xBBox))
                                {
                                    nextX = transformComponent.LocalPosition.X;
                                }

                                Rectangle yBBox = new Rectangle(
                                      (int)nextX + spriteDescriptor.BoundingBox.X,
                                      (int)nextY + spriteDescriptor.BoundingBox.Y,
                                      (int)spriteDescriptor.BoundingBox.Width,
                                      (int)spriteDescriptor.BoundingBox.Height);

                                if (tileRect.Intersects(yBBox))
                                {
                                    nextY = transformComponent.LocalPosition.Y;
                                }
                            }
                        }
                    }
                }
            }

            transformComponent.LocalPosition = new Vector2(nextX, nextY);
            ServiceManager.Camera.Move(-transformComponent.LocalPosition);

            if (transformComponent.Velocity.X != transformComponent.Velocity.Y)
                if (transformComponent.Velocity.X < 0)
                    transformComponent.DirectionalCache = DirectionalCache.Left;
                else if (transformComponent.Velocity.X > 0)
                    transformComponent.DirectionalCache = DirectionalCache.Right;
                else if (transformComponent.Velocity.Y > 0)
                    transformComponent.DirectionalCache = DirectionalCache.Down;
                else if (transformComponent.Velocity.Y < 0)
                    transformComponent.DirectionalCache = DirectionalCache.Up;

            if ((_lastReaction > MovementRate && transformComponent.Velocity != Vector2.Zero) || transformComponent.Velocity != transformComponent.LastVelocity)
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
                entityInterpolator.Value.PeformInterpolationStep(gameTime, MovementRate);
        }

        public override void HandleInput(InputState inputState)
        {

        }
    }
}
