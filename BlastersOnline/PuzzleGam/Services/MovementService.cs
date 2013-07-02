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
using BlastersGame;
using BlastersGame.Network;
using BlastersGame.Levels;
using TiledSharp;
using BlastersGame.Components;
using BlastersShared.Services.Sprites;
using C3.XNA;
using BlastersShared.Game.Components.PowerUp;

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
        private Vector2 lastTransformVector = Vector2.Zero;

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

                if (transformComponent.Velocity.X != transformComponent.Velocity.Y)
                {
                    if (obj.Velocity.X < 0)
                        transformComponent.DirectionalCache = DirectionalCache.Left;

                    if (obj.Velocity.X > 0)
                        transformComponent.DirectionalCache = DirectionalCache.Right;

                    if (obj.Velocity.Y > 0)
                        transformComponent.DirectionalCache = DirectionalCache.Down;

                    if (obj.Velocity.Y < 0)
                        transformComponent.DirectionalCache = DirectionalCache.Up;
                }
            }

            // Retreieve an interpolator
            var interpolator = _entityInterpolators[obj.EntityID];
            interpolator.ResetProgress(obj.Location);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //return;

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

            foreach (TmxLayer layer in ServiceManager.Map.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    

                    // TODO: Sucks. Temporary, incomplete code. Needs to get fixed.
                    if (tile.GID == 7)
                    {
                        Rectangle tileRect = new Rectangle(tile.X * 32, tile.Y * 32, 32, 32);
                        spriteBatch.DrawRectangle(tileRect, Color.Red, 2f);
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
            var playerTransform = (TransformComponent)entity.GetComponent(typeof(TransformComponent));
            var playerMovementModifier = (MovementModifierComponent)entity.GetComponent(typeof(MovementModifierComponent));

            // Get the skin information so we can calculate bounding box collisions
            var playerSkin = (SkinComponent)entity.GetComponent(typeof(SkinComponent));
            var playerDescriptor = SpriteDescriptorLookup[playerSkin.SpriteDescriptorName];
            
            // Move the camera
            ServiceManager.Camera.Move(-lastTransformVector);

            // Determine the movement bonus multiplier
            float movementBonus = 1.0f;
            if (playerMovementModifier != null)
                movementBonus = playerMovementModifier.Bonus;

            // Apply the multiplier to the velocity and move the position
            Vector2 nextPosition = playerTransform.LocalPosition;
            nextPosition += playerTransform.Velocity * movementBonus;

            // Clamp the x and y so the player won't keep walking offscreen
            float nextX = MathHelper.Clamp(nextPosition.X + playerDescriptor.BoundingBox.X, 0, ServiceManager.Map.WorldSizePixels.X / 2 - playerDescriptor.BoundingBox.Width);
            float nextY = MathHelper.Clamp(nextPosition.Y + playerDescriptor.BoundingBox.Y, 0, ServiceManager.Map.WorldSizePixels.Y / 2 - playerDescriptor.BoundingBox.Height);

            // TODO: This is shitty. Needs to be redone.
            if (playerTransform.Velocity.LengthSquared() != 0)
            {
                // Check Entity Collision
                if (ServiceManager.Entities != null && ServiceManager.Entities.Count() > 0)
                {
                    foreach (Entity e in ServiceManager.Entities)
                    {
                        // TODO: Bugfix! Currently 2 arrow keys are needed to get off of bombs. Fix it bitches.
                        if (e != entity && e.HasComponent(typeof(ExplosiveComponent)))
                        {
                            TransformComponent bombTransform = (TransformComponent)e.GetComponent(typeof(TransformComponent));
                            SkinComponent bombSkin = (SkinComponent)e.GetComponent(typeof(SkinComponent));
                            SpriteDescriptor bombDescriptor = SpriteDescriptorLookup[bombSkin.SpriteDescriptorName];

                            Vector2 playerOrigin = playerTransform.LocalPosition + new Vector2(playerDescriptor.BoundingBox.X, playerDescriptor.BoundingBox.Y) + new Vector2(playerDescriptor.BoundingBox.Width, playerDescriptor.BoundingBox.Height) / 2;
                            Vector2 bombOrigin = bombTransform.LocalPosition + new Vector2(bombDescriptor.BoundingBox.X, bombDescriptor.BoundingBox.Y) + new Vector2(bombDescriptor.BoundingBox.Width, bombDescriptor.BoundingBox.Height) / 2;

                            Vector2 relativePosition = playerOrigin - bombOrigin;
                            if ((Math.Sign(playerTransform.Velocity.X) != 0 && Math.Sign(relativePosition.X) != Math.Sign(playerTransform.Velocity.X)) || (Math.Sign(playerTransform.Velocity.Y) != 0 && Math.Sign(relativePosition.Y) != Math.Sign(playerTransform.Velocity.Y)))
                            {
                                Rectangle bombRect = new Rectangle(
                                    (int)bombTransform.LocalPosition.X + bombDescriptor.BoundingBox.X,
                                    (int)bombTransform.LocalPosition.Y + bombDescriptor.BoundingBox.Y,
                                    (int)bombDescriptor.BoundingBox.Width,
                                    (int)bombDescriptor.BoundingBox.Height);

                                Rectangle xBBox = new Rectangle((int)nextX,
                                      (int)playerTransform.LocalPosition.Y + playerDescriptor.BoundingBox.Y,
                                      (int)playerDescriptor.BoundingBox.Width,
                                      (int)playerDescriptor.BoundingBox.Height);

                                if (bombRect.Intersects(xBBox))
                                {
                                    nextX = playerTransform.LocalPosition.X + playerDescriptor.BoundingBox.X;
                                }

                                Rectangle yBBox = new Rectangle((int)nextX, (int)nextY,
                                      (int)playerDescriptor.BoundingBox.Width,
                                      (int)playerDescriptor.BoundingBox.Height);

                                if (bombRect.Intersects(yBBox))
                                {
                                    nextY = playerTransform.LocalPosition.Y + playerDescriptor.BoundingBox.Y;
                                }
                            }
                        }
                    }
                }

                // Check Tile Collision
                if (ServiceManager.Map != null)
                {
                    foreach (TmxLayer layer in ServiceManager.Map.Layers)
                    {
                        foreach (TmxLayerTile tile in layer.Tiles)
                        {

                            var set = (TmxTileset) ServiceManager.Map.Tilesets[0];
                            PropertyDict dict;

                            set.Tiles.TryGetValue((int) tile.GID, out dict);


                            // TODO: Sucks. Temporary, incomplete code. Needs to get fixed.
                            if (dict != null && dict.ContainsKey("blocked"))
                            {
                                Rectangle tileRect = new Rectangle(tile.X * 32, tile.Y * 32, 32, 32);

                                Rectangle xBBox = new Rectangle((int)nextX,
                                      (int)playerTransform.LocalPosition.Y + playerDescriptor.BoundingBox.Y,
                                      (int)playerDescriptor.BoundingBox.Width,
                                      (int)playerDescriptor.BoundingBox.Height);

                                if (tileRect.Intersects(xBBox))
                                {
                                    nextX = playerTransform.LocalPosition.X + playerDescriptor.BoundingBox.X;
                                }

                                Rectangle yBBox = new Rectangle((int)nextX, (int)nextY,
                                      (int)playerDescriptor.BoundingBox.Width,
                                      (int)playerDescriptor.BoundingBox.Height);

                                if (tileRect.Intersects(yBBox))
                                {
                                    nextY = playerTransform.LocalPosition.Y + playerDescriptor.BoundingBox.Y;
                                }
                            }
                        }
                    }
                }
            }

            playerTransform.LocalPosition = new Vector2(nextX - playerDescriptor.BoundingBox.X, nextY - playerDescriptor.BoundingBox.Y);

            float transformX = playerTransform.LocalPosition.X;
            float transformY = playerTransform.LocalPosition.Y;

            float offsetX = playerDescriptor.BoundingBox.Left + playerDescriptor.BoundingBox.Width / 2;
            float offsetY = playerDescriptor.BoundingBox.Bottom;

            transformX = Math.Max(transformX - 319 + offsetX, 0);
            transformX = Math.Min(transformX, ServiceManager.Map.WorldSizePixels.X / 2 - 638);
            transformY = Math.Max(transformY - 283 + offsetY, 0);
            transformY = Math.Min(transformY, ServiceManager.Map.WorldSizePixels.Y / 2 - 566);
            
            // Move the camera back
            lastTransformVector = new Vector2(transformX, transformY);
            ServiceManager.Camera.Move(lastTransformVector);

            if (playerTransform.Velocity.X != playerTransform.Velocity.Y)
            {
                if (playerTransform.Velocity.X < 0)
                    playerTransform.DirectionalCache = DirectionalCache.Left;
                
                if (playerTransform.Velocity.X > 0)
                    playerTransform.DirectionalCache = DirectionalCache.Right;
                
                if (playerTransform.Velocity.Y > 0)
                    playerTransform.DirectionalCache = DirectionalCache.Down;
                
                if (playerTransform.Velocity.Y < 0)
                    playerTransform.DirectionalCache = DirectionalCache.Up;
            }

            var directionalChange = (playerTransform.Velocity != playerTransform.LastVelocity &&
                                     playerTransform.Velocity != Vector2.Zero);
            directionalChange = false;

            if ((_lastReaction > MovementRate && playerTransform.Velocity != Vector2.Zero) ||  directionalChange)
            {
                // Alert the server out this change in events if needed
                var packet = new NotifyMovementPacket(playerTransform.Velocity, playerTransform.LocalPosition);
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
