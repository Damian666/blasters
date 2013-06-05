using System;
using System.Collections.Generic;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
using BlastersShared.GameSession;
using Microsoft.Xna.Framework;

namespace AppServer.Services.Simulation
{
    /// <summary>
    /// A simulation state factory is used for preparing a game for playing.
    /// This generates a fresh state based on a game type passed from the session object.
    /// 
    /// This class is especially handy when generating things like monsters, random objects and the like are required.
    /// </summary>
    public static class SimulationStateFactory
    {


        /// <summary>
        /// Creates a simulation state based solely on the game session type.
        /// </summary>
        /// <param name="gameSession">The game session to generate a particular simulation state for</param>
        /// <returns></returns>
        public static SimulationState CreateSimulationState(GameSession gameSession)
        {
            var sessionType = gameSession.Configuration.SessionType;
            SimulationState simulationState;

            switch (sessionType)
            {
               case GameSessionType.Normal:
                    simulationState = CreateDeathmatchState(gameSession); 
                   break;

                default:
                    throw new NotImplementedException("The game type on the session give as '" + sessionType + "' isn't implemented.");
                    break;
            }

            return simulationState;
        }

        private static SimulationState CreateDeathmatchState(GameSession gameSession)
        {

            // An empty simulation state; we'll begin filling it up
            var simulationState = new SimulationState();

            foreach (var user in gameSession.Users)
            {
                // Create a player from each user and shove them down the container
                var player = EntityFactory.CreatePlayer(user, new Vector2(116, 86));
                simulationState.Entities.Add(player);
            }


            return simulationState;

        }




    }
}
