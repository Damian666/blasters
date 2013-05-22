using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.GameSession;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.AppServer
{
    public class SessionEndedLobbyPacket : Packet
    {

        public SessionEndedLobbyPacket(uint sessionID, SessionEndStatistics sessionStats)
        {
            SessionStatistics = sessionStats;
            SessionID = sessionID;
        }

        public SessionEndStatistics SessionStatistics { get; set; }
        public uint SessionID { get; set; }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            // Get byte data
            var buffer = SerializationHelper.ObjectToByteArray(SessionStatistics);
            netOutgoingMessage.Write(SessionID);
            netOutgoingMessage.Write(buffer.Length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var sessionID = incomingMessage.ReadUInt32();
            var o = (SessionEndStatistics) SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(incomingMessage.ReadInt32()));
            var packet = new SessionEndedLobbyPacket(sessionID ,o);
            return packet;
        }


    }
}
