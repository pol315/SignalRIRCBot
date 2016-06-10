using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.SignalR;

namespace SignalRTest1 {
    public class IRC {

        public string IRCServer = "127.0.0.1";
        public int IRCPort = 6667;
        public string IRCNick = "testl";
        public string IRCRealName = "testl";
        public string IRCChannel = "#test";

        public List<string> messages;
        public List<string> toWrite;

        private TcpClient IrcConnection;
        private NetworkStream IrcStream;
        public StreamReader IrcReader;
        public StreamWriter IrcWriter;

        public IRC(string s, int p, string n, string r, string c) {
            IRCServer = s;
            IRCPort = p;
            IRCNick = n;
            IRCRealName = r;
            IRCChannel = c;
            messages = new List<string>();
            toWrite = new List<string>();
        }

        public void Connect() {
            try {
                string read = "";
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

                Task.Factory.StartNew(() => getData(ref read, ref connected));
                Task.Factory.StartNew(() => sendData());

            } catch (Exception ex) {
                
            }
        }

        public void WriteToIRC(string message) {
            IrcWriter.WriteLine("PRIVMSG " + IRCChannel + " :" + message);
            IrcWriter.Flush();
        }

        public void getData(ref string read, ref bool connected) {
            while (true) {                
                if ((read = IrcReader.ReadLine()) != null) {
                    messages.Add(read);
                    if (connected) {
                        List<string> msg = read.Split(':').ToList();
                        if (msg.Count > 2) {
                            List<string> nick = msg[1].Split('!').ToList();
                            GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients.All.broadcastMessage(nick[0], msg.Last());
                        }
                    }
                    if (read.Contains("PING")) {
                        read = read.Replace("PING", "PONG");
                        IrcWriter.WriteLine(read);
                        IrcWriter.Flush();
                    }
                    if (read.Contains("Welcome")) {
                        IrcWriter.WriteLine("JOIN " + IRCChannel);
                        IrcWriter.Flush();
                    }
                    if (read.Contains(IRCChannel)) {
                        connected = true;

                    }
                }
                if (toWrite.Count != 0) {
                    WriteToIRC(toWrite.First());
                    toWrite.Remove(toWrite.First());
                }
            }
        }

        public void sendData() {
            while (true) {
                if (toWrite.Count != 0) {
                    WriteToIRC(toWrite.First());
                    toWrite.Remove(toWrite.First());
                }
            }
        }






    }
}