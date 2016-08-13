using Backend.Persistence;
using System.Data.Entity;
using System.Web;
using System.Web.Http;

namespace Frontend
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new WolfBookingInitializer());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
