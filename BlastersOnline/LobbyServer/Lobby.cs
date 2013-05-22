using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlastersShared;
using LobbyServer.Services;
using LobbyServer.Services.Chat;

namespace LobbyServer
{
    class Lobby
    {

        static AppServerService _appServerService = new AppServerService();
        static GameSessionService _gameSessionService = new GameSessionService();
        static AuthenticationService _authenticationService = new AuthenticationService();
        static ChatService _chatService = new ChatService();
        static BlastersShared.Services.ServiceContainer _serviceContainer = new BlastersShared.Services.ServiceContainer();

        static void Main(string[] args)
        {

            // Setup some stuff, because why not
            Console.Title = "Blasters Lobby Gateway";
            Console.WindowWidth = 100;

            // Write some welcoming info
            PrintLine(ConsoleColor.Yellow, "********************************************************************");
            PrintLine(ConsoleColor.Yellow, "This is beta software. You have been warned. Code by: Vaughan Hilts");
            PrintLine(ConsoleColor.Yellow, "********************************************************************\n");

            Logger.Instance.Log(Level.Info, "The lobby server has succesfully been started.");


            // Add services
            _serviceContainer.RegisterService(_authenticationService);
            _serviceContainer.RegisterService(_gameSessionService);
            _serviceContainer.RegisterService(_appServerService);
            _serviceContainer.RegisterService(_chatService);


            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // We populate the game with some matches in debug mode


#if DEUBG
      
#endif
            // Create ten default games
            for (int i = 0; i < 10; i++)
                _gameSessionService.CreateSession();


            while (true)
            {

                _serviceContainer.PerformUpdates();

                Thread.Sleep(1);
            }



            Console.ReadLine();


        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.Log(Level.Fatal, e.ExceptionObject.ToString());
        }


        public static void PrintLine(ConsoleColor consoleColor, string message)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
