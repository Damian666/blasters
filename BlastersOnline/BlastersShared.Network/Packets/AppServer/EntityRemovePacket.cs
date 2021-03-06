﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Entities;

namespace BlastersShared.Network.Packets.AppServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Lidgren.Network;

    namespace BlastersShared.Network.Packets.AppServer
    {
        /// <summary>
        /// This packet is sent from the app server to a user to indicate that an entity is being removed.
        /// </summary>
        public class EntityRemovePacket : Packet
        {

            public EntityRemovePacket(ulong entityID)
            {
                EntityID = entityID;
            }

            /// <summary>
            /// The ID of the entity that is being removed from the game simulation state. 
            /// </summary>
            public ulong EntityID { get; set; }

            public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
            {
                base.ToNetBuffer(ref netOutgoingMessage);

                netOutgoingMessage.Write(EntityID);

                return netOutgoingMessage;
            }


            public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
            {
                var entityID = incomingMessage.ReadUInt64();
                var packet = new EntityRemovePacket(entityID);
                return packet;
            }

        }
    }

}
