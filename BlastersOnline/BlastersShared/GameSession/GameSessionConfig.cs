using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastersShared.GameSession
{
    /// <summary>
    /// A configuration file for a <see cref="GameSession"/>. Contains settings pertaining to a particular game instance.
    /// </summary>
    public class GameSessionConfig
    {

        /// <summary>
        /// The name of this <see cref="GameSession"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The max amount of players allowed to participate
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// The type of session this is
        /// </summary>
        public GameSessionType SessionType { get; set; }

        public GameSessionConfig(string name, int maxPlayers, GameSessionType sessionType)
        {
            Name = name;
            MaxPlayers = maxPlayers;
            SessionType = sessionType;
        }


        public GameSessionConfig()
        {
            
        }

    }   

}
