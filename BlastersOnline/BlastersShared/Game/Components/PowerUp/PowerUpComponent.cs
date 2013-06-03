using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The PowerUp component contains information about world power ups for an entity.
    /// </summary>
    public class PowerUpComponent : Component
    {
        public PowerUpComponent()
        {

        }

        public PowerUpComponent(double time, byte multiplier)
        {
            Duration = time;
            Multiplier = multiplier;
        }

        /// <summary>
        /// How long the it lasts.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// How many times it can be stacked.
        /// </summary>
        public byte Multiplier { get; set; }
    }
}
