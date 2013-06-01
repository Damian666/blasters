using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.AppServer
{
    /// <summary>
    /// This packet is sent from the app server to a user to indicate that an entity is being spawned
    /// </summary>
    public class EntityAddPacket : Packet
    {

        public EntityAddPacket(Entity entity)
        {
            Entity = entity;
        }


        public Entity Entity { get; set; }



        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            // Get byte data
            var buffer = SerializationHelper.ObjectToByteArray(Entity);
            netOutgoingMessage.Write(buffer.Length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var o = (Entity)SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(incomingMessage.ReadInt32()));
            var packet = new EntityAddPacket(o);
            return packet;
        }

    }
}
