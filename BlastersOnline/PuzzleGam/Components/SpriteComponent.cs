using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame.Components
{
    public class SpriteComponent : Component
    {
        /// <summary>
        /// The texture assosciated with this particular component
        /// </summary>
        public Texture2D Texture { get; set; }

    }
}
