using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using System.Web;

namespace SignalR.Relay
{
    public class SignalRHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var c = GlobalHost.ConnectionManager.GetHubContext<RelayHub>();
            c.Clients.All.foo(2);

            context.Response.Write("EXTERNAL CHECK COMPLETE");
        }
    }
}