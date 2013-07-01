using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components.PowerUp
{
    /// <summary>
    /// The RangeModifier component contains information about how much range an object has.
    /// </summary>
    public class RangeModifier : PowerUpComponent
    {

        const int IntRange = 32;
        
        /// <summary>
        /// The amount of range this powerup additionally gives for having
        /// </summary>
        public int Amount
        {
           get { return Math.Min(Strength*32 + RangeModifier.IntRange, 8 * 32); }
        }


        public override string SkinName
        {
            get { return "RangeUp"; }
        }
    }
}
