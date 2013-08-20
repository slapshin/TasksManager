using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Web.SPA
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Filters.Add(new HostAuthenticationFilter(IdentityConfig.Bearer.AuthenticationType));
            config.SuppressDefaultHostAuthentication();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "AreaActionApi",
                routeTemplate: "api/{area}/{controller}/{action}/{id}",
                defaults: new { action = "DefaultAction", id = System.Web.Http.RouteParameter.Optional }
           );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
                //defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an
            // IQueryable or IQueryable<T> return type. To avoid processing unexpected or malicious
            // queries, use the validation settings on QueryableAttribute to validate incoming
            // queries. For more information, visit http://go.microsoft.com/fwlink/?LinkId=301869.
            //config.EnableQuerySupport();

            // Uncomment the following line of code to enable tracing in your application. For more
            // information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}