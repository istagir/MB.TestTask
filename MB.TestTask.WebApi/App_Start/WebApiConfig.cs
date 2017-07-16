using MB.TestTask.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace MB.TestTask.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new RequestHeaderMapping("Accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, "application/json"));

            var exceptionHandlerFilter = (GlobalExceptionHandlerAttribute)config.DependencyResolver.GetService(typeof(GlobalExceptionHandlerAttribute));
            config.Filters.Add(exceptionHandlerFilter);

            var authenticationFilter = (TokenAuthenticationFilter)config.DependencyResolver.GetService(typeof(TokenAuthenticationFilter));
            config.Filters.Add(authenticationFilter);

            var validateModelFilter = (ValidateModelAttribute)config.DependencyResolver.GetService(typeof(ValidateModelAttribute));
            config.Filters.Add(validateModelFilter);

            // Enforce HTTPS
            //config.Filters.Add(new LocalAccountsApp.Filters.RequireHttpsAttribute());
        }
    }
}
