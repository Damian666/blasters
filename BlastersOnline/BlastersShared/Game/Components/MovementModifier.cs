﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The MovementModifier component contains information about how the movement speed is modified.
    /// </summary>
    class MovementModifier
    {
        public MovementModifier(byte multiplier)
        {
            Multiplier = multiplier;
        }

        public MovementModifier()
        {

        }

        /// <summary>
        /// How much the movement speed is multiplied by.
        /// </summary>
        public byte Multiplier { get; set; }
    }
}
