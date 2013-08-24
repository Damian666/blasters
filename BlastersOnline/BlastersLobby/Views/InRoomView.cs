using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppServer.Network;
using Awesomium.Core;
using BlastersLobby.Controllers;
using BlastersLobby.Models;
using BlastersLobby.Network;
using BlastersShared.Network.Packets;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.Lobby;

namespace BlastersLobby.Views
{
    /// <summary>
    /// The room select view is used 
    /// </summary>
    public class InRoomView : View
    {
        private const string VIEW_PATH_HTML = @"Content\LobbyBridge\GameReadyView\index.html";
        private const string VIEW_PATH_JS = @"Content\LobbyBridge\GameReadyView\functions.js";

        // The model and controller are here
        private RoomSelectController _controller;
        private RoomSelectModel _model;

        public override void OnViewAppeared()
        {
            FlowController.WebControl.Source = new Uri(Environment.CurrentDirectory + @"\" + VIEW_PATH_HTML);


            PacketService.RegisterPacket<ChatPacket>(ProcessChatPacket);
            PacketService.RegisterPacket<SessionBeginNotificationPacket>(Handler);
            PacketService.RegisterPacket<SessionEndedLobbyPacket>(Handler);

            _model = new RoomSelectModel();
          


            FlowController.WebControl.DocumentReady += WebControlOnDocumentReady;

        }

        private void Handler(SessionEndedLobbyPacket sessionEndedLobbyPacket)
        {

            //MessageBox.Show("The winner was " + sessionEndedLobbyPacket.SessionStatistics.Winner);
        }

        private void Handler(SessionBeginNotificationPacket sessionBeginNotificationPacket)
        {
            var args = sessionBeginNotificationPacket.SecureToken + " " + sessionBeginNotificationPacket.RemoteEndpoint + " " + sessionBeginNotificationPacket.SessionID;
         
            var proc = Process.Start("BlastersGame.exe", args);
            proc.Exited += proc_Exited;


            FlowController.WebControl.Enabled = false;

        }

        void proc_Exited(object sender, EventArgs e)
        {
            FlowController.WebControl.Enabled = true;
        }

        private void ProcessChatPacket(ChatPacket obj)
        {
            var js = "document.getElementById('chatarea').innerHTML +=' " + obj.Message + "';";
            FlowController.WebControl.ExecuteJavascript(js);
        }

        private void WebControlOnDocumentReady(object sender, UrlEventArgs urlEventArgs)
        {


            JSObject jsobject = FlowController.WebControl.CreateGlobalJavascriptObject("hook");

            jsobject.Bind("sendchat", false, Handler);


            return;

            // Updates the view immediately
            UpdateView();
        }

        private void Handler(object sender, JavascriptMethodEventArgs javascriptMethodEventArgs)
        {

            var chatid = "chatinputs";

            var chattext = FlowController.WebControl.ExecuteJavascriptWithResult("document.getElementById('chatinputs').value;");

            var packet = new ChatPacket(chattext);
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

        public override void OnClose()
        {
            throw new NotImplementedException();
        }
    }
}
