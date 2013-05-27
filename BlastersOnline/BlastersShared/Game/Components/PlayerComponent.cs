using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// This component is a mere tag for an entity, indiciating this entity contains a player.
    /// </summary>
    public class PlayerComponent : Component
    {

        public PlayerComponent()
        {
            
        }

        /// <summary>
        /// Just a field to shove some data; nothing super useful to see here
        /// </summary>
        public uint ReservedData { get; set; }

    }
}
