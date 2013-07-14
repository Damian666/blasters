using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppServer.Network;
using BlastersShared;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Services;
using Microsoft.Xna.Framework;
using BlastersShared.Utilities;

namespace AppServer.Services.Simulation.Services
{
    /// <summary>
    /// This service manages spawning and maintaining powerups that are applied to entites. 
    /// </summary>
    public class PowerupService : SimulationService
    {
        private double _lastPowerupTime;

        private double _spawnTime = 15f;
        private List<Type> _powerUpTypes;

        // This is our lookup of types
        public PowerupService()
        {
            // Set the current time to the spawn time
            _lastPowerupTime = _spawnTime;

            // Generate a list of types that can be used
            _powerUpTypes = FindDerivedTypes(GetType().Assembly, typeof(PowerUpComponent)).ToList();
        }

        public IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(baseType.IsAssignableFrom);
        }

        public override void Update(double deltaTime)
        {
            if (_lastPowerupTime - _spawnTime > _spawnTime)
            {
                // Reset
                _lastPowerupTime = _spawnTime;

                _spawnTime = 15f;

                // Get a random object
                var rand = new Random();
                var key = rand.Next(0, 5);

                var x = rand.Next(0, 22);
                var y = rand.Next(0, 22);

                while (MapUtility.IsSolid(ServiceManager.Map, x, y))
                {
                    x = rand.Next(0, 22);
                    y = rand.Next(0, 22);
                }

                var location = new Vector2(x * 32, y * 32);

                switch (key)
                {
                    // Gas Flask
                    case 0:
                        var xx = EntityFactory.CreateRangeModifierPowerupPackage(location);
                        ServiceManager.AddEntity(xx);
                        break;
                    // Ultra Flask
                    case 1:
                        var yy = EntityFactory.CreateRangeModifierMaxPowerupPackage(location);
                        ServiceManager.AddEntity(yy);
                        break;
                    // Extra Bomb
                    case 2:
                        var z = EntityFactory.CreateBombCountUpPackage(location);
                        ServiceManager.AddEntity(z);
                        break;
                    // Bomb Bag
                    case 3:
                        var t = EntityFactory.CreateBombCountMaxPackage(location);
                        ServiceManager.AddEntity(t);
                        break;
                    // Hermes Shoes
                    case 4:
                        var m = EntityFactory.CreateMovementModifierPackage(location);
                        ServiceManager.AddEntity(m);
                        break;

                    default:
                        throw new Exception("Attempted to create a powerup that was not handled - possible poweurp was inherited but not handled in PowerupService!");
                }

            }

            _lastPowerupTime += deltaTime;

        }

        public override void Initialize()
        {
            // Retreieve the spawn time value off the map
            var spawnTimeString = ServiceManager.Map.RetrieveProperty("spawntime");

            if (spawnTimeString != null)
            {
                int parsedTime = 15;
                var result = int.TryParse(spawnTimeString, out parsedTime);

                if (result)
                    _spawnTime = parsedTime;
            }
        }

    }
}
