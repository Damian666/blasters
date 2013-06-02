using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using Microsoft.Xna.Framework;

namespace AppServer.Services.Simulation.Services
{
    /// <summary>
    /// The detonation service is used to monitor entities that are going to explode.
    /// This service manages them and count's down their destruction timer.
    /// </summary>
    public class DetonationService : Service
    {
        /// <summary>
        /// The blast width is the amount of space the explosive entity will consume.
        /// </summary>
        private const int BLAST_WIDTH = 32;

        public override void Update(float deltaTime)
        {

            foreach (var entity in ServiceManager.Entities)
            {

                var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

                // If this has a detonation timer on it...
                if (explosiveComponent != null)
                {
                    explosiveComponent.DetonationTime -= deltaTime;
                    
                    if(explosiveComponent.DetonationTime < 0f)
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

            var transformComponent = (TransformComponent) entity.GetComponent(typeof (TransformComponent));
            var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

            // Construct the blast rectangle
            Rectangle blastUpRectangle = new Rectangle((int) transformComponent.LocalPosition.X, (int) transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);
            Rectangle blastDownRectangle = new Rectangle((int)transformComponent.LocalPosition.X, (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);
            Rectangle blastLeftRectangle = new Rectangle((int)transformComponent.LocalPosition.X, (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);
            Rectangle blastRightRectangle = new Rectangle((int)transformComponent.LocalPosition.X, (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);

            // Construct the range on the blasts
            blastUpRectangle.Height += -explosiveComponent.Range;
            blastDownRectangle.Height += explosiveComponent.Range;
            blastRightRectangle.Width += explosiveComponent.Range;
            blastLeftRectangle.Width += -explosiveComponent.Range;


        }


        public override void Initialize()
        {

        }
    }
}
