﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components.PowerUp
{
    /// <summary>
    /// The PowerUp component contains information about world power ups for an entity.
    /// </summary>
    public class PowerUpComponent : Component
    {
   

        public PowerUpComponent()
        {
            Strength = 1;
            Duration = double.MaxValue;
        }

        /// <summary>
        /// How long the it lasts.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The strength of this given power up
        /// 
        public byte Strength { get; set; }
    }
}
