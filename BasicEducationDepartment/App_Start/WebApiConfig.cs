using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BasicEducationDepartment
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //setup cors START
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            //setup cors END
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
