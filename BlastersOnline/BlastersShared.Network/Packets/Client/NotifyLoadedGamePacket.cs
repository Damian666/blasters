using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.Client
{
    /// <summary>
    /// A packet used to request authentcation to the lobby server
    /// </summary>
    public class NotifyLoadedGamePacket : Packet
    {

        /// <summary>
        /// The secure token used to notify the server who the client is.
        /// </summary>
        public Guid SecureToken { get; set; }


        public NotifyLoadedGamePacket(Guid secureToken)
        {
            SecureToken = secureToken;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            netOutgoingMessage.Write(SecureToken.ToString());           

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var secureToken = Guid.Empty;

            try
            {
                secureToken = Guid.Parse(incomingMessage.ReadString());
            }
            catch (Exception)
            {                
                throw new UnauthorizedAccessException("A user tried to login with an invalid token. Bad client parsing or hacking attempt!");
            }



            var packet = new NotifyLoadedGamePacket(secureToken);
            return packet;
        }


    }
}
