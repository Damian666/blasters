using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlastersShared.Game.Entities
{
    /// <summary>
    /// A player is just a character that is driven by user input. 
    /// </summary>
    public class Player : Character
    {
        public Player(Vector2 position, Vector2 size, string name) : base(position, size, name)
        {
        }
    }
}
