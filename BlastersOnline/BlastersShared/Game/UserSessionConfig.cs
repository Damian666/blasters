﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game
{
    /// <summary>
    /// The user match config object contains session specific configurations for a given user.
    /// Such things include a players chosen skin.
    /// </summary>
    public class UserSessionConfig
    {

        public UserSessionConfig()
        {
            
        }

        public UserSessionConfig(string skin)
        {
            Skin = skin;
        }

        /// <summary>
        /// A chosen skin for a player; this is variable per match and thus is sent down as a seperate object.
        /// </summary>
        public string Skin { get; set;  }

    }
}