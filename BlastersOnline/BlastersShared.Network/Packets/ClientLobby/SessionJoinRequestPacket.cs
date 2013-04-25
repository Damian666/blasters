using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.ClientLobby
{
    /// <summary>
    /// A client initiated request to join a session
    /// </summary>
    public class SessionJoinRequestPacket : Packet
    {
        public SessionJoinRequestPacket(uint sessionID)
        {
            SessionID = sessionID;
        }

        /// <summary>
        /// The session ID that the lobby client wants to try and join
        /// </summary>
        public uint SessionID { get; set; }
    


        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write(SessionID);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var sessioNID = incomingMessage.ReadUInt32();
            var packet = new SessionJoinRequestPacket(sessioNID);
            return packet;
        }


    }
}
