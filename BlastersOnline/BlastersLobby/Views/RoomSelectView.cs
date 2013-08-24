using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServer.Network;
using Awesomium.Core;
using BlastersLobby.Controllers;
using BlastersLobby.Models;
using BlastersLobby.Network;
using BlastersShared.Network.Packets.ClientLobby;
using BlastersShared.Network.Packets.Lobby;

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

        public override void OnClose()
        {
            PacketService.DeRegisterPacket<SessionJoinResultPacket>(ProcessJoinResult);
        }

        public override void OnViewAppeared()
        {
            FlowController.WebControl.Source = new Uri(Environment.CurrentDirectory + @"\" + VIEW_PATH_HTML);

            PacketService.RegisterPacket<SessionJoinResultPacket>(ProcessJoinResult);


            _model = new RoomSelectModel();
            _controller = new RoomSelectController(_model, this);


            FlowController.WebControl.DocumentReady += WebControlOnDocumentReady;

        }

        private void ProcessJoinResult(SessionJoinResultPacket obj)
        {
            if (obj.Result == SessionJoinResultPacket.SessionJoinResult.Succesful)
            {
                // Change the view
                FlowController.WebControl.DocumentReady -= WebControlOnDocumentReady;
                FlowController.ChangeView(new InRoomView());
            }

        }

        private void WebControlOnDocumentReady(object sender, UrlEventArgs urlEventArgs)
        {
            JSObject jsobject = FlowController.WebControl.CreateGlobalJavascriptObject("hook");
            jsobject.Bind("incrementRoom", false, IncrementRoom);
            jsobject.Bind("deincrementRoom", false, DeIncrementRoom);
            jsobject.Bind("enterRoom", false, EnterRoom);
         

            // Updates the view immediately
            UpdateView();
        }

        private void EnterRoom(object sender, JavascriptMethodEventArgs e)
        {
            // Join the first match
            var packet = new SessionJoinRequestPacket(0);
            NetworkManager.Instance.SendPacket(packet);


        }

        private void DeIncrementRoom(object sender, JavascriptMethodEventArgs e)
        {
            var roomValue = Math.Max(0, _model.RoomPage - 1);
            _controller.ChangeRoom(roomValue);

        }

        private void IncrementRoom(object sender, JavascriptMethodEventArgs e)
        {

            var roomValue = Math.Min(Math.Ceiling((decimal) (_model.SessionsAvailable.Count/8)), _model.RoomPage + 1);
            _controller.ChangeRoom((int)roomValue);
        }

        public override void UpdateView()
        {

            ExecuteJavascriptViaString("setPlayerNames", _model.OnlineUsers);
            var list = new List<string>();
            list.Add(_model.News);
            ExecuteJavascriptViaString("setNews", list);


            // Set the rooms
            var roomList = _model.SessionsAvailable.Select(room => room.Name).ToList();
            roomList = roomList.Skip(_model.RoomPage*8).ToList();

            ExecuteJavascriptViaString("setRoomNames", roomList);


                
        }



    }
}
