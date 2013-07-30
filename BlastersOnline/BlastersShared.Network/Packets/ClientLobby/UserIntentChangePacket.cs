using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.GameSession;
using BlastersShared.Models.Enum;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.ClientLobby
{
    /// <summary>
    /// This packet is sent when the lobby has decided it wants to change the type of push notifications it should recieve.
    /// </summary>
    public class UserIntentChangePacket : Packet
    {

        public UserIntentChangePacket(UserIntents userIntents)
        {
            UserIntents = userIntents;
        }

        /// <summary>
        /// The session ID that the lobby client wants to try and join
        /// </summary>
        public UserIntents UserIntents { get; set; }
    


        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write((byte)UserIntents);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var userIntents = (UserIntents)incomingMessage.ReadByte();
            var packet = new UserIntentChangePacket(userIntents);
            return packet;
        }
    }
}
