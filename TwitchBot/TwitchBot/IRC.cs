
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using TwitchBot;

public class IRC {
    private static byte[] data;
    NetworkStream stream;
    TcpClient client;
    string channel ;
    string loginstring;

    public int port = 6667;
    public string hostname = "irc.twitch.tv";
    public string username ;
    public string pass ;

    public IRC(string user, string pwd)
    {
        
        username = user;
        pass = pwd;
        init();
    }

    public IRC(Connection co)
    {

        username = co.username;
        pass = co.password;
        init();
    }

    void init()
    {
        channel = "#" + username;
        loginstring = "PASS " + pass + "\r\nNICK " + username + "\r\n";
        client = new TcpClient(hostname, port);
        stream = client.GetStream();

        Byte[] login = System.Text.Encoding.ASCII.GetBytes(loginstring);
        stream.Write(login, 0, login.Length);
        data = new Byte[512];

        string responseData = String.Empty;
        Int32 bytes = stream.Read(data, 0, data.Length);
        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        string joinstring = "JOIN " + channel + "\r\n";
        Byte[] join = System.Text.Encoding.ASCII.GetBytes(joinstring);
        stream.Write(join, 0, join.Length);
        stream.Flush();
    }


    public void say(string str)
    {
        try
        {
            string format = ":" + username + "!" + username + "@" + username + ".tmi.twitch.tv PRIVMSG " + channel + " :" + str + "\r\n";

            Byte[] say = System.Text.Encoding.ASCII.GetBytes(format);
            stream.Write(say, 0, say.Length);
            stream.Flush();
            Console.Write(str);
        }
        catch (Exception e)
        {
            Console.Write("SOMETHING WENT WRONG\r\n" + e);
        }
    }

    
   public void messages () {
        while (true)
        {
            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;

            while (stream.DataAvailable == false)
            {
                
            }

            // Incoming message may be larger than the buffer size.
            do
            {
                try
                {
                    
                    numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                }
                catch (Exception e)
                {
                    Console.Write("OH SHIT SOMETHING WENT WRONG\r\n"+ e);
                }

                myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
            }while (stream.DataAvailable);
            
            try
            {
                string messageParser = myCompleteMessage.ToString();
                string[] message = messageParser.Split(':');
                string[] preamble = message[1].Split(' ');
                string tochat;

                // This means it's a message to the channel.  Yes, PRIVMSG is IRC for messaging a channel too
                if (preamble[1] == "PRIVMSG")
                {
                    string[] sendingUser = preamble[0].Split('!');
                    tochat = sendingUser[0] + ": " + message[2];

                    // sometimes the carriage returns get lost (??)
                    if (tochat.Contains("\n") == false)
                    {
                        tochat = tochat + "\n";
                    }
                   
                    // user and message data extracted
                    //Console.Write(sendingUser[0]+": "+message[2]);
                    
                    //send the message to the prog

                }
                /*else if (preamble[1] == "JOIN")       // do something when user join
                {
                    string[] sendingUser = preamble[0].Split('!');
                    tochat = "JOINED: " + sendingUser[0];
                    //    Console.Write(tochat);
                    //SendKeys.SendWait(tochat.TrimEnd('\n'));
                    Console.Write(tochat);
                }*/
            }
            catch (Exception e)
            {
                Console.Write("SOMETHING WENT WRONG\r\n"+ e);
            }
            // Console.Write("Raw output: " + message[0] + "::" + message[1] + "::" + message[2]);
            // Console.Write("You received the following message : " + myCompleteMessage);
        }
        
    }


    public string getMessages()
    {
        if (client== null) return "";

        byte[] myReadBuffer = new byte[1024];
        StringBuilder myCompleteMessage = new StringBuilder();
        int numberOfBytesRead = 0;

        if (stream.DataAvailable == true)
        {
            do
            {
                try
                {

                    numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                }
                catch (Exception e)
                {
                    Console.Write("OH SHIT SOMETHING WENT WRONG\r\n" + e);
                }

                myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
            } while (stream.DataAvailable);

            try
            {
                string messageParser = myCompleteMessage.ToString();
                string[] message = messageParser.Split(':');
                string[] preamble = message[1].Split(' ');
                string tochat;
            
                if (preamble[1] == "PRIVMSG")
                {
                    string[] sendingUser = preamble[0].Split('!');
                    tochat = sendingUser[0] + ": " + message[2];
                    if (tochat.Contains("\n") == false)
                    {
                        tochat = tochat + "\n";
                    }

                    // user and message data extracted
                    //Console.Write(sendingUser[0]+": "+message[2]);

                    //send the message to the prog
                    return sendingUser[0] + ": " + message[2];
                }
                /*else if (preamble[1] == "JOIN")       // do something when user join
                {
                    string[] sendingUser = preamble[0].Split('!');
                    tochat = "JOINED: " + sendingUser[0];
                    //    Console.Write(tochat);
                    //SendKeys.SendWait(tochat.TrimEnd('\n'));
                    Console.Write(tochat);
                }*/
            }
            catch (Exception e)
            {
                Console.Write("SOMETHING WENT WRONG\r\n" + e);
            }
            
        }
        return "";
    }

}
