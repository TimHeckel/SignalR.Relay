﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Web.Http;

namespace SignalR.Relay
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var dynamicProvider = new RelayDescriptorProvider();
            GlobalHost.DependencyResolver.Register(typeof(IHubDescriptorProvider), () => dynamicProvider);
            var methodProvier = new RelayMethodDescriptorProvider();
            GlobalHost.DependencyResolver.Register(typeof(IMethodDescriptorProvider), () => methodProvier);

            RouteTable.Routes.MapHubs();
        }
    }

    public class AuthAttribute : Attribute, IAuthorizeHubMethodInvocation
    {
        public bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext)
        {
            return true;
        }
    }

    public class RelayMethodDescriptorProvider : IMethodDescriptorProvider
    {
        public IEnumerable<MethodDescriptor> GetMethods(HubDescriptor hub)
        {
            return Enumerable.Empty<MethodDescriptor>();
        }

        public bool TryGetMethod(HubDescriptor hub, string method, out MethodDescriptor descriptor, params IJsonValue[] parameters)
        {
            descriptor = new MethodDescriptor
            {
                Hub = hub,
                Invoker = (h, args) =>
                {
                    IClientProxy proxy = h.Clients.All;
                    return proxy.Invoke(method, args);
                },
                Name = method,
                Attributes = new List<AuthAttribute>() { new AuthAttribute() },
                Parameters = Enumerable.Range(0, parameters.Length).Select(i => new Microsoft.AspNet.SignalR.Hubs.ParameterDescriptor { Name = "p_" + i, Type = typeof(object) }).ToArray(),
                ReturnType = typeof(Task)
            };

            return true;
        }
    }

    /// <summary>
    /// Allows dynamic hub discovery
    /// </summary>
    public class RelayDescriptorProvider : IHubDescriptorProvider
    {
        public IList<HubDescriptor> GetHubs()
        {
            return new List<HubDescriptor>();
        }

        /// <summary>
        /// Returns a hub descriptor for the spec
        /// </summary>
        /// <param name="hubName"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public bool TryGetHub(string hubName, out HubDescriptor descriptor)
        {
            descriptor = new HubDescriptor { Name = hubName, Type = typeof(RelayHub) };
            return true;
        }
    }

    public class RelayHub : Hub
    {

    }
}