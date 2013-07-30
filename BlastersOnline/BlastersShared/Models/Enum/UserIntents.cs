using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastersShared.Models.Enum
{
    /// <summary>
    /// The user intents enum
    /// </summary>
    [Flags]
    public enum UserIntents : long 
    {   
        // This intent value indicates this user is interested in session updates
        SessionUpdates = 0x0,

        // This intent value indicates this user is interested in updates about friends signing in and out
        FriendsUpdates = 0x1
    }
}
