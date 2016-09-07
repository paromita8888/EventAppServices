using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

using System.Net.Http.Formatting;

using System.Web.Http.Cors;
using WebApiContrib.Formatting.Jsonp;

namespace EventAppServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
            var enableCorsAttribute = new EnableCorsAttribute("*", "*", "*");

           config.EnableCors(enableCorsAttribute);
            config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
            config.Routes.MapHttpRoute( name: "DefaultApi",   routeTemplate: "api/{controller}/{id}",            defaults: new { id = RouteParameter.Optional }
            );
          GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter(new JsonMediaTypeFormatter()));
         
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}
