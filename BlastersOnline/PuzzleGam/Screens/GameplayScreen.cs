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
        private SimulationState _simulationState;
        private ulong _playerID;

        private ServiceContainer _serviceContainer;
        private DebugService _debugService;

        public GameplayScreen(SimulationState simulationState, ulong playerID)
        {
            _simulationState = simulationState;
            _playerID = playerID;
        }

        public override void LoadContent()
        {
            _serviceContainer = new ServiceContainer(_simulationState, ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice);
            _serviceContainer.Map = new Map(_simulationState.MapName);
            _serviceContainer.Camera = new Camera2D(new Viewport(0, 0, 638, 566), (int)_serviceContainer.Map.WorldSizePixels.X, (int)_serviceContainer.Map.WorldSizePixels.Y, 1.0f);
            _serviceContainer.Camera.Move(new Vector2(319, 248));

            var mapRenderingService = new MapRenderingService();
            var spriteRenderingService = new SpriteRenderingService();
            var networkInputService = new NetworkInputService(_playerID);
            var explosionService = new ExplosionSpriteService();

            var movementService = new MovementService(_playerID);
            movementService.SpriteDescriptorLookup = spriteRenderingService.SpriteDescriptorLookup;

            var entitySyncService = new EntitySyncService();
            var interfaceRenderingService = new InterfaceRenderingService();
            _debugService = new DebugService();

            _serviceContainer.AddService(mapRenderingService);
            _serviceContainer.AddService(spriteRenderingService);
            _serviceContainer.AddService(networkInputService);
            _serviceContainer.AddService(movementService);
            _serviceContainer.AddService(entitySyncService);
            _serviceContainer.AddService(_debugService);
            _serviceContainer.AddService(explosionService);
            _serviceContainer.AddService(interfaceRenderingService);


            base.LoadContent();
        }

        public override void HandleInput(InputState input)
        {
            if (input.EnableDebugMode())
                _debugService.Visible = !_debugService.Visible;

            _serviceContainer.UpdateInput(input);
            
            base.HandleInput(input);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Lets try drawing our level on screen
            var spriteBatch = ScreenManager.SpriteBatch;

            _serviceContainer.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _serviceContainer.UpdateService(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}