using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppServer.Network;
using BlastersLobby.Network;
using BlastersShared.GameSession;
using BlastersShared.Network.Packets;
using BlastersShared.Network.Packets.ClientLobby;
using BlastersShared.Network.Packets.Lobby;

namespace BlastersLobby
{
    public partial class Form1 : Form
    {
        private List<GameSession> _sessions = new List<GameSession>();
        private uint _pendingJoin = uint.MaxValue;

        public Form1()
        {
            InitializeComponent();


            // Register network callbacks
            PacketService.RegisterPacket<SessionListInformationPacket>(ProcessSessionInformation);
            PacketService.RegisterPacket<SessionJoinResultPacket>(ProcessJoinResult);
            PacketService.RegisterPacket<ChatPacket>(ProcessChatPacket);
            PacketService.RegisterPacket<SessionBeginNotificationPacket>(ProcessSessionBegin);

        }

        private void ProcessSessionBegin(SessionBeginNotificationPacket obj)
        {
            // This form should not be enabled whilst a game is running
            //Enabled = false;

            var args = obj.SecureToken + " " + obj.RemoteEndpoint + " " + obj.SessionID;
            Process.Start("BlastersGame.exe", args);
        }

        private void ProcessChatPacket(ChatPacket obj)
        {
           txtChat.AppendText(obj.Message + Environment.NewLine);
        }

        private void ProcessJoinResult(SessionJoinResultPacket obj)
        {
            var result = obj.Result;

            if (result == SessionJoinResultPacket.SessionJoinResult.Succesful)
            {
                UpdateSessionView();
            }

            else
            {
                MessageBox.Show("Failed to join the game; possibly already in a session or this room is full.");
            }

        }

        private void UpdateSessionView()
        {
            if(_pendingJoin == uint.MaxValue)
                return;

            var session = (from x in _sessions where x.SessionID == _pendingJoin select x).First();

            propertySessionInfo.SelectedObject = session;
            lstUsers.DataSource = session.Users;


        }

        private void ProcessSessionInformation(SessionListInformationPacket obj)
        {
           // Clear the old list
            lstSessions.DataSource = null;
           lstSessions.Items.Clear();
            lstSessions.DataSource = obj.GameSessions;
            _sessions = obj.GameSessions;


            UpdateSessionView();
        }


        private string randomName = "";

        private string GetRandomName()
        {
            string[] arrays = File.ReadAllText(Environment.CurrentDirectory + "\\NAMES.DAT").Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Random random = new Random((int) DateTime.Now.Ticks);
            int index = random.Next(arrays.Length - 1);

            return arrays[index];

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            randomName = GetRandomName();
            Text = "Lobby - Hi, " + randomName;

            var packet = new LoginRequestPacket(randomName, "password");
            NetworkManager.Instance.SendPacket(packet);


        }

       


        private void timerNetwork_Tick(object sender, EventArgs e)
        {
            NetworkManager.Instance.Update();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void lstSessions_DoubleClick(object sender, EventArgs e)
        {
            var selectedSession = (GameSession) lstSessions.SelectedItem;

            if (selectedSession != null)
            {
                _pendingJoin = selectedSession.SessionID;
                var packet = new SessionJoinRequestPacket(selectedSession.SessionID);
                NetworkManager.Instance.SendPacket(packet);
            }


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                var packet = new ChatPacket(textBox1.Text);
                NetworkManager.Instance.SendPacket(packet);
                textBox1.Text = string.Empty;
            }

        }

        private void newMatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormNewMatch();
            form.ShowDialog();
        }

        private void lstSessions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
