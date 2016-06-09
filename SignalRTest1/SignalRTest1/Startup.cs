using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using SignalRTest1;

namespace SignalRTest1 {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.MapSignalR();
        }
    }
}