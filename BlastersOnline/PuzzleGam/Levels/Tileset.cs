using BlastersGame.Services;
using Microsoft.Xna.Framework;
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
        private int _tilesAcross;
        private int _tilesDown;

        public Tileset(TmxTileset tileset)
        {
            _tileset = tileset;
            _tilesAcross = _tileset.Image.Width / _tileset.TileWidth;
            _tilesDown = _tileset.Image.Height / _tileset.TileHeight;
        }

        public Texture2D Texture { get; internal set; }

        public uint TileCount
        {
            get { return (uint)(_tilesAcross * _tilesDown); }
        }

        public int TileWidth
        {
            get { return _tileset.TileWidth; }
        }

        public int TileHeight
        {
            get { return _tileset.TileHeight; }
        }

        public Vector2 TileSize
        {
            get { return new Vector2(TileWidth, TileHeight); }
        }

        public int TilesAcross
        {
            get { return _tilesAcross; }
        }

        public int TilesDown
        {
            get { return _tilesDown; }
        }

        public bool IsValidTile(TmxLayerTile tile)
        {
            return tile.GID >= _tileset.FirstGid && tile.GID < _tileset.FirstGid + TileCount;
        }

        public uint RelativeTileID(TmxLayerTile tile)
        {
            return tile.GID - _tileset.FirstGid;
        }

        public Rectangle GetTileRectangle(TmxLayerTile tile)
        {
            if (!IsValidTile(tile))
                return new Rectangle();

            return new Rectangle(
                (int)(RelativeTileID(tile) % _tilesAcross) * TileWidth,
                (int)(RelativeTileID(tile) / _tilesAcross) * TileHeight,
                TileWidth,
                TileHeight);
        }
    }
}