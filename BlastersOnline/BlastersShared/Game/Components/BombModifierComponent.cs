using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The BombModifier component contains information about how many bombs an entity can possess.
    /// </summary>
    class BombModifierComponent
    {
        public BombModifierComponent(byte amount)
        {
            Amount = amount;
        }

        public BombModifierComponent()
        {

        }

        /// <summary>
        /// The amount of bombs.
        /// </summary>
        public byte Amount { get; set; }
    }
}
