using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using BlastersShared.Game;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleGame.Network;
using PuzzleGame.Utilities;
using XNATweener;

namespace PuzzleGame.Screens
{
    /// <summary>
    /// The title screen that is displayed before the game can actually begin
    /// </summary>
    public class TitleScreen : GameScreen
    {
        private Texture2D _bg;
        private Texture2D _logo;
        private GameTimer _timer = new GameTimer(5);
        private float i_y = 50;
        private float logoY = 50;

        private float targetY = 250;

        public override void Draw(GameTime gameTime)
        {

            // We draw this and just start writing in the background


            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                                            SamplerState.LinearWrap, null, null);
            ScreenManager.SpriteBatch.Draw(_bg, new Vector2(0, 0),
                                           new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                                                         ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            ScreenManager.SpriteBatch.End();


            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            var positionRectangle = new Rectangle(1024 / 2 - _logo.Width / 2, (int) logoY, _logo.Width, _logo.Height);
            ScreenManager.SpriteBatch.Draw(_logo, positionRectangle, Color.White);
            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private double time = 0;

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _timer.Update(gameTime);

            time += gameTime.ElapsedGameTime.TotalSeconds / 1.5;

            if (time > 1)
                time = 1;

            logoY = MathHelper.SmoothStep(i_y, targetY, (float) time);



            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private Tweener tweener;

        public override void LoadContent()
        {

            TransitionOffTime = new TimeSpan(0, 0, 0, 3);
            _timer.Completed += DoStuff;
    

            PacketService.RegisterPacket<SessionSendSimulationStatePacket>(Handler);
         
            _bg = ScreenManager.Game.Content.Load<Texture2D>(@"Screens\Title\bg");
            _logo = ScreenManager.Game.Content.Load<Texture2D>(@"bmo_logo");

            // Send off a packet :)

            var list = Environment.GetCommandLineArgs().ToList();

            
            _myToken = Guid.Parse(list[1]);
            string[] info = list[2].Split(Convert.ToChar(":"));

#if DEBUG_MOCK
            info[0] = "localhost";
            info[1] = "7798";
            _myToken = Guid.Empty;

            Thread.Sleep(6000);
#endif

            NetworkManager.Instance.ConnectTo(info[0], int.Parse(info[1]));




            base.LoadContent();
        }


        private void Handler(SessionSendSimulationStatePacket sessionSendSimulationStatePacket)
        {
            var state = sessionSendSimulationStatePacket.SimulationState;


            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new GameplayScreen(state), null);
        }

        private Guid _myToken;
        private bool _done = false;

        private void DoStuff(object sender, EventArgs e)
        {

            if (_done)
                return;
          

            var packet = new NotifyLoadedGamePacket(_myToken);
            NetworkManager.Instance.SendPacket(packet);
            _done = true;

            return;


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
