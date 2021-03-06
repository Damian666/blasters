﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets
{
    /// <summary>
    /// A generic chat packet that is used for communication purposes. 
    /// </summary>
    public class ChatPacket : Packet
    {


        /// <summary>
        /// The message being sent out
        /// </summary>
        public string Message { get; set; }

        public ChatPacket(string message)
        {
            Message = message;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write(Message);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var message = incomingMessage.ReadString();
            return new ChatPacket(message);
        }

      



    }
}
