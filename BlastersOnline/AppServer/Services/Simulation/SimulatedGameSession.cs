using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.GameSession;

namespace AppServer.Services.Simulation
{
    /// <summary>
    /// A simulated game session that is being completed on this application server.
    /// </summary>
    public class SimulatedGameSession
    {

        /// <summary>
        /// The game session is being simulated.
        /// </summary>
        public GameSession Session { get; set; }

        public SimulatedGameSession(GameSession session)
        {
            Session = session;
        }



        /// <summary>
        /// Sets the simulation up by configurating parameters and the like
        /// </summary>
        private void SetupSimulation()
        {
            
        }

        
        /// <summary>
        /// Performs updates on the simulation state
        /// </summary>
        public void PerformUpdate()
        {
            
        }



    }
}
