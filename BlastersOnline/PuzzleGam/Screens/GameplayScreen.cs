using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersGame.Services;
using BlastersShared.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleGame.Levels;
using TiledSharp;

namespace PuzzleGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private Texture2D _tileset; 
        private Map _map = new Map("SomeMap");
        private SimulationState _simulationState;


        public GameplayScreen(SimulationState simulationState)
        {
            _simulationState = simulationState;

        }


        public override void LoadContent()
        {
            _tileset = ScreenManager.Game.Content.Load<Texture2D>(@"Levels\BMOTiles");

            SpriteService service = new SpriteService();



            base.LoadContent();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {


            // Lets try drawing our level on screen
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null);

            foreach (TmxLayer layer in _map.Layers)
            {

                foreach (var tile in layer.Tiles)
                {
                    var texX = (tile.GID - 1) % 16;
                    var texY = (int)((tile.GID - 1) / 16);

                    spriteBatch.Draw(_tileset, new Vector2(tile.X * 32, tile.Y * 32),
                                     new Rectangle((int)(texX * 32), texY * 32, 32, 32), Color.White);
                }

            }

            spriteBatch.End();


            base.Draw(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
