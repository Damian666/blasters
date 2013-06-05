﻿using System;
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
        /// The modifier for the maximum bomb count
        /// </summary>
        public byte Amount { get; set; }

        /// <summary>
        /// The current total amount of bombs that are in play
        /// </summary>
        public byte CurrentBombCount { get; set; }

    }
}
