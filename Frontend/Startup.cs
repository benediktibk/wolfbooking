using Microsoft.Owin;
using Owin;

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
