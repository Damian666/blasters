using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlastersGame.Utilities;
using BlastersShared.Services;
using BlastersShared.Services.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame.Services
{
    /// <summary>
    /// The sprite service is responsible for
    /// </summary>
    public class SpriteService 
    {
        // This is used to look up sprites for drawing. It's cached in memory for ease of use
        Dictionary<string, SpriteDescriptor> _spriteDescriptorsLookup = new Dictionary<string, SpriteDescriptor>(); 


        public SpriteService()
        {
            LoadDescriptors();
        }

        private void LoadDescriptors()
        {



            // It's much easier to grab all the descriptors in one go
            // Then, they're all available in memory and there's nothing to worry about
            foreach (var file in Directory.GetFiles(PathUtility.SpriteDescriptorPath))
            {
                var descriptor = SpriteDescriptor.FromFile(file);
                _spriteDescriptorsLookup.Add(descriptor.Name, descriptor);
            }




        }


        /// <summary>
        /// Draws the sprite systems onto the screen using the logic neccessary.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

    }
}
