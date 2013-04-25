using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Network.Packets;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.Lobby
{
    /// <summary>
    /// A packet sent to all clients that are about to begin a game session.
    /// Contains the SessionID, end-point, and auth token used to get in.
    /// </summary>
    public class SessionBeginNotificationPacket : Packet
    {

        /// <summary>
        /// The secure token generated for this user to get into the game
        /// </summary>
        public Guid SecureToken { get; set; }

        /// <summary>
        /// A string containing the remote endpoint information of the application server to connect to
        /// </summary>
        public string RemoteEndpoint { get; set; }


        /// <summary>
        /// The session ID to join into
        /// </summary>
        public uint SessionID { get; set; }


        public SessionBeginNotificationPacket(Guid secureToken, string remoteEndpoint, uint sessionID)
        {
            SecureToken = secureToken;
            RemoteEndpoint = remoteEndpoint;
            SessionID = sessionID;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            // Get byte data
            var guid = SecureToken.ToString();
            
            netOutgoingMessage.Write(guid);
            netOutgoingMessage.Write(RemoteEndpoint);
            netOutgoingMessage.Write(SessionID);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {

            var guid = Guid.Parse(incomingMessage.ReadString());
            var endpoint = incomingMessage.ReadString();
            var id = incomingMessage.ReadUInt32();

            var packet = new SessionBeginNotificationPacket(guid, endpoint, id);
            return packet;
        }



    }
}
