using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppServer.Network;
using AppServer.Services;
using BlastersShared;
using BlastersShared.Services;

namespace AppServer
{
    /// <summary>
    /// The application server program entry point.
    /// An application server is responsbile for playing a game and simulating them.
    /// </summary>
    class AppServerMain
    {


        static LobbyCommunicatorService _lobbyCommunicator = new LobbyCommunicatorService();
        static GameSessionSimulationService _gameSessionService = new GameSessionSimulationService();
        static ServiceContainer _serviceContainer = new ServiceContainer();


        static void Main(string[] args)
        {
            Console.Title = "Blasters Application Server";
            Console.WindowWidth = 100;

            Logger.Instance.Log(Level.Info, "The application server has succesfully finished loading.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _serviceContainer.RegisterService(_lobbyCommunicator);
            _serviceContainer.RegisterService(_gameSessionService);

            while (true)
            {
                ClientNetworkManager.Instance.Update();

                _serviceContainer.PerformUpdates();

                Thread.Sleep(1);
            }



        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.Log(Level.Fatal, e.ExceptionObject.ToString());

            //TODO: Potentially, notify the lobby and all clients about doom
        }






    }
}
