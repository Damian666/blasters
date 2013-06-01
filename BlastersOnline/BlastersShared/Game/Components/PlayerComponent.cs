using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

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
        /// The secure token this user has used to authenticate
        /// </summary>
        public Guid SecureToken { get; set; }

        /// <summary>
        /// A connection object that refers to this player entity
        /// </summary>
        [DoNotSerialize]
        public NetConnection Connection { get; set; }

        /// <summary>
        /// Just a field to shove some data; nothing super useful to see here
        /// </summary>
        public uint ReservedData { get; set; }

    }
}
