using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.GameSession;

namespace BlastersShared.Models
{
    /// <summary>
    /// This is a <see cref="BlastersShared.GameSession"/> that is being entered into the database
    /// </summary>
    public class GameSessionEntry
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameSession">The game session to create this log entry from</param>
        public GameSessionEntry(GameSession.GameSession gameSession)
        {
            MatchName = gameSession.Name;           
            
        }

        /// <summary>
        /// The name of this match
        /// </summary>
        public string MatchName { get; set; }

        /// <summary>
        /// The unique index of this GameSession entry
        /// </summary>
        public long SessionId { get; set;  }


        /// <summary>
        /// The time in which this game offically started (that is, when someoen began playing)
        /// </summary>
        public DateTime StartDate { get; set; }


        /// <summary>
        /// This is the time when the game had offically ended
        /// </summary>
        public DateTime EndDate { get; set; }

       // public virtual ICollection<User> Users { get; set; } 

        /// <summary>
        /// The type of game that was played
        /// </summary>
        public GameSessionType GameType { get; set; } 

    }
}
