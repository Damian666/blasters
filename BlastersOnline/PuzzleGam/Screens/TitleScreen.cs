using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleGame.Utilities;

namespace PuzzleGame.Screens
{
    /// <summary>
    /// The title screen that is displayed before the game can actually begin
    /// </summary>
    public class TitleScreen : GameScreen
    {
        private Texture2D _bg;
        private Texture2D _logo;
        private GameTimer _timer = new GameTimer(1);

        public override void Draw(GameTime gameTime)
        {

            // We draw this and just start writing in the background


            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                                            SamplerState.LinearWrap, null, null);
            ScreenManager.SpriteBatch.Draw(_bg, new Vector2(0, 0), 
                                           new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                                                         ScreenManager.GraphicsDevice.Viewport.Height),  Color.White);
            ScreenManager.SpriteBatch.End();
            
            
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            var positionRectangle = new Rectangle(1024/2 - _logo.Width / 2, 50, _logo.Width, _logo.Height);
            ScreenManager.SpriteBatch.Draw(_logo, positionRectangle, Color.White);
            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
           _timer.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void LoadContent()
        {
            TransitionOffTime = new TimeSpan(0, 0, 0, 1);
            _timer.Completed += DoStuff;
            

            _bg = ScreenManager.Game.Content.Load<Texture2D>(@"Screens\Title\bg");
            _logo = ScreenManager.Game.Content.Load<Texture2D>(@"bmo_logo");

            base.LoadContent();
        }

        private void DoStuff(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);         
            ScreenManager.AddScreen(new GameplayScreen(), null);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

    }
}
