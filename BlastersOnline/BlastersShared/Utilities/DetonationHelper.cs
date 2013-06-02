using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using Microsoft.Xna.Framework;

namespace BlastersShared.Utilities
{
    /// <summary>
    /// Provides utilities for things like blast boxes.
    /// </summary>
    public static class DetonationHelper
    {
        /// <summary>
        /// The blast width is the amount of space the explosive entity will consume.
        /// </summary>
        private const int BLAST_WIDTH = 32;

        public static List<Rectangle> GetBlastRadiusFrom(Entity entity)
        {
            var transformComponent = (TransformComponent)entity.GetComponent(typeof(TransformComponent));
            var explosiveComponent = (ExplosiveComponent)entity.GetComponent(typeof(ExplosiveComponent));

            // Construct the blast rectangle
            Rectangle blastUpRectangle = new Rectangle((int)transformComponent.LocalPosition.X,
                                                       (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);
            Rectangle blastDownRectangle = new Rectangle((int)transformComponent.LocalPosition.X,
                                                         (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);
            Rectangle blastLeftRectangle = new Rectangle((int)transformComponent.LocalPosition.X,
                                                         (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);
            Rectangle blastRightRectangle = new Rectangle((int)transformComponent.LocalPosition.X,
                                                          (int)transformComponent.LocalPosition.Y, BLAST_WIDTH, 32);


            // Construct the range on the blasts
            blastUpRectangle.Height += explosiveComponent.Range;
            blastLeftRectangle.Width += explosiveComponent.Range;

            blastUpRectangle.Y -= explosiveComponent.Range;
            blastLeftRectangle.X -= explosiveComponent.Range;


            blastDownRectangle.Height += explosiveComponent.Range;
            blastRightRectangle.Width += explosiveComponent.Range;


            List<Rectangle> blastBoxes = new List<Rectangle>();
            blastBoxes.Add(blastUpRectangle);
            blastBoxes.Add(blastDownRectangle);
            blastBoxes.Add(blastLeftRectangle);
            blastBoxes.Add(blastRightRectangle);
            return blastBoxes;
        }
    }
}
