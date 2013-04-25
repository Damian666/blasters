using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.Lobby
{
    /// <summary>
    /// A packet sent to an application server that has been selected to simulate a game.
    /// Contains information about the simulation and the like. Further information will be tacked
    /// onto this packet as needed. 
    /// </summary>
    public  class NotifySessionBeginAppServerPacket : Packet
    {

        /// <summary>
        /// The session to be simulated and sent to the application server to simulate. 
        /// </summary>
        public GameSession.GameSession Session { get; set; }


        public NotifySessionBeginAppServerPacket(GameSession.GameSession session)
        {
            Session = session;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            var buffer = SerializationHelper.ObjectToByteArray(Session);
            var length = buffer.Length;

            netOutgoingMessage.Write(length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var length = incomingMessage.ReadInt32();
            var session = (GameSession.GameSession) SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(length));
            var packet = new NotifySessionBeginAppServerPacket(session);

            return packet;
        }


    }
}
