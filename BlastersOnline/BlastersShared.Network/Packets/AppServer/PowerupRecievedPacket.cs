using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace BlastersShared.Network.Packets.AppServer
{
    public class PowerupRecievedPacket : Packet
    {
        /// <summary>
        /// The strength of the powerup
        /// </summary>
        public PowerUpComponent Powerup { get; set; }

        /// <summary>
        /// The ID of the entity this belongs to
        /// </summary>
        public ulong EntityID { get; set; }

        public PowerupRecievedPacket(PowerUpComponent powerUpComponent, ulong id)
        {
            Powerup = powerUpComponent;
            EntityID = id;
        }


        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            var buffer = SerializationHelper.ObjectToByteArray(Powerup);
            netOutgoingMessage.Write(buffer.Length);
            netOutgoingMessage.Write(buffer);
            netOutgoingMessage.Write(EntityID);

            return netOutgoingMessage;
        }

        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {

            var o = (PowerUpComponent)SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(incomingMessage.ReadInt32()));
            var id = incomingMessage.ReadUInt64();
            var packet = new PowerupRecievedPacket(o, id);
            return packet;

        }

    }
}
