using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;
using BlastersShared.GameSession;
using Lidgren.Network;

namespace BlastersShared.Network.Packets.AppServer
{
    public class SessionSendSimulationStatePacket : Packet
    {
        public SessionSendSimulationStatePacket(SimulationState simulationState)
        {
            SimulationState = simulationState;
        }

        public SimulationState SimulationState { get; set; }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            // Get byte data
            var buffer = SerializationHelper.ObjectToByteArray(SimulationState);
            netOutgoingMessage.Write(buffer.Length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var o = (SimulationState) SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(incomingMessage.ReadInt32()));
            var packet = new SessionSendSimulationStatePacket(o);
            return packet;
        }

    }

}
