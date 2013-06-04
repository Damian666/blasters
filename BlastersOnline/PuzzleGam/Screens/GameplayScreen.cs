using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Awesomium.Core;
using AwesomiumUiLib;
using BlastersGame;
using BlastersGame.Components;
using BlastersGame.Levels;
using BlastersGame.Services;
using BlastersShared.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using BlastersShared.Game.Entities;
using BlastersShared.Game.Components;

namespace BlastersGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private Texture2D _tileset;
        private Texture2D _curTexture;
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
            _curTexture = ScreenManager.Game.Content.GetTexture(@"Sprites\cursor", ScreenManager.Game.GraphicsDevice);

            _serviceContainer = new ServiceContainer(_simulationState, ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice);

            _serviceContainer.Map = new Map(_simulationState.MapName);

            _serviceContainer.Camera = new Camera2D(new Viewport(0, 0, 638, 566), (int)_serviceContainer.Map.WorldSizePixels.X, (int)_serviceContainer.Map.WorldSizePixels.Y, 1.0f);
            _serviceContainer.Camera.Move(new Vector2(319, 283));

            // TODO: Make this dynamic for multiple tilesets
            _tileset = ScreenManager.Game.Content.GetTexture(@"Levels\" + (_serviceContainer.Map.Tilesets[0] as TmxTileset).Image.Source.Replace(".png", ""), ScreenManager.Game.GraphicsDevice);

            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            UI = new AwesomiumUI();
            var width = ScreenManager.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = ScreenManager.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            UI.Initialize(ScreenManager.Game.GraphicsDevice, width, height, executionPath);

            UI.Load(@"Content\UI\index.html");
            UI.OnLoadCompleted = OnLoadCompleted;
            UI.OnDocumentCompleted = OnDocumentCompleted;

            var spriteRenderingService = new SpriteRenderingService();
            var networkInputService = new NetworkInputService(_playerID);
            
            var movementService = new MovementService(_playerID);
            movementService.SpriteDescriptorLookup = spriteRenderingService.SpriteDescriptorLookup;

            var entitySyncService = new EntitySyncService();
            _debugService = new DebugService();

            _serviceContainer.AddService(spriteRenderingService);
            _serviceContainer.AddService(networkInputService);
            _serviceContainer.AddService(movementService);
            _serviceContainer.AddService(entitySyncService);
            _serviceContainer.AddService(_debugService);

            base.LoadContent();
        }

        private void OnDocumentCompleted()
        {
            UI.CallJavascript("document.getElementById(\"room\").innerHTML = 'The Elite'");
            UI.CallJavascript("document.getElementById(\"gamemap\").innerHTML = '" + _serviceContainer.Map.LevelID + "'");

            string[] names = { "", "", "", "Seth", "Robbie", "Vaughan", "Justin", "Rory" };
            SetSidebarInfo(names);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        private void SetSidebarInfo(string[] names)
        {
            JSObject window = UI.webView.ExecuteJavascriptWithResult("window");
            names.Reverse();
            string paramSet = String.Join("|", names);

            using (window)
            {
                window.Invoke("setPlayerNames", paramSet);
            }
        }


        private void OnLoadCompleted()
        {
            
        }

        public override void HandleInput(InputState input)
        {
            if (input.EnableDebugMode())
                _debugService.Visible = !_debugService.Visible;

            _serviceContainer.UpdateInput(input);
            curPosition = input.MousePosition;
            base.HandleInput(input);
        }

        private DebugService _debugService;


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Lets try drawing our level on screen
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, _serviceContainer.Camera.GetTransformation());

            // Draw tiles
            foreach (TmxLayer layer in _serviceContainer.Map.Layers)
            {
                foreach (var tile in layer.Tiles)
                {
                    // TODO: Make this dynamic for multiple tilesets
                    var relativeGID = tile.GID - 1;
                    var tmxTileset = _serviceContainer.Map.Tilesets[0] as TmxTileset;
                    var tilesAcross = _tileset.Width / tmxTileset.TileWidth;

                    var texX = (int)(relativeGID % tilesAcross);
                    var texY = (int)(relativeGID / tilesAcross);

                    // TODO: 35px offset needs to be abstracted out
                    spriteBatch.Draw(_tileset, new Vector2(tile.X * tmxTileset.TileWidth, 35 + tile.Y * tmxTileset.TileHeight),
                                     new Rectangle(texX * tmxTileset.TileWidth, texY * tmxTileset.TileHeight, tmxTileset.TileWidth, tmxTileset.TileHeight), Color.White);
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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.Draw(_curTexture, curPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 curPosition = Vector2.Zero;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            UI.Update();
            _serviceContainer.UpdateService(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
