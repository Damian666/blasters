using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The BombModifier component contains information about the modifier of bombs.
    /// </summary>
    class BombModifier
    {
        public BombModifier(byte amount)
        {
            Amount = amount;
        }

        public BombModifier()
        {

        }

        /// <summary>
        /// The amount of bombs an entity can possess.
        /// </summary>
        public byte Amount { get; set; }
    }
}
