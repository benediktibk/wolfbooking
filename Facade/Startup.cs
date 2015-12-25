using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Facade.Startup))]

namespace Facade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
