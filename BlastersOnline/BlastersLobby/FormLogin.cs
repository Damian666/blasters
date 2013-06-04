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

namespace BlastersLobby
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();

            NetworkManager.Instance.Update();
            NetworkManager.Instance.Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FormWebView form = new FormWebView();
            form.Show();
            Visible = false;

            return;
            Form1 form1 = new Form1();
            form1.Show();
            Visible = false;
        }
    }
}
