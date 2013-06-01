using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace BlastersShared.Network.Packets.Client
{
    /// <summary>
    /// A simple request to place a bomb at the current position of the player.
    /// </summary>
    public class RequestPlaceBombPacket : Packet
    {

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {

            var packet = new RequestPlaceBombPacket();
            return packet;
        }

    }
}
