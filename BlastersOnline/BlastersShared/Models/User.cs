using System;
using System.ComponentModel.DataAnnotations.Schema;
using BlastersShared.Game;
using Lidgren.Network;
using System.ComponentModel.DataAnnotations;

namespace BlastersShared.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public int BlastersMembersID { get; set; }
        public string Name { get; set; }

        [DoNotSerialize]
        public virtual blastersmember BlastersMember { get; set; }

        /// <summary>
        /// A secure token is a token a user sends down to an app server upon joining a game so that the app server
        /// can verify the user incoming is actually who they say they are. This also helps identify different users on a network.
        /// Security tokens are unique so they are represented as a GUID which is long and guarenteed to be unique.
        /// </summary>
        [NotMapped]
        public Guid SecureToken { get; set; }

        /// <summary>
        /// A session config is a set of properties used for a specific session
        /// </summary>
        [NotMapped]
        public UserSessionConfig SessionConfig { get; set; }

        /// <summary>
        /// The current session this user is in, null if the user is not in a session.
        /// </summary>
        [NotMapped]
        public GameSession.GameSession CurrentSession { get; set; }

        /// <summary>
        /// The date in which this user was registered and created for the game
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The connection associated with this given user
        /// </summary>
        [DoNotSerialize]
        [NotMapped]
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
