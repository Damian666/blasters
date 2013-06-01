using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame
{
    public static class ContentManagerOverride
    {
        public static Texture2D GetTexture(this ContentManager contentManager, string path, GraphicsDevice device)
        {           
            path = contentManager.RootDirectory + "\\" + path + ".png";
            var stream = new FileStream(path, FileMode.Open);
            var texture  = Texture2D.FromStream(device,stream);
            stream.Close();
            return texture;
        }

    }
}
