using BlastersGame.Services;
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
        private uint _tilesAcross;
        private uint _tilesDown;

        public Tileset(TmxTileset tileset)
        {
            _tileset = tileset;
            _tilesAcross = (uint)(_tileset.Image.Width / _tileset.TileWidth);
            _tilesDown = (uint)(_tileset.Image.Height / _tileset.TileHeight);
        }

        public Texture2D Texture { get; internal set; }

        public uint TileCount
        {
            get { return _tilesAcross * _tilesDown; }
        }

        public bool IsValidTile(TmxLayerTile tile)
        {
            return tile.GID >= _tileset.FirstGid && tile.GID < _tileset.FirstGid + TileCount;
        }

        public uint RelativeTileID(TmxLayerTile tile)
        {
            return tile.GID - (_tileset.FirstGid + 1);
        }
    }
}