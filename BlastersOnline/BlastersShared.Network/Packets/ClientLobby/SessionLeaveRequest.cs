using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.ClientLobby
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionLeaveRequest : Packet
    {

        public SessionLeaveRequest()
        {

        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);



            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var packet = new SessionLeaveRequest();
            return packet;
        }

    }
}
