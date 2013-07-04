using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace BlastersShared.Utilities
{
    public static class MapUtility
    {



        public static bool IsSolid(TmxMap map, int x, int y)
        {

            if (y < 0 || x < 0)
                return true;


            var tileset_ = (TmxTileset) map.Tilesets[0];

            var gid = (y *  map.Width) + x;

            if (gid == 768)
                gid = 0;



            if (gid > map.Width * map.Height - 1)
                return true;



            var tileset = (TmxTileset)map.Tilesets[0];

            foreach (TmxLayer layer in map.Layers)
            {
                PropertyDict dict;
                var tile = layer.Tiles[gid];
                tileset.Tiles.TryGetValue((int)tile.GID, out dict);
                if (dict != null && dict.ContainsKey("blocked"))
                    return true;
            }
            return false;
        }


        public static TmxLayerTile GetSolidBlock(TmxMap map, int x, int y)
        {

            if (y < 0 || x < 0)
                return null;


            var tileset_ = (TmxTileset)map.Tilesets[0];

            var gid = (y * map.Width) + x;

            if (gid == 768)
                gid = 0;

            if (gid > map.Width*map.Height - 1)
                return null;

            var tileset = (TmxTileset)map.Tilesets[0];

            foreach (TmxLayer layer in map.Layers)
            {
                PropertyDict dict;
                var tile = layer.Tiles[gid];
                tileset.Tiles.TryGetValue((int)tile.GID, out dict);
                if (dict != null && dict.ContainsKey("destroyable"))
                    return tile;
            }
            return null;
        }


    }
}
