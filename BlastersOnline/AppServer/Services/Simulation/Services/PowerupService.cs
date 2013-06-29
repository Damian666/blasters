﻿using System;
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

                   Logger.Instance.Log(Level.Debug, "A powerup should be spawned now");

                   var power = EntityFactory.CreateRangeModifierPowerupPackage(new Vector2(160, 96));
                   ServiceManager.AddEntity(power);

                   // Get a random object
                   Random rand = new Random();                                     
                   var key = rand.Next(0, 4);

                   switch (key)
                   {
                       case 0:
                       case 1:
                       case 2:
                       case 3:
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
