using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace BlastersShared
{
    /// <summary>
    /// A user is a person ready to play a game.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique name of this User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A secure token is a token a user sends down to an app server upon joining a game so that the app server
        /// can verify the user incoming is actually who they say they are. This also helps identify different users on a network.
        /// Security tokens are unique so they are represented as a GUID which is long and guarenteed to be unique.
        /// </summary>
        public Guid SecureToken { get; set; }


        /// <summary>
        /// The current session this user is in, null if the user is not in a session.
        /// </summary>
        public GameSession.GameSession CurrentSession { get; set; }

        /// <summary>
        /// The connection associated with this given user
        /// </summary>
        [DoNotSerialize]
        public NetConnection Connection { get; set; }

        public User(NetConnection connection, string username)
        {
            Name = username;
            Connection = connection;
        }

        public User()
        {
            
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
