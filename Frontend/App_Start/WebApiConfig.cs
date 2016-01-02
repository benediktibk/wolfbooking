using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http.Headers;

namespace Frontend
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Routes.MapHttpRoute(
                name: "ItemSelection",
                routeTemplate: "{controller}/{id}"
            );
            config.Routes.MapHttpRoute(
                name: "ViewSite",
                routeTemplate: "{controller}"
            );
        }
    }
}
