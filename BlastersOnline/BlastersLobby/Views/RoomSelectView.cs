using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awesomium.Core;
using BlastersLobby.Controllers;
using BlastersLobby.Models;

namespace BlastersLobby.Views
{
    /// <summary>
    /// The room select view is used 
    /// </summary>
    public class RoomSelectView : View
    {
        private const string VIEW_PATH_HTML = @"Content\LobbyBridge\RoomSelectView\index.html";
        private const string VIEW_PATH_JS = @"Content\LobbyBridge\RoomSelectView\functions.js";

        // The model and controller are here
        private RoomSelectController _controller;
        private RoomSelectModel _model;

        public override void OnViewAppeared()
        {
            FlowController.WebControl.Source = new Uri(Environment.CurrentDirectory + @"\" + VIEW_PATH_HTML);



            _model = new RoomSelectModel();
            _controller = new RoomSelectController(_model, this);


            FlowController.WebControl.DocumentReady += WebControlOnDocumentReady;

        }

        private void WebControlOnDocumentReady(object sender, UrlEventArgs urlEventArgs)
        {



            // Updates the view immediately
            UpdateView();
        }

        public override void UpdateView()
        {

            ExecuteJavascriptViaString("setPlayerNames", _model.OnlineUsers);
            var list = new List<string>();
            list.Add("TEST NEWS: NEW UI IS IN PLACE. THANKS WINSTON!");
            ExecuteJavascriptViaString("setNews", list);

            // Set the rooms
            var roomList = _model.SessionsAvailable.Select(room => room.Name).ToList();
            ExecuteJavascriptViaString("setRoomNames", roomList);
                
        }



    }
}
