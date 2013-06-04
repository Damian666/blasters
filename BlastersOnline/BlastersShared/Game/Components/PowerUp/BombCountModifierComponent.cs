using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components.PowerUp
{
    /// <summary>
    /// The BombModifier component contains information about how many bombs an entity can possess.
    /// </summary>
    public class BombCountModifierComponent : PowerUpComponent
    {
        public BombCountModifierComponent()
        {

        }

        public BombCountModifierComponent(byte amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// The modifier for bomb count.
        /// </summary>
        public byte Amount { get; set; }
    }
}
