using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersGame.Components;
using BlastersShared.Game.Components;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.AppServer.BlastersShared.Network.Packets.AppServer;
using BlastersShared.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlastersGame;
using BlastersGame.Network;

namespace BlastersGame.Services
{
    /// <summary>
    /// The entity sync service is responsbile for creating and removing entities in this particular service container instance.
    /// </summary>
    public class EntitySyncService : Service
    {
        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void HandleInput(InputState inputState)
        {

        }

        public override void Initialize()
        {
            PacketService.RegisterPacket<EntityAddPacket>(HandleEntityAdd);
            PacketService.RegisterPacket<EntityRemovePacket>(HandleEntityRemove);
            PacketService.RegisterPacket<PowerupRecievedPacket>(HandlePowerupObtain);
        }

        private void HandlePowerupObtain(PowerupRecievedPacket obj)
        {
            var playerToPowerup = ServiceManager.RetrieveEntityByID(obj.EntityID);
            var recievType = obj.Powerup.GetType();

            // Check if the player is null
            if (playerToPowerup != null)
            {
                var toPoweurp =
                    (MovementModifierComponent) playerToPowerup.GetComponent(typeof (MovementModifierComponent));
                toPoweurp.Strength += obj.Powerup.Strength;
            }





        }

        private void HandleEntityRemove(EntityRemovePacket entityRemovePacket)
        {

            var entity = ServiceManager.RetrieveEntityByID(entityRemovePacket.EntityID);


            // If an entity was removed, check if it's blow-up-able
            var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

            if (explosiveComponent != null)
            {
                // Get the owner
                var owner = ServiceManager.RetrieveEntityByID(explosiveComponent.OwnerID);
                var ownerBombModifier =
                    (BombCountModifierComponent)owner.GetComponent(typeof(BombCountModifierComponent));
                ownerBombModifier.CurrentBombCount--;
            }


            ServiceManager.RemoveEntityByID(entityRemovePacket.EntityID);


            HandleRemovalBehaviour(entity);


        }

        /// <summary>
        /// This method handles behaviours for when an entity is being removed
        /// </summary>
        /// <param name="entity"></param>
        private void HandleRemovalBehaviour(Entity entity)
        {

            // If thie entity had an eplosiv
            var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));
            var transformComponent = (TransformComponent)entity.GetComponent(typeof(TransformComponent));

            // Spawn an explosion
            if (explosiveComponent != null)
            {
                var entitesToAdd = new List<Entity>();

                // They're returned in order of up, down, left, right
                var rectangles = DetonationHelper.GetBlastRadiusFrom(ServiceManager.Map.TmxMap, entity);
                var up = Math.Ceiling((decimal)(rectangles[0].Height / 32));
                var down = Math.Ceiling((decimal)(rectangles[1].Height / 32));
                var left = Math.Ceiling((decimal)(rectangles[2].Width / 32));
                var right = Math.Ceiling((decimal)(rectangles[3].Width / 32));

                // Do damage
                var list = DetonationHelper.GetDetonatedTiles(ServiceManager.Map.TmxMap, entity,rectangles);

                foreach (var tile in list)
                {
                    var location = new Vector2(tile.X * 32, tile.Y * 32);
                    var n = EntityFactory.CreateBreakingBlock(location, "StandardBlock");
                    ServiceManager.AddEntity(n);
                }

                    
                


                for (int i = 0; i < up - 2; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2(0, (i * 32 + 32)), ExplosiveType.Up);
                    entitesToAdd.Add(x);
                }

                for (int i = 0; i < down - 2; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2(0, (i * 32 + 32)), ExplosiveType.Down);
                    entitesToAdd.Add(x);
                }



                for (int i = 0; i < right - 2; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2((i * 32 + 32), 0), ExplosiveType.Right);
                    entitesToAdd.Add(x);
                }




                for (int i = 0; i < left - 2; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2((i * 32 + 32), 0), ExplosiveType.Left);
                    entitesToAdd.Add(x);
                }




                Entity topEdge = null;
                if (up != 1)
                    topEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2(0, (float)(up * 32) - 32), ExplosiveType.UpE);




                Entity bottomEdge = null;
                if (down != 1)
                    bottomEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2(0, (float)(down * 32) - 32), ExplosiveType.DownE);

                Entity leftEdge = null;

                if (left != 1)
                    leftEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2((float)(left * 32) - 32, 0), ExplosiveType.LeftE);

                Entity rightEdge = null;
                if (right != 1)
                    rightEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2((float)(right * 32) - 32, 0), ExplosiveType.RightE);

                if (topEdge != null)
                    entitesToAdd.Add(topEdge);


                if (bottomEdge != null)
                    entitesToAdd.Add(bottomEdge);

                if (leftEdge != null)
                    entitesToAdd.Add(leftEdge);

                if(rightEdge != null)
                    entitesToAdd.Add(rightEdge);

                var y = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition, ExplosiveType.Center);
                entitesToAdd.Add(y);


                foreach (var add in entitesToAdd)
                    ServiceManager.AddEntity(add);



            }




        }


        /// <summary>
        /// Responsible for recieving and piping entities into the game network.
        /// </summary>
        /// <param name="entityAddPacket"></param>
        private void HandleEntityAdd(EntityAddPacket entityAddPacket)
        {
            ServiceManager.AddEntity(entityAddPacket.Entity);
        }


    }
}
