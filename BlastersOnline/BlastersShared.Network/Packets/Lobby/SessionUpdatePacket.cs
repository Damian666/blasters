using System;
using System.Collections.Generic;
using System.Linq;
using BlastersShared;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.Lobby
{

    /// <summary>
    /// This packet is sent out when there is an update to a session - it's used for incremental updates
    /// </summary>
    public class SessionUpdatePacket : Packet
    {


        public byte UserCount { get; set; }

        public SessionUpdatePacket(byte newUsers)
        {
            UserCount = newUsers;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write(UserCount);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var result = incomingMessage.ReadByte();
            var packet = new SessionUpdatePacket(result);
            return packet;
        }


    }
}
