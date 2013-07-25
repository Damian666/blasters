using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppServer.Network;
using BlastersLobby.Views;
using BlastersShared.Audio;
using BlastersShared.Network.Packets.ClientLobby;

namespace BlastersLobby
{
    public partial class FormWebView : Form
    {
        ViewFlowController _flowController;

        public FormWebView()
        {
            InitializeComponent();


        }

        private void FormWebView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormWebView_Load(object sender, EventArgs e)
        {
            // Start playing music
            AudioManager audioManager = new AudioManager();
            audioManager.Play(@"Content/Audio/Music/lobby.ogg");


            // Initalize our flow controller
            _flowController = new ViewFlowController(webControl1);

            webControl1.ConsoleMessage += webControl1_ConsoleMessage;       

            // Change the view as needed
            var roomSelectView = new LoginView();
            _flowController.ChangeView(roomSelectView);
        }

        void webControl1_ConsoleMessage(object sender, Awesomium.Core.ConsoleMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void timerPacketPoller_Tick(object sender, EventArgs e)
        {
            NetworkManager.Instance.Update();
        }


    }
}
