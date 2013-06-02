using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The RangeModifier component contains information about the modifier of the range.
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
        /// The amount of range that is modified from the range.
        /// </summary>
        public byte Amount { get; set; }
    }
}
