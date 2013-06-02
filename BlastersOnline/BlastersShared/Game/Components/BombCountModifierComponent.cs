using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The BombModifier component contains information about how many bombs an entity can possess.
    /// </summary>
    class BombCountModifierComponent
    {
        public BombCountModifierComponent(byte amount)
        {
            Amount = amount;
        }

        public BombCountModifierComponent()
        {

        }

        /// <summary>
        /// The modifier for bomb count.
        /// </summary>
        public byte Amount { get; set; }
    }
}
