using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastersShared.GameSession
{
    public class GameSession
    {
        private static uint _idCounter = 0;

        /// <summary>
        /// The configruation for this paticular session
        /// </summary>
        public GameSessionConfig Configuration { get; set; }


        /// <summary>
        /// Indicates whether a session is full or not
        /// </summary>
        public bool IsFull
        {
            get{ return Configuration.MaxPlayers == Users.Count; }
        }

        /// <summary>
        /// Determines whether or not the session is in progress or not
        /// </summary>
        public bool InProgress { get; set; }


        /// <summary>
        /// A list of users wanting to participate in this session
        /// </summary>
        public List<User> Users { get; set; }


        /// <summary>
        /// Returns the leader of this room. This is the person who is authornized to make configuraiton changes
        /// via the network and control various settings. This is the first person who entered the room. Typically,
        /// that is the person who created the room. However, if this person disconnects it becomes the 2nd. 
        /// </summary>
        public User RoomLeader
        {
            get
            {
                if (Users.Count == 0)
                    return null;
                else
                    return Users[0];
            }
        }

        /// <summary>
        /// A unique ID to this session, automatically created and generated during construction
        /// </summary>
        public uint SessionID { get; set; }

        public string Name
        {
            get { return Configuration.Name; }
        }

       
        public static GameSession CreateDefaultDeathmatch()
        {
            var rUID = new Random().Next(45345435);
            var session = new GameSession(null);
            session.Configuration = new GameSessionConfig("Deathmatch Game #" + session.SessionID, 4, GameSessionType.Normal);
            return session;
        }


        private GameSession(GameSessionConfig config)
        {
            lock (this)
            {

                // Assign a random ID that hasn't been used before
                SessionID = _idCounter;
                _idCounter++;
            }

            Users = new List<User>();
            InProgress = false;
        }

        public GameSession()
        {
            
        }
   

        public override string ToString()
        {

            return Name;

            return base.ToString();
        }



    }
}
