using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace SignalRTest1 {
    public partial class index : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            IRC tIRC = new IRC("xxx.xxx.xxx.xxx", 6667, "testo", "testo");
            tIRC.eventReceiving += new IRC.CommandReceived(IrcCommandReceived);
            Task.Factory.StartNew( () => tIRC.Connect() );

        }

        static void IrcCommandReceived(string comm) {
            Console.WriteLine(comm);
        }
    }
}