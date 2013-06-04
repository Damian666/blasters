using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets
{
    /// <summary>
    /// This packet is sent to players who need a list of players who are online
    /// </summary>
    public class NotifyUsersOnlinePacket : Packet
    {


        /// <summary>
        /// A list of names of online users
        /// </summary>
        public List<string> OnlineUsers { get; set; } 


        public NotifyUsersOnlinePacket(List<string> onlineUsers)
        {
            OnlineUsers = onlineUsers;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);


            var buffer = SerializationHelper.ObjectToByteArray(OnlineUsers);
            var length = buffer.Length;

            netOutgoingMessage.Write(length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var length = incomingMessage.ReadInt32();
            var onlineUsers = (List<string> ) SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(length));
            var packet = new NotifyUsersOnlinePacket(onlineUsers);

            return packet;
        }
    }
}
