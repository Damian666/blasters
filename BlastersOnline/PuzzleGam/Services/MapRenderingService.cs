using BlastersGame.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledSharp;


namespace BlastersGame.Services
{
    /// <summary>
    /// The map rendering service is responsible for drawing layers and their respective tiles
    /// </summary>
    public class MapRenderingService : Service
    {
        private Texture2D _tileset;
        private Dictionary<uint, Tileset> _tilesets = new Dictionary<uint, Tileset>();

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, ServiceManager.Camera.GetTransformation());

            // Draw tiles
            foreach (TmxLayer layer in ServiceManager.Map.Layers)
            {

                if(layer.Properties.ContainsKey("fringed"))
                    continue;

                foreach (var tile in layer.Tiles)
                {
                    // Ignore empty tiles
                    if (tile.GID < 1)
                        continue;

                    var tileset = _tilesets[tile.GID];
                    spriteBatch.Draw(tileset.Texture,
                        new Vector2(tile.X, tile.Y) * tileset.TileSize,
                        tileset.GetTileRectangle(tile), Color.White,
                        0f, Vector2.Zero, 0f, SpriteEffects.None,
                        -(ServiceManager.Map.WorldSizePixels.Y - tile.X));
                }
            }

            spriteBatch.End();
        }

        public void DrawAfter(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, ServiceManager.Camera.GetTransformation());

            // Draw tiles
            foreach (TmxLayer layer in ServiceManager.Map.Layers)
            {

                if (!layer.Properties.ContainsKey("fringed"))
                    continue;

                foreach (var tile in layer.Tiles)
                {
                    // Ignore empty tiles
                    if (tile.GID < 1)
                        continue;

                    var tileset = _tilesets[tile.GID];
                    spriteBatch.Draw(tileset.Texture,
                        new Vector2(tile.X, tile.Y) * tileset.TileSize,
                        tileset.GetTileRectangle(tile),
                        Color.White);
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
            foreach (TmxTileset tmxTileset in ServiceManager.Map.Tilesets)
            {
                Tileset tileset = new Tileset(tmxTileset);
                tileset.Texture = ContentManager.GetTexture(
                        @"Levels\" + tmxTileset.Image.Source.Replace(".png", ""),
                        ServiceManager.GraphicsDevice);

                for (uint i = 0; i < tileset.TileCount; i++)
                    _tilesets.Add(i + tmxTileset.FirstGid, tileset);
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void HandleInput(InputState inputState)
        {
            
        }
    }
}