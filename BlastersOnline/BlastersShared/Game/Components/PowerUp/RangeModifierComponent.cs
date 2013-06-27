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
           get { return Strength*32 + RangeModifier.IntRange ; }
        }


    }
}
