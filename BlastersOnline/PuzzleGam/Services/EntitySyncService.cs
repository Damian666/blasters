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
        }

        private void HandleEntityRemove(EntityRemovePacket entityRemovePacket)
        {

            var entity = ServiceManager.RetrieveEntityByID(entityRemovePacket.EntityID);


            // If an entity was removed, check if it's blow-up-able
            var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

            // Get the owner
            var owner = ServiceManager.RetrieveEntityByID(explosiveComponent.OwnerID);
            var ownerBombModifier =
                (BombCountModifierComponent)owner.GetComponent(typeof(BombCountModifierComponent));
            ownerBombModifier.CurrentBombCount--;



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
                var rectangles = DetonationHelper.GetBlastRadiusFrom(entity);
                var up = Math.Ceiling((decimal)(rectangles[0].Height / 32));
                var down = Math.Ceiling((decimal)(rectangles[1].Height / 32));  
                var left = Math.Ceiling((decimal)(rectangles[2].Width / 32));
                var right = Math.Ceiling((decimal)(rectangles[3].Width / 32));

                for (int i = 0; i < up - 1; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2(0, (i * 32 + 32)), ExplosiveType.Up);
                    entitesToAdd.Add(x);
                }

                for (int i = 0; i < down - 1; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2(0, (i * 32 + 32)), ExplosiveType.Down);
                    entitesToAdd.Add(x);
                }



                for (int i = 0; i < right - 1; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2((i * 32 + 32), 0), ExplosiveType.Right);
                    entitesToAdd.Add(x);
                }




                for (int i = 0; i < left - 1; i++)
                {
                    var x = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2((i * 32 + 32), 0), ExplosiveType.Left);
                    entitesToAdd.Add(x);
                }





                var topEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2(0, (float) (up * 32)), ExplosiveType.UpE);
                var bottomEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2(0, (float)(down * 32)), ExplosiveType.DownE);
                var leftEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition - new Vector2((float)(down * 32), 0), ExplosiveType.LeftE);
                var rightEdge = EntityFactory.CreateExplosionSprite(transformComponent.LocalPosition + new Vector2((float)(down * 32), 0), ExplosiveType.RightE);

                entitesToAdd.Add(topEdge);
                entitesToAdd.Add(bottomEdge);
                entitesToAdd.Add(leftEdge);
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
