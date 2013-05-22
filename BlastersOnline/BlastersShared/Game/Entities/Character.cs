using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlastersShared.Game.Entities
{
    /// <summary>
    /// A player is an interactive user that is capable of interacting with the world.
    /// A character can be AI controllered or more typically driven by the input of a user.
    /// </summary>
    public class Character : Entity
    {
        public Character(Vector2 position, Vector2 size, string name) : base(position, size)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the given entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Characters when hit lose their bubbles.
        /// </summary>
        public bool HasBubble { get; set; }



    }
}
