#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using BlastersGame.Network;
using BlastersGame.Screens;
using BlastersShared.Game.Entities;
using BlastersShared.Network.Packets.AppServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

#endregion

namespace BlastersGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PuzzleGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private ScreenManager screenManager;

        public PuzzleGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Window.Title = "Blasters Online";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;



            PacketService.RegisterPacket<SessionEndedLobbyPacket>(Instance_ClientDisconnected);

            foreach (var launchParameter in Environment.GetCommandLineArgs())
            {
                Console.Write(launchParameter);
            }

            // Time to hook the exception reporter if not connected to a debugger
            if (!Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Grab the exception
            var exception = (Exception)e.ExceptionObject;

            var fromAddress = new MailAddress("dev@blastersonline.com", "Developer Logger Service");
            var toAddress = new MailAddress("dev@blastersonline.com", "Blasters Development Team");
            string subject = "Exception Report - " + Environment.MachineName;
            string body = exception.Message + "\n" + exception.Data + "\n" + exception.StackTrace + "\n" + exception.Source + "\n";

            string systemInfo = Environment.OSVersion + Environment.NewLine + exception.InnerException;
            body += systemInfo;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("blastersdeveloperservice@gmail.com", "ru-aBRuc6E*+UJ"),
                Timeout = 3000
            };

            smtp.SendCompleted += smtp_SendCompleted;
            smtp.Timeout = 2;

            var message = new MailMessage(fromAddress, toAddress);
            message.From = fromAddress;
            message.Subject = subject;
            message.Body = body;

            smtp.Send(message);

            MessageBox.Show(
                "Unfortunately, Blasters Online has encountered a problem and been forced to shutdown. An error report has been sent to help fix this issue.");


        }

        void smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine(e.Error);
        }

        private void Instance_ClientDisconnected(SessionEndedLobbyPacket obj)
        {
            Process.GetCurrentProcess().Kill();

        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenManager = new ScreenManager(this);
            screenManager.Initialize();
            screenManager.AddScreen(new TitleScreen(), null);

            //HACK: Defintely don't want this here forever
            Entity._counter = ulong.MaxValue - 10000;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            screenManager.Update(gameTime);


            NetworkManager.Instance.Update();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Process.GetCurrentProcess().Kill();

            base.OnExiting(sender, args);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);


            base.Draw(gameTime);
        }
    }
}
