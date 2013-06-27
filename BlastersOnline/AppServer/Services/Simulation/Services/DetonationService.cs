using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Game.Components;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using BlastersShared.Utilities;
using Microsoft.Xna.Framework;
using BlastersShared;

namespace AppServer.Services.Simulation.Services
{
    /// <summary>
    /// The detonation service is used to monitor entities that are going to explode.
    /// This service manages them and count's down their destruction timer.
    /// </summary>
    public class DetonationService : SimulationService
    {
        public override void Update(double deltaTime)
        {

            foreach (var entity in ServiceManager.Entities)
            {

                var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

                // If this has a detonation timer on it...
                if (explosiveComponent != null)
                {
                    explosiveComponent.DetonationTime -= deltaTime;

                    if (explosiveComponent.DetonationTime < 0f)
                        DetonateEntity(entity);

                }
            }
        }

        /// <summary>
        /// Detonates a given entity immediately. This function will signal the removal of the entity
        /// and inform all clients of the loss of the entity.
        /// </summary>
        /// <param name="entity"></param>
        private void DetonateEntity(Entity entity)
        {

            var blastBoxes = DetonationHelper.GetBlastRadiusFrom(entity);


            // Detonate and hurt players
            foreach (var player in ServiceManager.Entities)
            {
                var playerTransformComponent = (TransformComponent)player.GetComponent(typeof(TransformComponent));
                var playerComponent = (PlayerComponent)player.GetComponent(typeof(PlayerComponent));

                // If is a player, attempt to detonate
                if (playerComponent != null)
                {
                    var x = playerTransformComponent.LocalPosition.X;
                    var y = playerTransformComponent.LocalPosition.Y;
                    var width = playerTransformComponent.Size.X;
                    var height = playerTransformComponent.Size.Y;

                    Rectangle playerBoundingBox = new Rectangle((int)x, (int)y, (int)width, (int)height);
                    bool toDestroy = false;

                    foreach (var blastRectangle in blastBoxes)
                    {
                        if (playerBoundingBox.Intersects(blastRectangle))
                        {
                            toDestroy = true;
                        }
                    }

                    if (toDestroy)
                    {
                        Logger.Instance.Log(Level.Debug, "{} has been bombed! TODO: Do something about it...");
                        ServiceManager.RemoveEntity(player);
                    }


                }


            }

            // Remove the entity
            ServiceManager.RemoveEntity(entity);
        }



        public override void Initialize()
        {
            ServiceManager.EntityRemoved += ServiceManager_EntityRemoved;
        }

        /// <summary>
        /// Fired when an entity is removed
        /// </summary>
        /// <param name="entity"></param>
        void ServiceManager_EntityRemoved(Entity entity)
        {
            var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

            if (explosiveComponent != null)
            {
                // Get the owner
                var owner = ServiceManager.RetrieveEntityByID(explosiveComponent.OwnerID);
                var ownerBombModifier =
                    (BombCountModifierComponent)owner.GetComponent(typeof(BombCountModifierComponent));
                ownerBombModifier.CurrentBombCount--;
            }

        }


    }
}
