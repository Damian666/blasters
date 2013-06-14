using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.GameSession;

namespace BlastersLobby.Models
{
    public class RoomSelectModel
    {

        /// <summary>
        /// A list of all online, available users at the time of the models construction.
        /// </summary>
        public List<String> OnlineUsers { get; set; }

        /// <summary>
        /// The currently available sessions that are listed.
        /// These are potentially paginated; so the values are only what the server has sent thus far.
        /// </summary>
        public List<GameSession> SessionsAvailable { get; set; }

        public string News { get; set; }

        /// <summary>
        /// The current page for this room
        /// </summary>
        public int RoomPage { get; set; }

        public RoomSelectModel()
        {
            OnlineUsers = new List<string>();
            SessionsAvailable = new List<GameSession>();
        }

    }
}
