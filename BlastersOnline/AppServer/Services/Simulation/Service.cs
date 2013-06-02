using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppServer.Services.Simulation
{
    /// <summary>
    /// A service that is used to manipulate and manage entities within a world
    /// </summary>
    public abstract class Service
    {

        /// <summary>
        /// Updates this particular service
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// The parent container
        /// </summary>
        public ServiceContainer ServiceManager { get; set; }


        /// <summary>
        /// This method is called when the service is added and laoded succesfully. 
        /// </summary>
        public abstract void Initialize();


    }
}
