using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using BlastersShared.Services;
using Microsoft.Xna.Framework;

namespace AppServer.Services.Simulation.Services
{
    /// <summary>
    /// This service manages spawning and maintaining powerups that are applied to entites. 
    /// </summary>
    public class PowerupService : SimulationService
    {
        private double _lastPowerupTime;
        private const double _spawnTime = 15f;

        public PowerupService()
        {
            // Set the current time to the spawn time
            _lastPowerupTime = _spawnTime;
        }

        public override void Update(double deltaTime)
        {
            if (_lastPowerupTime - _spawnTime > _spawnTime)
            {
                // Reset
                _lastPowerupTime = _spawnTime;


                // Get a random object
                Random rand = new Random();
                var key = rand.Next(0, 4);

                var x = rand.Next(0, 22);
                var y = rand.Next(0, 22);

                var location = new Vector2(x*32, y*32);


                switch (key)
                {
                    case 0:
                        var xx = EntityFactory.CreateRangeModifierPowerupPackage(location);
                        ServiceManager.AddEntity(xx);
                        break;
                    case 1:
                        var yy = EntityFactory.CreateRangeModifierMaxPowerupPackage(location);
                        ServiceManager.AddEntity(yy);
                        break;
                    case 2:
                        var z = EntityFactory.CreateRangeModifierPowerupPackage(location);
                        ServiceManager.AddEntity(z);
                        break;
                    case 3:
                        var t = EntityFactory.CreateRangeModifierPowerupPackage(location);
                        ServiceManager.AddEntity(t);
                        break;


                    default:
                        throw new Exception("Attempted to create a powerup that was not handled - possible poweurp was inherited but not handled in PowerupService!");
                        break;
                }



            }


            _lastPowerupTime += deltaTime;

        }




        public override void Initialize()
        {

        }



    }
}
