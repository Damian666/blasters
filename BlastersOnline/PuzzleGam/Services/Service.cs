using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame.Services
{
    /// <summary>
    /// A service that is used to manipulate and manage entities within a world
    /// </summary>
    public abstract class Service
    {

        /// <summary>
        /// Draws this particular service 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Updates this particular service
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// The parent container
        /// </summary>
        public ServiceContainer ServiceManager { get; set; }

        /// <summary>
        /// The content manager for this service - useful for loading in assets.
        /// </summary>
        public ContentManager ContentManager { get; set; }

        /// <summary>
        /// This method is called when the service is added and laoded succesfully. 
        /// </summary>
        public abstract void Initialize();


    }
}
