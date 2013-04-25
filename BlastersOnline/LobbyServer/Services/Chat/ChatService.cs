using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Network.Packets;
using BlastersShared.Services;
using LobbyServer.Network;

namespace LobbyServer.Services.Chat
{
    /// <summary>
    /// A service that handles and covers chatting in the lobby
    /// </summary>
    public class ChatService : Service
    {

        public ChatService()
        {
            RegisterNetworkCallbacks();
        }

        private void RegisterNetworkCallbacks()
        {
            PacketService.RegisterPacket<ChatPacket>(ProcessChat);
        }

        private void ProcessChat(ChatPacket obj)
        {

            // Do some validation on chat and then send it off
            var user = ServiceContainer.Users[obj.Sender];

            // Sending this to nothing is useless
            if (user.CurrentSession == null)
                return;

            var newMessage = user.Name + ": " + obj.Message;
         
            foreach (var recipient in user.CurrentSession.Users)
            {
                var packet = new ChatPacket(newMessage);
                ClientNetworkManager.Instance.SendPacket(packet, recipient.Connection);
            }

        }


        public override void PeformUpdate()
        {
            // No updates are required for this service
            return;
        }
    }
}
