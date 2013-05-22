using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Entities;

namespace BlastersShared.Game
{
    /// <summary>
    /// The game state is a simple container for logic and data that is present in the game.
    /// 
    /// </summary>
    public class SimulationState
    {
        private List<Entity> _entities = new List<Entity>();

        /// <summary>
        /// This is a container of game entities within this game state.
        /// </summary>
        public List<Entity> Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }



        /// <summary>
        /// This blank constructor is used for AltSerializer to create the object
        /// Most of these objects should be created from the SimulationStateFactory.
        /// </summary>
        public SimulationState()
        {
            
        }


    }
}
