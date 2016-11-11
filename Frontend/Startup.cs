using Backend;
using Frontend.Controllers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Optimization;

[assembly: OwinStartup(typeof(Frontend.Startup))]

namespace Frontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}