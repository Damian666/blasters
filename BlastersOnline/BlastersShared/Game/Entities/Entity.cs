using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlastersShared.Game.Entities
{
    /// <summary>
    /// An entity is a game object within the Blasters World.
    /// They have spatial cordinates, a size and are basic template for existing game objects.
    /// 
    /// Anything that is interactive should derive from this. Players, bombs, crates etc.
    /// Even powerups are entities.
    /// </summary>
    public abstract class Entity
    {

        /// <summary>
        /// The local position of this entity
        /// </summary>
        public Vector2 LocalPosition { get; set; }

        /// <summary>
        /// The server position of this entity
        /// </summary>
        public Vector2 ServerPosition { get; set; }

        /// <summary>
        /// The last local position of this entity
        /// </summary>
        public Vector2 LastLocalPosition { get; set; }

        /// <summary>
        /// The size this particular entity occupies
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// A sprite descriptor is just an XML based file with information this particular entity.
        /// Contains information about potential image properties, sizing, bounding boxes, and animation information.
        /// </summary>
        public string SpriteDescriptor { get; set; }


        public Entity(Vector2 position, Vector2 size)
        {
            // All positions are considered equal to beign with
            LocalPosition = position;
            ServerPosition = position;
            LastLocalPosition = position;

            Size = size;
        }


        public Entity()
        {
            
        }

    }
}
