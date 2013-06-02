using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The MovementModifier component contains information about how the movement speed is modified.
    /// </summary>
    public class MovementModifierComponent : PowerUpComponent
    {
        public MovementModifierComponent(byte bonus)
        {
            Bonus = bonus;
        }

        public MovementModifierComponent()
        {
            Bonus = 1;
        }

        /// <summary>
        /// How much movement speed is added.
        /// </summary>
        public byte Bonus { get; set; }
    }
}
