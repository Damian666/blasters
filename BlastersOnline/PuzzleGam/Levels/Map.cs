using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiledSharp;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame.Levels
{
    /// <summary>
    /// A level is a model of the data contained within a specific level. 
    /// Wraps around <see cref="TmxMap"/> and other things to abstract away tasks.
    /// </summary>
    public class Map
    {
        //The internal map of the level, TiledSharp does most of our heavy lifting here
        private TmxMap _map;
        private string _levelID;



        /// <summary>
        /// Intiailizes a new level; automatically fetching the given level 
        /// </summary>
        /// <param name="levelID"></param>
        public Map(string levelID)
        {
            _map = new TmxMap(string.Format(@"Content\Levels\{0}.tmx", levelID));
            _levelID = levelID;
        }

        public TmxMap TmxMap
        {
            get { return _map; }
        }

        public string LevelID
        {
            get { return _levelID; }
        }

        public TmxList Tilesets
        {
            get { return _map.Tilesets; }
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
            var set = (TmxTileset)Tilesets[0];
            PropertyDict dict = null;

            foreach (TmxLayer layer in _map.Layers)
            {
                return true;
            }

           // set.Tiles.TryGetValue((int)tile.GID, out dict);


            // TODO: Sucks. Temporary, incomplete code. Needs to get fixed.
            if (dict != null && dict.ContainsKey("blocked"))
            {
            }

            return true;

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
            get
            {
                var o = ((TmxObjectGroup)_map.ObjectGroups[0]);
                var playerObject = (TmxObjectGroup.TmxObject)o.Objects["player"];
                return new Vector2(playerObject.X, playerObject.Y);
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
