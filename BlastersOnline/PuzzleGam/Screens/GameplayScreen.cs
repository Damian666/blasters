using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AwesomiumUiLib;
using BlastersGame;
using BlastersGame.Components;
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
        private ServiceContainer _serviceContainer;
        private ulong _playerID;


        public GameplayScreen(SimulationState simulationState, ulong playerID)
        {
            _simulationState = simulationState;
            _playerID = playerID;

        }

        private AwesomiumUI UI;

        public override void LoadContent()
        {
            _tileset = ScreenManager.Game.Content.GetTexture(@"Levels\BMOTiles", ScreenManager.Game.GraphicsDevice);

            _serviceContainer = new ServiceContainer(_simulationState, ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice);


            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            UI = new AwesomiumUI();
            var width = ScreenManager.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = ScreenManager.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            UI.Initialize(ScreenManager.Game.GraphicsDevice, width, height, executionPath);


            UI.Load(@"Content\UI\index.html");
            UI.OnLoadCompleted = OnLoadCompleted;
            




            var service = new SpriteService();
            var networkService = new NetworkInputService(_playerID);
            var movementService = new MovementService(_playerID);
            _serviceContainer.AddService(service);
            _serviceContainer.AddService(networkService);
            _serviceContainer.AddService(movementService);

            base.LoadContent();
        }

        private void OnLoadCompleted()
        {
            UI.CallJavascript("document.getElementById(\"room\").innerHTML = 'PARTY ROOM WOOOOOOOOOOOOO 666'");
        }

        public override void HandleInput(InputState input)
        {
            _serviceContainer.UpdateInput(input);
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

           





            _serviceContainer.Draw(spriteBatch);



            ScreenManager.Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            var width = ScreenManager.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = ScreenManager.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            spriteBatch.Begin();
            if (UI.webTexture != null)
                spriteBatch.Draw(UI.webTexture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();


            UI.RenderWebView();


            base.Draw(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _serviceContainer.UpdateService(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
