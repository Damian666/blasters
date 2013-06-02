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
        public MovementModifierComponent(byte multiplier)
        {
            Multiplier = multiplier;
        }

        public MovementModifierComponent()
        {

        }

        /// <summary>
        /// How much movement speed is modified.
        /// </summary>
        public byte Multiplier { get; set; }
    }
}
