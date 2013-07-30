using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Models;
using BlastersShared.Network.Packets.ClientLobby;
using BlastersShared.Network.Packets.Lobby;
using BlastersShared.Services;
using LobbyServer.Models;
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

            if (AreCredentialsValid(username, password))
            {
                var user = AddUser(obj, username);

                Logger.Instance.Log(Level.Info, user.Name + " has joined the lobby.");

           
                var packet = new LoginResultPacket(LoginResultPacket.LoginResult.Succesful);
                ClientNetworkManager.Instance.SendPacket(packet, obj.Sender);

                // Send the user a list of sessions going on 
                //TODO: Send this when they register the intent - it's more generic     
                var sessionService = (GameSessionService)ServiceContainer.GetService(typeof(GameSessionService));
                sessionService.SendUserSessions(user);
                sessionService.SendToUsersOnlineList();

            }

            
            else
            {
                // Reject the user if they aren't able to authenticate

                var packet = new LoginResultPacket(LoginResultPacket.LoginResult.Failed);
                ClientNetworkManager.Instance.SendPacket(packet, obj.Sender);
            }
        }


        public User AddUser(LoginRequestPacket obj, string username)
        {
            User result;
            blastersmember member;

            // Fetch the member this belongs to
            using (var context = new BlastersContext())
                member = context.blastersmembers.FirstOrDefault(x => x.members_l_username.ToLower() == username.ToLower());

            using (var context = new BlastersContext())
                result = context.users.FirstOrDefault(x => x.Name.ToLower() == obj.Username.ToLower());


            if (result == null)
                result = CreateUserAccount(member);

            var user = result;

            result.Connection = obj.Sender;
            result.BlastersMember = null;

            ServiceContainer.Users.Add(user.Connection, user);
            return user;
        }

        private User CreateUserAccount(blastersmember member)
        {
            using (var context = new BlastersContext())
            {
                var user = new User();
                user.Name = member.members_display_name;
                user.BlastersMembersID = member.member_id;
                user.CreationDate = DateTime.UtcNow;
                context.users.Add(user);
                context.SaveChanges();
                ((IObjectContextAdapter)context).ObjectContext.Detach(user);             
                return user;
            }
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

            #if DEBUG
                        return true;         
            #endif

            var context = new BlastersContext();

            // Retrieve that user from the db
            var member = context.blastersmembers.FirstOrDefault(x => x.members_l_username.ToLower() == username.ToLower());

            Logger.Instance.Log(Level.Debug, "Finished login query");

            if (member == null)
                return false;

            // Check if the password is ok
            var salt = member.members_pass_salt;
            var hash = member.members_pass_hash;

            // $hash = md5( md5( $salt ) . md5( $password ) );
            var password_x = hash;
            var password_y = CalculateMD5Hash(CalculateMD5Hash(salt) + CalculateMD5Hash(password));

            if (password_x == password_y)
                return true;
            return false;
        }

        public override void PeformUpdate()
        {

        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }


    }
}
