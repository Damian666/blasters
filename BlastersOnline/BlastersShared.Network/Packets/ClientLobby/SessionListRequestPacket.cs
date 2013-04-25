using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.ClientLobby
{
    /// <summary>
    /// A packet used to request a list of the current sessions going on.
    /// //TODO: Add filtering settings to this packet to only pull certain kinds of games
    /// </summary>
    public class SessionListRequestPacket : Packet
    {

        public SessionListRequestPacket()
        {
            
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

  

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var packet = new SessionListRequestPacket();
            return packet;
        }


    }
}
