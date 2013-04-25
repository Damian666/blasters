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
        GameTimer _timer = new GameTimer(0.01d);

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_bg, new Vector2(0, 0), Color.White);
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
            base.LoadContent();
        }

        private void DoStuff(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);         
            ScreenManager.AddScreen(new LevelScreen(), null);
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
