using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AwesomiumUiLib;
using Awesomium.Core;
using System.IO;
using System.Reflection;

namespace BlastersGame.Services
{
    public class InterfaceRenderingService : Service
    {
        private AwesomiumUI _awesomiumUi;
        private Texture2D _cursorTexture;
        private Vector2 _cursorPosition;

        public override void Draw(SpriteBatch spriteBatch)
        {
            ServiceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            var width = ServiceManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = ServiceManager.GraphicsDevice.PresentationParameters.BackBufferHeight;

            spriteBatch.Begin();

            if (_awesomiumUi.webTexture != null)
                spriteBatch.Draw(_awesomiumUi.webTexture, new Rectangle(0, 0, width, height), Color.White);

            spriteBatch.End();

            _awesomiumUi.RenderWebView();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.Draw(_cursorTexture, _cursorPosition, Color.White);
            spriteBatch.End();
        }

        public override void Initialize()
        {
            _cursorTexture = ContentManager.GetTexture(@"Sprites\cursor", ServiceManager.GraphicsDevice);
            _cursorPosition = Vector2.Zero;

            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            _awesomiumUi = new AwesomiumUI();
            var width = ServiceManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = ServiceManager.GraphicsDevice.PresentationParameters.BackBufferHeight;
            _awesomiumUi.Initialize(ServiceManager.GraphicsDevice, width, height, executionPath);

            _awesomiumUi.Load(@"Content\UI\index.html");
            _awesomiumUi.OnLoadCompleted = OnLoadCompleted;
            _awesomiumUi.OnDocumentCompleted = OnDocumentCompleted;
        }

        public override void Update(GameTime gameTime)
        {
            _awesomiumUi.Update();
        }

        private void OnDocumentCompleted()
        {
            var properties = ServiceManager.Map.TmxMap.Properties;

            var mapName = "Untitled Map";

            if (properties.ContainsKey("name"))
                mapName = properties["name"];

            _awesomiumUi.CallJavascript("document.getElementById(\"room\").innerHTML = 'The Elite'");
            _awesomiumUi.CallJavascript("document.getElementById(\"gamemap\").innerHTML = '" + mapName + "'");

            string[] names = { "", "", "", "Seth", "Robbie", "Vaughan", "Justin", "Rory" };
            SetSidebarInfo(names);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        private void SetSidebarInfo(string[] names)
        {
            JSObject window = _awesomiumUi.webView.ExecuteJavascriptWithResult("window");
            //names.Reverse();
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
            _cursorPosition = input.MousePosition;
        }
    }
}