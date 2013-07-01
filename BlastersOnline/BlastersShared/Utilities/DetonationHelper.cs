using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using Microsoft.Xna.Framework;
using TiledSharp;

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

        public static List<Rectangle> GetBlastRadiusFrom(TmxMap map, Entity entity)
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
            blastLeftRectangle.X = (int) transformComponent.LocalPosition.X;


            blastDownRectangle.Height += explosiveComponent.Range;
            blastRightRectangle.Width += explosiveComponent.Range;


            // Trim left
            var count = -1;
            for (int i = 0;  i > -(explosiveComponent.Range/32); i--)
            {
                var x = transformComponent.LocalPosition.X + (32 * i);
                var y = transformComponent.LocalPosition.Y;

                var solid = MapUtility.IsSolid(map, (int)(x / 32), (int)(y / 32));

                // Check if this is ok
                if (solid)
                {
                    count = Math.Abs(i) - 1;
                    break;
                }

            }

            if (count == -1)
                count = explosiveComponent.Range/32;

            // Start at the left, and trim down as needed
            blastLeftRectangle.X = blastLeftRectangle.X +  -(count*32);
            blastLeftRectangle.Width = count * 32 + 32;


            // Trim the right
            count = -1;

            for (int i = 0; i < (explosiveComponent.Range / 32); i++)
            {
                var x = transformComponent.LocalPosition.X + (32 * i);
                var y = transformComponent.LocalPosition.Y;

                var solid = MapUtility.IsSolid(map, (int)(x / 32), (int)(y / 32));

                // Check if this is ok
                if (solid)
                {
                    count = Math.Abs(i) - 1;
                    break;
                }

            }

            if (count == -1)
                count = explosiveComponent.Range/32;

            blastRightRectangle.Width = count*32 + 32;

            // Trim the bottom
            count = -1;

            for (int i = 0; i < (explosiveComponent.Range / 32); i++)
            {
                var x = transformComponent.LocalPosition.X;
                var y = transformComponent.LocalPosition.Y + (32 * i);

                var solid = MapUtility.IsSolid(map, (int)(x / 32), (int)(y / 32));

                // Check if this is ok
                if (solid)
                {
                    count = Math.Abs(i) - 1;
                    break;
                }

            }

            if (count == -1)
                count = explosiveComponent.Range/32;

            blastDownRectangle.Height = count*32 + 32;


            count = -1;

            for (int i = 0; i > -(explosiveComponent.Range / 32); i--)
            {
                var x = transformComponent.LocalPosition.X;
                var y = transformComponent.LocalPosition.Y + (32 * i);

                var solid = MapUtility.IsSolid(map, (int)(x / 32), (int)(y / 32));

                // Check if this is ok
                if (solid)
                {
                    count = Math.Abs(i) - 1;
                    break;
                }

            }


            if (count == -1)
                count = explosiveComponent.Range/32;


            blastUpRectangle.Y = (int) (transformComponent.LocalPosition.Y + -(count * 32));
            blastUpRectangle.Height = count * 32 + 32;



            List<Rectangle> blastBoxes = new List<Rectangle>();
            blastBoxes.Add(blastUpRectangle);
            blastBoxes.Add(blastDownRectangle);
            blastBoxes.Add(blastLeftRectangle);
            blastBoxes.Add(blastRightRectangle);
            return blastBoxes;
        }
    }
}
