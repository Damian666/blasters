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
            get { return Configuration.MaxPlayers == Users.Count; }
        }


        /// <summary>
        /// A list of users wanting to participate in this session
        /// </summary>
        public List<User> Users { get; set; }



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
            session.Configuration = new GameSessionConfig("Blasters Game - ID: " + session.SessionID, 4, GameSessionType.Normal);
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
