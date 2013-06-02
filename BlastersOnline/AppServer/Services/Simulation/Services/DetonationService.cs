using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
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
        /// <summary>
        /// The blast width is the amount of space the explosive entity will consume.
        /// </summary>
        private const int BLAST_WIDTH = 32;

        public override void Update(double deltaTime)
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

            List<Rectangle> blastBoxes = new List<Rectangle>();
            blastBoxes.Add(blastUpRectangle);
            blastBoxes.Add(blastDownRectangle);
            blastBoxes.Add(blastLeftRectangle);
            blastBoxes.Add(blastRightRectangle);

            // Construct the range on the blasts
            blastUpRectangle.Height += -explosiveComponent.Range;
            blastDownRectangle.Height += explosiveComponent.Range;
            blastRightRectangle.Width += explosiveComponent.Range;
            blastLeftRectangle.Width += -explosiveComponent.Range;

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
                    
                    Rectangle playerBoundingBox = new Rectangle((int) x, (int) y, (int) width, (int) height);

                    foreach (var blastRectangle in blastBoxes)
                    {
                        if (playerBoundingBox.Intersects(blastRectangle))
                        {
                            Logger.Instance.Log(Level.Debug,  "{} has been bombed! TODO: Do something about it...");
                            ServiceManager.RemoveEntity(entity);
                        }
                    }


                }


            }

        }


        public override void Initialize()
        {

        }
    }
}
