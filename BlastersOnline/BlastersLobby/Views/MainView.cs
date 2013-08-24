using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppServer.Network;
using Awesomium.Core;
using BlastersLobby.Controllers;
using BlastersLobby.Models;
using BlastersLobby.Network;
using BlastersShared.Network.Packets.ClientLobby;
using System.Diagnostics;

namespace BlastersLobby.Views
{
    public class MainView : View
    {
        private const string VIEW_PATH_HTML = @"Content\LobbyBridge\main\index.html";

        public override void OnClose()
        {
            
        }

        public override void OnViewAppeared()
        {
            FlowController.WebControl.Source = new Uri(Environment.CurrentDirectory + @"\" + VIEW_PATH_HTML);     
            FlowController.WebControl.DocumentReady += WebControlOnDocumentReady;

        

        }

        private void WebControlOnDocumentReady(object sender, UrlEventArgs urlEventArgs)
        {
            FlowController.ChangeView(new LoginView());

        }


        public override void UpdateView()
        {

        }

    }
}
