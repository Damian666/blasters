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
    public class SessionJoinResultPacket : Packet
    {
        /// <summary>
        /// An enumeration representing all the possible results
        /// </summary>
        public enum SessionJoinResult
        {
            Succesful,
            Failed
        }

        /// <summary>
        /// The result of the request; whether it was successful or not
        /// </summary>
        public SessionJoinResult Result { get; set;
        
        }

        public SessionJoinResultPacket(SessionJoinResult result)
        {
            Result = result;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write((byte) Result);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var result = (SessionJoinResult) incomingMessage.ReadByte();
            var packet = new SessionJoinResultPacket(result);
            return packet;
        }


    }
}
