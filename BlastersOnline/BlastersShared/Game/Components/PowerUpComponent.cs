﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The PowerUp component contains information about world power ups for an entity.
    /// </summary>
    class PowerUpComponent
    {
        public PowerUpComponent(double time, byte multiplier)
        {
            Duration = time;
            Multiplier = multiplier;
        }

        public PowerUpComponent()
        {

        }

        /// <summary>
        /// The amount of time the component lasts.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The amount of time the component can be stacked.
        /// </summary>
        public byte Multiplier { get; set; }
    }
}
