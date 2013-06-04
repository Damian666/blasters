using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Network.Packets.ClientLobby;
using BlastersShared.Services;
using LobbyServer.Network;

namespace LobbyServer.Services

{
    /// <summary>
    /// Provides authentication related utilities to handle login requests. 
    /// This service writes to the user dictionary. 
    /// </summary>
    public class AuthenticationService : Service
    {

        public AuthenticationService()
        {
            
            RegisterNetworkCallbacks();
        }

        void RegisterNetworkCallbacks()
        {
            PacketService.RegisterPacket<LoginRequestPacket>(ProcessLoginRequest);
        }

        private void ProcessLoginRequest(LoginRequestPacket obj)
        {
            var username = obj.Username;
            var password = obj.Password;

            var validPassword = password;

            if (password == validPassword)
            {
                var user = AddUser(obj, username);

                Logger.Instance.Log(Level.Info, user.Name + " has joined the lobby.");

                // Send the user a list of sessions going on 
                var sessionService = (GameSessionService) ServiceContainer.GetService(typeof (GameSessionService));
                sessionService.SendUserSessions(user);
                sessionService.SendToUsersOnlineList();

            }



        }


        public User AddUser(LoginRequestPacket obj, string username)
        {
            var user = new User(obj.Sender, username);
            ServiceContainer.Users.Add(user.Connection, user);
            return user;
        }

        public void AddUser(User user)
        {
            ServiceContainer.Users.Add(user.Connection, user);
        }

        /// <summary>
        /// Determines whether the given username and password are valid to authenticate with.
        /// </summary>
        /// <param name="username">The username to challenge</param>
        /// <param name="password">The password to challenge</param>
        bool AreCredentialsValid(string username, string password)
        {
            //TODO: Do real work instead of approving everyone
            return true;
        }

        public override void PeformUpdate()
        {
       
        }


    }
}
