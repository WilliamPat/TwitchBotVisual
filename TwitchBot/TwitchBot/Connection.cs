using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public class Connection
    {
        public string username;
        public string password;


        public Connection()
        {

        }

        public Connection(string u, string p)
        {
            username = u;
            password = p;
        }
    }
}
