using System;
using System.Collections.Generic;
using System.Linq;
using BlastersShared;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.Lobby
{
    /// <summary>
    /// A packet containing a list of sessions information - pushed down to the clients so they can make decisssions
    /// on what games they may want to join.
    /// </summary>
    public class SessionListInformationPacket : Packet
    {

        /// <summary>
        /// The list of game sessions to be sent down
        /// </summary>
        public List<GameSession.GameSession> GameSessions { get; set; }

        public SessionListInformationPacket(List<GameSession.GameSession> gameSessions)
        {
            GameSessions = gameSessions;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            // Get byte data
            var buffer = SerializationHelper.ObjectToByteArray(GameSessions);
            netOutgoingMessage.Write(buffer.Length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var o =  (List<GameSession.GameSession>) SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(incomingMessage.ReadInt32()));
            var packet = new SessionListInformationPacket(o);
            return packet;
        }


    }
}
