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
        public SessionSendSimulationStatePacket(SimulationState simulationState, ulong playerUID, string mapName)
        {
            SimulationState = simulationState;
            PlayerUID = playerUID;
            MapName = mapName;
        }

        public SessionSendSimulationStatePacket()
        {
            
        }

        public SimulationState SimulationState { get; set; }

        /// <summary>
        /// The unique ID of the player this state package is being sent to.
        /// </summary>
        public ulong PlayerUID { get; set; }

        /// <summary>
        /// The map name that the player will be playing in.
        /// </summary>
        public string MapName { get; set; }

        public override NetOutgoingMessage ToNetBuffer(ref NetOutgoingMessage netOutgoingMessage)
        {
            base.ToNetBuffer(ref netOutgoingMessage);

            // Get byte data
            var buffer = SerializationHelper.ObjectToByteArray(SimulationState);
            netOutgoingMessage.Write(PlayerUID);
            netOutgoingMessage.Write(MapName);
            netOutgoingMessage.Write(buffer.Length);
            netOutgoingMessage.Write(buffer);

            return netOutgoingMessage;
        }


        public new static Packet FromNetBuffer(NetIncomingMessage incomingMessage)
        {
            var uid = incomingMessage.ReadUInt64();
            var map = incomingMessage.ReadString();
            var o = (SimulationState) SerializationHelper.ByteArrayToObject(incomingMessage.ReadBytes(incomingMessage.ReadInt32()));
            var packet = new SessionSendSimulationStatePacket(o, uid, map);
            return packet;
        }

    }

}
