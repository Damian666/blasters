using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace BlastersShared.Network.Packets.Client
{
    /// <summary>
    /// A simple request to place a bomb at the current position of the player.
    /// </summary>
    public class RequestPlaceBombPacket : Packet
    {

        /// <summary>
        /// Get the current position to place a bomb
        /// </summary>
        public Vector2 CurrentPosition { get; set; }

        public RequestPlaceBombPacket(Vector2 currentPosition)
        {
            CurrentPosition = currentPosition;
        }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);
            netOutgoingMessage.Write(CurrentPosition.X);
            netOutgoingMessage.Write(CurrentPosition.Y);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            float x = incomingMessage.ReadFloat();
            float y = incomingMessage.ReadFloat();
            Vector2 position = new Vector2(x, y);
            var packet = new RequestPlaceBombPacket(position);
            return packet;
        }

    }
}
