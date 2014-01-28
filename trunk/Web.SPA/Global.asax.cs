using NLog;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Common;

namespace Web.SPA
{
    public class MvcApplication : HttpApplication
    {
        private static Logger logger = LogManager.GetLogger(Consts.LOGGER_NAME);

        protected void Application_Start()
        {
            logger.Info("Запуск приложения");

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Приложение запущено");
        }

        protected void Application_End()
        {
            logger.Info("Приложение завершено");
        }
    }
}