using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Game.Components;
using Microsoft.Xna.Framework;

namespace AppServer.Services.Simulation.Services.Powerup
{

    /// <summary>
    /// An enum depicting how a given powerup can spawn
    /// </summary>
    public enum PoweurpSpawnType
    {
        // Indicates that powerups will spawn one at a time, waiting for each of them to spawn before starting another
        Synchronous,

        // Indicates that it's okay to begin all the timers at once
        Asynchronous
    }

    /// <summary>
    /// Contains information about potential spawn points on the map
    /// </summary>
    public class PowerupSpawnNode
    {

        /// <summary>
        /// The collection object this given spawn node is containing. 
        /// </summary>
        public PowerUpCollectionComponent PoweurpCollection { get; set; }

        /// <summary>
        /// The position this spawn object is supposed to spawn at
        /// </summary>
        public Vector2 Position { get; set; }

        public float SpawnTime { get; set; }


        /// <summary>
        /// Determines how this given powerup will spawn
        /// </summary>
        public PoweurpSpawnType SpawnType { get; set; }


        public PowerupSpawnNode(PowerUpCollectionComponent poweurpCollection, Vector2 position, float spawnTime)
        {
            PoweurpCollection = poweurpCollection;
            Position = position;
            SpawnTime = spawnTime;
        }

    }
}
