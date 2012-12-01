using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace SignalR.Relay.Controllers
{
    public class TestController : Controller
    {
        [GET("TestHub")]
        public string Test()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<RelayHub>(); //
            context.Clients.All.foo(2); // (new { top = "DELETE" });


            return "value";
        }
    }
}