using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Models;
using BlastersShared.Models.Enum;
using BlastersShared.Network.Packets.ClientLobby;
using BlastersShared.Services;
using LobbyServer.Network;

namespace LobbyServer.Services
{
    /// <summary>
    /// The user intent service is used for managing intents that a particular client has expressed interest in.
    /// </summary>
    public class UserIntentService : Service
    {

        public UserIntentService()
        {

            // Register our intent packets
            PacketService.RegisterPacket<UserIntentChangePacket>(Handler);
        }

        private void Handler(UserIntentChangePacket userIntentChangePacket)
        {
            // Get the user
            var user = ServiceContainer.Users[userIntentChangePacket.Sender];

            // Set the intentions for this user
            if (user != null)
                SetIntent(user, userIntentChangePacket.UserIntents);

        }


        /// <summary>
        /// Proccesses a given user and their expected intents.
        /// </summary>
        /// <param name="user">The user to apply this particular intent to</param>
        /// <param name="userIntents">The new intent state that the client has requested</param>
        public void SetIntent(User user, UserIntents userIntents)
        {
            // Assign the intents directly
            user.UserIntents = userIntents;

            //TODO:  Fire off an event to any service that might be listening that cares for this sort of thing
        }

        public override void PeformUpdate()
        {
           
        }
    }
}
