using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.AspNet.SignalR;

namespace SignalRTest1 {
    public class IRC {

        public string IRCServer = "xxx.xxx.xxx.xxx";
        public int IRCPort = 6667;
        public string IRCNick = "testl";
        public string IRCRealName = "testl";
        public string IRCChannel = "#test";

        public delegate void CommandReceived(string comm);
        public event CommandReceived eventReceiving;

        private TcpClient IrcConnection;
        private NetworkStream IrcStream;
        private StreamReader IrcReader;
        private StreamWriter IrcWriter;

        public IRC(string s, int p, string n, string r) {
            IRCServer = s;
            IRCPort = p;
            IRCNick = n;
            IRCRealName = r;
        }

        public void Connect() {
            string read;
            bool connected = false;

            //Connect to server
            IrcConnection = new TcpClient(IRCServer, IRCPort);
            IrcStream = IrcConnection.GetStream();
            IrcReader = new StreamReader(IrcStream);
            IrcWriter = new StreamWriter(IrcStream);

            //Authenticate
            IrcWriter.WriteLine("USER " + IRCRealName + " * * :" + IRCRealName);
            IrcWriter.Flush();
            IrcWriter.WriteLine("NICK " + IRCNick);
            IrcWriter.Flush();

            

            while ((read = IrcReader.ReadLine()) != null) {
                if (connected) {
                    GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients.All.broadcastMessage(IRCNick, read);
                }
                if (read.Contains("PING")) {
                    read = read.Replace("PING", "PONG");
                    IrcWriter.WriteLine(read);
                    IrcWriter.Flush();
                }
                if(read.Contains("Welcome")) {
                    IrcWriter.WriteLine("JOIN " + IRCChannel);
                    IrcWriter.Flush();
                    connected = true;
                }                
            }

            

            
        }

        




    }
}