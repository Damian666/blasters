using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.AppServer
{
    /// <summary>
    /// A packet that is sent when the application server wants to join the lobby server cluster.
    /// This contains a strongly typed password that is known between the two applications.
    /// </summary>
    public class ClusterAddPacket : Packet
    {

        /// <summary>
        /// The private password needed to join the cluster network
        /// </summary>
        public string PrivatePassword { get; set; }

        public ClusterAddPacket(string privatePassword)
        {
            PrivatePassword = privatePassword;
        }


        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write(PrivatePassword);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            return new ClusterAddPacket(incomingMessage.ReadString());
        }

    }
}
