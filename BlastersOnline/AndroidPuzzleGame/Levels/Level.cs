using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace PuzzleGame.Levels
{
    /// <summary>
    /// A level is a model of the data contained within a specific level. 
    /// Wraps around <see cref="TmxMap"/> and other things to abstract away tasks.
    /// </summary>
    public class Level
    {
        //The internal map of the level, TiledSharp does most of our heavy lifting here
        private TmxMap _map;
        private int _levelID;

        /// <summary>
        /// Intiailizes a new level; automatically fetching the given level 
        /// </summary>
        /// <param name="levelID"></param>
        public Level(int levelID)
        {
            _map = new TmxMap(string.Format(@"Content/Levels/{0}.tmx", levelID));
            _levelID = levelID;
        }


        public int LevelID
        {
            get { return _levelID; }
        }

        public Vector2 WorldSizePixels
        {
            get
            {
                return new Vector2(_map.Width * 64, _map.Height * 64);
            }
        }

        /// <summary>
        /// Gets whether a given point is solid (unwalkable) or not
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsSolid(int x, int y)
        {


            if (x < 0 || y < 0 || x > _map.Width - 1 || y > _map.Height - 1)
                return true;

            var gid = y * _map.Height + x;
    

            //Get tile at ALL layers
            var set =
            (TmxTileset) _map.Tilesets[0];


            foreach (TmxLayer layer in _map.Layers)
            {

                foreach (var tb in layer.Tiles)
                {
                    if (tb.X == x && tb.Y == y)
                    {

                        var tile = tb;

                        if (set.Tiles.ContainsKey((int) tile.GID - 1))
                        {
                            foreach (var dict in set.Tiles)
                            {
                                if (dict.Value.ContainsKey("solid"))
                                {
                                    return true;
                                }
                            }

                        }
                    }

                }

            }


            return false;

        }

        /// <summary>
        /// The list of layers inside of this level.
        /// </summary>
        public TmxList Layers
        {
            get { return _map.Layers; }
        }

        /// <summary>
        /// The starting spawn position of the player for this level
        /// </summary>
        public Vector2 PlayerPosition
        {
            get { var o = ((TmxObjectGroup) _map.ObjectGroups[0]);
                var playerObject = (TmxObjectGroup.TmxObject) o.Objects["player"];
                return  new Vector2(playerObject.X, playerObject.Y);
            }           
        }

        /// <summary>
        /// The goal position on this level that the player must reach.
        /// </summary>
        public Vector2 GoalPosition
        {
            get
            {
                var o = ((TmxObjectGroup)_map.ObjectGroups[0]);
                var playerObject = (TmxObjectGroup.TmxObject)o.Objects["goal"];
                return new Vector2(playerObject.X, playerObject.Y);
            }
        }


    }
}
