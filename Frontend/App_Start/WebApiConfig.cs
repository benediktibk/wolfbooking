﻿using System;
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
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { controller = "Home", action = "Get" }
            );
        }
    }
}