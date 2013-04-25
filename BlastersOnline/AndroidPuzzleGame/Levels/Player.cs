using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace PuzzleGame.Levels
{
    /// <summary>
    /// The model of the player on a level; has information like cordinates, inventory and the like 
    /// </summary>
    public class Player
    {
        // The amount of time it takes to move one tile (seconds)
        private const float TILE_SPEED = 0.2f;

        private Vector2 _newPosition;
        private Vector2 _position;
        private Vector2 _orginalPosition;
        private float _timer;
  

        /// <summary>
        /// Determines whether or not the player is currently moving to their destination
        /// </summary>
        public bool IsMoving
        {
            get
            {

                if (_newPosition.Y == 0)
                    return false;

                return (Vector2.Distance(_position, _newPosition) > 2.1f);

            }
        }

        /// <summary>
        /// The current position of this player
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
        }

        /// <summary>
        /// An event that is invoked when the movement is completed
        /// </summary>
        public EventHandler CompletedMovement;

        /// <summary>
        /// Begins interpolation towards a given tile on the player object.
        /// </summary>
        /// <param name="position">The position to begin moving towards</param>
        public void MoveTo(Vector2 position)
        {
            if (!IsMoving)
            {
                _newPosition = position;
                _orginalPosition = Position;
                _timer = 0f;
            }
        }

        public static Vector2 TextureCordinates
        {
            get { return new Vector2(0, 448); }
        }

        public Player(Vector2 position)
        {
            _position = position;
        }

        private bool _wasMoving = false;

        /// <summary>
        /// Updates the player state; used to tween animations and the like
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {


            if (IsMoving)
            {
                var dX = Math.Abs(_orginalPosition.X - _newPosition.X) / 64;
                var dY = Math.Abs(_orginalPosition.Y - _newPosition.Y) / 64;
                var total = dX + dY;

                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds / TILE_SPEED / total;

                var x = MathHelper.Lerp(_orginalPosition.X, _newPosition.X, _timer);
                var y = MathHelper.Lerp(_orginalPosition.Y, _newPosition.Y, _timer);

                _position = new Vector2(x, y);

                _wasMoving = true;
            }
            else
            {
                if (_wasMoving)
                {
                    _position = _newPosition;
                    _wasMoving = false;
                    CompletedMovement(this, null);
                }
            }


        }

    }
}
