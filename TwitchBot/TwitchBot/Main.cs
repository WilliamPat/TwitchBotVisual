using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace TwitchBot
{
    public partial class Main : Form
    {
        SavingXML<Connection> saveConnection;
        Regex rgx;
        IRC twitch;
        public Main()
        {
            saveConnection = new SavingXML<Connection>();
            //IRC("youtheshepherd", "oauth:0x2sdluao24dmajxwjgmopoi6fh5t3");
            rgx = new Regex("[^a-zA-Z0-9 -]");
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void Output_Click(object sender, EventArgs e)
        {

        }

        private void timerCall(object sender, EventArgs e)
        {
            getMessage();
        }

        void getMessage()
        {
            string msg;
            if(twitch!=null)
            {
                msg = twitch.getMessages();
                if (msg != "")
                {
                    output.Text += msg;
                }
            }
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string text = rgx.Replace(input.Text, "");
            twitch.say(text);
            output.Text += "You: " + text + "\r\n";
            input.Clear();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connection co = new Connection(txtUsername.Text, txtPassword.Text);
            twitch = new IRC(co);
            txtPassword.Text = "";
            this.Name = "Twitch Bot - " + txtUsername;
            saveConnection.SerializeObject(co,co.username);
            saveConnection.SerializeObject(co, "lastConnection");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnSend.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            Connection co = saveConnection.DeSerializeObject("lastConnection");
            twitch = new IRC(co);
            output.Text += "Connected to " + co.username + "\r\n";
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
