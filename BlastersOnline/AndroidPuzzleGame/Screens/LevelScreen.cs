using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using PuzzleGame.Levels;
using TiledSharp;

namespace PuzzleGame.Screens
{
    public class LevelScreen : GameScreen
    {
        /// <summary>
        /// The current level; it's just a simple model for the data
        /// </summary>
        private Level _level;

        private Camera2D _camera;
        private Player _player;
        private SoundEffect _soundEffect;
        private SoundEffect _winSound;
        private Song _music;

        private Texture2D _textureAtlas;

        public override void UnloadContent()
        {
            _music.Dispose();
            _soundEffect.Dispose();
            base.UnloadContent();
        }

        public override void LoadContent()
        {
   
            EnabledGestures = GestureType.HorizontalDrag | GestureType.VerticalDrag;

            ChangeLevel(1);
            _textureAtlas = ScreenManager.Game.Content.Load<Texture2D>(@"Levels\TextureAtlas");
            _soundEffect = ScreenManager.Game.Content.Load<SoundEffect>("Hit");
            _winSound = ScreenManager.Game.Content.Load<SoundEffect>("success");
         //   _music = ScreenManager.Game.Content.Load<Song>("puzzle-1.wav");
          //  MediaPlayer.IsRepeating = true;
           // MediaPlayer.Play(_music);

            base.LoadContent();
        }



        private void ChangeLevel(int ID)
        {
            

            _level = new Level(ID);
            _player = new Player(_level.PlayerPosition);
            _camera = new Camera2D(ScreenManager.GraphicsDevice.Viewport, (int)_level.WorldSizePixels.X, (int)_level.WorldSizePixels.Y, 1f);
            _camera.Zoom = ScreenManager.Zoom;
            _player.CompletedMovement -= CompletedPlayerMovement;
            _player.CompletedMovement += CompletedPlayerMovement;
        }

        private void CompletedPlayerMovement(object sender, EventArgs e)
        {
            Console.WriteLine("Finished! ");
            Console.WriteLine(sender);
            _soundEffect.Play();

            if (_player.Position == _level.GoalPosition)
            {
                _winSound.Play();
               ChangeLevel(_level.LevelID + 1);
            }
        }


        void AttemptMovement(int dirID)
        {
            if(_player.IsMoving)
                return;

            if(dirID > 3)
                throw new ArgumentException("dirID must be between 0 and 3");

            // The current 'check' position
            var x = (int) _player.Position.X/64;
            var y = (int) _player.Position.Y/64;


            switch (dirID)
            {
                // Down
                case 0:
                    while (_level.IsSolid((int) x, (int) (y + 1)) == false)
                        y++;

                    break;
                // Up
                case 1:
                    while (_level.IsSolid((int) x, (int) (y - 1)) == false)
                        y--;

                    break;

                // Left
                case 2:
                    while (_level.IsSolid((int) (x - 1), (int) y) == false)
                        x--;

                    break;

                // Right
                case 3:
                    while (_level.IsSolid((int) (x + 1), (int) y) == false)
                        x++;

                    break;
            }

            _player.MoveTo(new Vector2(x * 64, y * 64));
        }

        public override void HandleInput(InputState input)
        {

            if (!_player.IsMoving)
            {

                if (input.MoveDownIssued())
                {
                    AttemptMovement(0);
                    return;
                }

                if (input.MoveUpIssued())
                {
                    AttemptMovement(1);
                    return;
                }

                if (input.MoveRightIssued())
                {
                    AttemptMovement(3);
                    return;
                }

                if (input.MoveLeftIssued())
                {
                    AttemptMovement(2);
                    return;
                }

         
            }

            base.HandleInput(input);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            // Lets try drawing our level on screen
            var spriteBatch = ScreenManager.SpriteBatch;

            var affineTransform = _camera.GetTransformation(ScreenManager.Scale);


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, affineTransform);

            foreach (TmxLayer layer in _level.Layers)
            {

                foreach (var tile in layer.Tiles)
                {                   
                    var texX = (tile.GID - 1) % 8;
                    var texY = (int)((tile.GID - 1) / 8);

                    spriteBatch.Draw(_textureAtlas, new Vector2(tile.X * 64, tile.Y * 64),
                                     new Rectangle((int)(texX * 64), texY * 64, 64, 64), Color.White);
                }

            }



            spriteBatch.Draw(_textureAtlas, _level.GoalPosition,
                             new Rectangle(192, 320, 64, 64), Color.White);


            spriteBatch.Draw(_textureAtlas, _player.Position,
                             new Rectangle((int)Player.TextureCordinates.X, (int)Player.TextureCordinates.Y, 64, 64), Color.White);


            spriteBatch.End();



            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            
            _player.Update(gameTime);
            _camera.Pos = _player.Position;
          

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
