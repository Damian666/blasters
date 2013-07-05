using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Components.PowerUp;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace BlastersShared.Network.Packets.AppServer
{
    public class PowerupRecievedPacket : Packet
    {
        /// <summary>
        /// The strength of the powerup
        /// </summary>
        public byte Strength;

        public PowerupRecievedPacket(byte strength)
        {
            Strength = strength;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write(Strength);

            return netOutgoingMessage;
        }

        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            // Read value back in
            var strength = incomingMessage.ReadByte();

            var packet = new PowerupRecievedPacket(strength);
            return packet;
        }

    }
}
