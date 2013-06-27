using System;
using System.Collections.Generic;
using BlastersShared.Game;
using BlastersShared.Game.Entities;
using BlastersShared.GameSession;
using Microsoft.Xna.Framework;
using TiledSharp;

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

        private static Stack<Vector2> GetPlayerSpawnPositions()
        {
            var currentMap = new TmxMap(string.Format(@"Content\Levels\{0}.tmx", "Battle_Royale"));
            var pos = new List<Vector2>();

            foreach (var obj in ((TmxObjectGroup) currentMap.ObjectGroups[0]).Objects )
            {
                var mapObject = (TmxObjectGroup.TmxObject) obj;

                if (mapObject.Type == "PlayerSpawn")
                {
                    Vector2 vector = new Vector2(mapObject.X - 16, mapObject.Y - 6);
                    pos.Add(vector);
                }

            }

            pos.Shuffle();
            return new Stack<Vector2>(pos);

        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        private static SimulationState CreateDeathmatchState(GameSession gameSession)
        {

            // An empty simulation state; we'll begin filling it up
            var simulationState = new SimulationState();
            var positionStack = GetPlayerSpawnPositions();

            foreach (var user in gameSession.Users)
            {
                // Create a player from each user and shove them down the container
                var player = EntityFactory.CreatePlayer(user, positionStack.Pop() );
                simulationState.Entities.Add(player);
            }


            return simulationState;

        }




    }
}
