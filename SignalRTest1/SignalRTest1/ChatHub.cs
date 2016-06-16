using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRTest1 {
    public class ChatHub : Hub {
        public void Send(string name, string message) {
            //if(action.Equals("Message")) {
                Clients.All.broadcastMessage(name, message);
            //} else if (action.Equals("User")) {
                Clients.All.broadcastUser(name);
            //}
            
        }        
    }
}