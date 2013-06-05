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

namespace BlastersLobby.Views
{
    public class LoginView : View
    {
        private const string VIEW_PATH_HTML = @"Content\LobbyBridge\LoginView\index.html";



        public override void OnViewAppeared()
        {
            FlowController.WebControl.Source = new Uri(Environment.CurrentDirectory + @"\" + VIEW_PATH_HTML);

            

            FlowController.WebControl.DocumentReady += WebControlOnDocumentReady;

        

        }

        private void WebControlOnDocumentReady(object sender, UrlEventArgs urlEventArgs)
        {

            JSObject jsobject = FlowController.WebControl.CreateGlobalJavascriptObject("hook");

            jsobject.Bind("loginRequest", false, Handler);

            // Updates the view immediately
            UpdateView();
        }

        private void Handler(object sender, JavascriptMethodEventArgs javascriptMethodEventArgs)
        {

            var username = FlowController.WebControl.ExecuteJavascriptWithResult("document.getElementById('tusername').value");
            var password = FlowController.WebControl.ExecuteJavascriptWithResult("document.getElementById('tpassword').value");

            if (username.ToString() == string.Empty)
            {
                var code = "bootbox.alert('You must specify a valid username and password. The given credentials are incorrect.');";
                FlowController.WebControl.ExecuteJavascript(code);
                return;
            }

            var packet = new LoginRequestPacket(username, password);
            NetworkManager.Instance.SendPacket(packet);

            // Wait a moment
            Thread.Sleep(500);

            // Change the view
            FlowController.ChangeView(new RoomSelectView());


        }

        public override void UpdateView()
        {

        }

    }
}
