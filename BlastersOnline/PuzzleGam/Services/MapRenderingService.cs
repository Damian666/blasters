using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;


namespace BlastersGame.Services
{
    /// <summary>
    /// The map rendering service is responsible for drawing layers and their respective tiles
    /// </summary>
    public class MapRenderingService : Service
    {
        private Texture2D _tileset;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, ServiceManager.Camera.GetTransformation());

            // Draw tiles
            foreach (TmxLayer layer in ServiceManager.Map.Layers)
            {
                foreach (var tile in layer.Tiles)
                {
                    // Ignore empty tiles
                    if (tile.GID < 1)
                        continue;

                    var relativeGID = tile.GID - 1;

                    // TODO: Make this dynamic for multiple tilesets
                    var tmxTileset = ServiceManager.Map.Tilesets[0] as TmxTileset;
                    var tilesAcross = _tileset.Width / tmxTileset.TileWidth;

                    var texX = (int)(relativeGID % tilesAcross);
                    var texY = (int)(relativeGID / tilesAcross);

                    spriteBatch.Draw(_tileset, new Vector2(tile.X * tmxTileset.TileWidth, tile.Y * tmxTileset.TileHeight),
                                     new Rectangle(texX * tmxTileset.TileWidth, texY * tmxTileset.TileHeight, tmxTileset.TileWidth, tmxTileset.TileHeight), Color.White);
                }
            }

            spriteBatch.End();
        }

        public override void Initialize()
        {
            LoadTilesetTextures();
        }

        private void LoadTilesetTextures()
        {
            // TODO: Make this dynamic for multiple tilesets
            _tileset = ContentManager.GetTexture(@"Levels\" + (ServiceManager.Map.Tilesets[0] as TmxTileset).Image.Source.Replace(".png", ""), ServiceManager.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void HandleInput(InputState inputState)
        {
            //throw new NotImplementedException();
        }
    }
}