using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components.PowerUp
{
    /// <summary>
    /// The MovementModifier component contains information about how the movement speed is modified.
    /// </summary>
    public class MovementModifierComponent : PowerUpComponent
    {
        public MovementModifierComponent()
        {
            Strength = 0;
        }

        /// <summary>
        /// The modifier for the maximum movement speed count
        /// </summary>
        public double Amount
        {
            get { return (Strength * 0.25); }
        }

        public override string SkinName
        {
            get { return "MovementUp"; }
        }
    }
}
