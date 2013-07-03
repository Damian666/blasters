using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.GameSession;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.ClientLobby
{

    /// <summary>
    /// A packet that is sent out when a user has requested to create a room
    /// </summary>
    public class SessionCreateRequestPacket : Packet
    {

        public SessionCreateRequestPacket(GameSessionType  sessionType)
        {
            SessionType = sessionType;
        }

        /// <summary>
        /// The session ID that the lobby client wants to try and join
        /// </summary>
        public GameSessionType SessionType { get; set; }
    


        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write((byte)SessionType);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var sessioNID = (GameSessionType) incomingMessage.ReadByte();
            var packet = new SessionCreateRequestPacket(sessioNID);
            return packet;
        }

    }
}
