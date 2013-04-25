using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppServer.Services;
using BlastersShared.Services;

namespace AppServer
{
    /// <summary>
    /// The application server program entry point.
    /// An application server is responsbile for playing a game and simulating them.
    /// </summary>
    class Program
    {


        static LobbyCommunicatorService _lobbyCommunicator = new LobbyCommunicatorService();
        static GameSessionSimulationService _gameSessionService = new GameSessionSimulationService();
        static ServiceContainer _serviceContainer = new ServiceContainer();


        static void Main(string[] args)
        {
            Console.Title = "Blasters Application Server";
            Console.WriteLine("Application server is loading...");

            _serviceContainer.RegisterService(_lobbyCommunicator);
            _serviceContainer.RegisterService(_gameSessionService);

            while (true)
            {

           _serviceContainer.PerformUpdates();

                Thread.Sleep(1);               
            }


        }
    }
}
