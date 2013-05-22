using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;

namespace BlastersShared.GameSession

{
    /// <summary>
    /// Contains information about a particular match and statistics associated with it.
    /// </summary>
    public class SessionEndStatistics
    {

        /// <summary>
        /// The winner of this particular match
        /// </summary>
        public User Winner { get; set; }

        /// <summary>
        /// The duration of the match in total (seconds)
        /// </summary>
        public double MatchDuration { get; set; }

        public SessionEndStatistics(User winningUser, double matchDuration)
        {
            Winner = winningUser;
            MatchDuration = matchDuration;
        }

        public SessionEndStatistics()
        {
            
        }

    }
}
