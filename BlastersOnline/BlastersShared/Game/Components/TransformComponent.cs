using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The transformation component contains informationa about world transformations for an entity.
    /// </summary>
    public class TransformComponent : Component
    {
        public TransformComponent(Vector2 localPosition, Vector2 size)
        {
            LocalPosition = localPosition;
            ServerPosition = localPosition;
            LastLocalPosition = localPosition;
            Size = size;
        }

        public TransformComponent()
        {
            
        }

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

    }
}
