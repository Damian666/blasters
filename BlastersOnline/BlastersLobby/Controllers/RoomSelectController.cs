using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersLobby.Models;
using BlastersLobby.Network;
using BlastersLobby.Views;
using BlastersShared.Network.Packets;
using BlastersShared.Network.Packets.Lobby;

namespace BlastersLobby.Controllers
{
    /// <summary>
    ///  The controller is responsible for updating the model that the view will consume.
    /// </summary>
    public class RoomSelectController
    {
        // These are okay to be deeply coupled as they're very related
        private RoomSelectModel _viewModel;
        private RoomSelectView _view;


        public RoomSelectController(RoomSelectModel viewModel, RoomSelectView view)
        {
            _viewModel = viewModel;
            _view = view;

            CreateNetworkCallbacks();
        }

        private void CreateNetworkCallbacks()
        {
            PacketService.RegisterPacket<SessionListInformationPacket>(ProcessSessionInformation);
            PacketService.RegisterPacket<NotifyUsersOnlinePacket>(Handler); 
        }

        private void Handler(NotifyUsersOnlinePacket notifyUsersOnlinePacket)
        {
            _viewModel.OnlineUsers = notifyUsersOnlinePacket.OnlineUsers;
            _view.UpdateView();
        }


        private void ProcessSessionInformation(SessionListInformationPacket obj)
        {
            // All we need to do is update the model
            _viewModel.SessionsAvailable = obj.GameSessions;
            _view.UpdateView();
        }



    }
}
