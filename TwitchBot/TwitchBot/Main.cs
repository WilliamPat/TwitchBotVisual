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

namespace TwitchBot
{
    public partial class Main : Form
    {
        Regex rgx;
        IRC twitch;
        public Main()
        {
            twitch = new IRC("youtheshepherd", "oauth:0x2sdluao24dmajxwjgmopoi6fh5t3");
            rgx = new Regex("[^a-zA-Z0-9 -]");
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
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
            msg = twitch.getMessages();
            if(msg != "")
            {
                output.Text += msg;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string text = rgx.Replace(input.Text, "");
            twitch.say(text);
            output.Text += "You: " + text + "\r\n";
            input.Text = "";
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
