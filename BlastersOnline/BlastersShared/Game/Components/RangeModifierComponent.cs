using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The RangeModifier component contains information about how much range an object has.
    /// </summary>
    class RangeModifier
    {
        public RangeModifier(byte amount)
        {
            Amount = amount;
        }

        public RangeModifier()
        {

        }

        /// <summary>
        /// The amount of range.
        /// </summary>
        public byte Amount { get; set; }
    }
}
