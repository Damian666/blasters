using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace BlastersGame.Levels
{
    /// <summary>
    /// A Tileset is a model of the data contained within a specific tileset.
    /// Wraps around <see cref="TmxTileset"/> and other things to abstract away tasks.
    /// </summary>
    public class Tileset
    {
        private TmxTileset _tileset;
        private Texture2D _texture;

        public Tileset(TmxTileset tileset)
        {
            _tileset = tileset;
            // TODO: Figure out how to "efficiently" link tile GIDs to tilesets
        }

        public int GetTileTextureX(int gid)
        {
            long relativeGid = gid - (_tileset.FirstGid + 1);
            int tiles = _tileset.Image.Width / _tileset.TileWidth;

            return (int)(relativeGid % tiles);
        }

        public int GetTileTextureY(int gid)
        {
            long relativeGid = gid - (_tileset.FirstGid + 1);
            int tiles = _tileset.Image.Width / _tileset.TileWidth;

            return (int)(relativeGid / tiles);
        }
    }
}