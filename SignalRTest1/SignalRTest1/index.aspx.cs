using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Script.Services;

namespace SignalRTest1 {
    public partial class index : System.Web.UI.Page {

        public static IRC tIRC;

        protected void Page_Load(object sender, EventArgs e) {
            tIRC = new IRC("xxx.xxx.xxx.xxx", 6667, "testo", "testo", "#test");            
            Task.Factory.StartNew( () => tIRC.Connect() );

        }
        
        [WebMethod]
        public static void IRCWrite(string message) {
            tIRC.toWrite.Add(message);            
        }                
    }
}