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
            var gid = (y * 24) + x;

            if (gid == 768)
                gid = 0;

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


    }
}
